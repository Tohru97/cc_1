using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerInfoManager : Singleton<PlayerInfoManager>, IInitializable
{
    public PlayerBaseInfo _playerBaseInfo { get; private set; } = null;
    public PlayerUnlockInfo _playerUnlockInfo { get; private set; } = null;
    public PlayerBookInfo _playerBookInfo { get; private set; } = null;

    public UniTask InitializeAsync()
    {
        Debug.Log("PlayerInfoManager initialization started.");

        _playerBaseInfo = new PlayerBaseInfo();
        _playerUnlockInfo = new PlayerUnlockInfo();
        _playerBookInfo = new PlayerBookInfo();

        return UniTask.CompletedTask;
    }
}