using System.Windows.Threading;
using Desktop.Common;
using Desktop.Tasks;

namespace Desktop;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IRemoteTaskRepository backend;

    private static readonly TimeSpan CheckBackendAvailabilityInterval =
        TimeSpan.FromSeconds(2);

    private string backendAvailability = "Available";
    private readonly DispatcherTimer periodicBackendAvailabilityCheckTimer;

    public string BackendAvailability
    {
        get => backendAvailability;
        set
        {
            backendAvailability = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(IRemoteTaskRepository backend)
    {
        this.backend = backend;
        periodicBackendAvailabilityCheckTimer = new DispatcherTimer
        {
            Interval = CheckBackendAvailabilityInterval
        };
        periodicBackendAvailabilityCheckTimer.Tick += async (_, _) =>
            await CheckBackendAvailability();
        periodicBackendAvailabilityCheckTimer.Start();
    }

    private async Task CheckBackendAvailability()
    {
        BackendAvailability = await backend.IsAvailable() ? "Available" : "Non available";
    }
}