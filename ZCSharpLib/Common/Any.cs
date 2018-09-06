namespace ZCSharpLib.Common
{
    public class Any : System.Object
    {
        public T As<T>() where T : Any
        {
            return (T)this;
        }
    }
}
