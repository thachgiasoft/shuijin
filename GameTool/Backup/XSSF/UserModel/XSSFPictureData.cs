// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFPictureData
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI.XSSF.UserModel
{
  public class XSSFPictureData : POIXMLDocumentPart, IPictureData
  {
    internal static Dictionary<int, POIXMLRelation> RELATIONS = new Dictionary<int, POIXMLRelation>(8);

    static XSSFPictureData()
    {
      XSSFPictureData.RELATIONS[2] = (POIXMLRelation) XSSFRelation.IMAGE_EMF;
      XSSFPictureData.RELATIONS[3] = (POIXMLRelation) XSSFRelation.IMAGE_WMF;
      XSSFPictureData.RELATIONS[4] = (POIXMLRelation) XSSFRelation.IMAGE_PICT;
      XSSFPictureData.RELATIONS[5] = (POIXMLRelation) XSSFRelation.IMAGE_JPEG;
      XSSFPictureData.RELATIONS[6] = (POIXMLRelation) XSSFRelation.IMAGE_PNG;
      XSSFPictureData.RELATIONS[7] = (POIXMLRelation) XSSFRelation.IMAGE_DIB;
      XSSFPictureData.RELATIONS[XSSFWorkbook.PICTURE_TYPE_GIF] = (POIXMLRelation) XSSFRelation.IMAGE_GIF;
      XSSFPictureData.RELATIONS[XSSFWorkbook.PICTURE_TYPE_TIFF] = (POIXMLRelation) XSSFRelation.IMAGE_TIFF;
      XSSFPictureData.RELATIONS[XSSFWorkbook.PICTURE_TYPE_EPS] = (POIXMLRelation) XSSFRelation.IMAGE_EPS;
      XSSFPictureData.RELATIONS[XSSFWorkbook.PICTURE_TYPE_BMP] = (POIXMLRelation) XSSFRelation.IMAGE_BMP;
      XSSFPictureData.RELATIONS[XSSFWorkbook.PICTURE_TYPE_WPG] = (POIXMLRelation) XSSFRelation.IMAGE_WPG;
    }

    public XSSFPictureData()
    {
    }

    internal XSSFPictureData(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    public string SuggestFileExtension()
    {
      return this.GetPackagePart().PartName.Extension;
    }

    public int GetPictureType()
    {
      string contentType = this.GetPackagePart().ContentType;
      foreach (PictureType key in XSSFPictureData.RELATIONS.Keys)
      {
        if (XSSFPictureData.RELATIONS[(int) key].ContentType.Equals(contentType))
          return (int) key;
      }
      return 0;
    }

    public byte[] Data
    {
      get
      {
        try
        {
          return IOUtils.ToByteArray(this.GetPackagePart().GetInputStream());
        }
        catch (IOException ex)
        {
          throw new POIXMLException((Exception) ex);
        }
      }
    }

    public string MimeType
    {
      get
      {
        return this.GetPackagePart().ContentType;
      }
    }
  }
}
