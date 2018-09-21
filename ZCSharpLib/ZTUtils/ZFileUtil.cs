using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZCSharpLib.Common;

namespace ZCSharpLib.ZTUtils
{
    public class ZFileUtil
    {
        public static byte[] ReadBytes(string path)
        {
            byte[] bytes = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public static string ReadUTF8(byte[] buffer)
        {
            if (buffer == null) return null;
            if (buffer.Length <= 3) return Encoding.UTF8.GetString(buffer);
            byte[] array = new byte[] { 239, 187, 191 };
            if (buffer[0] == array[0] && buffer[1] == array[1] && buffer[2] == array[2])
            {
                return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        public static string ReadUTF8(string path)
        {
            return ReadUTF8(ReadBytes(path));
        }

        public static void WriteBytes(string sPath, byte[] bytes)
        {
            if (bytes == null) { throw new NullReferenceException(); }
            using (FileStream fs = new FileStream(sPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteUTF8(string sPath, string sContent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sContent);
            WriteBytes(sPath, bytes);
        }
    }
}