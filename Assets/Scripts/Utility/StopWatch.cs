using System;
using UnityEngine;

/// <summary>
///     A simple countdown timer that is driven manually by calling <see cref="Tick"/>,
///     usually from a MonoBehaviour's Update. Can run once or loop indefinitely.
///     This is useful for handling simple timings on the fly.
/// </summary>
public class StopWatch
{
    public float TimerSeconds { get; private set; }
    public float ElapsedSeconds { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsLooped { get; }
    public bool IsCompleted { get; private set; }
    
    private Action onComplete;
    
    public StopWatch(float timerSeconds, bool isLooped = false)
    {
        TimerSeconds = timerSeconds;
        IsLooped = isLooped;
    }
    
    /// <summary>
    ///     Registers a callback that is invoked each time the timer completes.
    /// </summary>
    public void SubscribeToOnComplete(Action callback) => onComplete += callback;
    
    /// <summary>
    ///     Removes a previously registered completion callback.
    /// </summary>
    public void UnsubscribeFromOnComplete(Action callback) => onComplete -= callback;
    
    /// <summary>
    ///     Changes the timer duration. Does not reset <see cref="ElapsedSeconds"/>,
    ///     so the new duration applies to the current run.
    /// </summary>
    public void ChangeTimer(float timerSeconds) => TimerSeconds = timerSeconds;

    /// <summary>
    ///     Resets the elapsed time to zero and starts counting.
    /// </summary>
    public void Start()
    {
        Reset();
        Resume();
    }
    
    /// <summary>
    ///     Stops counting and resets the elapsed time to zero.
    /// </summary>
    public void Stop()
    {
        Pause();
        Reset();
    }
    
    /// <summary>
    ///     Stops counting but keeps the elapsed time, so <see cref="Resume"/> continues where it left off.
    /// </summary>
    public void Pause() => IsRunning = false;
    
    /// <summary>
    /// Continues counting from the current elapsed time.
    /// </summary>
    public void Resume() => IsRunning = true;

    /// <summary>
    ///     Sets the elapsed time back to zero without changing <see cref="IsRunning"/>.
    /// </summary>
    public void Reset()
    {
        ElapsedSeconds = 0.0f;
        IsCompleted = false;
    }
    
    /// <summary>
    ///     Advances the timer by <see cref="Time.deltaTime"/>. Call this once per frame.
    ///     Does nothing if the timer is not running.
    /// </summary>
    /// <remarks>
    ///     On completion, the callback is invoked and any overshoot is carried over
    ///     (<c>ElapsedSeconds -= TimerSeconds</c>), so looped timers don't drift.
    ///     Non-looped timers call <see cref="Stop"/> after the callback.
    /// </remarks>
    public void Tick()
    {
        if (!IsRunning) return;
        
        ElapsedSeconds += Time.deltaTime;
        if (ElapsedSeconds < TimerSeconds) return;
        
        ElapsedSeconds -= TimerSeconds;
        IsCompleted = true;
        onComplete?.Invoke();
        
        if (!IsLooped) Stop();
    }
}
