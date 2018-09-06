using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZCSharpLib.ZTUtils
{
    public class CommUtil
    {
        /// <summary>
        /// 获取程序集的类型
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypes()
        {
            List<Type> types = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                types.AddRange(assemblies[i].GetTypes());
            }

            return types.ToArray();
        }

        /// <summary>
        /// IL创建一个对象
        /// </summary>
        /// <typeparam name="T">需要创建对象的类型</typeparam>
        /// <returns>创建的对象</returns>
        public static T ILCreateInstance<T>() where T : class, new()
        {
            Type type = typeof(T);
            return ILCreateInstance(type) as T;
        }

        /// <summary>
        /// IL创建一个对象
        /// </summary>
        /// <param name="type">需要创建对象的类型</param>
        /// <returns>对象</returns>
        public static object ILCreateInstance(Type type)
        {
            //ConstructorInfo defaultCtor = type.GetConstructor(new Type[] { });

            //System.Reflection.Emit.DynamicMethod dynMethod = new System.Reflection.Emit.DynamicMethod(
            //    name: string.Format("_{0:N}", Guid.NewGuid()),
            //    returnType: type,
            //    parameterTypes: null);

            //var gen = dynMethod.GetILGenerator();
            //gen.Emit(System.Reflection.Emit.OpCodes.Newobj, defaultCtor);
            //gen.Emit(System.Reflection.Emit.OpCodes.Ret);

            //return dynMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;

            System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod(string.Empty, typeof(object), Type.EmptyTypes);
            var gen = dm.GetILGenerator();
            gen.Emit(System.Reflection.Emit.OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            gen.Emit(System.Reflection.Emit.OpCodes.Ret);
            return (Func<object>)dm.CreateDelegate(typeof(Func<object>));
        }

        /// <summary>
        /// 生成唯一GUID
        /// </summary>
        /// <returns></returns>
        public static long CenerateGUID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 获取时间戳,以毫秒为单位
        /// </summary>
        /// <returns>毫秒</returns>
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long msdiff = Convert.ToInt64(ts.TotalMilliseconds);
            return msdiff;
        }

        public static string GenerateMD5(string path)
        {
            string MD5 = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                MD5 = GenerateMD5(bytes);
            }
            return MD5;
        }

        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <returns></returns>
        public static string GenerateMD5(byte[] input)
        {
            string md5Str = string.Empty;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(input);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                md5Str = sb.ToString();
            }
            md5Str = md5Str.Replace("-", "");
            return md5Str;
        }
    }
}
