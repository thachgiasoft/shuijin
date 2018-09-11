using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCSharpLib.ZTObject;

namespace ZDataAccess.Db
{
    public enum DbType
    {
        None,
        MySQL,
    }

    public class DbAccess
    {
        private ZObjectPool<DbAccesser> AccesserPool;

        public DbAccess(int num, DbType dbType)
        {
            AccesserPool = new ZObjectPool<DbAccesser>(num);
            for (int i = 0; i < AccesserPool.Count; i++)
            {

            }
        }
    }
}
