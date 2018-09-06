// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFStyle
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;

namespace NPOI.XWPF.UserModel
{
  public class XWPFStyle
  {
    private CT_Style ctStyle;
    protected XWPFStyles styles;

    public XWPFStyle(CT_Style style)
      : this(style, (XWPFStyles) null)
    {
    }

    public XWPFStyle(CT_Style style, XWPFStyles styles)
    {
      this.ctStyle = style;
      this.styles = styles;
    }

    public string GetStyleId()
    {
      return this.ctStyle.styleId;
    }

    public ST_StyleType GetStyleType()
    {
      return this.ctStyle.type;
    }

    public void SetStyle(CT_Style style)
    {
      this.ctStyle = style;
    }

    public CT_Style GetCTStyle()
    {
      return this.ctStyle;
    }

    public void SetStyleId(string styleId)
    {
      this.ctStyle.styleId = styleId;
    }

    public void SetType(ST_StyleType type)
    {
      this.ctStyle.type = type;
    }

    public XWPFStyles GetStyles()
    {
      return this.styles;
    }

    public string GetBasisStyleID()
    {
      if (this.ctStyle.basedOn != null)
        return this.ctStyle.basedOn.val;
      return (string) null;
    }

    public string GetLinkStyleID()
    {
      if (this.ctStyle.link != null)
        return this.ctStyle.link.val;
      return (string) null;
    }

    public string GetNextStyleID()
    {
      if (this.ctStyle.next != null)
        return this.ctStyle.next.val;
      return (string) null;
    }

    public string GetName()
    {
      if (this.ctStyle.IsSetName())
        return this.ctStyle.name.val;
      return (string) null;
    }

    public bool HasSameName(XWPFStyle compStyle)
    {
      return compStyle.GetCTStyle().name.val.Equals(this.ctStyle.name.val);
    }
  }
}
