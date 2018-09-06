// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Model.XWPFCommentsDecorator
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System.Text;

namespace NPOI.XWPF.Model
{
  public class XWPFCommentsDecorator : XWPFParagraphDecorator
  {
    private StringBuilder commentText;

    public XWPFCommentsDecorator(XWPFParagraphDecorator nextDecorator)
      : this(nextDecorator.paragraph, nextDecorator)
    {
    }

    public XWPFCommentsDecorator(XWPFParagraph paragraph, XWPFParagraphDecorator nextDecorator)
      : base(paragraph, nextDecorator)
    {
      this.commentText = new StringBuilder();
      foreach (CT_MarkupRange commentRangeStart in paragraph.GetCTP().GetCommentRangeStartList())
      {
        XWPFComment commentById;
        if ((commentById = paragraph.GetDocument().GetCommentByID(commentRangeStart.id)) != null)
          this.commentText.Append("\tComment by " + commentById.GetAuthor() + ": " + commentById.GetText());
      }
    }

    public string GetCommentText()
    {
      return this.commentText.ToString();
    }

    public override string GetText()
    {
      return base.GetText() + (object) this.commentText;
    }
  }
}
