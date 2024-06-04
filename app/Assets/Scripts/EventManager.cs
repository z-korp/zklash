using System;

public static class EventManager
{
    public static event Action OnStartBattle;

    public static void StartBattle()
    {
        OnStartBattle?.Invoke();
    }
}