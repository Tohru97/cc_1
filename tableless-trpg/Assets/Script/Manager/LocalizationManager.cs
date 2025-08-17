using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System;

public class LocalizationManager : Singleton<LocalizationManager>, IInitializable
{
    private Dictionary<string, Dictionary<SystemLanguage, string>> localizedText = new Dictionary<string, Dictionary<SystemLanguage, string>>();
    private SystemLanguage currentLanguage;

    public async UniTask InitializeAsync()
    {
        localizedText = new Dictionary<string, Dictionary<SystemLanguage, string>>();

        TextAsset localizationData = await AddressableManager.Instance.LoadAssetAsync<TextAsset>("Localization");

        if (localizationData == null)
        {
            Debug.LogError("Failed to load Localization data.");
            return;
        }

        // 2. Decrypt and parse using CSVTool
        List<Dictionary<string, string>> tableData = CSVTool.GetDecryptData(localizationData.bytes);
        if (tableData == null)
        {
            Debug.LogError("Failed to load Localization.byte from Addressables.");
            return;
        }

        AddressableManager.Instance.ReleaseAsset("Localization");

        // 3. Populate the localizedText dictionary
        foreach (var row in tableData)
        {
            if (!row.TryGetValue("id", out string id) || string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("Localization row is missing or has an empty 'id'. Skipping.");
                continue;
            }

            var languageDict = new Dictionary<SystemLanguage, string>();
            foreach (var col in row)
            {
                if (string.Equals(col.Key, "id", StringComparison.OrdinalIgnoreCase)) continue;

                if (Enum.TryParse(col.Key, true, out SystemLanguage lang))
                {
                    languageDict[lang] = col.Value;
                }
                else
                {
                    Debug.LogWarning($"Could not parse '{col.Key}' as a SystemLanguage. Skipping column in row '{id}'.");
                }
            }

            if (!localizedText.ContainsKey(id))
            {
                localizedText.Add(id, languageDict);
            }
            else
            {
                localizedText[id] = languageDict;
            }
        }

        // Set current language
        SetLanguage(Application.systemLanguage);
        
        Debug.Log("LocalizationManager initialized with data from Localization.byte.");
    }

    public void SetLanguage(SystemLanguage language)
    {
        currentLanguage = language;
        // Fallback to English if the selected language is not supported for a given key.
        // The fallback logic is handled in GetLocalizedText.
        Debug.Log($"Current language set to: {currentLanguage}");
    }

    public string GetLocalizedText(string key)
    {
        if (localizedText == null)
        {
            Debug.LogError("Localization data not loaded.");
            return key;
        }

        if (localizedText.TryGetValue(key, out var languageDict))
        {
            if (languageDict.TryGetValue(currentLanguage, out var text) && !string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (languageDict.TryGetValue(SystemLanguage.English, out var fallbackText) && !string.IsNullOrEmpty(fallbackText))
            {
                return fallbackText;
            }
        }
        
        Debug.LogWarning($"Localization key not found: {key}");
        return key;
    }

    public static string Get(string key)
    {
        if (Instance == null)
        {
            Debug.LogError("LocalizationManager instance not found.");
            return key;
        }
        return Instance.GetLocalizedText(key);
    }
}

public static class LocalizationExtensions
{
    public static string Localize(this string key)
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogError("LocalizationManager instance not found.");
            return key;
        }
        return LocalizationManager.Instance.GetLocalizedText(key);
    }
}
