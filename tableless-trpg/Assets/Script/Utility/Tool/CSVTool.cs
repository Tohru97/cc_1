using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using Spine;
using Newtonsoft.Json;

public class CSVTool
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF0123456789");

    [MenuItem("Utilitys/CSVTool/EncryptCsvFile")]
    public static void EncryptCsvFile()
    {
        string inputPath = Path.Combine(Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName, "metadata");
        string outputPath = Application.dataPath + "/AddressableAssets/Table";

        if(string.IsNullOrEmpty(inputPath) || string.IsNullOrEmpty(outputPath))
        {
            Debug.LogError("Input or output path is null or empty");
            return;
        }

        string[] csvFiles = Directory.GetFiles(inputPath, "*.csv", SearchOption.AllDirectories);

        if(csvFiles.Length == 0)
        {
            Debug.LogError("No csv files found");
            return;
        }

        foreach(string csvFilePath in csvFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(csvFilePath);
            string encryptedPath = Path.Combine(outputPath, fileName + ".bytes");

            EncryptCsvFile(csvFilePath, encryptedPath);
        }
    }

    private static void EncryptCsvFile(string inputPath, string outputPath)
    {
        using (FileStream fsIn = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
        using (FileStream fsOut = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (CryptoStream cryptoStream = new CryptoStream(fsOut, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                fsIn.CopyTo(cryptoStream);
            }
        }
    }

    public static string DecryptCsvFile(byte[] encryptedBytes)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                byte[] plainText = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(plainText);
            }
        }
    }

    public static List<Dictionary<string, string>> GetDecryptData(byte[] encryptedBytes)
    {
        string decryptedCsv = DecryptCsvFile(encryptedBytes);

        var list = new List<Dictionary<string, string>>();
        
        var lines = decryptedCsv.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        if (lines.Length < 2)
        {
            Debug.LogError("CSV data must have a header and at least one data line.");
            return list;
        }

        var header = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');
            if (values.Length != header.Length)
            {
                Debug.LogWarning($"CSVTool: Row {i} has {values.Length} columns, but header has {header.Length}. Skipping row.");
                continue;
            }

            var entry = new Dictionary<string, string>();
            for (int j = 0; j < header.Length; j++)
            {
                entry[header[j].Trim()] = values[j].Trim();
            }
            list.Add(entry);
        }

        return list;
    }

    private static Dictionary<string, List<object>> parseCSV(string csvFile)
    {
        var result = new Dictionary<string, List<object>>();

        string[] lines = csvFile.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 3)
        {
            Debug.LogWarning("CSV 데이터가 충분하지 않습니다 (최소 3행 필요).");
            return result;
        }

        string[] keys = lines[0].Split(',');
        string[] types = lines[1].Split(',');

        int columnCount = keys.Length;

        for (int i = 0; i < columnCount; i++)
        {
            string key = keys[i].Trim();
            result[key] = new List<object>();
        }

        for (int row = 2; row < lines.Length; row++)
        {
            string[] values = lines[row].Split(',');

            for (int col = 0; col < columnCount; col++)
            {
                string key = keys[col].Trim();
                string type = col < types.Length ? types[col].Trim().ToLower() : "string";
                string valueStr = col < values.Length ? values[col].Trim() : "";

                object parsedValue = ParseValueWithType(valueStr, type);
                result[key].Add(parsedValue);
            }
        }

        return result;
    }

    private static object ParseValueWithType(string value, string type)
    {
        try
        {
            // 리스트 타입 처리
            if (type.StartsWith("List<") && type.EndsWith(">"))
            {
                string elementType = type.Substring(5, type.Length - 6);
                string[] items = value.Split(';');

                switch (elementType)
                {
                    case "int":
                        var intList = new List<int>();
                        foreach (var item in items)
                            if (int.TryParse(item, out var iv)) intList.Add(iv);
                        return intList;
                    case "float":
                        var floatList = new List<float>();
                        foreach (var item in items)
                            if (float.TryParse(item, out var fv)) floatList.Add(fv);
                        return floatList;
                    case "bool":
                        var boolList = new List<bool>();
                        foreach (var item in items)
                            if (bool.TryParse(item, out var bv)) boolList.Add(bv);
                        return boolList;
                    case "string":
                    default:
                        return new List<string>(items);
                }
            }

            // 기본 타입 처리
            switch (type)
            {
                case "int":
                    return int.TryParse(value, out var iv) ? iv : 0;
                case "float":
                    return float.TryParse(value, out var fv) ? fv : 0f;
                case "bool":
                    return bool.TryParse(value, out var bv) ? bv : false;
                case "string":
                default:
                    return value;
            }
        }
        catch
        {
            return null;
        }
    }
}
