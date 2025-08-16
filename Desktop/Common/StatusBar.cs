namespace Desktop.Common;

public class StatusBar : IMessageNotifier
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public StatusBar(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
    }

    public void Notify(string message)
    {
        mainWindowViewModel.StatusMessage = message;
    }
}