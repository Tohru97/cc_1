using Cysharp.Threading.Tasks;
using UnityEngine;

public class MatchScene : BaseScene
{
    public override eScene GetCurrentSceneType()
    {
        return eScene.MatchScene;
    }

    public override UniTask LoadAsync()
    {
        return UniTask.CompletedTask;
    }
}
