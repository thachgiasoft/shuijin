// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFPicture
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.Util;
using System;
using System.Drawing;
using System.IO;

namespace NPOI.XSSF.UserModel
{
  public class XSSFPicture : XSSFShape, NPOI.SS.UserModel.IPicture
  {
    private static POILogger logger = POILogFactory.GetLogger(typeof (XSSFPicture));
    private static float DEFAULT_COLUMN_WIDTH = 585f / 64f;
    private static CT_Picture prototype = (CT_Picture) null;
    private CT_Picture ctPicture;

    public XSSFPicture(XSSFDrawing drawing, CT_Picture ctPicture)
    {
      this.drawing = drawing;
      this.ctPicture = ctPicture;
    }

    internal static CT_Picture Prototype()
    {
      if (XSSFPicture.prototype == null)
      {
        CT_Picture ctPicture = new CT_Picture();
        CT_PictureNonVisual pictureNonVisual = ctPicture.AddNewNvPicPr();
        CT_NonVisualDrawingProps visualDrawingProps = pictureNonVisual.AddNewCNvPr();
        visualDrawingProps.id = 1U;
        visualDrawingProps.name = "Picture 1";
        visualDrawingProps.descr = "Picture";
        pictureNonVisual.AddNewCNvPicPr().AddNewPicLocks().noChangeAspect = true;
        CT_BlipFillProperties blipFillProperties = ctPicture.AddNewBlipFill();
        blipFillProperties.AddNewBlip().embed = "";
        blipFillProperties.AddNewStretch().AddNewFillRect();
        CT_ShapeProperties ctShapeProperties = ctPicture.AddNewSpPr();
        CT_Transform2D ctTransform2D = ctShapeProperties.AddNewXfrm();
        CT_PositiveSize2D ctPositiveSize2D = ctTransform2D.AddNewExt();
        ctPositiveSize2D.cx = 0L;
        ctPositiveSize2D.cy = 0L;
        CT_Point2D ctPoint2D = ctTransform2D.AddNewOff();
        ctPoint2D.x = 0L;
        ctPoint2D.y = 0L;
        CT_PresetGeometry2D presetGeometry2D = ctShapeProperties.AddNewPrstGeom();
        presetGeometry2D.prst = ST_ShapeType.rect;
        presetGeometry2D.AddNewAvLst();
        XSSFPicture.prototype = ctPicture;
      }
      return XSSFPicture.prototype;
    }

    internal void SetPictureReference(PackageRelationship rel)
    {
      this.ctPicture.blipFill.blip.embed = rel.Id;
    }

    internal CT_Picture GetCTPicture()
    {
      return this.ctPicture;
    }

    public void Resize()
    {
      this.Resize(1.0);
    }

    public void Resize(double scale)
    {
      IClientAnchor anchor = (IClientAnchor) this.GetAnchor();
      IClientAnchor preferredSize = this.GetPreferredSize(scale);
      int num1 = anchor.Row1 + (preferredSize.Row2 - preferredSize.Row1);
      int num2 = anchor.Col1 + (preferredSize.Col2 - preferredSize.Col1);
      anchor.Col2 = num2;
      anchor.Dx1 = 0;
      anchor.Dx2 = preferredSize.Dx2;
      anchor.Row2 = num1;
      anchor.Dy1 = 0;
      anchor.Dy2 = preferredSize.Dy2;
    }

    public IClientAnchor GetPreferredSize()
    {
      return this.GetPreferredSize(1.0);
    }

    public IClientAnchor GetPreferredSize(double scale)
    {
      XSSFClientAnchor anchor = (XSSFClientAnchor) this.GetAnchor();
      XSSFPictureData pictureData = (XSSFPictureData) this.PictureData;
      Size imageDimension = XSSFPicture.GetImageDimension(pictureData.GetPackagePart(), pictureData.GetPictureType());
      double num1 = (double) imageDimension.Width * scale;
      double num2 = (double) imageDimension.Height * scale;
      float num3 = 0.0f;
      int col1 = anchor.Col1;
      int num4 = 0;
      while (true)
      {
        num3 += this.GetColumnWidthInPixels(col1);
        if ((double) num3 <= num1)
          ++col1;
        else
          break;
      }
      if ((double) num3 > num1)
      {
        double columnWidthInPixels = (double) this.GetColumnWidthInPixels(col1);
        double num5 = (double) num3 - num1;
        num4 = (int) ((double) XSSFShape.EMU_PER_PIXEL * (columnWidthInPixels - num5));
      }
      anchor.Col2 = col1;
      anchor.Dx2 = num4;
      double num6 = 0.0;
      int row1 = anchor.Row1;
      int num7 = 0;
      while (true)
      {
        num6 += (double) this.GetRowHeightInPixels(row1);
        if (num6 <= num2)
          ++row1;
        else
          break;
      }
      if (num6 > num2)
      {
        double rowHeightInPixels = (double) this.GetRowHeightInPixels(row1);
        double num5 = num6 - num2;
        num7 = (int) ((double) XSSFShape.EMU_PER_PIXEL * (rowHeightInPixels - num5));
      }
      anchor.Row2 = row1;
      anchor.Dy2 = num7;
      CT_PositiveSize2D ext = this.ctPicture.spPr.xfrm.ext;
      ext.cx = (long) (num1 * (double) XSSFShape.EMU_PER_PIXEL);
      ext.cy = (long) (num2 * (double) XSSFShape.EMU_PER_PIXEL);
      return (IClientAnchor) anchor;
    }

    private float GetColumnWidthInPixels(int columnIndex)
    {
      CT_Col column = ((XSSFSheet) this.GetDrawing().GetParent()).GetColumnHelper().GetColumn((long) columnIndex, false);
      return (column == null || !column.IsSetWidth() ? XSSFPicture.DEFAULT_COLUMN_WIDTH : (float) column.width) * XSSFWorkbook.DEFAULT_CHARACTER_WIDTH;
    }

    private float GetRowHeightInPixels(int rowIndex)
    {
      XSSFSheet parent = (XSSFSheet) this.GetDrawing().GetParent();
      IRow row = parent.GetRow(rowIndex);
      return (row != null ? row.HeightInPoints : parent.DefaultRowHeightInPoints) * (float) XSSFShape.PIXEL_DPI / (float) XSSFShape.POINT_DPI;
    }

    protected static Size GetImageDimension(PackagePart part, int type)
    {
      try
      {
        return Image.FromStream(part.GetInputStream()).Size;
      }
      catch (IOException ex)
      {
        XSSFPicture.logger.Log(5, (Exception) ex);
        return new Size();
      }
    }

    protected internal override CT_ShapeProperties GetShapeProperties()
    {
      return this.ctPicture.spPr;
    }

    public new int CountOfAllChildren
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public new int FillColor
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

    public new LineStyle LineStyle
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        base.LineStyle = value;
      }
    }

    public new int LineStyleColor
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int LineWidth
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        this.LineWidth = (double) value;
      }
    }

    public new void SetLineStyleColor(int lineStyleColor)
    {
      throw new NotImplementedException();
    }

    public IPictureData PictureData
    {
      get
      {
        return (IPictureData) this.GetDrawing().GetRelationById(this.ctPicture.blipFill.blip.embed);
      }
    }
  }
}
