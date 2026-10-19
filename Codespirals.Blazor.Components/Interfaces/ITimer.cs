namespace Codespirals.Blazor.Components.Interfaces;

/// <summary>
/// A timer 
/// </summary>
public interface ITimer
{
    /// <summary>
    /// The time as a <see cref="TimeSpan"/>
    /// </summary>
    public TimeSpan Time { get; }
    /// <summary>
    /// The time formatted as a string to the dev's preferences
    /// </summary>
    string TimeString { get; }
    /// <summary>
    /// Is the timer running?
    /// </summary>
    bool IsTimerRunning { get; set; }
    /// <summary>
    /// Is the timer paused?
    /// </summary>
    bool Paused { get; set; }
    /// <summary>
    /// Show hours
    /// </summary>
    bool ShowHours { get; }
    /// <summary>
    /// Show minutes
    /// </summary>
    bool ShowMinutes { get; }
    /// <summary>
    /// Show seconds
    /// </summary>
    bool ShowSeconds { get; }
    /// <summary>
    /// Show milliseconds
    /// </summary>
    bool ShowMilliseconds { get; }

    /// <summary>
    /// Pause the timer, but leave it resumable
    /// </summary>
    void PauseTimer();
    /// <summary>
    /// End the pause and let the timer run further
    /// </summary>
    void UnpauseTimer();
    /// <summary>
    /// Start the timer
    /// </summary>
    void StartTimer();
    /// <summary>
    /// Stop the timer, unable to resume
    /// </summary>
    void StopTimer();
}