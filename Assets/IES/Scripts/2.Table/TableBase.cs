using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class TableBase<TTabledata, TTable> : Singleton<TTable>
    where TTabledata : TabledataBase, new()
    where TTable : class, new()
{
    Dictionary<int, TTabledata> _cache = new Dictionary<int, TTabledata>();

    protected void load(string tablePath)
    {
        MemoryStream tableStream;
        // 开发期，读Project/Config
#if UNITY_EDITOR
        var srcPath = Application.dataPath + "/../Config/" + tablePath + ".csv";
        tableStream = new MemoryStream(File.ReadAllBytes(srcPath));
#else
        // 发布期，读Resources/Config
        //用内存流读表
        var table = Resources.Load<TextAsset>(Config + tablePath + ".csv");
        tableStream = new MemoryStream(table.bytes);
#endif
        //table的二进制作为内存流的数据源

        using (var reader = new StreamReader(tableStream, Encoding.GetEncoding("gb2312")))
        {
            //跳过注释（表中第一行）
            reader.ReadLine();

            //反射到 字段名
            var fileNameStr = reader.ReadLine();
            var fileNameArray = fileNameStr.Split(',');
            List<FieldInfo> allFileInfo = new List<FieldInfo>();
            foreach (var fileName in fileNameArray)
            {
                var fieldType = typeof(TTabledata).GetField(fileName);
                if (fieldType != null)
                {
                    //字符串映射到字段 并将字段加入列表
                    allFileInfo.Add(fieldType);
                }
                else
                {
                    Debug.LogError("表中字段未在程序中定义" + fieldType);
                }
            }


            //从第二行开始读
            var lineStr = reader.ReadLine();
            while (lineStr != null)
            {
                TTabledata DB = readLine(allFileInfo, lineStr);

                _cache[DB.ID] = DB;

                lineStr = reader.ReadLine();
            }
        }
    }

    /// <summary>
    /// 从第三行开始读取数据
    /// </summary>
    /// <param name="allFileInfo"></param>
    /// <param name="lineStr"></param>
    /// <returns></returns>
    private static TTabledata readLine(List<FieldInfo> allFileInfo, string lineStr)
    {
        //从第二行开始读到的数据
        var itemStrArray = lineStr.Split(','); //分割
        var tabledata = new TTabledata();

        for (int i = 0; i < allFileInfo.Count; i++)
        {
            var field = allFileInfo[i];
            var data = itemStrArray[i];

            if (field.FieldType == typeof(string))
            {
                field.SetValue(tabledata, data);
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(tabledata, int.Parse(data));
            }
            else if (field.FieldType == typeof(float))
            {
                field.SetValue(tabledata, float.Parse(data));
            }
            // else if (field.FieldType == typeof(Transform))
            // {
            // }
            // else if (field.FieldType == typeof(Light))
            // {
            // }
        }

        return tabledata;
    }


    /// <summary>
    /// 索引器 获取表格数据_cache
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TTabledata this[int id]
    {
        get
        {
            TTabledata data;
            _cache.TryGetValue(id, out data);
            return data;
        }
    }

    public Dictionary<int, TTabledata> GetAll()
    {
        return _cache;
    }
}