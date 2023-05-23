using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.App.Resources;
using Cringules.NGram.App.View;
using Cringules.NGram.App.ViewModel;
using Cringules.NGram.Lib;
using Microsoft.Extensions.Configuration;
using OxyPlot;

namespace Cringules.NGram.App.Model;

public partial class WorkSession : ObservableObject
{
    [ObservableProperty] private Xray _data;

    [ObservableProperty] private double _waveLength;

    [ObservableProperty] private int _minSmoothingDegree = 50;
    [ObservableProperty] private int _maxSmoothingDegree = 120;

    [ObservableProperty] private bool _smoothingEnabled = true;
    [ObservableProperty] private int _smoothingDegree = 70;
    [ObservableProperty] private Xray? _smoothedData;

    [ObservableProperty] private List<Point> _peakBoundaries = new();

    [ObservableProperty] private ObservableCollection<PeakData> _peaks = new();

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdatePeakCommand))] [NotifyCanExecuteChangedFor(nameof(DeletePeakCommand))]
    private PeakData? _selectedPeak;

    [ObservableProperty] private bool _peakShown;

    private WaveLengthSelection? _waveLengthSelection;

    [JsonIgnore] public DiffractogramPlotModel Model { get; } = new();
    [JsonIgnore] public PlotController PlotController { get; } = new();

    [JsonIgnore] public PeakPlotModel PeakModel { get; } = new();

    private readonly IDialogService _dialogService = new DialogService();

    public WorkSession(Xray xray)
    {
        Data = xray;
        PlotController.BindMouseDown(OxyMouseButton.Left,
            new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) =>
                controller.AddMouseManipulator(view, new PlotSelectionManipulator(view, Model), args)));

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(@"appsettings.json")
            .Build();
        _waveLengthSelection = config.GetSection(@"WaveLengthSelection").Get<WaveLengthSelection>();
        WaveLength = _waveLengthSelection?.DefaultValue ?? 0;

        Model.PropertyChanged += (_, _) => SelectPeakCommand.NotifyCanExecuteChanged();
        Model.PropertyChanged += (_, _) => CancelSelectionCommand.NotifyCanExecuteChanged();
        Model.PropertyChanged += (_, _) => AddPeakCommand.NotifyCanExecuteChanged();
        Model.PropertyChanged += (_, _) => UpdatePeakCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void GetWaveLength()
    {
        var viewModel = new WaveLengthViewModel() {SelectedValue = WaveLength};
        if (_waveLengthSelection != null)
        {
            viewModel.WaveElements = new ObservableCollection<WaveElement>(_waveLengthSelection.Values);
        }

        var window = new WaveLengthWindow() {DataContext = viewModel};
        if ((window.ShowDialog() ?? false) && viewModel.SelectedValue.HasValue)
        {
            WaveLength = viewModel.SelectedValue.Value;
        }
    }

    [RelayCommand]
    private void CleanUpData()
    {
    }

    public void SmoothData()
    {
        SmoothedData = Data.SmoothXray(SmoothingDegree);
        Model.SmoothedPoints = SmoothedData.ToPlotPoints();
    }

    [RelayCommand]
    private void StartAnalysis()
    {
        Xray data = Data;

        if (SmoothingEnabled)
        {
            SmoothedData = Data.SmoothXray(SmoothingDegree);
            Model.SmoothedPoints = SmoothedData.ToPlotPoints();

            data = SmoothedData;
        }
        else
        {
            SmoothedData = null;
            Model.SmoothedPoints = null;
        }

        PeakBoundaries = data.GetPeakBoundaries();
        Model.PeakBoundaries = PeakBoundaries;

        var xrayPeaks = new List<XrayPeak>();
        for (var i = 0; i < PeakBoundaries.Count - 1; i++)
        {
            xrayPeaks.Add(data.GetPeak(PeakBoundaries[i].X, PeakBoundaries[i + 1].X));
        }

        Peaks = new ObservableCollection<PeakData>(xrayPeaks.Select(peak => new PeakData(peak, WaveLength)));
        foreach (PeakData peak in Peaks)
        {
            peak.Approximate();
        }

        if (Peaks.Count > 0)
        {
            UpdatePeakIntensity();
        }
    }

    private bool CanSelectPeak()
    {
        return !Model.CanSelect;
    }

    [RelayCommand(CanExecute = nameof(CanSelectPeak))]
    private void SelectPeak()
    {
        Model.CanSelect = true;
    }

    private bool CanCancelSelection()
    {
        return Model.CanSelect;
    }

    [RelayCommand(CanExecute = nameof(CanCancelSelection))]
    private void CancelSelection()
    {
        Model.CanSelect = false;
    }

    private bool CanAddPeak()
    {
        return Model.FinishedSelection;
    }

    [RelayCommand(CanExecute = nameof(CanAddPeak))]
    private void AddPeak()
    {
        List<double> points = Model.SelectedBoundary.ToList();
        points.Sort();
        XrayPeak peak = (SmoothedData ?? Data).GetPeak(points[0], points[1]);
        Peaks.Add(new PeakData(peak, WaveLength));

        Model.CanSelect = false;
        UpdatePeakIntensity();
    }

    private bool CanUpdatePeak()
    {
        return Model.FinishedSelection && SelectedPeak != null;
    }

    [RelayCommand(CanExecute = nameof(CanUpdatePeak))]
    private void UpdatePeak()
    {
        if (SelectedPeak == null)
        {
            return;
        }

        List<double> points = Model.SelectedBoundary.ToList();
        points.Sort();
        SelectedPeak.XrayPeak = (SmoothedData ?? Data).GetPeak(points[0], points[1]);

        Model.CanSelect = false;
        UpdatePeakIntensity();
    }

    partial void OnDataChanged(Xray value)
    {
        Model.PlotPoints = value.ToPlotPoints();
    }

    partial void OnSelectedPeakChanged(PeakData? value)
    {
        if (value == null)
        {
            PeakShown = false;
        }

        Model.SelectedPeak = value;
        PeakModel.SelectedPeak = value;
    }

    [RelayCommand]
    private void ShowPeak()
    {
        if (SelectedPeak != null)
        {
            PeakShown = true;
        }
    }

    private bool CanDeletePeak()
    {
        return SelectedPeak != null;
    }

    [RelayCommand(CanExecute = nameof(CanDeletePeak))]
    private void DeletePeak()
    {
        if (SelectedPeak == null)
        {
            return;
        }

        if (_dialogService.AskConfirmation(Strings.PeakDeleteQuestion))
        {
            Peaks.Remove(SelectedPeak);
        }
        
        if (Peaks.Count > 0)
        {
            UpdatePeakIntensity();
        }
    }

    private void UpdatePeakIntensity()
    {
        double maxPeakMaxIntensity = Peaks.Select(data => data.MaxIntensity).Max();
        double maxPeakIntegralIntensity = Peaks.Select(data => data.IntegralIntensity).Max();

        foreach (PeakData peak in Peaks)
        {
            peak.MaxPeakMaxIntensity = maxPeakMaxIntensity;
            peak.MaxPeakIntegralIntensity = maxPeakIntegralIntensity;
        }
    }
}
