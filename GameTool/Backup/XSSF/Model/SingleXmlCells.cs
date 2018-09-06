// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.SingleXmlCells
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.UserModel.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class SingleXmlCells : POIXMLDocumentPart
  {
    private CT_SingleXmlCells SingleXMLCells;

    public SingleXmlCells()
    {
      this.SingleXMLCells = new CT_SingleXmlCells();
    }

    internal SingleXmlCells(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      try
      {
        this.SingleXMLCells = SingleXmlCellsDocument.Parse(is1).GetSingleXmlCells();
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    public XSSFSheet GetXSSFSheet()
    {
      return (XSSFSheet) this.GetParent();
    }

    protected void WriteTo(Stream out1)
    {
      SingleXmlCellsDocument xmlCellsDocument = new SingleXmlCellsDocument();
      xmlCellsDocument.SetSingleXmlCells(this.SingleXMLCells);
      xmlCellsDocument.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }

    public CT_SingleXmlCells GetCTSingleXMLCells()
    {
      return this.SingleXMLCells;
    }

    public List<XSSFSingleXmlCell> GetAllSimpleXmlCell()
    {
      List<XSSFSingleXmlCell> xssfSingleXmlCellList = new List<XSSFSingleXmlCell>();
      foreach (CT_SingleXmlCell SingleXmlCell in this.SingleXMLCells.singleXmlCell)
        xssfSingleXmlCellList.Add(new XSSFSingleXmlCell(SingleXmlCell, this));
      return xssfSingleXmlCellList;
    }
  }
}
