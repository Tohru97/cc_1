using System.Collections.Generic;
using UnityEngine;

public class EventInfo : BaseInfo
{
    public class Info : BaseDataInfo
    {
        public int id = default;
        public string event_title = default;
        public string event_desc = default;
        public int event_options_count = default;
        public List<int> event_option_id = new List<int>();

        public Info(int id, string event_title, string event_desc, int event_options_count, List<int> event_option_id)
            : base(id)
        {
            this.id = id;
            this.event_title = event_title;
            this.event_desc = event_desc;
            this.event_options_count = event_options_count;
            this.event_option_id = event_option_id;
        }
    }

    readonly Dictionary<int, Info> infos = new Dictionary<int, Info>();

    public override void ClearDatas()
    {
        infos.Clear();
    }

    public Info GetInfo(int id)
    {
        if (infos.TryGetValue(id, out Info info))
            return info;

        return null;
    }

    public override bool Parsing(List<Dictionary<string, string>> datas)
    {
        if (datas == null)
            return false;

        for (int dataIndex = 1, max = datas.Count; dataIndex < max; dataIndex++)
        {
            Info info = null;

            int id = ParseInt(datas[dataIndex]["id"]);
            string event_title = ParseString(datas[dataIndex]["event_title"]);
            string event_desc = ParseString(datas[dataIndex]["event_desc"]);
            int event_options_count = ParseInt(datas[dataIndex]["event_options_count"]);
            List<int> event_option_id = ParseIntList(datas[dataIndex]["event_option_id"]);

            info = new Info(id, event_title, event_desc, event_options_count, event_option_id);

            infos.Add(id, info);
        }

        return true;
    }
}
