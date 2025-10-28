using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MatchControllerBase : MonoBehaviour
{
    public List<CardBase> _playerDeck = new List<CardBase>();
    public List<CardBase> _opponentDeck = new List<CardBase>();

    public MatchPhase _currentPhase;

    private List<MatchPhase> _matchPhaseList = new List<MatchPhase>();

    public void StartMatch()
    {

    }

    private async UniTask StartMatchPhaseLoop()
    {
        while (true)
        {
            foreach(MatchPhase phase in _matchPhaseList)
            {
                _currentPhase = phase;
                _currentPhase.StartPhase();

                await UniTask.WaitUntil(() => _currentPhase._isPhaseActive == false);
            }
        }
    }
    
    public void EndMatch()
    {

    }
}
