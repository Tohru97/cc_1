using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class Metadata
{
    public static EventInfo _eventInfo = new EventInfo();

    public static async UniTask Init()
    {
        _eventInfo = new EventInfo();

        _eventInfo.ClearDatas();

        TextAsset eventData = await AddressableManager.Instance.LoadAssetAsync<TextAsset>("event");
        
        _eventInfo.Parsing(CSVTool.GetDecryptData(eventData.bytes));
    }
}
