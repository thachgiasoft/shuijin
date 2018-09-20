using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib;
using ZCSharpLib.ZTObject;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;

namespace ZCSharpLib.Features.Model
{
    public abstract class ModelObject : ZEventObject
    {
        public abstract int DataCount { get; }
        public abstract Type DataType { get; }
        public IModelAccesser Accesser { get; protected set; }
        public abstract void SetAccesser(IModelAccesser accesser);
        public abstract void InjectData(object obj);
    }

    public abstract class BaseModel<T> : ModelObject where T : BaseData, new()
    {
        protected Dictionary<string, T> DataTable { get; set; }

        public override int DataCount
        {
            get { return DataTable.Count; }
        }

        public override Type DataType
        {
            get { return typeof(T); }
        }

        public BaseModel()
        {
            DataTable = new Dictionary<string, T>();
        }

        public override void SetAccesser(IModelAccesser accesser)
        {
            Accesser = accesser;
        }

        public override void InjectData(object obj)
        {
            IList<T> oDatas = obj as IList<T>;
            for (int i = 0; i < oDatas.Count; i++)
            {
                T oData = oDatas[i];
                DataTable.Add(oData.Guid, oData);
            }
        }

        #region 数据访问
        public virtual bool Add(string oGuid, T oData)
        {
            if (!DataTable.ContainsKey(oGuid))
            {
                DataTable.Add(oGuid, oData);
                Event.Notify(EventCall, new ModelEventArgs() { Data = oData, OpType = ModelOpType.Add });
                if (Accesser != null)
                {
                    Accesser.Add(oData);
                }
                return true;
            }
            else
            {
                ZLogger.Error("{0} 已经存在数据: GUID={1}", GetType().Name, oGuid);
                return false;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="oGuid">数据ID</param>
        /// <param name="isClear">是否清理数据包含的非代码资源</param>
        /// <returns></returns>
        public virtual bool Remove(string oGuid, bool isClear = false)
        {
            T obj = null;
            if (DataTable.TryGetValue(oGuid, out obj))
            {
                DataTable.Remove(oGuid);
                Event.Notify(EventCall, new ModelEventArgs() { Data = obj, OpType = ModelOpType.Remove });
                if (Accesser != null) Accesser.Remove(obj);
                obj.Dispose(); obj = null; 
                return true;
            }
            else
            {
                ZLogger.Error("{0} 没有找到对应数据：{1}", typeof(T), oGuid);
                return false;
            }
        }

        public virtual void RemoveAll(bool isClearAll = false)
        {
            List<string> oGuids = new List<string>(DataTable.Keys);
            for (int i = 0; i < oGuids.Count; i++)
            {
                Remove(oGuids[i], isClearAll);
            }
        }

        public virtual bool Modify(string oGuid, T t)
        {
            T obj = null;
            if (DataTable.TryGetValue(oGuid, out obj))
            {
                t.CopyTo(obj);
                IEventArgs oIEventArgs = new ModelEventArgs() { Data = obj, OpType = ModelOpType.Modify };
                Event.Notify(EventCall, oIEventArgs);
                obj.Notify(oIEventArgs);
                if (Accesser != null) Accesser.Modify(obj);
                return true;
            }
            else
            {
                ZLogger.Error("{0} 没有找到对应数据: GUID={1}", GetType().Name, oGuid);
                return false;
            }
        }

        /// <summary>
        /// 从数据缓存中查找数据
        /// </summary>
        public virtual T Search(string oGuid)
        {
            T oData = null;
            if (!DataTable.TryGetValue(oGuid, out oData))
            {
                ZLogger.Error("{0} 没有找到对应数据: GUID={1}", GetType().Name, oGuid);
            }
            T t = oData.Clone() as T;
            return t;
        }

        /// <summary>
        /// 查找所有数据
        /// </summary>
        public virtual IList<T> SearchAll()
        {
            IList<T> oLists = new List<T>();
            foreach (var item in DataTable.Values)
            {
                T t = item.Clone() as T;
                oLists.Add(t);
            }
            return oLists;
        }
        #endregion
    }
}

