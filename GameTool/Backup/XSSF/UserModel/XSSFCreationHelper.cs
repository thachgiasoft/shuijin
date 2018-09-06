// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFCreationHelper
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFCreationHelper : ICreationHelper
  {
    private XSSFWorkbook workbook;

    public XSSFCreationHelper(XSSFWorkbook wb)
    {
      this.workbook = wb;
    }

    public IRichTextString CreateRichTextString(string text)
    {
      XSSFRichTextString xssfRichTextString = new XSSFRichTextString(text);
      xssfRichTextString.SetStylesTableReference(this.workbook.GetStylesSource());
      return (IRichTextString) xssfRichTextString;
    }

    public IDataFormat CreateDataFormat()
    {
      return this.workbook.CreateDataFormat();
    }

    public IHyperlink CreateHyperlink(HyperlinkType type)
    {
      return (IHyperlink) new XSSFHyperlink(type);
    }

    public IFormulaEvaluator CreateFormulaEvaluator()
    {
      return (IFormulaEvaluator) new XSSFFormulaEvaluator(this.workbook);
    }

    public IClientAnchor CreateClientAnchor()
    {
      return (IClientAnchor) new XSSFClientAnchor();
    }
  }
}
