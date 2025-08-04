using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class LocalizationManager : Singleton<LocalizationManager>, IInitializable
{
    private Dictionary<string, Dictionary<SystemLanguage, string>> localizedText;

    private SystemLanguage currentLanguage;

    public UniTask InitializeAsync()
    {
        Debug.Log("LocalizationManager initialized.");
        return UniTask.CompletedTask;
    }
}