// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFAutoFilter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFAutoFilter : IAutoFilter
  {
    private XSSFSheet _sheet;

    public XSSFAutoFilter(XSSFSheet sheet)
    {
      this._sheet = sheet;
    }
  }
}
