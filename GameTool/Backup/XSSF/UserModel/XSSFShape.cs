// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFShape
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.UserModel;
using System;

namespace NPOI.XSSF.UserModel
{
  public abstract class XSSFShape : IShape
  {
    public static int EMU_PER_PIXEL = 9525;
    public static int EMU_PER_POINT = 12700;
    public static int POINT_DPI = 72;
    public static int PIXEL_DPI = 96;
    protected XSSFDrawing drawing;
    public XSSFShapeGroup parent;
    internal XSSFAnchor anchor;

    public XSSFDrawing GetDrawing()
    {
      return this.drawing;
    }

    public IShape Parent
    {
      get
      {
        return (IShape) this.parent;
      }
    }

    public XSSFAnchor GetAnchor()
    {
      return this.anchor;
    }

    protected internal abstract CT_ShapeProperties GetShapeProperties();

    public bool IsNoFill
    {
      get
      {
        return this.GetShapeProperties().noFill != null;
      }
      set
      {
        CT_ShapeProperties shapeProperties = this.GetShapeProperties();
        if (shapeProperties.IsSetPattFill())
          shapeProperties.unsetPattFill();
        if (shapeProperties.IsSetSolidFill())
          shapeProperties.unsetSolidFill();
        shapeProperties.noFill = new CT_NoFillProperties();
      }
    }

    public void SetFillColor(int red, int green, int blue)
    {
      CT_ShapeProperties shapeProperties = this.GetShapeProperties();
      (shapeProperties.IsSetSolidFill() ? shapeProperties.solidFill : shapeProperties.AddNewSolidFill()).srgbClr = new CT_SRgbColor()
      {
        val = new byte[3]
        {
          (byte) red,
          (byte) green,
          (byte) blue
        }
      };
    }

    public void SetLineStyleColor(int red, int green, int blue)
    {
      CT_ShapeProperties shapeProperties = this.GetShapeProperties();
      CT_LineProperties ctLineProperties = shapeProperties.IsSetLn() ? shapeProperties.ln : shapeProperties.AddNewLn();
      (ctLineProperties.IsSetSolidFill() ? ctLineProperties.solidFill : ctLineProperties.AddNewSolidFill()).srgbClr = new CT_SRgbColor()
      {
        val = new byte[3]
        {
          (byte) red,
          (byte) green,
          (byte) blue
        }
      };
    }

    public int CountOfAllChildren
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int FillColor
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

    public LineStyle LineStyle
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        CT_ShapeProperties shapeProperties = this.GetShapeProperties();
        (shapeProperties.IsSetLn() ? shapeProperties.ln : shapeProperties.AddNewLn()).prstDash = new CT_PresetLineDashProperties()
        {
          val = (ST_PresetLineDashVal) (value + 1)
        };
      }
    }

    public int LineStyleColor
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public double LineWidth
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        CT_ShapeProperties shapeProperties = this.GetShapeProperties();
        (shapeProperties.IsSetLn() ? shapeProperties.ln : shapeProperties.AddNewLn()).w = (int) (value * (double) XSSFShape.EMU_PER_POINT);
      }
    }

    public void SetLineStyleColor(int lineStyleColor)
    {
      throw new NotImplementedException();
    }
  }
}
