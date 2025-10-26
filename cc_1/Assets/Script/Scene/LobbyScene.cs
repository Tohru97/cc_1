using Cysharp.Threading.Tasks;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public override eScene GetCurrentSceneType()
    {
        return eScene.LobbyScene;
    }

    public override UniTask LoadAsync()
    {
        return UniTask.CompletedTask;
    }
}
