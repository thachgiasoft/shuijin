﻿// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFRelation
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;
using System.Collections.Generic;

namespace NPOI.XWPF.UserModel
{
  public class XWPFRelation : POIXMLRelation
  {
    protected static Dictionary<string, XWPFRelation> _table = new Dictionary<string, XWPFRelation>();
    public static XWPFRelation DOCUMENT = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/word/document.xml", (Type) null);
    public static XWPFRelation TEMPLATE = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.template.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/word/document.xml", (Type) null);
    public static XWPFRelation MACRO_DOCUMENT = new XWPFRelation("application/vnd.ms-word.document.macroEnabled.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/word/document.xml", (Type) null);
    public static XWPFRelation MACRO_TEMPLATE_DOCUMENT = new XWPFRelation("application/vnd.ms-word.template.macroEnabledTemplate.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/word/document.xml", (Type) null);
    public static XWPFRelation GLOSSARY_DOCUMENT = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.document.glossary+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/glossaryDocument", "/word/glossary/document.xml", (Type) null);
    public static XWPFRelation NUMBERING = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/numbering", "/word/numbering.xml", typeof (XWPFNumbering));
    public static XWPFRelation FONT_TABLE = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/fontTable", "/word/fontTable.xml", (Type) null);
    public static XWPFRelation SETTINGS = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.settings+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/settings", "/word/settings.xml", typeof (XWPFSettings));
    public static XWPFRelation STYLES = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", "/word/styles.xml", typeof (XWPFStyles));
    public static XWPFRelation WEB_SETTINGS = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.webSettings+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/webSettings", "/word/webSettings.xml", (Type) null);
    public static XWPFRelation HEADER = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/header", "/word/header#.xml", typeof (XWPFHeader));
    public static XWPFRelation FOOTER = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footer", "/word/footer#.xml", typeof (XWPFFooter));
    public static XWPFRelation HYPERLINK = new XWPFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", (string) null, (Type) null);
    public static XWPFRelation COMMENT = new XWPFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments", (string) null, (Type) null);
    public static XWPFRelation FOOTNOTE = new XWPFRelation("application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footnotes", "/word/footnotes.xml", typeof (XWPFFootnotes));
    public static XWPFRelation ENDNOTE = new XWPFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/endnotes", (string) null, (Type) null);
    public static XWPFRelation IMAGE_EMF = new XWPFRelation("image/x-emf", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.emf", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_WMF = new XWPFRelation("image/x-wmf", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.wmf", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_PICT = new XWPFRelation("image/pict", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.pict", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_JPEG = new XWPFRelation("image/jpeg", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.jpeg", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_PNG = new XWPFRelation("image/png", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.png", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_DIB = new XWPFRelation("image/dib", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.dib", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_GIF = new XWPFRelation("image/gif", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.gif", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_TIFF = new XWPFRelation("image/tiff", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.tiff", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_EPS = new XWPFRelation("image/x-eps", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.eps", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_BMP = new XWPFRelation("image/x-ms-bmp", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.bmp", typeof (XWPFPictureData));
    public static XWPFRelation IMAGE_WPG = new XWPFRelation("image/x-wpg", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/word/media/image#.wpg", typeof (XWPFPictureData));
    public static XWPFRelation IMAGES = new XWPFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", (string) null, (Type) null);

    private XWPFRelation(string type, string rel, string defaultName, Type cls)
      : base(type, rel, defaultName, cls)
    {
      if (cls == null || XWPFRelation._table.ContainsKey(rel))
        return;
      XWPFRelation._table.Add(rel, this);
    }

    public static XWPFRelation GetInstance(string rel)
    {
      if (XWPFRelation._table.ContainsKey(rel))
        return XWPFRelation._table[rel];
      return (XWPFRelation) null;
    }
  }
}
