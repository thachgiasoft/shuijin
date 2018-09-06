// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDataFormat
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.UserModel;
using NPOI.XSSF.Model;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDataFormat : IDataFormat
  {
    private StylesTable stylesSource;

    public XSSFDataFormat(StylesTable stylesSource)
    {
      this.stylesSource = stylesSource;
    }

    public short GetFormat(string format)
    {
      int num = BuiltinFormats.GetBuiltinFormat(format);
      if (num == -1)
        num = this.stylesSource.PutNumberFormat(format);
      return (short) num;
    }

    public string GetFormat(short index)
    {
      return this.stylesSource.GetNumberFormatAt((int) index) ?? BuiltinFormats.GetBuiltinFormat((int) index);
    }
  }
}
