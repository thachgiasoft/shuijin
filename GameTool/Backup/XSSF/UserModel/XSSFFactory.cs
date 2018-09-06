// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFFactory
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using System;
using System.Reflection;

namespace NPOI.XSSF.UserModel
{
  public class XSSFFactory : POIXMLFactory
  {
    private static POILogger logger = POILogFactory.GetLogger(typeof (XSSFFactory));
    private static XSSFFactory inst = new XSSFFactory();

    private XSSFFactory()
    {
    }

    public static XSSFFactory GetInstance()
    {
      return XSSFFactory.inst;
    }

    public override POIXMLDocumentPart CreateDocumentPart(POIXMLDocumentPart parent, PackageRelationship rel, PackagePart part)
    {
      POIXMLRelation instance = (POIXMLRelation) XSSFRelation.GetInstance(rel.RelationshipType);
      if (instance != null)
      {
        if (instance.RelationClass != null)
        {
          try
          {
            return (POIXMLDocumentPart) instance.RelationClass.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[2]{ typeof (PackagePart), typeof (PackageRelationship) }, (ParameterModifier[]) null).Invoke(new object[2]{ (object) part, (object) rel });
          }
          catch (Exception ex)
          {
            throw new POIXMLException(ex);
          }
        }
      }
      XSSFFactory.logger.Log(1, (object) ("using default POIXMLDocumentPart for " + rel.RelationshipType));
      return new POIXMLDocumentPart(part, rel);
    }

    public override POIXMLDocumentPart CreateDocumentPart(POIXMLRelation descriptor)
    {
      try
      {
        Type relationClass = descriptor.RelationClass;
        Console.WriteLine(relationClass.ToString());
        return (POIXMLDocumentPart) relationClass.GetConstructor(new Type[0]).Invoke(new object[0]);
      }
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }
  }
}
