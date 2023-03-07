using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private WorkSession? _session;

    private readonly IDialogService _dialogService = new DialogService();
    private readonly IFileDataSource _fileDataSource = new TextFileDataSource();

    [RelayCommand]
    private void ImportData()
    {
        if (_dialogService.ShowOpenFileDialog())
        {
            try
            {
                PlotData data = _fileDataSource.GetPlotData(_dialogService.OpenFilePath);
                Session = new WorkSession(data);
            }
            catch (FileFormatException e)
            {
                _dialogService.ShowErrorMessage(e.Message);
            }
        }
    }

    [RelayCommand]
    private void OpenSession()
    {
        if (_dialogService.ShowOpenFileDialog())
        {
            Session = SessionOpener.OpenSession(_dialogService.OpenFilePath);
        }
    }

    [RelayCommand]
    private void CloseSession()
    {
        Session = null;
    }

    [RelayCommand]
    private void SaveSession()
    {
        if (Session == null)
        {
            throw new NullReferenceException();
        }
        if (_dialogService.ShowSaveFileDialog())
        {
            SessionSaver.SaveSession(Session, _dialogService.SaveFilePath);
        }
    }

    [RelayCommand]
    private void SaveSessionAs()
    {
        if (Session == null)
        {
            throw new NullReferenceException();
        }
        if (_dialogService.ShowSaveFileDialog())
        {
            SessionSaver.SaveSession(Session, _dialogService.SaveFilePath);
        }
    }

    [RelayCommand]
    private void ExportAs()
    {
        
    }
}
