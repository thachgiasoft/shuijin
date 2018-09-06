// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFHyperlinkRun
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;

namespace NPOI.XWPF.UserModel
{
  public class XWPFHyperlinkRun : XWPFRun
  {
    private CT_Hyperlink1 hyperlink;

    public XWPFHyperlinkRun(CT_Hyperlink1 hyperlink, CT_R Run, XWPFParagraph p)
      : base(Run, p)
    {
      this.hyperlink = hyperlink;
    }

    public CT_Hyperlink1 GetCTHyperlink()
    {
      return this.hyperlink;
    }

    public string GetAnchor()
    {
      return this.hyperlink.anchor;
    }

    public string GetHyperlinkId()
    {
      return this.hyperlink.id;
    }

    public void SetHyperlinkId(string id)
    {
      this.hyperlink.id = id;
    }

    public XWPFHyperlink GetHyperlink(XWPFDocument document)
    {
      string hyperlinkId = this.GetHyperlinkId();
      if (hyperlinkId == null)
        return (XWPFHyperlink) null;
      return document.GetHyperlinkByID(hyperlinkId);
    }
  }
}
