using System;
using System.Collections.Generic;
using UnityEngine;

public static class ConstString
{
    public const string ExceptionStrKey = "-1";
    public const int ExceptionIndex = -1;
}

public abstract class BaseInfo
{
    public abstract class BaseDataInfo
    {
        readonly public object key;
        public BaseDataInfo(object key)
        {
            this.key = key;
        }
    }

    public abstract bool Parsing(List<Dictionary<string, string>> datas);
    public abstract void ClearDatas();

    protected string ParseString(string data)
    {
        string retData = data;
        if (string.IsNullOrEmpty(retData))
        {
            return ConstString.ExceptionStrKey;
        }
        return retData;
    }

    protected List<string> ParseStringList(string data)
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;

        List<string> retList = new List<string>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (!string.IsNullOrEmpty(tempStr))
            {
                retList.Add(tempStr);
            }
            else
            {
                Debug.LogErrorFormat("Data is null or empty, index: {0}, data: {1}", index, data);
            }
        }
        return retList;
    }

    protected long ParseLong(string data)
    {
        long retData = default;
        if (!long.TryParse(data, out retData))
        {
            return ConstString.ExceptionIndex;
        }
        return retData;
    }

    protected List<long> ParseLongList(string data)
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;
        
        List<long> retList = new List<long>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (long.TryParse(tempStr, out long val))
            {
                retList.Add(val);
            }
            else
            {
                Debug.LogErrorFormat("Failed to parse long, index: {0}, data: {1}", index, tempStr);
            }
        }
        return retList;
    }

    protected int ParseInt(string data)
    {
        int retData = default;
        if (!int.TryParse(data, out retData))
        {
            return ConstString.ExceptionIndex;
        }
        return retData;
    }

    protected List<int> ParseIntList(string data)
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;

        List<int> retList = new List<int>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (int.TryParse(tempStr, out int val))
            {
                retList.Add(val);
            }
            else
            {
                Debug.LogErrorFormat("Failed to parse int, index: {0}, data: {1}", index, tempStr);
            }
        }
        return retList;
    }

    protected float ParseFloat(string data)
    {
        float retData = default;
        if (!float.TryParse(data, out retData))
        {
            return ConstString.ExceptionIndex;
        }
        return retData;
    }

    protected List<float> ParseFloatList(string data)
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;

        List<float> retList = new List<float>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (float.TryParse(tempStr, out float val))
            {
                retList.Add(val);
            }
            else
            {
                Debug.LogErrorFormat("Failed to parse float, index: {0}, data: {1}", index, tempStr);
            }
        }
        return retList;
    }

    protected bool ParseBool(string data)
    {
        bool retData = default;
        if (!bool.TryParse(data, out retData))
        {
            return false;
        }
        return retData;
    }

    protected List<bool> ParseBoolList(string data)
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;

        List<bool> retList = new List<bool>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (bool.TryParse(tempStr, out bool val))
            {
                retList.Add(val);
            }
            else
            {
                Debug.LogErrorFormat("Failed to parse bool, index: {0}, data: {1}", index, tempStr);
            }
        }
        return retList;
    }

    protected TEnum ParseEnum<TEnum>(string data) where TEnum : struct
    {
        TEnum retType = default;
        if (!Enum.TryParse<TEnum>(data, out retType))
        {
            Debug.LogErrorFormat("Data dont exist, data: {0}", data);
        }
        return retType;
    }

    protected List<TEnum> ParseEnumList<TEnum>(string data) where TEnum : struct
    {
        if (string.IsNullOrEmpty(data) || string.Equals(data, ConstString.ExceptionStrKey))
            return null;

        List<TEnum> retList = new List<TEnum>();
        string[] splitStrs = data.Split(';');
        for (int index = 0; index < splitStrs.Length; index++)
        {
            string tempStr = splitStrs[index];
            if (Enum.TryParse<TEnum>(tempStr, out TEnum val))
            {
                retList.Add(val);
            }
            else
            {
                Debug.LogErrorFormat("Data dont exist, index: {0}, data: {1}", index, tempStr);
            }
        }
        return retList;
    }
}