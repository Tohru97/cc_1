using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LocalizationManager : Singleton<LocalizationManager>
{
    private Dictionary<string, Dictionary<SystemLanguage, string>> localizedText;

    private SystemLanguage currentLanguage;

    public event Action OnLanguageChanged;

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, Dictionary<SystemLanguage, string>>();

        // TODO: change AddressableAssets
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile == null)
        {
            Debug.LogError($"[LocalizationManager] CSV file not found: {fileName}.csv in Resources folder.");
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string headerLine = reader.ReadLine();
            if (headerLine == null) return;

            string[] headers = headerLine.Split(',');
            List<SystemLanguage> supportedLanguages = new List<SystemLanguage>();
            for (int i = 1; i < headers.Length; i++)
            {
                try
                {
                    SystemLanguage lang = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), headers[i], true);
                    supportedLanguages.Add(lang);
                }
                catch (ArgumentException)
                {
                    Debug.LogWarning($"[LocalizationManager] Language '{headers[i]}' is not a valid SystemLanguage. Skipping column.");
                    supportedLanguages.Add(SystemLanguage.Unknown);
                }
            }

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                if (values.Length < headers.Length) continue;

                string key = values[0];
                if (string.IsNullOrEmpty(key)) continue;

                var translations = new Dictionary<SystemLanguage, string>();
                for (int i = 0; i < supportedLanguages.Count; i++)
                {
                    if (supportedLanguages[i] != SystemLanguage.Unknown && (i + 1) < values.Length)
                    {
                        translations[supportedLanguages[i]] = values[i + 1];
                    }
                }
                localizedText[key] = translations;
            }
        }

        Debug.Log("[LocalizationManager] Localization data loaded successfully.");
    }

    public void SetLanguage(SystemLanguage language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            OnLanguageChanged?.Invoke();
            Debug.Log($"[LocalizationManager] Language changed to: {currentLanguage}");
        }
    }

    public string GetLocalizedValue(string key, params object[] args)
    {
        if (localizedText == null)
        {
            Debug.LogError("[LocalizationManager] Localization data is not loaded.");
            return key;
        }

        string result = string.Empty;

        if (localizedText.TryGetValue(key, out var baseString))
        {
            result = baseString;

            if (args.Length > 0)
            {
                try
                {
                    result = string.Format(baseString, args);
                }
                catch (FormatException e)
                {
                    Debug.LogError($"[LocalizationManager] Format error for key '{key}': {e.Message}");
                }
            }
            else
            {
                if (baseString.TryGetValue(currentLanguage, out var localizedString))
                {
                    result = localizedString;
                }
            }
        }
        else
        {
            Debug.LogWarning($"[LocalizationManager] Key '{key}' not found in localization data.");
            result = key; // Return the key as a fallback
        }

        return result;
    }
}