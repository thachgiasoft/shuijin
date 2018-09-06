using ZCSharpLib.Features.Database;
using ZCSharpLib.Features.Model;
using ZCSharpLib.Common;

namespace ZCSharpLib.ZSupport.Adapter
{
    public class ModelAccesser : IModelAccesser
    {
        public void Add<T>(T t) where T : class, new()
        {
            App.Singleton<DbMgr>().Insert(t);
        }

        public void Modify<T>(T t) where T : class, new()
        {
            App.Singleton<DbMgr>().Modify(t);
        }

        public void Remove<T>(T t) where T : class, new()
        {
            App.Singleton<DbMgr>().Delete(t);
        }
    }
}
