// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFHyperlink
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFHyperlink : IHyperlink
  {
    private HyperlinkType _type;
    private PackageRelationship _externalRel;
    private CT_Hyperlink _ctHyperlink;
    private string _location;

    public XSSFHyperlink(HyperlinkType type)
    {
      this._type = type;
      this._ctHyperlink = new CT_Hyperlink();
    }

    public XSSFHyperlink(CT_Hyperlink ctHyperlink, PackageRelationship hyperlinkRel)
    {
      this._ctHyperlink = ctHyperlink;
      this._externalRel = hyperlinkRel;
      if (ctHyperlink.location != null)
      {
        this._type = HyperlinkType.DOCUMENT;
        this._location = ctHyperlink.location;
      }
      else if (this._externalRel == null)
      {
        if (ctHyperlink.id != null)
          throw new InvalidOperationException("The hyperlink for cell " + ctHyperlink.@ref + " references relation " + ctHyperlink.id + ", but that didn't exist!");
        this._type = HyperlinkType.DOCUMENT;
      }
      else
      {
        this._location = this._externalRel.TargetUri.ToString();
        if (this._location.StartsWith("http://") || this._location.StartsWith("https://") || this._location.StartsWith("ftp://"))
          this._type = HyperlinkType.URL;
        else if (this._location.StartsWith("mailto:"))
          this._type = HyperlinkType.EMAIL;
        else
          this._type = HyperlinkType.FILE;
      }
    }

    public CT_Hyperlink GetCTHyperlink()
    {
      return this._ctHyperlink;
    }

    public bool NeedsRelationToo()
    {
      return this._type != HyperlinkType.DOCUMENT;
    }

    internal void GenerateRelationIfNeeded(PackagePart sheetPart)
    {
      if (this._externalRel != null || !this.NeedsRelationToo())
        return;
      this._ctHyperlink.id = sheetPart.AddExternalRelationship(this._location, XSSFRelation.SHEET_HYPERLINKS.Relation).Id;
    }

    public HyperlinkType Type
    {
      get
      {
        return this._type;
      }
    }

    public string GetCellRef()
    {
      return this._ctHyperlink.@ref;
    }

    public string Address
    {
      get
      {
        return this._location;
      }
      set
      {
        this.Validate(value);
        this._location = value;
        if (this._type != HyperlinkType.DOCUMENT)
          return;
        this.Location = value;
      }
    }

    private void Validate(string address)
    {
      switch (this._type)
      {
        case HyperlinkType.URL:
        case HyperlinkType.EMAIL:
        case HyperlinkType.FILE:
          try
          {
            Uri uri = new Uri(address, UriKind.RelativeOrAbsolute);
            break;
          }
          catch (UriFormatException ex)
          {
            throw new InvalidOperationException("Address of hyperlink must be a valid URI:" + address, (Exception) ex);
          }
      }
    }

    public string Label
    {
      get
      {
        return this._ctHyperlink.display;
      }
      set
      {
        this._ctHyperlink.display = value;
      }
    }

    public string Location
    {
      get
      {
        return this._ctHyperlink.location;
      }
      set
      {
        this._ctHyperlink.location = value;
      }
    }

    internal void SetCellReference(string ref1)
    {
      this._ctHyperlink.@ref = ref1;
    }

    private CellReference buildCellReference()
    {
      return new CellReference(this._ctHyperlink.@ref);
    }

    public int FirstColumn
    {
      get
      {
        return (int) this.buildCellReference().Col;
      }
      set
      {
        this._ctHyperlink.@ref = new CellReference(this.FirstRow, value).FormatAsString();
      }
    }

    public int LastColumn
    {
      get
      {
        return (int) this.buildCellReference().Col;
      }
      set
      {
        this.FirstColumn = value;
      }
    }

    public int FirstRow
    {
      get
      {
        return this.buildCellReference().Row;
      }
      set
      {
        this._ctHyperlink.@ref = new CellReference(value, this.FirstColumn).FormatAsString();
      }
    }

    public int LastRow
    {
      get
      {
        return this.buildCellReference().Row;
      }
      set
      {
        this.FirstRow = value;
      }
    }

    public string TextMark
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string Tooltip
    {
      get
      {
        return this._ctHyperlink.tooltip;
      }
      set
      {
        this._ctHyperlink.tooltip = value;
      }
    }
  }
}
