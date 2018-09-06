// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Extensions.XSSFCellAlignment
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel.Extensions
{
  public class XSSFCellAlignment
  {
    private CT_CellAlignment cellAlignement;

    public XSSFCellAlignment(CT_CellAlignment cellAlignment)
    {
      this.cellAlignement = cellAlignment;
    }

    public VerticalAlignment GetVertical()
    {
      return (VerticalAlignment) this.cellAlignement.vertical;
    }

    public void SetVertical(VerticalAlignment align)
    {
      this.cellAlignement.vertical = (ST_VerticalAlignment) align;
      this.cellAlignement.verticalSpecified = true;
    }

    public HorizontalAlignment GetHorizontal()
    {
      return (HorizontalAlignment) this.cellAlignement.horizontal;
    }

    public void SetHorizontal(HorizontalAlignment align)
    {
      this.cellAlignement.horizontal = (ST_HorizontalAlignment) align;
      this.cellAlignement.horizontalSpecified = true;
    }

    public long GetIndent()
    {
      return this.cellAlignement.indent;
    }

    public void SetIndent(long indent)
    {
      this.cellAlignement.indent = indent;
      this.cellAlignement.indentSpecified = true;
    }

    public long GetTextRotation()
    {
      return this.cellAlignement.textRotation;
    }

    public void SetTextRotation(long rotation)
    {
      this.cellAlignement.textRotation = rotation;
      this.cellAlignement.textRotationSpecified = true;
    }

    public bool GetWrapText()
    {
      return this.cellAlignement.wrapText;
    }

    public void SetWrapText(bool wrapped)
    {
      this.cellAlignement.wrapText = wrapped;
      this.cellAlignement.wrapTextSpecified = true;
    }

    public CT_CellAlignment GetCTCellAlignment()
    {
      return this.cellAlignement;
    }
  }
}
