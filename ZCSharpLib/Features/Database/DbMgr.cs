using System;
using System.Collections.Generic;
using System.Threading;
using ZCSharpLib.Common;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Features.Database
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DbAttribute : Attribute
    {
        public string TableName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DbUnsaveAttribute : Attribute { }

    public class DbMgr
    {
        private BaseDb BaseDatabase { get; set; }
        private DbStructure SQLStructure { get; set; }
        private Queue<BaseDbOperate> ExeucteQueue { get; set; }
        private BaseDbOperate CurOperate { get; set; }

        public int m_Remain;
        public int Remain { get { return m_Remain; } }

        #region 异步数据库操作相关
        private Thread Thread { get; set; }
        private bool IsExecute { get; set; }
        private bool IsConnect { get; set; }
        private bool IsDisconnect { get; set; }
        private object _lock = new object();
        #endregion

        public DbMgr()
        {
            SQLStructure = new DbStructure();
            ExeucteQueue = new Queue<BaseDbOperate>();
            Type[] types = ZCommUtil.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.IsClass && type.BaseType == typeof(BaseDb))
                {
                    BaseDb oBaseSQL = Activator.CreateInstance(type) as BaseDb;
                    Setup(oBaseSQL);
                    break;
                }
            }
            Thread = new Thread(ThreadExecute);
        }

        public void Open()
        {
            App.AttachTick(Loop);
            Connect();
            IsExecute = true;
            Thread.Start();
        }

        public void Close()
        {
            App.DetachTick(Loop);
            IsExecute = false;
            Thread.Abort();
            Disconnect();
        }

        public void Loop(float deltaTime)
        {
            if (ExeucteQueue.Count > 0 && CurOperate == null)
            {
                CurOperate = ExeucteQueue.Dequeue();
            }

            if (CurOperate != null)
            {
                if (CurOperate.IsOperateComplated)
                {
                    CurOperate.OnExecCallback();
                    Interlocked.Decrement(ref m_Remain);
                    CurOperate = null;
                }
            }
        }

        private void ThreadExecute()
        {
            QueryLoop();
        }

        private void QueryLoop()
        {
            while (IsExecute)
            {
                lock (_lock)
                {
                    if (CurOperate != null)
                    {
                        CurOperate.Execute();
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void Setup(BaseDb oSQL)
        {
            BaseDatabase = oSQL;
        }

        public void Connect()
        {
            BaseDatabase.Connect();
        }

        public void Disconnect()
        {
            BaseDatabase.Disconnect();
        }

        public DatabaseReader Query(string query)
        {
            // 如果数据库断开连接则重新进行连接
            if (BaseDatabase.ConnectionType == ConnectionType.CLOSE)
            {
                Connect();
            }
            DatabaseReader reader = null;
            try
            {
                reader = BaseDatabase.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                App.Logger.Error("数据库操作出错!{0}\n{1}", query, e);
            }
            return reader;
        }

        public void Enqueue(BaseDbOperate operate)
        {
            ExeucteQueue.Enqueue(operate);
            Interlocked.Increment(ref m_Remain);
        }

        public void Insert<T>(T obj, Action<object> onFinished = null) where T : class, new()
        {
            lock (_lock)
            {
                DbStructure.DbStructContainer container = SQLStructure.Insert(obj);
                if (container != null)
                {
                    InsertData<T> operate = new InsertData<T>();
                    operate.Setup(this, obj, container, onFinished);
                    Enqueue(operate);
                }
            }
        }

        public void Delete<T>( T obj, Action<object> onFinished = null) where T : class, new()
        {
            lock (_lock)
            {
                DbStructure.DbStructContainer container = SQLStructure.Delete(obj);
                if (container != null)
                {
                    DeleteData<T> operate = new DeleteData<T>();
                    operate.Setup(this, obj, container, onFinished);
                    Enqueue(operate);
                }
            }
        }

        public void Modify<T>(T obj, Action<object> onFinished = null) where T : class, new()
        {
            lock (_lock)
            {
                DbStructure.DbStructContainer container = SQLStructure.Modify(obj);
                if (container != null)
                {
                    ModifyData<T> operate = new ModifyData<T>();
                    operate.Setup(this, obj, container, onFinished);
                    Enqueue(operate);
                }
            }
        }

        public void Search<T>(T obj, Action<object> onFinished = null) where T : class, new()
        {
            lock (_lock)
            {
                DbStructure.DbStructContainer container = SQLStructure.Modify(obj);
                if (container != null)
                {
                    SearchData<T> operate = new SearchData<T>();
                    operate.Setup(this, obj, container, onFinished);
                    Enqueue(operate);
                }
            }
        }

        public void SearchAll<T>(Action<object> action = null) where T : class, new()
        {
            DbStructure.DbStructContainer container = SQLStructure.SearchAll<T>();
            if (container != null)
            {
                SearchAllData<T> operate = new SearchAllData<T>();
                operate.Setup(this, null, container, action);
                Enqueue(operate);
            }
        }
    }
}
