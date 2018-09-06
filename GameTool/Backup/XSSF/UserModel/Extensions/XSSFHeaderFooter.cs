// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Extensions.XSSFHeaderFooter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel.Helpers;

namespace NPOI.XSSF.UserModel.Extensions
{
  public abstract class XSSFHeaderFooter : IHeaderFooter
  {
    private HeaderFooterHelper helper;
    private CT_HeaderFooter headerFooter;
    private bool stripFields;

    public XSSFHeaderFooter(CT_HeaderFooter headerFooter)
    {
      this.headerFooter = headerFooter;
      this.helper = new HeaderFooterHelper();
    }

    public CT_HeaderFooter GetHeaderFooter()
    {
      return this.headerFooter;
    }

    public string GetValue()
    {
      return this.Text ?? "";
    }

    public bool AreFieldsStripped()
    {
      return this.stripFields;
    }

    public void SetAreFieldsStripped(bool stripFields)
    {
      this.stripFields = stripFields;
    }

    public abstract string Text { get; set; }

    public static string StripFields(string text)
    {
      return HeaderFooter.StripFields(text);
    }

    public string Center
    {
      get
      {
        string centerSection = this.helper.GetCenterSection(this.Text);
        if (this.stripFields)
          return XSSFHeaderFooter.StripFields(centerSection);
        return centerSection;
      }
      set
      {
        this.Text = this.helper.SetCenterSection(this.Text, value);
      }
    }

    public string Left
    {
      get
      {
        string leftSection = this.helper.GetLeftSection(this.Text);
        if (this.stripFields)
          return XSSFHeaderFooter.StripFields(leftSection);
        return leftSection;
      }
      set
      {
        this.Text = this.helper.SetLeftSection(this.Text, value);
      }
    }

    public string Right
    {
      get
      {
        string rightSection = this.helper.GetRightSection(this.Text);
        if (this.stripFields)
          return XSSFHeaderFooter.StripFields(rightSection);
        return rightSection;
      }
      set
      {
        this.Text = this.helper.SetRightSection(this.Text, value);
      }
    }
  }
}
