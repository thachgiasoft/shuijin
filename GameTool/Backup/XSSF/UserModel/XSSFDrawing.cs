// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDrawing
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDrawing : POIXMLDocumentPart, IDrawing
  {
    private CT_Drawing drawing = XSSFDrawing.NewDrawing();
    public const string NAMESPACE_A = "http://schemas.openxmlformats.org/drawingml/2006/main";
    public const string NAMESPACE_C = "http://schemas.openxmlformats.org/drawingml/2006/chart";
    private long numOfGraphicFrames;

    public XSSFDrawing()
    {
      this.drawing = XSSFDrawing.NewDrawing();
    }

    internal XSSFDrawing(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.drawing = CT_Drawing.Parse(part.GetInputStream());
    }

    private static CT_Drawing NewDrawing()
    {
      return new CT_Drawing();
    }

    public CT_Drawing GetCTDrawing()
    {
      return this.drawing;
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.drawing.Save(outputStream);
      outputStream.Close();
    }

    public IClientAnchor CreateAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
    {
      return (IClientAnchor) new XSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
    }

    public ITextbox CreateTextbox(IClientAnchor anchor)
    {
      long num = this.newShapeId();
      NPOI.OpenXmlFormats.Dml.Spreadsheet.CT_Shape ctShape = this.CreateTwoCellAnchor(anchor).AddNewSp();
      ctShape.Set(XSSFSimpleShape.Prototype());
      ctShape.nvSpPr.cNvPr.id = (uint) num;
      XSSFTextBox xssfTextBox = new XSSFTextBox(this, ctShape);
      xssfTextBox.anchor = (XSSFAnchor) anchor;
      return (ITextbox) xssfTextBox;
    }

    public IPicture CreatePicture(XSSFClientAnchor anchor, int pictureIndex)
    {
      PackageRelationship rel = this.AddPictureReference(pictureIndex);
      long num = this.newShapeId();
      CT_Picture ctPicture = this.CreateTwoCellAnchor((IClientAnchor) anchor).AddNewPic();
      ctPicture.Set(XSSFPicture.Prototype());
      ctPicture.nvPicPr.cNvPr.id = (uint) num;
      XSSFPicture xssfPicture = new XSSFPicture(this, ctPicture);
      xssfPicture.anchor = (XSSFAnchor) anchor;
      xssfPicture.SetPictureReference(rel);
      return (IPicture) xssfPicture;
    }

    public IPicture CreatePicture(IClientAnchor anchor, int pictureIndex)
    {
      return this.CreatePicture((XSSFClientAnchor) anchor, pictureIndex);
    }

    public IChart CreateChart(IClientAnchor anchor)
    {
      int idx = this.GetPackagePart().Package.GetPartsByContentType(XSSFRelation.CHART.ContentType).Count + 1;
      XSSFChart relationship = (XSSFChart) this.CreateRelationship((POIXMLRelation) XSSFRelation.CHART, (POIXMLFactory) XSSFFactory.GetInstance(), idx);
      string id = relationship.GetPackageRelationship().Id;
      this.CreateGraphicFrame((XSSFClientAnchor) anchor).SetChart(relationship, id);
      return (IChart) relationship;
    }

    internal PackageRelationship AddPictureReference(int pictureIndex)
    {
      XSSFPictureData allPicture = (XSSFPictureData) ((XSSFWorkbook) this.GetParent().GetParent()).GetAllPictures()[pictureIndex];
      PackageRelationship rel = this.GetPackagePart().AddRelationship(allPicture.GetPackagePart().PartName, TargetMode.Internal, XSSFRelation.IMAGES.Relation);
      this.AddRelation(rel.Id, (POIXMLDocumentPart) new XSSFPictureData(allPicture.GetPackagePart(), rel));
      return rel;
    }

    public XSSFSimpleShape CreateSimpleShape(XSSFClientAnchor anchor)
    {
      long num = this.newShapeId();
      NPOI.OpenXmlFormats.Dml.Spreadsheet.CT_Shape ctShape = this.CreateTwoCellAnchor((IClientAnchor) anchor).AddNewSp();
      ctShape.Set(XSSFSimpleShape.Prototype());
      ctShape.nvSpPr.cNvPr.id = (uint) num;
      XSSFSimpleShape xssfSimpleShape = new XSSFSimpleShape(this, ctShape);
      xssfSimpleShape.anchor = (XSSFAnchor) anchor;
      return xssfSimpleShape;
    }

    public XSSFConnector CreateConnector(XSSFClientAnchor anchor)
    {
      CT_Connector ctShape = this.CreateTwoCellAnchor((IClientAnchor) anchor).AddNewCxnSp();
      ctShape.Set(XSSFConnector.Prototype());
      XSSFConnector xssfConnector = new XSSFConnector(this, ctShape);
      xssfConnector.anchor = (XSSFAnchor) anchor;
      return xssfConnector;
    }

    public XSSFShapeGroup CreateGroup(XSSFClientAnchor anchor)
    {
      CT_GroupShape ctGroup = this.CreateTwoCellAnchor((IClientAnchor) anchor).AddNewGrpSp();
      ctGroup.Set(XSSFShapeGroup.Prototype());
      XSSFShapeGroup xssfShapeGroup = new XSSFShapeGroup(this, ctGroup);
      xssfShapeGroup.anchor = (XSSFAnchor) anchor;
      return xssfShapeGroup;
    }

    public IComment CreateCellComment(IClientAnchor anchor)
    {
      XSSFClientAnchor xssfClientAnchor = (XSSFClientAnchor) anchor;
      XSSFSheet parent = (XSSFSheet) this.GetParent();
      CommentsTable commentsTable = parent.GetCommentsTable(true);
      NPOI.OpenXmlFormats.Vml.CT_Shape vmlShape = parent.GetVMLDrawing(true).newCommentShape();
      if (xssfClientAnchor.IsSet())
      {
        string str = xssfClientAnchor.Col1.ToString() + ", 0, " + (object) xssfClientAnchor.Row1 + ", 0, " + (object) xssfClientAnchor.Col2 + ", 0, " + (object) xssfClientAnchor.Row2 + ", 0";
        vmlShape.GetClientDataArray(0).SetAnchorArray(0, str);
      }
      return (IComment) new XSSFComment(commentsTable, commentsTable.CreateComment(), vmlShape) { Column = xssfClientAnchor.Col1, Row = xssfClientAnchor.Row1 };
    }

    private XSSFGraphicFrame CreateGraphicFrame(XSSFClientAnchor anchor)
    {
      CT_GraphicalObjectFrame ctGraphicFrame = this.CreateTwoCellAnchor((IClientAnchor) anchor).AddNewGraphicFrame();
      ctGraphicFrame.Set(XSSFGraphicFrame.Prototype());
      long num = this.numOfGraphicFrames++;
      return new XSSFGraphicFrame(this, ctGraphicFrame) { Anchor = anchor, Id = num, Name = "Diagramm" + (object) num };
    }

    public List<XSSFChart> GetCharts()
    {
      List<XSSFChart> xssfChartList = new List<XSSFChart>();
      foreach (POIXMLDocumentPart relation in this.GetRelations())
      {
        if (relation is XSSFChart)
          xssfChartList.Add((XSSFChart) relation);
      }
      return xssfChartList;
    }

    private CT_TwoCellAnchor CreateTwoCellAnchor(IClientAnchor anchor)
    {
      CT_TwoCellAnchor ctTwoCellAnchor = this.drawing.AddNewTwoCellAnchor();
      XSSFClientAnchor xssfClientAnchor = (XSSFClientAnchor) anchor;
      ctTwoCellAnchor.from = xssfClientAnchor.GetFrom();
      ctTwoCellAnchor.to = xssfClientAnchor.GetTo();
      ctTwoCellAnchor.AddNewClientData();
      xssfClientAnchor.SetTo(ctTwoCellAnchor.to);
      xssfClientAnchor.SetFrom(ctTwoCellAnchor.from);
      NPOI.OpenXmlFormats.Dml.Spreadsheet.ST_EditAs stEditAs;
      switch (anchor.AnchorType)
      {
        case 0:
          stEditAs = NPOI.OpenXmlFormats.Dml.Spreadsheet.ST_EditAs.twoCell;
          break;
        case 2:
          stEditAs = NPOI.OpenXmlFormats.Dml.Spreadsheet.ST_EditAs.oneCell;
          break;
        case 3:
          stEditAs = NPOI.OpenXmlFormats.Dml.Spreadsheet.ST_EditAs.absolute;
          break;
        default:
          stEditAs = NPOI.OpenXmlFormats.Dml.Spreadsheet.ST_EditAs.oneCell;
          break;
      }
      ctTwoCellAnchor.editAs = stEditAs;
      ctTwoCellAnchor.editAsSpecified = true;
      return ctTwoCellAnchor;
    }

    private long newShapeId()
    {
      return (long) (this.drawing.SizeOfTwoCellAnchorArray() + 1);
    }

    public bool ContainsChart()
    {
      throw new NotImplementedException();
    }

    public List<XSSFShape> GetShapes()
    {
      return new List<XSSFShape>();
    }

    private XSSFAnchor GetAnchorFromParent(object obj)
    {
      return (XSSFAnchor) null;
    }
  }
}
