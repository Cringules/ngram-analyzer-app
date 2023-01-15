﻿using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.ViewModel;

public partial class WorkSession : ObservableObject
{
    [ObservableProperty] private PlotData _data = null!;

    [ObservableProperty] private List<Peak> _peaks = new();

    public DiffractogramPlotModel Model { get; } = new();

    public WorkSession(PlotData data)
    {
        Data = data;
    }
    
    [RelayCommand]
    private void UpdatePoints()
    {
        Data = PlotCleaner.GetCleanedPlot(Data);
    }
    
    partial void OnDataChanged(PlotData? value)
    {
        Model.Update(value?.Points);
        Peaks = PeakFinder.FindPeaks(value);
    }
}