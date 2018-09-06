using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZCSharpLib.Common;

namespace ZCSharpLib.ZTUtils
{
    public class FileUtils
    {
        public static string GetUTF8String(byte[] buffer)
        {
            if (buffer == null)
            {
                return null;
            }
            if (buffer.Length <= 3)
            {
                return Encoding.UTF8.GetString(buffer);
            }
            byte[] array = new byte[] { 239, 187, 191 };
            if (buffer[0] == array[0] && buffer[1] == array[1] && buffer[2] == array[2])
            {
                return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        public static string GetUTF8String(string path)
        {
            return GetUTF8String(GetBytes(path));
        }

        public static byte[] GetBytes(string path)
        {
            byte[] bytes = null;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                ZLogger.Error(e.Message);
            }
            return bytes;
        }
    }
}