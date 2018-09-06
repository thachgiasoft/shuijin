using GameTool.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Modules.IconGen
{
    public class IconGenerate : IApplication
    {
        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public bool ConvertToIcon(Stream input, Stream output, int size = 16, bool preserveAspectRatio = false)
        {
            Bitmap inputBitmap = (Bitmap)Bitmap.FromStream(input);
            if (inputBitmap != null)
            {
                int width, height;
                if (preserveAspectRatio)
                {
                    width = size;
                    height = inputBitmap.Height / inputBitmap.Width * size;
                }
                else
                {
                    width = height = size;
                }
                Bitmap newBitmap = new Bitmap(inputBitmap, new Size(width, height));
                if (newBitmap != null)
                {
                    // save the resized png into a memory stream for future use
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        newBitmap.Save(memoryStream, ImageFormat.Png);

                        BinaryWriter iconWriter = new BinaryWriter(output);
                        if (output != null && iconWriter != null)
                        {
                            iconWriter.Write((byte)0);
                            iconWriter.Write((byte)0);
                            iconWriter.Write((short)1);
                            iconWriter.Write((short)1);
                            iconWriter.Write((byte)width);
                            iconWriter.Write((byte)height);
                            iconWriter.Write((byte)0);
                            iconWriter.Write((byte)0);
                            iconWriter.Write((short)0);
                            iconWriter.Write((short)32);
                            iconWriter.Write((int)memoryStream.Length);
                            iconWriter.Write((int)(6 + 16));
                            iconWriter.Write(memoryStream.ToArray());
                            iconWriter.Flush();
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }

        public bool ConvertToIcon(string inputPath, string outputPath, int size = 16, bool preserveAspectRatio = false)
        {
            using (FileStream inputStream = new FileStream(inputPath, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                return ConvertToIcon(inputStream, outputStream, size, preserveAspectRatio);
            }
        }
    }
}
