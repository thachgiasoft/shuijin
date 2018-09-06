// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFVMLDrawing
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Vml.Office;
using NPOI.OpenXmlFormats.Vml.Spreadsheet;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFVMLDrawing : POIXMLDocumentPart
  {
    private static XmlQualifiedName QNAME_SHAPE_LAYOUT = new XmlQualifiedName("shapelayout", "urn:schemas-microsoft-com:office:office");
    private static XmlQualifiedName QNAME_SHAPE_TYPE = new XmlQualifiedName("shapetype", "urn:schemas-microsoft-com:vml");
    private static XmlQualifiedName QNAME_SHAPE = new XmlQualifiedName("shape", "urn:schemas-microsoft-com:vml");
    private static Regex ptrn_shapeId = new Regex("_x0000_s(\\d+)");
    private ArrayList _items = new ArrayList();
    private int _shapeId = 1024;
    private string _shapeTypeId;

    public XSSFVMLDrawing()
    {
      this.newDrawing();
    }

    protected XSSFVMLDrawing(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.Read(this.GetPackagePart().GetInputStream());
    }

    internal void Read(Stream is1)
    {
      XmlDocument xmlDocument = new XmlDocument();
      string end = new StreamReader(is1).ReadToEnd();
      xmlDocument.LoadXml(end.Replace("<br>", ""));
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
      nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
      nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
      nsmgr.AddNamespace("v", "urn:schemas-microsoft-com:vml");
      this._items = new ArrayList();
      foreach (XmlNode selectNode in xmlDocument.SelectNodes("/xml/*", nsmgr))
      {
        string outerXml = selectNode.OuterXml;
        if (selectNode.LocalName == XSSFVMLDrawing.QNAME_SHAPE_LAYOUT.Name)
          this._items.Add((object) CT_ShapeLayout.Parse(outerXml));
        else if (selectNode.LocalName == XSSFVMLDrawing.QNAME_SHAPE_TYPE.Name)
        {
          CT_Shapetype ctShapetype = CT_Shapetype.Parse(outerXml);
          this._shapeTypeId = ctShapetype.id;
          this._items.Add((object) ctShapetype);
        }
        else if (selectNode.LocalName == XSSFVMLDrawing.QNAME_SHAPE.Name)
        {
          CT_Shape ctShape = CT_Shape.Parse(outerXml);
          string id = ctShape.id;
          if (id != null)
          {
            MatchCollection matchCollection = XSSFVMLDrawing.ptrn_shapeId.Matches(id);
            if (matchCollection.Count > 0)
              this._shapeId = Math.Max(this._shapeId, int.Parse(matchCollection[0].Groups[1].Value));
          }
          this._items.Add((object) ctShape);
        }
        else
          this._items.Add((object) selectNode);
      }
    }

    internal ArrayList GetItems()
    {
      return this._items;
    }

    internal void Write(Stream out1)
    {
      XmlWriter xmlWriter = XmlWriter.Create(out1);
      xmlWriter.WriteStartElement("xml");
      xmlWriter.WriteAttributeString("xmlns", "v", (string) null, "urn:schemas-microsoft-com:vml");
      xmlWriter.WriteAttributeString("xmlns", "o", (string) null, "urn:schemas-microsoft-com:office:office");
      xmlWriter.WriteAttributeString("xmlns", "x", (string) null, "urn:schemas-microsoft-com:office:excel");
      for (int index = 0; index < this._items.Count; ++index)
      {
        object obj = this._items[index];
        if (obj is XmlNode)
          xmlWriter.WriteRaw(((XmlNode) obj).OuterXml.Replace(" xmlns:v=\"urn:schemas-microsoft-com:vml\"", "").Replace(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"", "").Replace(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"", ""));
        else
          xmlWriter.WriteRaw(obj.ToString().Replace(" xmlns:v=\"urn:schemas-microsoft-com:vml\"", "").Replace(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"", "").Replace(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"", ""));
      }
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.Write(outputStream);
      outputStream.Close();
    }

    private void newDrawing()
    {
      CT_ShapeLayout ctShapeLayout = new CT_ShapeLayout();
      ctShapeLayout.ext = ST_Ext.edit;
      CT_IdMap ctIdMap = ctShapeLayout.AddNewIdmap();
      ctIdMap.ext = ST_Ext.edit;
      ctIdMap.data = "1";
      this._items.Add((object) ctShapeLayout);
      CT_Shapetype ctShapetype = new CT_Shapetype();
      this._shapeTypeId = "_xssf_cell_comment";
      ctShapetype.id = this._shapeTypeId;
      ctShapetype.coordsize = "21600,21600";
      ctShapetype.spt = 202f;
      ctShapetype.path2 = "m,l,21600r21600,l21600,xe";
      ctShapetype.AddNewStroke().joinstyle = ST_StrokeJoinStyle.miter;
      CT_Path ctPath = ctShapetype.AddNewPath();
      ctPath.gradientshapeok = ST_TrueFalse.t;
      ctPath.connecttype = ST_ConnectType.rect;
      this._items.Add((object) ctShapetype);
    }

    internal CT_Shape newCommentShape()
    {
      CT_Shape ctShape = new CT_Shape();
      ctShape.id = "_x0000_s" + (object) ++this._shapeId;
      ctShape.type = "#" + this._shapeTypeId;
      ctShape.style = "position:absolute; visibility:hidden";
      ctShape.fillcolor = "#ffffe1";
      ctShape.insetmode = ST_InsetMode.auto;
      ctShape.AddNewFill().color = "#ffffe1";
      CT_Shadow ctShadow = ctShape.AddNewShadow();
      ctShadow.on = ST_TrueFalse.t;
      ctShadow.color = "black";
      ctShadow.obscured = ST_TrueFalse.t;
      ctShape.AddNewPath().connecttype = ST_ConnectType.none;
      ctShape.AddNewTextbox().style = "mso-direction-alt:auto";
      CT_ClientData ctClientData = ctShape.AddNewClientData();
      ctClientData.ObjectType = ST_ObjectType.Note;
      ctClientData.AddNewMoveWithCells();
      ctClientData.AddNewSizeWithCells();
      ctClientData.AddNewAnchor("1, 15, 0, 2, 3, 15, 3, 16");
      ctClientData.AddNewAutoFill(ST_TrueFalseBlank.@false);
      ctClientData.AddNewRow(0);
      ctClientData.AddNewColumn(0);
      this._items.Add((object) ctShape);
      return ctShape;
    }

    internal CT_Shape FindCommentShape(int row, int col)
    {
      foreach (object obj in this._items)
      {
        if (obj is CT_Shape)
        {
          CT_Shape ctShape = (CT_Shape) obj;
          if (ctShape.sizeOfClientDataArray() > 0)
          {
            CT_ClientData clientDataArray = ctShape.GetClientDataArray(0);
            if (clientDataArray.ObjectType == ST_ObjectType.Note)
            {
              int rowArray = clientDataArray.GetRowArray(0);
              int columnArray = clientDataArray.GetColumnArray(0);
              if (rowArray == row && columnArray == col)
                return ctShape;
            }
          }
        }
      }
      return (CT_Shape) null;
    }

    internal bool RemoveCommentShape(int row, int col)
    {
      CT_Shape commentShape = this.FindCommentShape(row, col);
      if (commentShape == null)
        return false;
      this._items.Remove((object) commentShape);
      return true;
    }
  }
}
