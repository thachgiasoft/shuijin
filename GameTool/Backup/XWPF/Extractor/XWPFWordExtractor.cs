// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Extractor.XWPFWordExtractor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace NPOI.XWPF.Extractor
{
  public class XWPFWordExtractor : POIXMLTextExtractor
  {
    public static XWPFRelation[] SUPPORTED_TYPES = new XWPFRelation[4]{ XWPFRelation.DOCUMENT, XWPFRelation.TEMPLATE, XWPFRelation.MACRO_DOCUMENT, XWPFRelation.MACRO_TEMPLATE_DOCUMENT };
    private XWPFDocument document;
    private bool fetchHyperlinks;

    public XWPFWordExtractor(OPCPackage Container)
      : this(new XWPFDocument(Container))
    {
    }

    public XWPFWordExtractor(XWPFDocument document)
      : base((POIXMLDocument) document)
    {
      this.document = document;
    }

    public void SetFetchHyperlinks(bool fetch)
    {
      this.fetchHyperlinks = fetch;
    }

    public override string Text
    {
      get
      {
        StringBuilder text = new StringBuilder();
        XWPFHeaderFooterPolicy headerFooterPolicy = this.document.GetHeaderFooterPolicy();
        this.extractHeaders(text, headerFooterPolicy);
        IEnumerator<XWPFParagraph> paragraphsEnumerator = this.document.GetParagraphsEnumerator();
        while (paragraphsEnumerator.MoveNext())
        {
          XWPFParagraph current = paragraphsEnumerator.Current;
          try
          {
            CT_SectPr sectPr = (CT_SectPr) null;
            if (current.GetCTP().pPr != null)
              sectPr = current.GetCTP().pPr.sectPr;
            XWPFHeaderFooterPolicy hfPolicy = (XWPFHeaderFooterPolicy) null;
            if (sectPr != null)
            {
              hfPolicy = new XWPFHeaderFooterPolicy(this.document, sectPr);
              this.extractHeaders(text, hfPolicy);
            }
            foreach (XWPFRun run in (IEnumerable<XWPFRun>) current.GetRuns())
            {
              text.Append(run.ToString());
              if (run is XWPFHyperlinkRun && this.fetchHyperlinks)
              {
                XWPFHyperlink hyperlink = ((XWPFHyperlinkRun) run).GetHyperlink(this.document);
                if (hyperlink != null)
                  text.Append(" <" + hyperlink.URL + ">");
              }
            }
            XWPFCommentsDecorator commentsDecorator = new XWPFCommentsDecorator(current, (XWPFParagraphDecorator) null);
            text.Append(commentsDecorator.GetCommentText()).Append('\n');
            string footnoteText = current.GetFootnoteText();
            if (footnoteText != null && footnoteText.Length > 0)
              text.Append(footnoteText + "\n");
            if (sectPr != null)
              this.extractFooters(text, hfPolicy);
          }
          catch (IOException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
          catch (XmlException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
        }
        IEnumerator<XWPFTable> tablesEnumerator = this.document.GetTablesEnumerator();
        while (tablesEnumerator.MoveNext())
          text.Append(tablesEnumerator.Current.GetText()).Append('\n');
        this.extractFooters(text, headerFooterPolicy);
        return text.ToString();
      }
    }

    private void extractFooters(StringBuilder text, XWPFHeaderFooterPolicy hfPolicy)
    {
      if (hfPolicy.GetFirstPageFooter() != null)
        text.Append(hfPolicy.GetFirstPageFooter().GetText());
      if (hfPolicy.GetEvenPageFooter() != null)
        text.Append(hfPolicy.GetEvenPageFooter().GetText());
      if (hfPolicy.GetDefaultFooter() == null)
        return;
      text.Append(hfPolicy.GetDefaultFooter().GetText());
    }

    private void extractHeaders(StringBuilder text, XWPFHeaderFooterPolicy hfPolicy)
    {
      if (hfPolicy.GetFirstPageHeader() != null)
        text.Append(hfPolicy.GetFirstPageHeader().GetText());
      if (hfPolicy.GetEvenPageHeader() != null)
        text.Append(hfPolicy.GetEvenPageHeader().GetText());
      if (hfPolicy.GetDefaultHeader() == null)
        return;
      text.Append(hfPolicy.GetDefaultHeader().GetText());
    }
  }
}
