using System.Windows.Threading;
using Desktop.Common;
using Desktop.Tasks;

namespace Desktop;

public class MainWindowViewModel : ViewModelBase
{
    private static readonly TimeSpan CheckBackendAvailabilityInterval =
        TimeSpan.FromSeconds(2);

    private bool isBackendAvailable;
    private readonly DispatcherTimer periodicBackendAvailabilityCheckTimer;

    public bool IsBackendAvailable
    {
        get => isBackendAvailable;
        set
        {
            isBackendAvailable = value;
            OnPropertyChanged();
        }
    }

    private string statusMessage;

    public string StatusMessage
    {
        get => statusMessage;
        set
        {
            statusMessage = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(IRemoteTaskRepository backend)
    {
        periodicBackendAvailabilityCheckTimer = new DispatcherTimer
        {
            Interval = CheckBackendAvailabilityInterval
        };
        periodicBackendAvailabilityCheckTimer.Tick += async (_, _) =>
            IsBackendAvailable = await backend.IsAvailable();
        periodicBackendAvailabilityCheckTimer.Start();
    }
}