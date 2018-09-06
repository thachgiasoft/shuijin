// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Model.XMLParagraph
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;

namespace NPOI.XWPF.Model
{
  public class XMLParagraph
  {
    protected CT_P paragraph;

    public XMLParagraph(CT_P paragraph)
    {
      this.paragraph = paragraph;
    }

    public CT_P GetCTP()
    {
      return this.paragraph;
    }
  }
}
