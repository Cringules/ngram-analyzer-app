using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Points))]
    private PlotData? _data;

    [ObservableProperty] private PlotPoint? _selectedPoint;

    public IEnumerable<PlotPoint>? Points => _data?.Points;

    public MainViewModel()
    {
        var points = new List<PlotPoint> { new(1, 1), new(2, 5), new(3, 4), new(4, 6) };
        _data = new PlotData(points);
    }

    [RelayCommand]
    private void UpdatePoints()
    {
        if (Data != null)
        {
            Data = PlotCleaner.GetCleanedPlot(Data);
        }
    }
}
