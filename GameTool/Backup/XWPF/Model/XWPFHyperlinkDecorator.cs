// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Model.XWPFHyperlinkDecorator
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System.Text;

namespace NPOI.XWPF.Model
{
  public class XWPFHyperlinkDecorator : XWPFParagraphDecorator
  {
    private StringBuilder hyperlinkText;

    public XWPFHyperlinkDecorator(XWPFParagraphDecorator nextDecorator, bool outputHyperlinkUrls)
      : this(nextDecorator.paragraph, nextDecorator, outputHyperlinkUrls)
    {
    }

    public XWPFHyperlinkDecorator(XWPFParagraph prgrph, XWPFParagraphDecorator nextDecorator, bool outputHyperlinkUrls)
      : base(prgrph, nextDecorator)
    {
      this.hyperlinkText = new StringBuilder();
      foreach (CT_Hyperlink1 hyperlink in this.paragraph.GetCTP().GetHyperlinkList())
      {
        foreach (CT_R ctR in hyperlink.GetRList())
        {
          foreach (CT_Text ctText in ctR.GetTList())
            this.hyperlinkText.Append(ctText.Value);
        }
        if (outputHyperlinkUrls && this.paragraph.GetDocument().GetHyperlinkByID(hyperlink.id) != null)
          this.hyperlinkText.Append(" <" + this.paragraph.GetDocument().GetHyperlinkByID(hyperlink.id).URL + ">");
      }
    }

    public override string GetText()
    {
      return base.GetText() + (object) this.hyperlinkText;
    }
  }
}
