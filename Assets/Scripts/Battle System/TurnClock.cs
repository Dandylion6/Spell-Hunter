using UnityEngine;

public class TurnClock
{
    public const int ClockThreshold = 1000;
    public int Progress { get; private set; }

    /// <summary>
    ///     Calculates the ticks required for the clock to reach the <see cref="ClockThreshold"/>. 
    /// </summary>
    /// <param name="speed">
    ///     is how fast the clock progresses;
    ///     <c>1000</c> would require only <c>1</c> tick to reach the <see cref="ClockThreshold"/>.
    /// </param>
    /// <returns>
    ///     The amount of ticks it will take to reach the <see cref="ClockThreshold"/>.
    /// </returns>
    public int TicksTillReady(int speed)
    {
        int remaining = ClockThreshold - Progress;
        speed = Mathf.Max(speed, 1);
        
        // Subtracting one makes sure enough ticks are added for clock to be ready.
        return remaining <= 0 ? 0 : (remaining + speed - 1) / speed;
    }
    
    public void AddProgress(int ticks, int speed) => Progress +=  ticks * speed;
    public void LoopClock() => Progress -= ClockThreshold;
}
