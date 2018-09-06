// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.IBody
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;
using System.Xml;

namespace NPOI.XWPF.UserModel
{
  public interface IBody
  {
    POIXMLDocumentPart GetPart();

    BodyType GetPartType();

    IList<IBodyElement> BodyElements { get; }

    IList<XWPFParagraph> Paragraphs { get; }

    IList<XWPFTable> GetTables();

    XWPFParagraph GetParagraph(CT_P p);

    XWPFTable GetTable(CT_Tbl ctTable);

    XWPFParagraph GetParagraphArray(int pos);

    XWPFTable GetTableArray(int pos);

    XWPFParagraph insertNewParagraph(XmlDocument cursor);

    XWPFTable insertNewTbl(XmlDocument cursor);

    void insertTable(int pos, XWPFTable table);

    XWPFTableCell GetTableCell(CT_Tc cell);

    XWPFDocument GetXWPFDocument();
  }
}
