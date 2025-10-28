using UnityEngine;

public abstract class MatchPhase
{
    public bool _isPhaseActive { get; private set; }

    public void StartPhase()
    {
        _isPhaseActive = true;

        ExecutePhase();
    }

    public abstract void ExecutePhase();

    public void EndPhase()
    {
        _isPhaseActive = false;
    }
}