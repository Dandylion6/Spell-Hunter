using System.Collections.Generic;

public interface IBattlePresenter
{
    public bool IsPlaying { get; }
    
    public void Play(Battle battle, List<BattleEvent> events);
}
