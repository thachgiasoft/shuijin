// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFFactory
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using System;
using System.Reflection;

namespace NPOI.XWPF.UserModel
{
  public class XWPFFactory : POIXMLFactory
  {
    private static POILogger logger = POILogFactory.GetLogger(typeof (XWPFFactory));
    private static XWPFFactory inst = new XWPFFactory();

    private XWPFFactory()
    {
    }

    public static XWPFFactory GetInstance()
    {
      return XWPFFactory.inst;
    }

    public override POIXMLDocumentPart CreateDocumentPart(POIXMLDocumentPart parent, PackageRelationship rel, PackagePart part)
    {
      POIXMLRelation instance = (POIXMLRelation) XWPFRelation.GetInstance(rel.RelationshipType);
      if (instance != null)
      {
        if (instance.RelationClass != null)
        {
          try
          {
            Type relationClass = instance.RelationClass;
            try
            {
              return relationClass.GetConstructor(new Type[3]{ typeof (POIXMLDocumentPart), typeof (PackagePart), typeof (PackageRelationship) }).Invoke(new object[3]{ (object) parent, (object) part, (object) rel }) as POIXMLDocumentPart;
            }
            catch (Exception ex)
            {
              return relationClass.GetConstructor(new Type[2]{ typeof (PackagePart), typeof (PackageRelationship) }).Invoke(new object[2]{ (object) part, (object) rel }) as POIXMLDocumentPart;
            }
          }
          catch (Exception ex)
          {
            throw new POIXMLException(ex);
          }
        }
      }
      XWPFFactory.logger.Log(1, (object) ("using default POIXMLDocumentPart for " + rel.RelationshipType));
      return new POIXMLDocumentPart(part, rel);
    }

    public override POIXMLDocumentPart CreateDocumentPart(POIXMLRelation descriptor)
    {
      try
      {
        return descriptor.RelationClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null).Invoke(new object[0]) as POIXMLDocumentPart;
      }
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }
  }
}
