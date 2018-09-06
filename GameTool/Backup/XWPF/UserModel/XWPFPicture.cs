// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFPicture
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Picture;

namespace NPOI.XWPF.UserModel
{
  public class XWPFPicture
  {
    private CT_Picture ctPic;
    private string description;
    private XWPFRun run;

    public XWPFPicture(CT_Picture ctPic, XWPFRun Run)
    {
      this.run = Run;
      this.ctPic = ctPic;
      this.description = ctPic.nvPicPr.cNvPr.descr;
    }

    public void SetPictureReference(PackageRelationship rel)
    {
      this.ctPic.blipFill.blip.embed = rel.Id;
    }

    public CT_Picture GetCTPicture()
    {
      return this.ctPic;
    }

    public XWPFPictureData GetPictureData()
    {
      CT_BlipFillProperties blipFill = this.ctPic.blipFill;
      if (blipFill == null || !blipFill.IsSetBlip())
        return (XWPFPictureData) null;
      string embed = blipFill.blip.embed;
      POIXMLDocumentPart part = this.run.GetParagraph().GetPart();
      if (part != null)
      {
        POIXMLDocumentPart relationById = part.GetRelationById(embed);
        if (relationById is XWPFPictureData)
          return (XWPFPictureData) relationById;
      }
      return (XWPFPictureData) null;
    }

    public string GetDescription()
    {
      return this.description;
    }
  }
}
