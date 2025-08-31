using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Text_UI _loadingText;

    private async UniTaskVoid Awake()
    {
        AppManager.CreateInstance();

        float progressValue = 0f;
        IProgress<float> progress = new Progress<float>(value =>
        {
            progressValue = value;
            // Update UI progress bar or any other UI element here
        });

        await AppManager.Instance.Init(progress, _loadingText.ShowText);

        await UniTask.WaitUntil(() => progressValue == 1f);
    }
}