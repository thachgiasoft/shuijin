// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLTextExtractor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;

namespace NPOI
{
  public abstract class POIXMLTextExtractor : POITextExtractor
  {
    private POIXMLDocument _document;

    public POIXMLTextExtractor(POIXMLDocument document)
      : base((POIDocument) null)
    {
      this._document = document;
    }

    public POIXMLProperties.CoreProperties GetCoreProperties()
    {
      return this._document.GetProperties().GetCoreProperties();
    }

    public POIXMLProperties.ExtendedProperties GetExtendedProperties()
    {
      return this._document.GetProperties().GetExtendedProperties();
    }

    public POIXMLProperties.CustomProperties GetCustomProperties()
    {
      return this._document.GetProperties().GetCustomProperties();
    }

    public POIXMLDocument Document
    {
      get
      {
        return this._document;
      }
    }

    public OPCPackage Package
    {
      get
      {
        return this._document.Package;
      }
    }

    public override POITextExtractor MetadataTextExtractor
    {
      get
      {
        return (POITextExtractor) new POIXMLPropertiesTextExtractor(this._document);
      }
    }
  }
}
