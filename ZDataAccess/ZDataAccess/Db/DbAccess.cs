using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCSharpLib.ZTObject;
using ZDataAccess.Db.DbSort;

namespace ZDataAccess.Db
{
    public enum DbType
    {
        None = 0,
        MySQL,
        SQLServer,
        SQLLite,
        Orcale,
        Odbc,
        Max
    }

    public class DbAccess
    {
        private ZObjectPool<DbAccesser> AccesserPool;

        public DbAccess(int num, DbType dbType)
        {
            AccesserPool = new ZObjectPool<DbAccesser>(num);
            for (int i = 0; i < AccesserPool.Count; i++)
            {
                DbAccesser dbAccesser = Create(dbType);
                if (dbAccesser != null) AccesserPool.Push(dbAccesser);
            }
        }

        private DbAccesser Create(DbType dbType)
        {
            DbAccesser dbAccesser = null;
            if (dbType == DbType.MySQL)
            {
                dbAccesser = new MySqlAccesser();
            }
            else if (dbType == DbType.SQLServer)
            {
                dbAccesser = new SqlServerAccesser();
            }
            else if (dbType == DbType.SQLLite)
            {
                dbAccesser = new SqlLiteAccesser();
            }
            else if (dbType == DbType.Orcale)
            {
                dbAccesser = new OrcaleAccesser();
            }
            else if (dbType == DbType.Odbc)
            {
                dbAccesser = new OdbcAccesser();
            }
            return dbAccesser;
        }
    }
}
