// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLPropertiesTextExtractor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.OpenXmlFormats;
using System;
using System.Text;

namespace NPOI
{
  public class POIXMLPropertiesTextExtractor : POIXMLTextExtractor
  {
    public POIXMLPropertiesTextExtractor(POIXMLDocument doc)
      : base(doc)
    {
    }

    public POIXMLPropertiesTextExtractor(POIXMLTextExtractor otherExtractor)
      : base(otherExtractor.Document)
    {
    }

    private void AppendIfPresent(StringBuilder text, string thing, bool value)
    {
      this.AppendIfPresent(text, thing, value.ToString());
    }

    private void AppendIfPresent(StringBuilder text, string thing, int value)
    {
      this.AppendIfPresent(text, thing, value.ToString());
    }

    private void AppendIfPresent(StringBuilder text, string thing, DateTime? value)
    {
      if (!value.HasValue)
        return;
      this.AppendIfPresent(text, thing, value.ToString());
    }

    private void AppendIfPresent(StringBuilder text, string thing, string value)
    {
      if (value == null)
        return;
      text.Append(thing);
      text.Append(" = ");
      text.Append(value);
      text.Append("\n");
    }

    public string GetCorePropertiesText()
    {
      StringBuilder text = new StringBuilder();
      PackagePropertiesPart underlyingProperties = this.Document.GetProperties().GetCoreProperties().GetUnderlyingProperties();
      this.AppendIfPresent(text, "Category", underlyingProperties.GetCategoryProperty());
      this.AppendIfPresent(text, "Category", underlyingProperties.GetCategoryProperty());
      this.AppendIfPresent(text, "ContentStatus", underlyingProperties.GetContentStatusProperty());
      this.AppendIfPresent(text, "ContentType", underlyingProperties.GetContentTypeProperty());
      this.AppendIfPresent(text, "Created", new DateTime?(underlyingProperties.GetCreatedProperty().Value));
      this.AppendIfPresent(text, "CreatedString", underlyingProperties.GetCreatedPropertyString());
      this.AppendIfPresent(text, "Creator", underlyingProperties.GetCreatorProperty());
      this.AppendIfPresent(text, "Description", underlyingProperties.GetDescriptionProperty());
      this.AppendIfPresent(text, "Identifier", underlyingProperties.GetIdentifierProperty());
      this.AppendIfPresent(text, "Keywords", underlyingProperties.GetKeywordsProperty());
      this.AppendIfPresent(text, "Language", underlyingProperties.GetLanguageProperty());
      this.AppendIfPresent(text, "LastModifiedBy", underlyingProperties.GetLastModifiedByProperty());
      this.AppendIfPresent(text, "LastPrinted", underlyingProperties.GetLastPrintedProperty());
      this.AppendIfPresent(text, "LastPrintedString", underlyingProperties.GetLastPrintedPropertyString());
      this.AppendIfPresent(text, "Modified", underlyingProperties.GetModifiedProperty());
      this.AppendIfPresent(text, "ModifiedString", underlyingProperties.GetModifiedPropertyString());
      this.AppendIfPresent(text, "Revision", underlyingProperties.GetRevisionProperty());
      this.AppendIfPresent(text, "Subject", underlyingProperties.GetSubjectProperty());
      this.AppendIfPresent(text, "Title", underlyingProperties.GetTitleProperty());
      this.AppendIfPresent(text, "Version", underlyingProperties.GetVersionProperty());
      return text.ToString();
    }

    public string GetExtendedPropertiesText()
    {
      StringBuilder text = new StringBuilder();
      CT_ExtendedProperties underlyingProperties = this.Document.GetProperties().GetExtendedProperties().GetUnderlyingProperties();
      this.AppendIfPresent(text, "Application", underlyingProperties.Application);
      this.AppendIfPresent(text, "AppVersion", underlyingProperties.AppVersion);
      this.AppendIfPresent(text, "Characters", underlyingProperties.Characters);
      this.AppendIfPresent(text, "CharactersWithSpaces", underlyingProperties.CharactersWithSpaces);
      this.AppendIfPresent(text, "Company", underlyingProperties.Company);
      this.AppendIfPresent(text, "HyperlinkBase", underlyingProperties.HyperlinkBase);
      this.AppendIfPresent(text, "HyperlinksChanged", underlyingProperties.HyperlinksChanged);
      this.AppendIfPresent(text, "Lines", underlyingProperties.Lines);
      this.AppendIfPresent(text, "LinksUpToDate", underlyingProperties.LinksUpToDate);
      this.AppendIfPresent(text, "Manager", underlyingProperties.Manager);
      this.AppendIfPresent(text, "Pages", underlyingProperties.Pages);
      this.AppendIfPresent(text, "Paragraphs", underlyingProperties.Paragraphs);
      this.AppendIfPresent(text, "PresentationFormat", underlyingProperties.PresentationFormat);
      this.AppendIfPresent(text, "Template", underlyingProperties.Template);
      this.AppendIfPresent(text, "TotalTime", underlyingProperties.TotalTime);
      return text.ToString();
    }

    public string GetCustomPropertiesText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (CT_Property property in this.Document.GetProperties().GetCustomProperties().GetUnderlyingProperties().GetPropertyList())
      {
        string str = "(not implemented!)";
        stringBuilder.Append(property.name + " = " + str + "\n");
      }
      return stringBuilder.ToString();
    }

    public override string Text
    {
      get
      {
        return this.GetCorePropertiesText() + this.GetExtendedPropertiesText() + this.GetCustomPropertiesText();
      }
    }

    public override POITextExtractor MetadataTextExtractor
    {
      get
      {
        throw new InvalidOperationException("You already have the Metadata Text Extractor, not recursing!");
      }
    }
  }
}
