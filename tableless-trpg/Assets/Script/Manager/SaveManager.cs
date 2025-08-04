using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// EasySave3를 사용하여 게임 데이터를 관리하는 싱글톤 매니저입니다.
/// UniTask.RunOnThreadPool을 사용하여 메인 스레드 멈춤을 방지합니다.
/// </summary>
public class SaveManager : SingletonMono<SaveManager>, IInitializable
{
    private const string DefaultSaveFileName = "GameData.es3";
    private string saveFilePath;

    public UniTask InitializeAsync()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, DefaultSaveFileName);
        Debug.Log($"SaveManager Initialized. Save path: {saveFilePath}");
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// 데이터를 비동기적으로 저장합니다. (UniTask 백그라운드 스레드 사용)
    /// </summary>
    public async UniTask SaveAsync<T>(string key, T data)
    {
        try
        {
            // 동기적인 Save 함수를 UniTask.RunOnThreadPool을 통해 백그라운드에서 실행합니다.
            await UniTask.RunOnThreadPool(() => ES3.Save<T>(key, data, saveFilePath));
            Debug.Log($"Data saved successfully. Key: {key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data for key '{key}'. Error: {e.Message}");
        }
    }

    /// <summary>
    /// 데이터를 비동기적으로 로드합니다. (UniTask 백그라운드 스레드 사용)
    /// </summary>
    public async UniTask<T> LoadAsync<T>(string key, T defaultValue = default)
    {
        if (!ES3.KeyExists(key, saveFilePath))
        {
            Debug.LogWarning($"No data found for key '{key}'. Returning default value.");
            return defaultValue;
        }

        try
        {
            // 동기적인 Load 함수를 UniTask.RunOnThreadPool을 통해 백그라운드에서 실행합니다.
            return await UniTask.RunOnThreadPool(() => ES3.Load<T>(key, saveFilePath, defaultValue));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data for key '{key}'. Error: {e.Message}");
            return defaultValue;
        }
    }

    /// <summary>
    /// 저장 파일의 존재 여부를 확인합니다.
    /// </summary>
    public bool DoesSaveExist()
    {
        return ES3.FileExists(saveFilePath);
    }

    /// <summary>
    /// 모든 저장 데이터를 삭제합니다.
    /// </summary>
    public void DeleteSaveData()
    {
        if (DoesSaveExist())
        {
            ES3.DeleteFile(saveFilePath);
            Debug.Log("Save data file deleted successfully.");
        }
    }
}