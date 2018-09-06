// Decompiled with JetBrains decompiler
// Type: NPOI.Util.PackageHelper
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.Exceptions;
using NPOI.OpenXml4Net.OPC;
using System;
using System.IO;

namespace NPOI.Util
{
  public class PackageHelper
  {
    public static OPCPackage Open(Stream is1)
    {
      try
      {
        return OPCPackage.Open(is1);
      }
      catch (InvalidFormatException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    public static OPCPackage Clone(OPCPackage pkg, string path)
    {
      OPCPackage tgt = OPCPackage.Create(path);
      foreach (PackageRelationship relationship in pkg.Relationships)
      {
        PackagePart part1 = pkg.GetPart(relationship);
        if (relationship.RelationshipType.Equals("http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties"))
        {
          PackageHelper.CopyProperties(pkg.GetPackageProperties(), tgt.GetPackageProperties());
        }
        else
        {
          tgt.AddRelationship(part1.PartName, relationship.TargetMode.Value, relationship.RelationshipType);
          PackagePart part2 = tgt.CreatePart(part1.PartName, part1.ContentType);
          Stream outputStream = part2.GetOutputStream();
          IOUtils.Copy(part1.GetInputStream(), outputStream);
          outputStream.Close();
          if (part1.HasRelationships)
            PackageHelper.Copy(pkg, part1, tgt, part2);
        }
      }
      tgt.Close();
      return OPCPackage.Open(path);
    }

    public string CreateTempFile()
    {
      return TempFile.GetTempFilePath("poi-ooxml-", ".tmp");
    }

    private static void Copy(OPCPackage pkg, PackagePart part, OPCPackage tgt, PackagePart part_tgt)
    {
      PackageRelationshipCollection relationships = part.Relationships;
      if (relationships == null)
        return;
      foreach (PackageRelationship packageRelationship in relationships)
      {
        TargetMode? targetMode = packageRelationship.TargetMode;
        if ((targetMode.GetValueOrDefault() != TargetMode.External ? 0 : (targetMode.HasValue ? 1 : 0)) != 0)
        {
          part_tgt.AddExternalRelationship(packageRelationship.TargetUri.ToString(), packageRelationship.RelationshipType, packageRelationship.Id);
        }
        else
        {
          Uri targetUri = packageRelationship.TargetUri;
          if (targetUri.Fragment != null)
          {
            part_tgt.AddRelationship(targetUri, packageRelationship.TargetMode.Value, packageRelationship.RelationshipType, packageRelationship.Id);
          }
          else
          {
            PackagePartName partName = PackagingUriHelper.CreatePartName(packageRelationship.TargetUri);
            PackagePart part1 = pkg.GetPart(partName);
            part_tgt.AddRelationship(part1.PartName, packageRelationship.TargetMode.Value, packageRelationship.RelationshipType, packageRelationship.Id);
            if (!tgt.ContainPart(part1.PartName))
            {
              PackagePart part2 = tgt.CreatePart(part1.PartName, part1.ContentType);
              Stream outputStream = part2.GetOutputStream();
              IOUtils.Copy(part1.GetInputStream(), outputStream);
              outputStream.Close();
              PackageHelper.Copy(pkg, part1, tgt, part2);
            }
          }
        }
      }
    }

    private static void CopyProperties(PackageProperties src, PackageProperties tgt)
    {
      tgt.SetCategoryProperty(src.GetCategoryProperty());
      tgt.SetContentStatusProperty(src.GetContentStatusProperty());
      tgt.SetContentTypeProperty(src.GetContentTypeProperty());
      tgt.SetCreatorProperty(src.GetCreatorProperty());
      tgt.SetDescriptionProperty(src.GetDescriptionProperty());
      tgt.SetIdentifierProperty(src.GetIdentifierProperty());
      tgt.SetKeywordsProperty(src.GetKeywordsProperty());
      tgt.SetLanguageProperty(src.GetLanguageProperty());
      tgt.SetRevisionProperty(src.GetRevisionProperty());
      tgt.SetSubjectProperty(src.GetSubjectProperty());
      tgt.SetTitleProperty(src.GetTitleProperty());
      tgt.SetVersionProperty(src.GetVersionProperty());
    }
  }
}
