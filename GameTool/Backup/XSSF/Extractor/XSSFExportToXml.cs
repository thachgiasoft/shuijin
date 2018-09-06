// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Extractor.XSSFExportToXml
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.UserModel.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace NPOI.XSSF.Extractor
{
  public class XSSFExportToXml : IComparer<string>
  {
    private XSSFMap map;

    public XSSFExportToXml(XSSFMap map)
    {
      this.map = map;
    }

    public void ExportToXML(Stream os, bool validate)
    {
      this.ExportToXML(os, "UTF-8", validate);
    }

    private XmlDocument GetEmptyDocument()
    {
      return new XmlDocument();
    }

    public void ExportToXML(Stream os, string encoding, bool validate)
    {
      List<XSSFSingleXmlCell> relatedSingleXmlCell = this.map.GetRelatedSingleXMLCell();
      List<XSSFTable> relatedTables = this.map.GetRelatedTables();
      string rootElement = this.map.GetCTMap().RootElement;
      XmlDocument emptyDocument = this.GetEmptyDocument();
      XmlElement xmlElement = !this.IsNamespaceDeclared() ? emptyDocument.CreateElement(rootElement) : emptyDocument.CreateElement(rootElement, this.GetNamespace());
      emptyDocument.AppendChild((XmlNode) xmlElement);
      List<string> stringList = new List<string>();
      Dictionary<string, XSSFSingleXmlCell> dictionary1 = new Dictionary<string, XSSFSingleXmlCell>();
      Dictionary<string, XSSFTable> dictionary2 = new Dictionary<string, XSSFTable>();
      foreach (XSSFSingleXmlCell xssfSingleXmlCell in relatedSingleXmlCell)
      {
        stringList.Add(xssfSingleXmlCell.GetXpath());
        dictionary1[xssfSingleXmlCell.GetXpath()] = xssfSingleXmlCell;
      }
      foreach (XSSFTable xssfTable in relatedTables)
      {
        string commonXpath = xssfTable.GetCommonXpath();
        stringList.Add(commonXpath);
        dictionary2[commonXpath] = xssfTable;
      }
      stringList.Sort();
      foreach (string index in stringList)
      {
        XSSFSingleXmlCell xssfSingleXmlCell = !dictionary1.ContainsKey(index) ? (XSSFSingleXmlCell) null : dictionary1[index];
        XSSFTable xssfTable = !dictionary2.ContainsKey(index) ? (XSSFTable) null : dictionary2[index];
        if (!Regex.IsMatch(index, ".*\\[.*"))
        {
          if (xssfSingleXmlCell != null)
          {
            XSSFCell referencedCell = (XSSFCell) xssfSingleXmlCell.GetReferencedCell();
            if (referencedCell != null)
            {
              XmlNode nodeByXpath = this.GetNodeByXPath(index, emptyDocument.FirstChild, emptyDocument, false);
              ST_XmlDataType xmlDataType = xssfSingleXmlCell.GetXmlDataType();
              this.mapCellOnNode(referencedCell, nodeByXpath, xmlDataType);
            }
          }
          if (xssfTable != null)
          {
            List<XSSFXmlColumnPr> xmlColumnPrs = xssfTable.GetXmlColumnPrs();
            XSSFSheet xssfSheet = xssfTable.GetXSSFSheet();
            int num = xssfTable.GetStartCellReference().Row + 1;
            int row1 = xssfTable.GetEndCellReference().Row;
            for (int rownum = num; rownum <= row1; ++rownum)
            {
              XSSFRow row2 = (XSSFRow) xssfSheet.GetRow(rownum);
              XmlNode nodeByXpath1 = this.GetNodeByXPath(xssfTable.GetCommonXpath(), emptyDocument.FirstChild, emptyDocument, true);
              short col = xssfTable.GetStartCellReference().Col;
              for (int cellnum = (int) col; cellnum <= (int) xssfTable.GetEndCellReference().Col; ++cellnum)
              {
                XSSFCell cell = (XSSFCell) row2.GetCell(cellnum);
                if (cell != null)
                {
                  XSSFXmlColumnPr xssfXmlColumnPr = xmlColumnPrs[cellnum - (int) col];
                  XmlNode nodeByXpath2 = this.GetNodeByXPath(xssfXmlColumnPr.GetLocalXPath(), nodeByXpath1, emptyDocument, false);
                  ST_XmlDataType xmlDataType = xssfXmlColumnPr.GetXmlDataType();
                  this.mapCellOnNode(cell, nodeByXpath2, xmlDataType);
                }
              }
            }
          }
        }
      }
      bool flag = true;
      if (validate)
        flag = this.IsValid(emptyDocument);
      if (!flag)
        return;
      using (XmlWriter w = XmlWriter.Create(os, new XmlWriterSettings() { Indent = true, Encoding = Encoding.GetEncoding(encoding) }))
        emptyDocument.WriteTo(w);
    }

    private bool IsValid(XmlDocument xml)
    {
      return true;
    }

    private void mapCellOnNode(XSSFCell cell, XmlNode node, ST_XmlDataType outputDataType)
    {
      string str = "";
      switch (cell.CellType)
      {
        case CellType.NUMERIC:
          str += cell.GetRawValue();
          break;
        case CellType.STRING:
          str = cell.StringCellValue;
          break;
        case CellType.FORMULA:
          str = cell.StringCellValue;
          break;
        case CellType.BOOLEAN:
          str += (string) (object) cell.BooleanCellValue;
          break;
        case CellType.ERROR:
          str = cell.ErrorCellString;
          break;
      }
      if (node is XmlElement)
        node.InnerText = str;
      else
        node.Value = str;
    }

    private string RemoveNamespace(string elementName)
    {
      if (!Regex.IsMatch(elementName, ".*:.*"))
        return elementName;
      return elementName.Split(':')[1];
    }

    private XmlNode GetNodeByXPath(string xpath, XmlNode rootNode, XmlDocument doc, bool CreateMultipleInstances)
    {
      string[] strArray = xpath.Split('/');
      XmlNode currentNode = rootNode;
      for (int index = 2; index < strArray.Length; ++index)
      {
        string axisName = this.RemoveNamespace(strArray[index]);
        if (!axisName.StartsWith("@"))
        {
          XmlNodeList childNodes = currentNode.ChildNodes;
          XmlNode xmlNode = (XmlNode) null;
          if (!CreateMultipleInstances || index != strArray.Length - 1)
            xmlNode = this.selectNode(axisName, childNodes);
          if (xmlNode == null)
            xmlNode = this.CreateElement(doc, currentNode, axisName);
          currentNode = xmlNode;
        }
        else
          currentNode = this.CreateAttribute(doc, currentNode, axisName);
      }
      return currentNode;
    }

    private XmlNode CreateAttribute(XmlDocument doc, XmlNode currentNode, string axisName)
    {
      string name = axisName.Substring(1);
      XmlAttributeCollection attributes = currentNode.Attributes;
      XmlNode node = attributes.GetNamedItem(name);
      if (node == null)
      {
        node = (XmlNode) doc.CreateAttribute(name);
        attributes.SetNamedItem(node);
      }
      return node;
    }

    private XmlNode CreateElement(XmlDocument doc, XmlNode currentNode, string axisName)
    {
      XmlNode newChild = !this.IsNamespaceDeclared() ? (XmlNode) doc.CreateElement(axisName) : (XmlNode) doc.CreateElement(axisName, this.GetNamespace());
      currentNode.AppendChild(newChild);
      return newChild;
    }

    private XmlNode selectNode(string axisName, XmlNodeList list)
    {
      XmlNode xmlNode1 = (XmlNode) null;
      for (int index = 0; index < list.Count; ++index)
      {
        XmlNode xmlNode2 = list[index];
        if (xmlNode2.Name.Equals(axisName))
        {
          xmlNode1 = xmlNode2;
          break;
        }
      }
      return xmlNode1;
    }

    private bool IsNamespaceDeclared()
    {
      string str = this.GetNamespace();
      if (str != null)
        return !str.Equals("");
      return false;
    }

    private string GetNamespace()
    {
      return this.map.GetCTSchema().Namespace;
    }

    public int Compare(string leftXpath, string rightXpath)
    {
      int num1 = 0;
      XmlNode schema = this.map.GetSchema();
      string[] strArray1 = leftXpath.Split('/');
      string[] strArray2 = rightXpath.Split('/');
      int num2 = strArray1.Length < strArray2.Length ? strArray1.Length : strArray2.Length;
      XmlNode xmlNode = schema;
      for (int index = 1; index < num2; ++index)
      {
        string elementName1 = strArray1[index];
        string elementName2 = strArray2[index];
        if (elementName1.Equals(elementName2))
        {
          xmlNode = this.GetComplexTypeForElement(elementName1, schema, xmlNode);
        }
        else
        {
          int num3 = this.IndexOfElementInComplexType(elementName1, xmlNode);
          int num4 = this.IndexOfElementInComplexType(elementName2, xmlNode);
          if (num3 != -1 && num4 != -1)
          {
            if (num3 < num4)
              num1 = -1;
            if (num3 > num4)
              num1 = 1;
          }
        }
      }
      return num1;
    }

    private int IndexOfElementInComplexType(string elementName, XmlNode complexType)
    {
      XmlNodeList childNodes = complexType.ChildNodes;
      int num = -1;
      for (int index = 0; index < childNodes.Count; ++index)
      {
        XmlNode xmlNode = childNodes[index];
        if (xmlNode is XmlElement && xmlNode.LocalName.Equals("element") && xmlNode.Attributes.GetNamedItem("name").Value.Equals(this.RemoveNamespace(elementName)))
        {
          num = index;
          break;
        }
      }
      return num;
    }

    private XmlNode GetComplexTypeForElement(string elementName, XmlNode xmlSchema, XmlNode localComplexTypeRootNode)
    {
      XmlNode xmlNode1 = (XmlNode) null;
      string str1 = this.RemoveNamespace(elementName);
      XmlNodeList childNodes1 = localComplexTypeRootNode.ChildNodes;
      string str2 = "";
      for (int index = 0; index < childNodes1.Count; ++index)
      {
        XmlNode xmlNode2 = childNodes1[index];
        if (xmlNode2 is XmlElement && xmlNode2.LocalName.Equals("element") && xmlNode2.Attributes.GetNamedItem("name").Value.Equals(str1))
        {
          XmlNode namedItem = xmlNode2.Attributes.GetNamedItem("type");
          if (namedItem != null)
          {
            str2 = namedItem.Value;
            break;
          }
        }
      }
      if (!"".Equals(str2))
      {
        XmlNodeList childNodes2 = xmlSchema.ChildNodes;
        for (int index1 = 0; index1 < childNodes2.Count; ++index1)
        {
          XmlNode xmlNode2 = childNodes1[index1];
          if (xmlNode2 is XmlElement && xmlNode2.LocalName.Equals("complexType") && xmlNode2.Attributes.GetNamedItem("name").Value.Equals(str2))
          {
            XmlNodeList childNodes3 = xmlNode2.ChildNodes;
            for (int index2 = 0; index2 < childNodes3.Count; ++index2)
            {
              XmlNode xmlNode3 = childNodes3[index2];
              if (xmlNode3 is XmlElement && xmlNode3.LocalName.Equals("sequence"))
              {
                xmlNode1 = xmlNode3;
                break;
              }
            }
            if (xmlNode1 != null)
              break;
          }
        }
      }
      return xmlNode1;
    }
  }
}
