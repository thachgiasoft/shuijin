// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFTable : POIXMLDocumentPart
  {
    private CT_Table ctTable;
    private List<XSSFXmlColumnPr> xmlColumnPr;
    private CellReference startCellReference;
    private CellReference endCellReference;
    private string commonXPath;

    public XSSFTable()
    {
      this.ctTable = new CT_Table();
    }

    internal XSSFTable(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      try
      {
        this.ctTable = TableDocument.Parse(is1).GetTable();
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

    public void WriteTo(Stream out1)
    {
      this.UpdateHeaders();
      TableDocument tableDocument = new TableDocument();
      tableDocument.SetTable(this.ctTable);
      tableDocument.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }

    public CT_Table GetCTTable()
    {
      return this.ctTable;
    }

    public bool MapsTo(long id)
    {
      bool flag = false;
      foreach (XSSFXmlColumnPr xmlColumnPr in this.GetXmlColumnPrs())
      {
        if (xmlColumnPr.GetMapId() == id)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public string GetCommonXpath()
    {
      if (this.commonXPath == null)
      {
        Array arr = (Array) null;
        foreach (CT_TableColumn ctTableColumn in this.ctTable.tableColumns.tableColumn)
        {
          if (ctTableColumn.xmlColumnPr != null)
          {
            string[] strArray = ctTableColumn.xmlColumnPr.xpath.Split('/');
            if (arr == null)
            {
              arr = (Array) strArray;
            }
            else
            {
              int num = arr.Length > strArray.Length ? strArray.Length : arr.Length;
              for (int index = 0; index < num; ++index)
              {
                if (!arr.GetValue(index).Equals((object) strArray[index]))
                {
                  arr = Arrays.AsList(arr).GetRange(0, index).ToArray(typeof (string));
                  break;
                }
              }
            }
          }
        }
        this.commonXPath = "";
        for (int index = 1; index < arr.Length; ++index)
        {
          XSSFTable xssfTable = this;
          xssfTable.commonXPath = xssfTable.commonXPath + "/" + arr.GetValue(index);
        }
      }
      return this.commonXPath;
    }

    public List<XSSFXmlColumnPr> GetXmlColumnPrs()
    {
      if (this.xmlColumnPr == null)
      {
        this.xmlColumnPr = new List<XSSFXmlColumnPr>();
        foreach (CT_TableColumn ctTableColum in this.ctTable.tableColumns.tableColumn)
        {
          if (ctTableColum.xmlColumnPr != null)
            this.xmlColumnPr.Add(new XSSFXmlColumnPr(this, ctTableColum, ctTableColum.xmlColumnPr));
        }
      }
      return this.xmlColumnPr;
    }

    public string GetName()
    {
      return this.ctTable.name;
    }

    public void SetName(string name)
    {
      this.ctTable.name = name;
    }

    public string GetDisplayName()
    {
      return this.ctTable.displayName;
    }

    public void SetDisplayName(string name)
    {
      this.ctTable.displayName = name;
    }

    public long GetNumerOfMappedColumns()
    {
      return (long) this.ctTable.tableColumns.count;
    }

    public CellReference GetStartCellReference()
    {
      if (this.startCellReference == null)
      {
        string str = this.ctTable.@ref;
        if (str != null)
          this.startCellReference = new CellReference(str.Split(":".ToCharArray())[0]);
      }
      return this.startCellReference;
    }

    public CellReference GetEndCellReference()
    {
      if (this.endCellReference == null)
        this.endCellReference = new CellReference(this.ctTable.@ref.Split(':')[1]);
      return this.endCellReference;
    }

    public int GetRowCount()
    {
      CellReference startCellReference = this.GetStartCellReference();
      CellReference endCellReference = this.GetEndCellReference();
      int num = -1;
      if (startCellReference != null && endCellReference != null)
        num = endCellReference.Row - startCellReference.Row;
      return num;
    }

    public void UpdateHeaders()
    {
      XSSFSheet parent = (XSSFSheet) this.GetParent();
      CellReference startCellReference = this.GetStartCellReference();
      if (startCellReference == null)
        return;
      int row1 = startCellReference.Row;
      int col = (int) startCellReference.Col;
      XSSFRow row2 = parent.GetRow(row1) as XSSFRow;
      if (row2 == null)
        return;
      foreach (CT_TableColumn ctTableColumn in this.GetCTTable().tableColumns.tableColumn)
      {
        int cellnum = (int) ctTableColumn.id - 1 + col;
        XSSFCell cell = row2.GetCell(cellnum) as XSSFCell;
        if (cell != null)
          ctTableColumn.name = cell.StringCellValue;
      }
    }
  }
}
