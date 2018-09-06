// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFSimpleShape
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace NPOI.XSSF.UserModel
{
  public class XSSFSimpleShape : XSSFShape
  {
    private static CT_Shape prototype;
    private CT_Shape ctShape;

    public XSSFSimpleShape(XSSFDrawing drawing, CT_Shape ctShape)
    {
      this.drawing = drawing;
      this.ctShape = ctShape;
    }

    internal static CT_Shape Prototype()
    {
      if (XSSFSimpleShape.prototype == null)
      {
        CT_Shape ctShape = new CT_Shape();
        CT_ShapeNonVisual ctShapeNonVisual = ctShape.AddNewNvSpPr();
        CT_NonVisualDrawingProps visualDrawingProps = ctShapeNonVisual.AddNewCNvPr();
        visualDrawingProps.id = 1U;
        visualDrawingProps.name = "Shape 1";
        ctShapeNonVisual.AddNewCNvSpPr();
        CT_ShapeProperties ctShapeProperties = ctShape.AddNewSpPr();
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
        CT_ShapeStyle ctShapeStyle = ctShape.AddNewStyle();
        CT_SchemeColor ctSchemeColor = ctShapeStyle.AddNewLnRef().AddNewSchemeClr();
        ctSchemeColor.val = ST_SchemeColorVal.accent1;
        ctSchemeColor.AddNewShade().val = 50000;
        ctShapeStyle.lnRef.idx = 2U;
        CT_StyleMatrixReference styleMatrixReference1 = ctShapeStyle.AddNewFillRef();
        styleMatrixReference1.idx = 1U;
        styleMatrixReference1.AddNewSchemeClr().val = ST_SchemeColorVal.accent1;
        CT_StyleMatrixReference styleMatrixReference2 = ctShapeStyle.AddNewEffectRef();
        styleMatrixReference2.idx = 0U;
        styleMatrixReference2.AddNewSchemeClr().val = ST_SchemeColorVal.accent1;
        CT_FontReference ctFontReference = ctShapeStyle.AddNewFontRef();
        ctFontReference.idx = ST_FontCollectionIndex.minor;
        ctFontReference.AddNewSchemeClr().val = ST_SchemeColorVal.lt1;
        CT_TextBody ctTextBody = ctShape.AddNewTxBody();
        CT_TextBodyProperties textBodyProperties = ctTextBody.AddNewBodyPr();
        textBodyProperties.anchor = ST_TextAnchoringType.ctr;
        textBodyProperties.rtlCol = false;
        CT_TextParagraph ctTextParagraph = ctTextBody.AddNewP();
        ctTextParagraph.AddNewPPr().algn = ST_TextAlignType.ctr;
        CT_TextCharacterProperties characterProperties = ctTextParagraph.AddNewEndParaRPr();
        characterProperties.lang = "en-US";
        characterProperties.sz = 1100;
        ctTextBody.AddNewLstStyle();
        XSSFSimpleShape.prototype = ctShape;
      }
      return XSSFSimpleShape.prototype;
    }

    public CT_Shape GetCTShape()
    {
      return this.ctShape;
    }

    public int GetShapeType()
    {
      return (int) this.ctShape.spPr.prstGeom.prst;
    }

    public void SetShapeType(int type)
    {
      this.ctShape.spPr.prstGeom.prst = (ST_ShapeType) type;
    }

    protected internal override CT_ShapeProperties GetShapeProperties()
    {
      return this.ctShape.spPr;
    }

    public void SetText(XSSFRichTextString str)
    {
      XSSFWorkbook parent = (XSSFWorkbook) this.GetDrawing().GetParent().GetParent();
      str.SetStylesTableReference(parent.GetStylesSource());
      CT_TextParagraph ctTextParagraph = new CT_TextParagraph();
      if (str.NumFormattingRuns == 0)
      {
        CT_RegularTextRun ctRegularTextRun = ctTextParagraph.AddNewR();
        CT_TextCharacterProperties characterProperties = ctRegularTextRun.AddNewRPr();
        characterProperties.lang = "en-US";
        characterProperties.sz = 1100;
        ctRegularTextRun.t = str.String;
      }
      else
      {
        for (int index = 0; index < str.GetCTRst().sizeOfRArray(); ++index)
        {
          CT_RElt rarray = str.GetCTRst().GetRArray(index);
          CT_RPrElt pr = rarray.rPr ?? rarray.AddNewRPr();
          CT_RegularTextRun ctRegularTextRun = ctTextParagraph.AddNewR();
          CT_TextCharacterProperties rPr = ctRegularTextRun.AddNewRPr();
          rPr.lang = "en-US";
          XSSFSimpleShape.ApplyAttributes(pr, rPr);
          ctRegularTextRun.t = rarray.t;
        }
      }
      this.ctShape.txBody.SetPArray(new CT_TextParagraph[1]
      {
        ctTextParagraph
      });
    }

    private static void ApplyAttributes(CT_RPrElt pr, CT_TextCharacterProperties rPr)
    {
      if (pr.sizeOfBArray() > 0)
        rPr.b = pr.GetBArray(0).val;
      if (pr.sizeOfUArray() > 0)
      {
        switch (pr.GetUArray(0).val)
        {
          case ST_UnderlineValues.none:
            rPr.u = ST_TextUnderlineType.none;
            break;
          case ST_UnderlineValues.single:
            rPr.u = ST_TextUnderlineType.sng;
            break;
          case ST_UnderlineValues.@double:
            rPr.u = ST_TextUnderlineType.dbl;
            break;
        }
      }
      if (pr.sizeOfIArray() > 0)
        rPr.i = pr.GetIArray(0).val;
      if (pr.sizeOfFamilyArray() > 0)
        rPr.AddNewLatin().typeface = pr.GetRFontArray(0).val;
      if (pr.sizeOfSzArray() > 0)
      {
        int num = (int) (pr.GetSzArray(0).val * 100.0);
        rPr.sz = num;
      }
      if (pr.sizeOfColorArray() <= 0)
        return;
      CT_SolidColorFillProperties colorFillProperties = rPr.IsSetSolidFill() ? rPr.solidFill : rPr.AddNewSolidFill();
      NPOI.OpenXmlFormats.Spreadsheet.CT_Color colorArray = pr.GetColorArray(0);
      if (colorArray.IsSetRgb())
      {
        (colorFillProperties.IsSetSrgbClr() ? colorFillProperties.srgbClr : colorFillProperties.AddNewSrgbClr()).val = colorArray.rgb;
      }
      else
      {
        if (!colorArray.IsSetIndexed())
          return;
        HSSFColor hssfColor = HSSFColor.GetIndexHash()[(object) (int) colorArray.indexed] as HSSFColor;
        if (hssfColor == null)
          return;
        byte[] numArray = new byte[3]{ (byte) hssfColor.GetTriplet()[0], (byte) hssfColor.GetTriplet()[1], (byte) hssfColor.GetTriplet()[2] };
        (colorFillProperties.IsSetSrgbClr() ? colorFillProperties.srgbClr : colorFillProperties.AddNewSrgbClr()).val = numArray;
      }
    }
  }
}
