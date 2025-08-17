using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    private async UniTaskVoid Awake()
    {
        AppManager.CreateInstance();

        float progressValue = 0f;
        IProgress<float> progress = new Progress<float>(value =>
        {
            progressValue = value;
            // Update UI progress bar or any other UI element here
            Debug.Log($"Initialization progress: {value:P0}");
        });

        await AppManager.Instance.Init(progress);

        await UniTask.WaitUntil(() => progressValue == 1f);

        Debug.Log("Initialization complete. Proceeding to next scene...");
    }
}