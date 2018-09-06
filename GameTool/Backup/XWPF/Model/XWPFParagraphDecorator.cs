// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Model.XWPFParagraphDecorator
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.XWPF.UserModel;

namespace NPOI.XWPF.Model
{
  public abstract class XWPFParagraphDecorator
  {
    internal XWPFParagraph paragraph;
    internal XWPFParagraphDecorator nextDecorator;

    public XWPFParagraphDecorator(XWPFParagraph paragraph)
      : this(paragraph, (XWPFParagraphDecorator) null)
    {
    }

    public XWPFParagraphDecorator(XWPFParagraph paragraph, XWPFParagraphDecorator nextDecorator)
    {
      this.paragraph = paragraph;
      this.nextDecorator = nextDecorator;
    }

    public virtual string GetText()
    {
      if (this.nextDecorator != null)
        return this.nextDecorator.GetText();
      return this.paragraph.GetText();
    }
  }
}
