using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eScene
{
    TitleScene = 0,
    MainScene = 1,
    InGameScene = 2
}

public class AppManager : SingletonMono<AppManager>
{
    [SerializeField] private MainLoading _mainLoading;
    [SerializeField] private MiniLoading _miniLoading;

    public async UniTask Init(IProgress<float> progress, Action<string> showLoadingText)
    {
        Debug.Log("AppManager initialization started.");

        List<IInitializable> managers = new List<IInitializable>();

        SaveManager.CreateInstance();
        AddressableManager.CreateInstance();
        DataManager.CreateInstance();
        LocalizationManager.CreateInstance();
        SoundManager.CreateInstance();
        TimeManager.CreateInstance();
        UIManager.CreateInstance();
        ObjectPoolManager.CreateInstance();
        PlayerInfoManager.CreateInstance();
        IngameManager.CreateInstance();
        NetworkController.CreateInstance();

        managers.Add(SaveManager.Instance);
        managers.Add(AddressableManager.Instance);
        managers.Add(DataManager.Instance);
        managers.Add(LocalizationManager.Instance);
        managers.Add(SoundManager.Instance);
        managers.Add(TimeManager.Instance);
        managers.Add(UIManager.Instance);
        managers.Add(ObjectPoolManager.Instance);
        managers.Add(PlayerInfoManager.Instance);
        managers.Add(IngameManager.Instance);
        managers.Add(NetworkController.Instance);

        for (int i = 0; i < managers.Count; i++)
        {
            await managers[i].InitializeAsync();
            string managerName = managers[i].GetType().Name;
            showLoadingText?.Invoke($"<bounce a=1 f=0.5 w=1>Initializing {managerName}...</bounce>");

            await UniTask.Delay(1000); // Simulate some delay for demonstration purposes

            // 진행률을 업데이트합니다. (0.0f ~ 1.0f)
            float currentProgress = (float)(i + 1) / managers.Count;
            progress?.Report(currentProgress);
        }

        Debug.Log("All managers initialized successfully.");
    }

    public async void ChangeScene(eScene scene)
    {
        _mainLoading.Show();
        await SceneManager.LoadSceneAsync(scene.ToString());
        _mainLoading.Hide();
    }
}
