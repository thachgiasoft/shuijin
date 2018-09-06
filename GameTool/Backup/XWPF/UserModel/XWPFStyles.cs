// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFStyles
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFStyles : POIXMLDocumentPart
  {
    private List<XWPFStyle> listStyle = new List<XWPFStyle>();
    private CT_Styles ctStyles;
    private XWPFLatentStyles latentStyles;

    public XWPFStyles(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    public XWPFStyles()
    {
    }

    internal override void OnDocumentRead()
    {
      try
      {
        this.ctStyles = StylesDocument.Parse(this.GetPackagePart().GetInputStream()).Styles;
        this.latentStyles = new XWPFLatentStyles(this.ctStyles.latentStyles, this);
      }
      catch (XmlException ex)
      {
        throw new POIXMLException("Unable to read styles", (Exception) ex);
      }
      foreach (CT_Style style in (IEnumerable<CT_Style>) this.ctStyles.GetStyleList())
        this.listStyle.Add(new XWPFStyle(style, this));
    }

    protected override void Commit()
    {
      if (this.ctStyles == null)
        throw new InvalidOperationException("Unable to write out styles that were never read in!");
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      StylesDocument stylesDocument = new StylesDocument(this.ctStyles);
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[8]{ new XmlQualifiedName("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006"), new XmlQualifiedName("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), new XmlQualifiedName("m", "http://schemas.openxmlformats.org/officeDocument/2006/math"), new XmlQualifiedName("v", "urn:schemas-microsoft-com:vml"), new XmlQualifiedName("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"), new XmlQualifiedName("w10", "urn:schemas-microsoft-com:office:word"), new XmlQualifiedName("wne", "http://schemas.microsoft.com/office/word/2006/wordml"), new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") });
      stylesDocument.Save(outputStream, namespaces);
      outputStream.Close();
    }

    public void SetStyles(CT_Styles styles)
    {
      this.ctStyles = styles;
    }

    public bool StyleExist(string styleID)
    {
      foreach (XWPFStyle xwpfStyle in this.listStyle)
      {
        if (xwpfStyle.GetStyleId().Equals(styleID))
          return true;
      }
      return false;
    }

    public void AddStyle(XWPFStyle style)
    {
      this.listStyle.Add(style);
      this.ctStyles.AddNewStyle();
      this.ctStyles.SetStyleArray(this.ctStyles.GetStyleList().Count - 1, style.GetCTStyle());
    }

    public XWPFStyle GetStyle(string styleID)
    {
      foreach (XWPFStyle xwpfStyle in this.listStyle)
      {
        if (xwpfStyle.GetStyleId().Equals(styleID))
          return xwpfStyle;
      }
      return (XWPFStyle) null;
    }

    public List<XWPFStyle> GetUsedStyleList(XWPFStyle style)
    {
      return this.GetUsedStyleList(style, new List<XWPFStyle>() { style });
    }

    private List<XWPFStyle> GetUsedStyleList(XWPFStyle style, List<XWPFStyle> usedStyleList)
    {
      XWPFStyle style1 = this.GetStyle(style.GetBasisStyleID());
      if (style1 != null && !usedStyleList.Contains(style1))
      {
        usedStyleList.Add(style1);
        this.GetUsedStyleList(style1, usedStyleList);
      }
      XWPFStyle style2 = this.GetStyle(style.GetLinkStyleID());
      if (style2 != null && !usedStyleList.Contains(style2))
      {
        usedStyleList.Add(style2);
        this.GetUsedStyleList(style2, usedStyleList);
      }
      XWPFStyle style3 = this.GetStyle(style.GetNextStyleID());
      if (style3 != null && !usedStyleList.Contains(style3))
      {
        usedStyleList.Add(style2);
        this.GetUsedStyleList(style2, usedStyleList);
      }
      return usedStyleList;
    }

    public void SetSpellingLanguage(string strSpellingLanguage)
    {
      CT_DocDefaults ctDocDefaults = (CT_DocDefaults) null;
      CT_RPr ctRpr = (CT_RPr) null;
      CT_Language ctLanguage = (CT_Language) null;
      if (this.ctStyles.IsSetDocDefaults())
      {
        ctDocDefaults = this.ctStyles.docDefaults;
        if (ctDocDefaults.IsSetRPrDefault())
        {
          CT_RPrDefault rPrDefault = ctDocDefaults.rPrDefault;
          if (rPrDefault.IsSetRPr())
          {
            ctRpr = rPrDefault.rPr;
            if (ctRpr.IsSetLang())
              ctLanguage = ctRpr.lang;
          }
        }
      }
      if (ctDocDefaults == null)
        ctDocDefaults = this.ctStyles.AddNewDocDefaults();
      if (ctRpr == null)
        ctRpr = ctDocDefaults.AddNewRPrDefault().AddNewRPr();
      if (ctLanguage == null)
        ctLanguage = ctRpr.AddNewLang();
      ctLanguage.val = strSpellingLanguage;
      ctLanguage.bidi = strSpellingLanguage;
    }

    public void SetEastAsia(string strEastAsia)
    {
      CT_DocDefaults ctDocDefaults = (CT_DocDefaults) null;
      CT_RPr ctRpr = (CT_RPr) null;
      CT_Language ctLanguage = (CT_Language) null;
      if (this.ctStyles.IsSetDocDefaults())
      {
        ctDocDefaults = this.ctStyles.docDefaults;
        if (ctDocDefaults.IsSetRPrDefault())
        {
          CT_RPrDefault rPrDefault = ctDocDefaults.rPrDefault;
          if (rPrDefault.IsSetRPr())
          {
            ctRpr = rPrDefault.rPr;
            if (ctRpr.IsSetLang())
              ctLanguage = ctRpr.lang;
          }
        }
      }
      if (ctDocDefaults == null)
        ctDocDefaults = this.ctStyles.AddNewDocDefaults();
      if (ctRpr == null)
        ctRpr = ctDocDefaults.AddNewRPrDefault().AddNewRPr();
      if (ctLanguage == null)
        ctLanguage = ctRpr.AddNewLang();
      ctLanguage.eastAsia = strEastAsia;
    }

    public void SetDefaultFonts(CT_Fonts fonts)
    {
      CT_DocDefaults ctDocDefaults = (CT_DocDefaults) null;
      CT_RPr ctRpr = (CT_RPr) null;
      if (this.ctStyles.IsSetDocDefaults())
      {
        ctDocDefaults = this.ctStyles.docDefaults;
        if (ctDocDefaults.IsSetRPrDefault())
        {
          CT_RPrDefault rPrDefault = ctDocDefaults.rPrDefault;
          if (rPrDefault.IsSetRPr())
            ctRpr = rPrDefault.rPr;
        }
      }
      if (ctDocDefaults == null)
        ctDocDefaults = this.ctStyles.AddNewDocDefaults();
      if (ctRpr == null)
        ctRpr = ctDocDefaults.AddNewRPrDefault().AddNewRPr();
      ctRpr.rFonts = fonts;
    }

    public XWPFLatentStyles GetLatentStyles()
    {
      return this.latentStyles;
    }

    public XWPFStyle GetStyleWithSameName(XWPFStyle style)
    {
      foreach (XWPFStyle xwpfStyle in this.listStyle)
      {
        if (xwpfStyle.HasSameName(style))
          return xwpfStyle;
      }
      return (XWPFStyle) null;
    }
  }
}
