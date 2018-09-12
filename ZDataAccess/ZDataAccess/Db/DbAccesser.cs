using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataAccess.Db
{
    public abstract class DbAccesser
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IntergratedSecurity { get; set; }

        public DbAccesser() { }

        public 
    }
}
