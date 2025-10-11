using System;

public interface IMinigameManager 
{
    event EventHandler OnLevelCompleted;
    event EventHandler OnLevelFailed;
}
