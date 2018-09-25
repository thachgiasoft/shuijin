using System;
using System.Collections.Generic;
using ZCSharpLib.Features.NetWork;
using ZCSharpLib.Common;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Features.Tmplate
{
    public class Tpl
    {
        class TMPWarp
        {
            public string Name { get { return Type.Name; } }
            public Type Type { get; set; }
            public TplMgr TplMgr { get; set; }
        }

        private static Dictionary<Type, TplMgr> TplDict = new Dictionary<Type, TplMgr>();

        public static void Initialize()
        {
            Type[] types = ZCommUtil.GetTypes();
            Dictionary<string, Type> allTypes = new Dictionary<string, Type>();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                allTypes[type.Name] = type;
            }

            List<TMPWarp> mgrWarpList = new List<TMPWarp>();
            foreach (var item in allTypes)
            {
                string typeName = item.Key;
                Type type = item.Value;
                if (type.IsClass && type.BaseType == typeof(TplMgr))
                {
                    TplMgr tplMgr = Activator.CreateInstance(type) as TplMgr;
                    string paramName = type.Name.Replace("Mgr", "") + "Data";
                    Type paramType = null;
                    if(allTypes.TryGetValue(paramName, out paramType))
                    {
                        mgrWarpList.Add(new TMPWarp() { Type = paramType, TplMgr = tplMgr });
                    }
                    else
                    {
                        App.Logger.Error("Tempate 没有找到类型:{0}", paramName);
                    }
                }
            }
            mgrWarpList.Sort((a, b) => { return a.Name.CompareTo(b.Name); });

            for (int i = 0; i < mgrWarpList.Count; i++)
            {
                TMPWarp mgrWarp = mgrWarpList[i];
                if (!TplDict.ContainsKey(mgrWarp.Type))
                {
                    TplDict.Add(mgrWarp.Type, mgrWarp.TplMgr);
                }
                else
                {
                    App.Logger.Error("TplDict已经包含同样名称的键:{0}", mgrWarp.Type);
                }
            }
            App.Logger.Info("Template 完成模板添加操作");
        }

        public static void Setup(byte[] bytes)
        {
            DynamicBuffer buffer = new DynamicBuffer(bytes);
            IList<Type> keys = new List<Type>(TplDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                Type key = keys[i];
                TplMgr oTplMgr = TplDict[key];
                oTplMgr.Setup(key, buffer);
            }
        }

        public static T Find<T>(int id) where T : BaseTpl, new()
        {
            TplMgr mgr = GetMgr(typeof(T));
            if (mgr == null) return default(T);
            else
            {
                return mgr.Find(id) as T;
            }
        }

        public static IList<T> FindAll<T>() where T : BaseTpl, new()
        {
            List<T> list = new List<T>();
            TplMgr mgr = GetMgr(typeof(T));
            if (mgr == null) return list;
            else
            {
                List<object> tmplist = mgr.FindAll();
                for (int i = 0; i < tmplist.Count; i++)
                {
                    list.Add(tmplist[i] as T);
                }
                return list;
            }
        }

        public static TplMgr GetMgr(Type type)
        {
            TplMgr mgr = null;
            if (!TplDict.TryGetValue(type, out mgr))
            {
                throw new Exception(string.Format("没有找到类型[{0}]的管理对象", type));
            }
            return mgr;
        }
    }

    public abstract class TplMgr
    {
        public int Count { get; protected set; }

        protected Dictionary<int, object> TplDict;
        
        public TplMgr()
        {
            TplDict = new Dictionary<int, object>();
        }

        public object Find(int id)
        {
            object t = null;
            if (!TplDict.TryGetValue(id, out t))
            {
                throw new Exception(string.Format("没有找到对应的ID. {0}", this.GetType()));
            }
            return t;
        }

        public List<object> FindAll()
        {
            List<object> list = new List<object>(TplDict.Values);
            return list;
        }

        public void Setup(Type type, DynamicBuffer buffer)
        {
            Count = buffer.ReadInt32(); // 读取数据行数
            for (int i = 0; i < Count; i++)
            {
                BaseTpl oTpl = Activator.CreateInstance(type) as BaseTpl;
                if (oTpl != null)
                {
                    oTpl.Deserialization(buffer);
                }
                TplDict.Add(oTpl.ID, oTpl);
            }
        }
    }

    public abstract class BaseTpl
    {
        public int ID { get; protected set; }

        public abstract void Deserialization(DynamicBuffer buffer);
    }
}
