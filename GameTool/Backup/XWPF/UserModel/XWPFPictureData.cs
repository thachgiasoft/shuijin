// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFPictureData
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using System;
using System.IO;

namespace NPOI.XWPF.UserModel
{
  public class XWPFPictureData : POIXMLDocumentPart
  {
    private long? checksum = new long?();
    internal static POIXMLRelation[] RELATIONS = new POIXMLRelation[13];

    static XWPFPictureData()
    {
      XWPFPictureData.RELATIONS[2] = (POIXMLRelation) XWPFRelation.IMAGE_EMF;
      XWPFPictureData.RELATIONS[3] = (POIXMLRelation) XWPFRelation.IMAGE_WMF;
      XWPFPictureData.RELATIONS[4] = (POIXMLRelation) XWPFRelation.IMAGE_PICT;
      XWPFPictureData.RELATIONS[5] = (POIXMLRelation) XWPFRelation.IMAGE_JPEG;
      XWPFPictureData.RELATIONS[6] = (POIXMLRelation) XWPFRelation.IMAGE_PNG;
      XWPFPictureData.RELATIONS[7] = (POIXMLRelation) XWPFRelation.IMAGE_DIB;
      XWPFPictureData.RELATIONS[8] = (POIXMLRelation) XWPFRelation.IMAGE_GIF;
      XWPFPictureData.RELATIONS[9] = (POIXMLRelation) XWPFRelation.IMAGE_TIFF;
      XWPFPictureData.RELATIONS[10] = (POIXMLRelation) XWPFRelation.IMAGE_EPS;
      XWPFPictureData.RELATIONS[11] = (POIXMLRelation) XWPFRelation.IMAGE_BMP;
      XWPFPictureData.RELATIONS[12] = (POIXMLRelation) XWPFRelation.IMAGE_WPG;
    }

    protected XWPFPictureData()
    {
    }

    public XWPFPictureData(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    internal override void OnDocumentRead()
    {
      base.OnDocumentRead();
    }

    public byte[] GetData()
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

    public string GetFileName()
    {
      string name = this.GetPackagePart().PartName.Name;
      return name?.Substring(name.LastIndexOf('/') + 1);
    }

    public string suggestFileExtension()
    {
      return this.GetPackagePart().PartName.Extension;
    }

    public int GetPictureType()
    {
      string contentType = this.GetPackagePart().ContentType;
      for (int index = 0; index < XWPFPictureData.RELATIONS.Length; ++index)
      {
        if (XWPFPictureData.RELATIONS[index] != null && XWPFPictureData.RELATIONS[index].ContentType.Equals(contentType))
          return index;
      }
      return 0;
    }

    public long Checksum
    {
      get
      {
        if (!this.checksum.HasValue)
        {
          Stream stream = (Stream) null;
          byte[] byteArray;
          try
          {
            stream = this.GetPackagePart().GetInputStream();
            byteArray = IOUtils.ToByteArray(stream);
          }
          catch (IOException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
          finally
          {
            try
            {
              stream.Close();
            }
            catch (IOException ex)
            {
              throw new POIXMLException((Exception) ex);
            }
          }
          this.checksum = new long?(IOUtils.CalculateChecksum(byteArray));
        }
        return this.checksum.Value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == this)
        return true;
      if (obj == null || !(obj is XWPFPictureData))
        return false;
      XWPFPictureData xwpfPictureData = (XWPFPictureData) obj;
      PackagePart packagePart1 = xwpfPictureData.GetPackagePart();
      PackagePart packagePart2 = this.GetPackagePart();
      if (packagePart1 != null && packagePart2 == null || packagePart1 == null && packagePart2 != null)
        return false;
      if (packagePart2 != null)
      {
        OPCPackage package1 = packagePart1.Package;
        OPCPackage package2 = packagePart2.Package;
        if (package1 != null && package2 == null || package1 == null && package2 != null || package2 != null && !package2.Equals((object) package1))
          return false;
      }
      if (!this.Checksum.Equals(xwpfPictureData.Checksum))
        return false;
      return Arrays.Equals((object) this.GetData(), (object) xwpfPictureData.GetData());
    }

    public override int GetHashCode()
    {
      return this.Checksum.GetHashCode();
    }
  }
}
