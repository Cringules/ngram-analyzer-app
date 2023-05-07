using System;
using System.ComponentModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.Export;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CloseSessionCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveSessionCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveSessionAsCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExportAsCommand))]
    private WorkSession? _session;

    [ObservableProperty] private bool _sessionSaved;
    [ObservableProperty] private string? _sessionFilename;

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
                Session = new WorkSession(data.ToXray());
                SessionSaved = false;
                SessionFilename = null;
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
            Session = SessionFileManager.OpenSession(_dialogService.OpenFilePath);
            SessionSaved = true;
            SessionFilename = _dialogService.OpenFilePath;
        }
    }

    private bool IsSessionOpened()
    {
        return Session != null;
    }

    [RelayCommand(CanExecute = nameof(IsSessionOpened))]
    private void CloseSession()
    {
        Session = null;
    }

    [RelayCommand(CanExecute = nameof(IsSessionOpened))]
    private void SaveSession()
    {
        SaveSessionCommon();
    }

    [RelayCommand(CanExecute = nameof(IsSessionOpened))]
    private void SaveSessionAs()
    {
        SaveSessionCommon(true);
    }

    private void SaveSessionCommon(bool forceSaveAs = false)
    {
        if (Session == null)
        {
            throw new NullReferenceException();
        }

        string? filename = SessionFilename;
        if (forceSaveAs || filename == null)
        {
            _dialogService.SaveFilePath = filename ?? string.Empty;

            if (!_dialogService.ShowSaveFileDialog())
            {
                return;
            }

            filename = _dialogService.SaveFilePath;
        }

        SessionFileManager.SaveSession(Session, filename);
        SessionSaved = true;
        SessionFilename = filename;
    }

    [RelayCommand(CanExecute = nameof(IsSessionOpened))]
    private void ExportAs(IResultExporter resultExporter)
    {
        if (Session == null)
        {
            throw new NullReferenceException();
        }
        
        string? filename = Path.ChangeExtension(SessionFilename, resultExporter.FileExtension);
        _dialogService.SaveFilePath = filename ?? string.Empty;

        if (!_dialogService.ShowSaveFileDialog())
        {
            return;
        }

        filename = _dialogService.SaveFilePath;

        resultExporter.Export(Session, filename);
    }

    partial void OnSessionChanged(WorkSession? value)
    {
        if (value != null)
        {
            value.PropertyChanged += OnSessionPropertyChanged;
        }
    }

    partial void OnSessionChanging(WorkSession? value)
    {
        if (Session != null)
        {
            Session.PropertyChanged -= OnSessionPropertyChanged;
        }
    }

    private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        SessionSaved = false;
    }
}
