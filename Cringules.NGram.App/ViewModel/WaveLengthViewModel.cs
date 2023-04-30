using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.App.Model;
using Microsoft.Extensions.Configuration;

namespace Cringules.NGram.App.ViewModel;

public partial class WaveLengthViewModel : ObservableObject
{
    [ObservableProperty] private double? _selectedValue;
    [ObservableProperty] private double? _selectedGridValue;

    [ObservableProperty] private ObservableCollection<WaveElement> _waveElements = new();

    public WaveLengthViewModel()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var waveLengthSelection = config.GetSection("WaveLengthSelection").Get<WaveLengthSelection>();
        if (waveLengthSelection != null)
        {
            _waveElements = new ObservableCollection<WaveElement>(waveLengthSelection.Values);
        }
    }

    partial void OnSelectedGridValueChanged(double? value)
    {
        if (value == null)
        {
            return;
        }

        SelectedValue = value;
    }

    [RelayCommand]
    private void CloseOk(IDialogWindow window)
    {
        window.DialogResult = true;
    }

    [RelayCommand]
    private void CloseCancel(IDialogWindow window)
    {
        window.DialogResult = false;
    }
}
