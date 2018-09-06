// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
//    <NameSpace>schemas</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NPOI.OpenXmlFormats.Spreadsheet
{
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_Metadata
    {

        private CT_MetadataTypes metadataTypesField;

        private CT_MetadataStrings metadataStringsField;

        private CT_MdxMetadata mdxMetadataField;

        private List<CT_FutureMetadata> futureMetadataField;

        private CT_MetadataBlocks cellMetadataField;

        private CT_MetadataBlocks valueMetadataField;

        private CT_ExtensionList extLstField;

        public CT_Metadata()
        {
            this.extLstField = new CT_ExtensionList();
            this.valueMetadataField = new CT_MetadataBlocks();
            this.cellMetadataField = new CT_MetadataBlocks();
            this.futureMetadataField = new List<CT_FutureMetadata>();
            this.mdxMetadataField = new CT_MdxMetadata();
            this.metadataStringsField = new CT_MetadataStrings();
            this.metadataTypesField = new CT_MetadataTypes();
        }

        public CT_MetadataTypes metadataTypes
        {
            get
            {
                return this.metadataTypesField;
            }
            set
            {
                this.metadataTypesField = value;
            }
        }

        public CT_MetadataStrings metadataStrings
        {
            get
            {
                return this.metadataStringsField;
            }
            set
            {
                this.metadataStringsField = value;
            }
        }

        public CT_MdxMetadata mdxMetadata
        {
            get
            {
                return this.mdxMetadataField;
            }
            set
            {
                this.mdxMetadataField = value;
            }
        }

        public List<CT_FutureMetadata> futureMetadata
        {
            get
            {
                return this.futureMetadataField;
            }
            set
            {
                this.futureMetadataField = value;
            }
        }

        public CT_MetadataBlocks cellMetadata
        {
            get
            {
                return this.cellMetadataField;
            }
            set
            {
                this.cellMetadataField = value;
            }
        }

        public CT_MetadataBlocks valueMetadata
        {
            get
            {
                return this.valueMetadataField;
            }
            set
            {
                this.valueMetadataField = value;
            }
        }

        public CT_ExtensionList extLst
        {
            get
            {
                return this.extLstField;
            }
            set
            {
                this.extLstField = value;
            }
        }
    }

    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_MetadataTypes
    {

        private List<CT_MetadataType> metadataTypeField;

        private uint countField;

        public CT_MetadataTypes()
        {
            this.metadataTypeField = new List<CT_MetadataType>();
            this.countField = ((uint)(0));
        }
        [XmlElement("metadataType")]
        public List<CT_MetadataType> metadataType
        {
            get
            {
                return this.metadataTypeField;
            }
            set
            {
                this.metadataTypeField = value;
            }
        }
        [XmlAttribute]
        [DefaultValueAttribute(typeof(uint), "0")]
        public uint count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]
    public class CT_MetadataType
    {

        private string nameField;

        private uint minSupportedVersionField;

        private bool ghostRowField;

        private bool ghostColField;

        private bool editField;

        private bool deleteField;

        private bool copyField;

        private bool pasteAllField;

        private bool pasteFormulasField;

        private bool pasteValuesField;

        private bool pasteFormatsField;

        private bool pasteCommentsField;

        private bool pasteDataValidationField;

        private bool pasteBordersField;

        private bool pasteColWidthsField;

        private bool pasteNumberFormatsField;

        private bool mergeField;

        private bool splitFirstField;

        private bool splitAllField;

        private bool rowColShiftField;

        private bool clearAllField;

        private bool clearFormatsField;

        private bool clearContentsField;

        private bool clearCommentsField;

        private bool assignField;

        private bool coerceField;

        private bool adjustField;

        private bool cellMetaField;

        public CT_MetadataType()
        {
            this.ghostRowField = false;
            this.ghostColField = false;
            this.editField = false;
            this.deleteField = false;
            this.copyField = false;
            this.pasteAllField = false;
            this.pasteFormulasField = false;
            this.pasteValuesField = false;
            this.pasteFormatsField = false;
            this.pasteCommentsField = false;
            this.pasteDataValidationField = false;
            this.pasteBordersField = false;
            this.pasteColWidthsField = false;
            this.pasteNumberFormatsField = false;
            this.mergeField = false;
            this.splitFirstField = false;
            this.splitAllField = false;
            this.rowColShiftField = false;
            this.clearAllField = false;
            this.clearFormatsField = false;
            this.clearContentsField = false;
            this.clearCommentsField = false;
            this.assignField = false;
            this.coerceField = false;
            this.adjustField = false;
            this.cellMetaField = false;
        }

        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public uint minSupportedVersion
        {
            get
            {
                return this.minSupportedVersionField;
            }
            set
            {
                this.minSupportedVersionField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool ghostRow
        {
            get
            {
                return this.ghostRowField;
            }
            set
            {
                this.ghostRowField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool ghostCol
        {
            get
            {
                return this.ghostColField;
            }
            set
            {
                this.ghostColField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool edit
        {
            get
            {
                return this.editField;
            }
            set
            {
                this.editField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool delete
        {
            get
            {
                return this.deleteField;
            }
            set
            {
                this.deleteField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool copy
        {
            get
            {
                return this.copyField;
            }
            set
            {
                this.copyField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteAll
        {
            get
            {
                return this.pasteAllField;
            }
            set
            {
                this.pasteAllField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteFormulas
        {
            get
            {
                return this.pasteFormulasField;
            }
            set
            {
                this.pasteFormulasField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteValues
        {
            get
            {
                return this.pasteValuesField;
            }
            set
            {
                this.pasteValuesField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteFormats
        {
            get
            {
                return this.pasteFormatsField;
            }
            set
            {
                this.pasteFormatsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteComments
        {
            get
            {
                return this.pasteCommentsField;
            }
            set
            {
                this.pasteCommentsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteDataValidation
        {
            get
            {
                return this.pasteDataValidationField;
            }
            set
            {
                this.pasteDataValidationField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteBorders
        {
            get
            {
                return this.pasteBordersField;
            }
            set
            {
                this.pasteBordersField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteColWidths
        {
            get
            {
                return this.pasteColWidthsField;
            }
            set
            {
                this.pasteColWidthsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool pasteNumberFormats
        {
            get
            {
                return this.pasteNumberFormatsField;
            }
            set
            {
                this.pasteNumberFormatsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool merge
        {
            get
            {
                return this.mergeField;
            }
            set
            {
                this.mergeField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool splitFirst
        {
            get
            {
                return this.splitFirstField;
            }
            set
            {
                this.splitFirstField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool splitAll
        {
            get
            {
                return this.splitAllField;
            }
            set
            {
                this.splitAllField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool rowColShift
        {
            get
            {
                return this.rowColShiftField;
            }
            set
            {
                this.rowColShiftField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool clearAll
        {
            get
            {
                return this.clearAllField;
            }
            set
            {
                this.clearAllField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool clearFormats
        {
            get
            {
                return this.clearFormatsField;
            }
            set
            {
                this.clearFormatsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool clearContents
        {
            get
            {
                return this.clearContentsField;
            }
            set
            {
                this.clearContentsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool clearComments
        {
            get
            {
                return this.clearCommentsField;
            }
            set
            {
                this.clearCommentsField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool assign
        {
            get
            {
                return this.assignField;
            }
            set
            {
                this.assignField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool coerce
        {
            get
            {
                return this.coerceField;
            }
            set
            {
                this.coerceField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool adjust
        {
            get
            {
                return this.adjustField;
            }
            set
            {
                this.adjustField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool cellMeta
        {
            get
            {
                return this.cellMetaField;
            }
            set
            {
                this.cellMetaField = value;
            }
        }
    }

    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]
    public class CT_MetadataRecord
    {

        private uint tField;

        private uint vField;
        [XmlAttribute]
        public uint t
        {
            get
            {
                return this.tField;
            }
            set
            {
                this.tField = value;
            }
        }
        [XmlAttribute]
        public uint v
        {
            get
            {
                return this.vField;
            }
            set
            {
                this.vField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_MetadataBlocks
    {

        private List<CT_MetadataRecord> bkField;

        private uint countField;

        public CT_MetadataBlocks()
        {
            this.bkField = new List<CT_MetadataRecord>();
            this.countField = ((uint)(0));
        }

        [XmlArray(Order = 0)]
        [XmlArrayItem("rc", typeof(CT_MetadataRecord), IsNullable = false)]
        public List<CT_MetadataRecord> bk
        {
            get
            {
                return this.bkField;
            }
            set
            {
                this.bkField = value;
            }
        }
        [XmlAttribute]
        [DefaultValueAttribute(typeof(uint), "0")]
        public uint count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_FutureMetadataBlock
    {

        private CT_ExtensionList extLstField;

        public CT_FutureMetadataBlock()
        {
            this.extLstField = new CT_ExtensionList();
        }

        public CT_ExtensionList extLst
        {
            get
            {
                return this.extLstField;
            }
            set
            {
                this.extLstField = value;
            }
        }
    }

    public class CT_FutureMetadata
    {

        private List<CT_FutureMetadataBlock> bkField;

        private CT_ExtensionList extLstField;

        private string nameField;

        private uint countField;

        public CT_FutureMetadata()
        {
            this.extLstField = new CT_ExtensionList();
            this.bkField = new List<CT_FutureMetadataBlock>();
            this.countField = ((uint)(0));
        }
        [XmlElement("bk")]
        public List<CT_FutureMetadataBlock> bk
        {
            get
            {
                return this.bkField;
            }
            set
            {
                this.bkField = value;
            }
        }

        public CT_ExtensionList extLst
        {
            get
            {
                return this.extLstField;
            }
            set
            {
                this.extLstField = value;
            }
        }
        [XmlAttribute]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        [XmlAttribute]
        [DefaultValueAttribute(typeof(uint), "0")]
        public uint count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    public class CT_MdxKPI
    {

        private uint nField;

        private uint npField;

        private ST_MdxKPIProperty pField;

        public uint n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        public uint np
        {
            get
            {
                return this.npField;
            }
            set
            {
                this.npField = value;
            }
        }

        public ST_MdxKPIProperty p
        {
            get
            {
                return this.pField;
            }
            set
            {
                this.pField = value;
            }
        }
    }

    public enum ST_MdxKPIProperty
    {

    
        v,

    
        g,

    
        s,

    
        t,

    
        w,

    
        m,
    }

    public class CT_MdxMemeberProp
    {

        private uint nField;

        private uint npField;

        public uint n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        public uint np
        {
            get
            {
                return this.npField;
            }
            set
            {
                this.npField = value;
            }
        }
    }

    public class CT_MdxSet
    {

        private List<CT_MetadataStringIndex> nField;

        private uint nsField;

        private uint cField;

        private ST_MdxSetOrder oField;

        public CT_MdxSet()
        {
            this.nField = new List<CT_MetadataStringIndex>();
            this.cField = ((uint)(0));
            this.oField = ST_MdxSetOrder.u;
        }

        public List<CT_MetadataStringIndex> n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        public uint ns
        {
            get
            {
                return this.nsField;
            }
            set
            {
                this.nsField = value;
            }
        }

        [DefaultValueAttribute(typeof(uint), "0")]
        public uint c
        {
            get
            {
                return this.cField;
            }
            set
            {
                this.cField = value;
            }
        }

        [DefaultValueAttribute(ST_MdxSetOrder.u)]
        public ST_MdxSetOrder o
        {
            get
            {
                return this.oField;
            }
            set
            {
                this.oField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_MetadataStringIndex
    {

        private uint xField;

        private bool sField;

        public CT_MetadataStringIndex()
        {
            this.sField = false;
        }

        public uint x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool s
        {
            get
            {
                return this.sField;
            }
            set
            {
                this.sField = value;
            }
        }
    }

    public enum ST_MdxSetOrder
    {

    
        u,

    
        a,

    
        d,

    
        aa,

    
        ad,

    
        na,

    
        nd,
    }

    public class CT_MdxTuple
    {

        private List<CT_MetadataStringIndex> nField;

        private uint cField;

        private string ctField;

        private uint siField;

        private bool siFieldSpecified;

        private uint fiField;

        private bool fiFieldSpecified;

        private byte[] bcField = null;

        private byte[] fcField = null;

        private bool iField;

        private bool uField;

        private bool stField;

        private bool bField;

        public CT_MdxTuple()
        {
            this.nField = new List<CT_MetadataStringIndex>();
            this.cField = ((uint)(0));
            this.iField = false;
            this.uField = false;
            this.stField = false;
            this.bField = false;
        }

        public List<CT_MetadataStringIndex> n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        [DefaultValueAttribute(typeof(uint), "0")]
        public uint c
        {
            get
            {
                return this.cField;
            }
            set
            {
                this.cField = value;
            }
        }

        public string ct
        {
            get
            {
                return this.ctField;
            }
            set
            {
                this.ctField = value;
            }
        }

        public uint si
        {
            get
            {
                return this.siField;
            }
            set
            {
                this.siField = value;
            }
        }

        [XmlIgnore]
        public bool siSpecified
        {
            get
            {
                return this.siFieldSpecified;
            }
            set
            {
                this.siFieldSpecified = value;
            }
        }

        public uint fi
        {
            get
            {
                return this.fiField;
            }
            set
            {
                this.fiField = value;
            }
        }

        [XmlIgnore]
        public bool fiSpecified
        {
            get
            {
                return this.fiFieldSpecified;
            }
            set
            {
                this.fiFieldSpecified = value;
            }
        }

        [XmlAttribute(DataType = "hexBinary")]
        // Type ST_UnsignedIntHex is base on xsd:hexBinary, Length 4 (octets!?)
        public byte[] bc
        {
            get
            {
                return this.bcField;
            }
            set
            {
                this.bcField = value;
            }
        }

        [XmlAttribute(DataType = "hexBinary")]
        // Type ST_UnsignedIntHex is base on xsd:hexBinary, Length 4 (octets!?)
        public byte[] fc
        {
            get
            {
                return this.fcField;
            }
            set
            {
                this.fcField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool i
        {
            get
            {
                return this.iField;
            }
            set
            {
                this.iField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool u
        {
            get
            {
                return this.uField;
            }
            set
            {
                this.uField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool st
        {
            get
            {
                return this.stField;
            }
            set
            {
                this.stField = value;
            }
        }

        [DefaultValueAttribute(false)]
        public bool b
        {
            get
            {
                return this.bField;
            }
            set
            {
                this.bField = value;
            }
        }
    }

    public class CT_Mdx
    {

        private object itemField;

        private uint nField;

        private ST_MdxFunctionType fField;

        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        public uint n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        public ST_MdxFunctionType f
        {
            get
            {
                return this.fField;
            }
            set
            {
                this.fField = value;
            }
        }
    }

    public enum ST_MdxFunctionType
    {

    
        m,

    
        v,

    
        s,

    
        c,

    
        r,

    
        p,

    
        k,
    }

    public class CT_MdxMetadata
    {

        private List<CT_Mdx> mdxField;

        private uint countField;

        public CT_MdxMetadata()
        {
            this.mdxField = new List<CT_Mdx>();
            this.countField = ((uint)(0));
        }

        public List<CT_Mdx> mdx
        {
            get
            {
                return this.mdxField;
            }
            set
            {
                this.mdxField = value;
            }
        }

        [DefaultValueAttribute(typeof(uint), "0")]
        public uint count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_MetadataStrings
    {

        private List<CT_XStringElement> sField;

        private uint countField;

        public CT_MetadataStrings()
        {
            this.sField = new List<CT_XStringElement>();
            this.countField = ((uint)(0));
        }

        public List<CT_XStringElement> s
        {
            get
            {
                return this.sField;
            }
            set
            {
                this.sField = value;
            }
        }

        [DefaultValueAttribute(typeof(uint), "0")]
        public uint count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", IsNullable = true)]

    public class CT_MetadataBlock
    {

        private List<CT_MetadataRecord> rcField;

        public CT_MetadataBlock()
        {
            this.rcField = new List<CT_MetadataRecord>();
        }
        
        public List<CT_MetadataRecord> rc
        {
            get
            {
                return this.rcField;
            }
            set
            {
                this.rcField = value;
            }
        }
    }
}
