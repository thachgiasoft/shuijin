using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCSharpLib.ZTObject;

namespace ZDataAccess.Db
{
    public class DbAccess
    {
        private ZObjectPool<DbAccesser> AccesserPool;

        public DbAccess(int num)
        {
            AccesserPool = new ZObjectPool<DbAccesser>(num);
        }
    }
}
