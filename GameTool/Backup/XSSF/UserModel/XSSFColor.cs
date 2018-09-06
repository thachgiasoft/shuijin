// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFColor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.Util;
using System;
using System.Drawing;
using System.Text;

namespace NPOI.XSSF.UserModel
{
  public class XSSFColor : IColor
  {
    private CT_Color ctColor;

    public XSSFColor(CT_Color color)
    {
      this.ctColor = color;
    }

    public XSSFColor()
    {
      this.ctColor = new CT_Color();
    }

    public XSSFColor(Color clr)
      : this()
    {
      this.ctColor.SetRgb(clr.R, clr.G, clr.B);
    }

    public XSSFColor(byte[] rgb)
      : this()
    {
      this.ctColor.SetRgb(rgb);
    }

    public bool IsAuto
    {
      get
      {
        return this.ctColor.auto;
      }
      set
      {
        this.ctColor.auto = value;
        this.ctColor.autoSpecified = true;
      }
    }

    public short Indexed
    {
      get
      {
        if (!this.ctColor.indexedSpecified)
          return 0;
        return (short) this.ctColor.indexed;
      }
      set
      {
        this.ctColor.indexed = (uint) value;
        this.ctColor.indexedSpecified = true;
      }
    }

    private byte[] CorrectRGB(byte[] rgb)
    {
      if (rgb.Length == 4)
        return rgb;
      if (rgb[0] == (byte) 0 && rgb[1] == (byte) 0 && rgb[2] == (byte) 0)
        rgb = new byte[3]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else if (rgb[0] == byte.MaxValue && rgb[1] == byte.MaxValue && rgb[2] == byte.MaxValue)
        rgb = new byte[3];
      return rgb;
    }

    public byte[] GetRgb()
    {
      byte[] rgbOrArgb = this.GetRGBOrARGB();
      if (rgbOrArgb == null)
        return (byte[]) null;
      if (rgbOrArgb.Length != 4)
        return rgbOrArgb;
      byte[] numArray = new byte[3];
      Array.Copy((Array) rgbOrArgb, 1, (Array) numArray, 0, 3);
      return numArray;
    }

    public byte[] GetARgb()
    {
      byte[] rgbOrArgb = this.GetRGBOrARGB();
      if (rgbOrArgb == null)
        return (byte[]) null;
      if (rgbOrArgb.Length != 3)
        return rgbOrArgb;
      byte[] numArray = new byte[4]{ byte.MaxValue, (byte) 0, (byte) 0, (byte) 0 };
      Array.Copy((Array) rgbOrArgb, 0, (Array) numArray, 1, 3);
      return numArray;
    }

    private byte[] GetRGBOrARGB()
    {
      if (this.ctColor.indexedSpecified && this.ctColor.indexed > 0U)
      {
        HSSFColor hssfColor = (HSSFColor) HSSFColor.GetIndexHash()[(object) (int) this.ctColor.indexed];
        if (hssfColor != null)
          return new byte[3]{ (byte) hssfColor.GetTriplet()[0], (byte) hssfColor.GetTriplet()[1], (byte) hssfColor.GetTriplet()[2] };
      }
      if (!this.ctColor.IsSetRgb())
        return (byte[]) null;
      return this.CorrectRGB(this.ctColor.GetRgb());
    }

    public byte[] GetRgbWithTint()
    {
      byte[] numArray1 = this.ctColor.GetRgb();
      if (numArray1 != null)
      {
        if (numArray1.Length == 4)
        {
          byte[] numArray2 = new byte[3];
          Array.Copy((Array) numArray1, 1, (Array) numArray2, 0, 3);
          numArray1 = numArray2;
        }
        for (int index = 0; index < numArray1.Length; ++index)
          numArray1[index] = XSSFColor.ApplyTint((int) numArray1[index] & (int) byte.MaxValue, this.ctColor.tint);
      }
      return numArray1;
    }

    public string GetARGBHex()
    {
      StringBuilder stringBuilder = new StringBuilder();
      byte[] argb = this.GetARgb();
      if (argb == null)
        return (string) null;
      foreach (int chr in argb)
      {
        if (chr < 0)
          chr += 256;
        string hexString = StringUtil.ToHexString(chr);
        if (hexString.Length == 1)
          stringBuilder.Append('0');
        stringBuilder.Append(hexString);
      }
      return stringBuilder.ToString().ToUpper();
    }

    private static byte ApplyTint(int lum, double tint)
    {
      if (tint > 0.0)
        return (byte) ((double) lum * (1.0 - tint) + ((double) byte.MaxValue - (double) byte.MaxValue * (1.0 - tint)));
      if (tint < 0.0)
        return (byte) ((double) lum * (1.0 + tint));
      return (byte) lum;
    }

    public void SetRgb(byte[] rgb)
    {
      this.ctColor.SetRgb(this.CorrectRGB(rgb));
    }

    public int GetTheme()
    {
      if (!this.ctColor.themeSpecified)
        return 0;
      return (int) this.ctColor.theme;
    }

    public void SetTheme(int theme)
    {
      this.ctColor.theme = (uint) theme;
    }

    public double Tint
    {
      get
      {
        return this.ctColor.tint;
      }
      set
      {
        this.ctColor.tint = value;
        this.ctColor.tintSpecified = true;
      }
    }

    internal CT_Color GetCTColor()
    {
      return this.ctColor;
    }

    public override int GetHashCode()
    {
      return this.ctColor.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (o == null || !(o is XSSFColor))
        return false;
      return this.ctColor.ToString().Equals(((XSSFColor) o).GetCTColor().ToString());
    }
  }
}
