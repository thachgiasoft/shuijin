// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFComment
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;
using System.Text;

namespace NPOI.XWPF.UserModel
{
  public class XWPFComment
  {
    protected string id;
    protected string author;
    protected StringBuilder text;

    public XWPFComment(CT_Comment comment, XWPFDocument document)
    {
      this.text = new StringBuilder();
      this.id = comment.id.ToString();
      this.author = comment.author;
      foreach (CT_P prgrph in (IEnumerable<CT_P>) comment.GetPList())
        this.text.Append(new XWPFParagraph(prgrph, (IBody) document).GetText());
    }

    public string GetId()
    {
      return this.id;
    }

    public string GetAuthor()
    {
      return this.author;
    }

    public string GetText()
    {
      return this.text.ToString();
    }
  }
}
