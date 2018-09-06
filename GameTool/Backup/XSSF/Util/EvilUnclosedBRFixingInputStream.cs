// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Util.EvilUnclosedBRFixingInputStream
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI.XSSF.Util
{
  public class EvilUnclosedBRFixingInputStream : Stream
  {
    private static byte[] detect = new byte[4]{ (byte) 60, (byte) 98, (byte) 114, (byte) 62 };
    private Stream source;
    private byte[] spare;

    public EvilUnclosedBRFixingInputStream(Stream source)
    {
      this.source = source;
    }

    public int Read()
    {
      return this.source.ReadByte();
    }

    public override int Read(byte[] b, int off, int len)
    {
      int num1 = this.ReadFromSpare(b, off, len);
      int num2 = this.source.Read(b, off + num1, len - num1);
      int read;
      switch (num2)
      {
        case -1:
        case 0:
          read = num1;
          break;
        default:
          read = num1 + num2;
          break;
      }
      if (read > 0)
        read = this.fixUp(b, off, read);
      return read;
    }

    public int Read(byte[] b)
    {
      return this.Read(b, 0, b.Length);
    }

    private int ReadFromSpare(byte[] b, int offset, int len)
    {
      if (this.spare == null)
        return 0;
      if (len == 0)
        throw new ArgumentException("Asked to read 0 bytes");
      if (this.spare.Length <= len)
      {
        Array.Copy((Array) this.spare, 0, (Array) b, offset, this.spare.Length);
        int length = this.spare.Length;
        this.spare = (byte[]) null;
        return length;
      }
      byte[] numArray = new byte[this.spare.Length - len];
      Array.Copy((Array) this.spare, 0, (Array) b, offset, len);
      Array.Copy((Array) this.spare, len, (Array) numArray, 0, numArray.Length);
      this.spare = numArray;
      return len;
    }

    private void AddToSpare(byte[] b, int offset, int len, bool atTheEnd)
    {
      if (this.spare == null)
      {
        this.spare = new byte[len];
        Array.Copy((Array) b, offset, (Array) this.spare, 0, len);
      }
      else
      {
        byte[] numArray = new byte[this.spare.Length + len];
        if (atTheEnd)
        {
          Array.Copy((Array) this.spare, 0, (Array) numArray, 0, this.spare.Length);
          Array.Copy((Array) b, offset, (Array) numArray, this.spare.Length, len);
        }
        else
        {
          Array.Copy((Array) b, offset, (Array) numArray, 0, len);
          Array.Copy((Array) this.spare, 0, (Array) numArray, len, this.spare.Length);
        }
        this.spare = numArray;
      }
    }

    private int fixUp(byte[] b, int offset, int read)
    {
      for (int index1 = 0; index1 < EvilUnclosedBRFixingInputStream.detect.Length - 1; ++index1)
      {
        int offset1 = offset + read - 1 - index1;
        if (offset1 >= 0)
        {
          bool flag = true;
          for (int index2 = 0; index2 <= index1 && flag; ++index2)
          {
            if ((int) b[offset1 + index2] != (int) EvilUnclosedBRFixingInputStream.detect[index2])
              flag = false;
          }
          if (flag)
          {
            this.AddToSpare(b, offset1, index1 + 1, true);
            --read;
            read -= index1;
            break;
          }
        }
      }
      List<int> intList = new List<int>();
      for (int index1 = offset; index1 <= offset + read - EvilUnclosedBRFixingInputStream.detect.Length; ++index1)
      {
        bool flag = true;
        for (int index2 = 0; index2 < EvilUnclosedBRFixingInputStream.detect.Length && flag; ++index2)
        {
          if ((int) b[index1 + index2] != (int) EvilUnclosedBRFixingInputStream.detect[index2])
            flag = false;
        }
        if (flag)
          intList.Add(index1);
      }
      if (intList.Count == 0)
        return read;
      int num1 = offset + read + intList.Count;
      int len = num1 - b.Length;
      if (len > 0)
      {
        int num2 = 0;
        foreach (int num3 in intList)
        {
          if (num3 > offset + read - EvilUnclosedBRFixingInputStream.detect.Length - len - num2)
          {
            len = num1 - num3 - 1 - num2;
            break;
          }
          ++num2;
        }
        this.AddToSpare(b, offset + read - len, len, false);
        read -= len;
      }
      for (int index = intList.Count - 1; index >= 0; --index)
      {
        int num2 = intList[index];
        if (num2 < read + offset && num2 <= read - 3)
        {
          byte[] numArray = new byte[read - num2 - 3];
          Array.Copy((Array) b, num2 + 3, (Array) numArray, 0, numArray.Length);
          b[num2 + 3] = (byte) 47;
          Array.Copy((Array) numArray, 0, (Array) b, num2 + 4, numArray.Length);
          ++read;
        }
      }
      return read;
    }

    public override bool CanRead
    {
      get
      {
        return true;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return true;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return false;
      }
    }

    public override void Flush()
    {
    }

    public override long Length
    {
      get
      {
        return this.source.Length;
      }
    }

    public override long Position
    {
      get
      {
        return this.source.Position;
      }
      set
      {
        this.source.Position = value;
      }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      return this.source.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
      throw new InvalidOperationException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new InvalidOperationException();
    }
  }
}
