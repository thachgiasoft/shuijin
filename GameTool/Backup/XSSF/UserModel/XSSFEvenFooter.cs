// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFEvenFooter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel.Extensions;

namespace NPOI.XSSF.UserModel
{
  public class XSSFEvenFooter : XSSFHeaderFooter, IFooter, IHeaderFooter
  {
    public XSSFEvenFooter(CT_HeaderFooter headerFooter)
      : base(headerFooter)
    {
      headerFooter.differentOddEven = true;
    }

    public override string Text
    {
      get
      {
        return this.GetHeaderFooter().evenFooter;
      }
      set
      {
        this.GetHeaderFooter().evenFooter = value;
      }
    }
  }
}
