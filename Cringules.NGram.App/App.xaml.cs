using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Cringules.NGram.App.ViewModel;

namespace Cringules.NGram.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IDialogService _dialogService = new DialogService();
    
    public App()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _dialogService.ShowErrorMessage(e.Exception.Message);
        e.Handled = true;
    }
}
