namespace Codespirals.Blazor.Components;

/// <summary>
/// A timer that counts up from 0
/// </summary>
public class UpCountingTimer : Interfaces.ITimer
{
    private readonly System.Threading.Timer _timer;
    private TimeSpan _runTime = TimeSpan.Zero;
    private readonly long _timeBetweenTicksInMilliseconds = 1000;
    private readonly TimerCallback? _callback;

    /// <inheritdoc />
    public bool IsTimerRunning { get; set; }
    /// <inheritdoc />
    public bool Paused { get; set; }
    /// <inheritdoc />
    public string TimeString => GetTimeString();
    /// <inheritdoc />
    public TimeSpan Time => _runTime;

    /// <inheritdoc />
    public bool ShowHours { get; set; }
    /// <inheritdoc />
    public bool ShowMinutes { get; set; } = true;
    /// <inheritdoc />
    public bool ShowSeconds => true;
    /// <inheritdoc />
    public bool ShowMilliseconds { get; set; }

    /// <inheritdoc />
    public UpCountingTimer(TimerCallback? callback)
    {
        _timeBetweenTicksInMilliseconds = ShowMilliseconds ? 1 : 1000;
        _timer = new Timer(Tick, null, 1000, _timeBetweenTicksInMilliseconds);
        _callback = callback;
    }

    /// <inheritdoc />
    public void StartTimer()
    {
        if (IsTimerRunning)
            return;
        _runTime = TimeSpan.Zero;
        IsTimerRunning = true;
        UnpauseTimer();
    }

    /// <inheritdoc />
    public void PauseTimer() => Paused = true;

    /// <inheritdoc />
    public void UnpauseTimer() => Paused = false;

    /// <inheritdoc />
    public void StopTimer()
    {
        UnpauseTimer();
        IsTimerRunning = false;
    }

    private void Tick(object? state)
    {
        if (!IsTimerRunning || Paused)
            return;
        _runTime = _runTime.Add(TimeSpan.FromMilliseconds(_timeBetweenTicksInMilliseconds));
        _callback?.Invoke(null);
    }
    private string GetTimeString()
    {
        var parts = new List<string>();
        if (ShowHours)
            parts.Add("hh");
        if (ShowMinutes)
            parts.Add("mm");
        if (ShowSeconds)
            parts.Add("ss");
        var timeFormat = string.Join("\\:", parts);
        if (ShowMilliseconds)
            timeFormat = $"{timeFormat}{(string.IsNullOrWhiteSpace(timeFormat) ? "" : ".")}fff";
        return Time.ToString(timeFormat);
    }
}
