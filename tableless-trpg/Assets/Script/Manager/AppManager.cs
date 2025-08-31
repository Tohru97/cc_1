using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    public async UniTask Init(IProgress<float> progress, Action<string> showLoadingText)
    {
        Debug.Log("AppManager initialization started.");

        List<IInitializable> managers = new List<IInitializable>();

        AddressableManager.CreateInstance();
        SaveManager.CreateInstance();
        LocalizationManager.CreateInstance();
        DataManager.CreateInstance();
        GameManager.CreateInstance();
        SoundManager.CreateInstance();
        TimeManager.CreateInstance();
        UIManager.CreateInstance();

        managers.Add(AddressableManager.Instance);
        managers.Add(SaveManager.Instance);
        managers.Add(LocalizationManager.Instance);
        managers.Add(DataManager.Instance);
        managers.Add(GameManager.Instance);
        managers.Add(SoundManager.Instance);
        managers.Add(TimeManager.Instance);
        managers.Add(UIManager.Instance);

        for (int i = 0; i < managers.Count; i++)
        {
            await managers[i].InitializeAsync();
            string managerName = managers[i].GetType().Name;
            showLoadingText?.Invoke($"<bounce a=1 f=1 w=1>Initializing {managerName}...</bounce>");

            await UniTask.Delay(1000); // Simulate some delay for demonstration purposes

            // 진행률을 업데이트합니다. (0.0f ~ 1.0f)
            float currentProgress = (float)(i + 1) / managers.Count;
            progress?.Report(currentProgress);
        }

        Debug.Log("All managers initialized successfully.");
    }
}
