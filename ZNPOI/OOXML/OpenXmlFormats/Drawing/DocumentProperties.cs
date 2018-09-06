// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
//    <NameSpace>NPOI.OpenXmlFormats.Dml</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>True</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace NPOI.OpenXmlFormats.Dml
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Collections.Generic;



    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_ConnectorLocking
    {

        private CT_OfficeArtExtensionList extLstField;

        private bool noGrpField;

        private bool noSelectField;

        private bool noRotField;

        private bool noChangeAspectField;

        private bool noMoveField;

        private bool noResizeField;

        private bool noEditPointsField;

        private bool noAdjustHandlesField;

        private bool noChangeArrowheadsField;

        private bool noChangeShapeTypeField;

        public CT_ConnectorLocking()
        {
            this.extLstField = new CT_OfficeArtExtensionList();
            this.noGrpField = false;
            this.noSelectField = false;
            this.noRotField = false;
            this.noChangeAspectField = false;
            this.noMoveField = false;
            this.noResizeField = false;
            this.noEditPointsField = false;
            this.noAdjustHandlesField = false;
            this.noChangeArrowheadsField = false;
            this.noChangeShapeTypeField = false;
        }

        [XmlElement(Order = 0)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool noGrp
        {
            get
            {
                return this.noGrpField;
            }
            set
            {
                this.noGrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noSelect
        {
            get
            {
                return this.noSelectField;
            }
            set
            {
                this.noSelectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noRot
        {
            get
            {
                return this.noRotField;
            }
            set
            {
                this.noRotField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeAspect
        {
            get
            {
                return this.noChangeAspectField;
            }
            set
            {
                this.noChangeAspectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noMove
        {
            get
            {
                return this.noMoveField;
            }
            set
            {
                this.noMoveField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noResize
        {
            get
            {
                return this.noResizeField;
            }
            set
            {
                this.noResizeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noEditPoints
        {
            get
            {
                return this.noEditPointsField;
            }
            set
            {
                this.noEditPointsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noAdjustHandles
        {
            get
            {
                return this.noAdjustHandlesField;
            }
            set
            {
                this.noAdjustHandlesField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeArrowheads
        {
            get
            {
                return this.noChangeArrowheadsField;
            }
            set
            {
                this.noChangeArrowheadsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeShapeType
        {
            get
            {
                return this.noChangeShapeTypeField;
            }
            set
            {
                this.noChangeShapeTypeField = value;
            }
        }
    }



    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_ShapeLocking
    {

        private CT_OfficeArtExtensionList extLstField;

        private bool noGrpField;

        private bool noSelectField;

        private bool noRotField;

        private bool noChangeAspectField;

        private bool noMoveField;

        private bool noResizeField;

        private bool noEditPointsField;

        private bool noAdjustHandlesField;

        private bool noChangeArrowheadsField;

        private bool noChangeShapeTypeField;

        private bool noTextEditField;

        public CT_ShapeLocking()
        {
            //this.extLstField = new CT_OfficeArtExtensionList();
            this.noGrpField = false;
            this.noSelectField = false;
            this.noRotField = false;
            this.noChangeAspectField = false;
            this.noMoveField = false;
            this.noResizeField = false;
            this.noEditPointsField = false;
            this.noAdjustHandlesField = false;
            this.noChangeArrowheadsField = false;
            this.noChangeShapeTypeField = false;
            this.noTextEditField = false;
        }

        [XmlElement(Order = 0)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool noGrp
        {
            get
            {
                return this.noGrpField;
            }
            set
            {
                this.noGrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noSelect
        {
            get
            {
                return this.noSelectField;
            }
            set
            {
                this.noSelectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noRot
        {
            get
            {
                return this.noRotField;
            }
            set
            {
                this.noRotField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeAspect
        {
            get
            {
                return this.noChangeAspectField;
            }
            set
            {
                this.noChangeAspectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noMove
        {
            get
            {
                return this.noMoveField;
            }
            set
            {
                this.noMoveField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noResize
        {
            get
            {
                return this.noResizeField;
            }
            set
            {
                this.noResizeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noEditPoints
        {
            get
            {
                return this.noEditPointsField;
            }
            set
            {
                this.noEditPointsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noAdjustHandles
        {
            get
            {
                return this.noAdjustHandlesField;
            }
            set
            {
                this.noAdjustHandlesField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeArrowheads
        {
            get
            {
                return this.noChangeArrowheadsField;
            }
            set
            {
                this.noChangeArrowheadsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeShapeType
        {
            get
            {
                return this.noChangeShapeTypeField;
            }
            set
            {
                this.noChangeShapeTypeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noTextEdit
        {
            get
            {
                return this.noTextEditField;
            }
            set
            {
                this.noTextEditField = value;
            }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_PictureLocking
    {

        private CT_OfficeArtExtensionList extLstField;

        private bool noGrpField;

        private bool noSelectField;

        private bool noRotField;

        private bool noChangeAspectField;

        private bool noMoveField;

        private bool noResizeField;

        private bool noEditPointsField;

        private bool noAdjustHandlesField;

        private bool noChangeArrowheadsField;

        private bool noChangeShapeTypeField;

        private bool noCropField;

        public CT_PictureLocking()
        {
            this.extLstField = new CT_OfficeArtExtensionList();
            this.noGrpField = false;
            this.noSelectField = false;
            this.noRotField = false;
            this.noChangeAspectField = false;
            this.noMoveField = false;
            this.noResizeField = false;
            this.noEditPointsField = false;
            this.noAdjustHandlesField = false;
            this.noChangeArrowheadsField = false;
            this.noChangeShapeTypeField = false;
            this.noCropField = false;
        }

        [XmlElement(Order = 0)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool noGrp
        {
            get
            {
                return this.noGrpField;
            }
            set
            {
                this.noGrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noSelect
        {
            get
            {
                return this.noSelectField;
            }
            set
            {
                this.noSelectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noRot
        {
            get
            {
                return this.noRotField;
            }
            set
            {
                this.noRotField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeAspect
        {
            get
            {
                return this.noChangeAspectField;
            }
            set
            {
                this.noChangeAspectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noMove
        {
            get
            {
                return this.noMoveField;
            }
            set
            {
                this.noMoveField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noResize
        {
            get
            {
                return this.noResizeField;
            }
            set
            {
                this.noResizeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noEditPoints
        {
            get
            {
                return this.noEditPointsField;
            }
            set
            {
                this.noEditPointsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noAdjustHandles
        {
            get
            {
                return this.noAdjustHandlesField;
            }
            set
            {
                this.noAdjustHandlesField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeArrowheads
        {
            get
            {
                return this.noChangeArrowheadsField;
            }
            set
            {
                this.noChangeArrowheadsField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeShapeType
        {
            get
            {
                return this.noChangeShapeTypeField;
            }
            set
            {
                this.noChangeShapeTypeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noCrop
        {
            get
            {
                return this.noCropField;
            }
            set
            {
                this.noCropField = value;
            }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_GroupLocking
    {

        private CT_OfficeArtExtensionList extLstField;

        private bool noGrpField;

        private bool noUngrpField;

        private bool noSelectField;

        private bool noRotField;

        private bool noChangeAspectField;

        private bool noMoveField;

        private bool noResizeField;

        public CT_GroupLocking()
        {
            this.extLstField = new CT_OfficeArtExtensionList();
            this.noGrpField = false;
            this.noUngrpField = false;
            this.noSelectField = false;
            this.noRotField = false;
            this.noChangeAspectField = false;
            this.noMoveField = false;
            this.noResizeField = false;
        }

        [XmlElement(Order = 0)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool noGrp
        {
            get
            {
                return this.noGrpField;
            }
            set
            {
                this.noGrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noUngrp
        {
            get
            {
                return this.noUngrpField;
            }
            set
            {
                this.noUngrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noSelect
        {
            get
            {
                return this.noSelectField;
            }
            set
            {
                this.noSelectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noRot
        {
            get
            {
                return this.noRotField;
            }
            set
            {
                this.noRotField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeAspect
        {
            get
            {
                return this.noChangeAspectField;
            }
            set
            {
                this.noChangeAspectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noMove
        {
            get
            {
                return this.noMoveField;
            }
            set
            {
                this.noMoveField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noResize
        {
            get
            {
                return this.noResizeField;
            }
            set
            {
                this.noResizeField = value;
            }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_GraphicalObjectFrameLocking
    {

        private CT_OfficeArtExtensionList extLstField;

        private bool noGrpField;

        private bool noDrilldownField;

        private bool noSelectField;

        private bool noChangeAspectField;

        private bool noMoveField;

        private bool noResizeField;

        public CT_GraphicalObjectFrameLocking()
        {
            //this.extLstField = new CT_OfficeArtExtensionList();
            this.noGrpField = false;
            this.noDrilldownField = false;
            this.noSelectField = false;
            this.noChangeAspectField = false;
            this.noMoveField = false;
            this.noResizeField = false;
        }

        [XmlElement(Order = 0)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool noGrp
        {
            get
            {
                return this.noGrpField;
            }
            set
            {
                this.noGrpField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noDrilldown
        {
            get
            {
                return this.noDrilldownField;
            }
            set
            {
                this.noDrilldownField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noSelect
        {
            get
            {
                return this.noSelectField;
            }
            set
            {
                this.noSelectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noChangeAspect
        {
            get
            {
                return this.noChangeAspectField;
            }
            set
            {
                this.noChangeAspectField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noMove
        {
            get
            {
                return this.noMoveField;
            }
            set
            {
                this.noMoveField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool noResize
        {
            get
            {
                return this.noResizeField;
            }
            set
            {
                this.noResizeField = value;
            }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualDrawingProps
    {

        private CT_Hyperlink hlinkClickField = null;

        private CT_Hyperlink hlinkHoverField = null;

        private CT_OfficeArtExtensionList extLstField = null;

        private uint idField;

        private string nameField = null;

        private string descrField;

        private bool? hiddenField = null;


        [XmlElement(Order = 0)]
        public CT_Hyperlink hlinkClick
        {
            get
            {
                return this.hlinkClickField;
            }
            set
            {
                this.hlinkClickField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_Hyperlink hlinkHover
        {
            get
            {
                return this.hlinkHoverField;
            }
            set
            {
                this.hlinkHoverField = value;
            }
        }

        [XmlElement(Order = 2)]
        public CT_OfficeArtExtensionList extLst
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
        public uint id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
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
        [DefaultValue("")]
        public string descr
        {
            get
            {
                return null == this.descrField ? "" : descrField;
            }
            set
            {
                this.descrField = value;
            }
        }
        [XmlIgnore]
        public bool descrSpecified
        {
            get { return (null != descrField); }
        }
        [XmlAttribute]
        [DefaultValue(false)]
        public bool hidden
        {
            get
            {
                return null == this.hiddenField ? false : (bool)hiddenField;
            }
            set
            {
                this.hiddenField = value;
            }
        }

        [XmlIgnore]
        public bool hiddenSpecified
        {
            get { return (null != hiddenField); }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualDrawingShapeProps
    {

        private CT_ShapeLocking spLocksField;

        private CT_OfficeArtExtensionList extLstField;

        private bool txBoxField;


        [XmlElement(Order = 0)]
        public CT_ShapeLocking spLocks
        {
            get
            {
                return this.spLocksField;
            }
            set
            {
                this.spLocksField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(false)]
        public bool txBox
        {
            get
            {
                return this.txBoxField;
            }
            set
            {
                this.txBoxField = value;
            }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualConnectorProperties
    {

        private CT_ConnectorLocking cxnSpLocksField;

        private CT_Connection stCxnField;

        private CT_Connection endCxnField;

        private CT_OfficeArtExtensionList extLstField;

        public CT_NonVisualConnectorProperties()
        {
            this.extLstField = new CT_OfficeArtExtensionList();
            this.endCxnField = new CT_Connection();
            this.stCxnField = new CT_Connection();
            this.cxnSpLocksField = new CT_ConnectorLocking();
        }

        [XmlElement(Order = 0)]
        public CT_ConnectorLocking cxnSpLocks
        {
            get
            {
                return this.cxnSpLocksField;
            }
            set
            {
                this.cxnSpLocksField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_Connection stCxn
        {
            get
            {
                return this.stCxnField;
            }
            set
            {
                this.stCxnField = value;
            }
        }

        [XmlElement(Order = 2)]
        public CT_Connection endCxn
        {
            get
            {
                return this.endCxnField;
            }
            set
            {
                this.endCxnField = value;
            }
        }

        [XmlElement(Order = 3)]
        public CT_OfficeArtExtensionList extLst
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
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualPictureProperties
    {

        private CT_PictureLocking picLocksField = null;

        private CT_OfficeArtExtensionList extLstField = null;

        private bool? preferRelativeResizeField = null;

        public CT_NonVisualPictureProperties()
        {
            this.preferRelativeResizeField = true;
        }

        public CT_PictureLocking AddNewPicLocks()
        {
            this.picLocksField = new CT_PictureLocking();
            return picLocksField;
        }

        [XmlElement(Order = 0)]
        public CT_PictureLocking picLocks
        {
            get
            {
                return this.picLocksField;
            }
            set
            {
                this.picLocksField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_OfficeArtExtensionList extLst
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
        [DefaultValue(true)]
        public bool preferRelativeResize
        {
            get
            {
                return null == this.preferRelativeResizeField ? true : (bool)preferRelativeResizeField;
            }
            set
            {
                this.preferRelativeResizeField = value;
            }
        }

        [XmlIgnore]
        public bool preferRelativeResizeSpecified
        {
            get { return (null != preferRelativeResizeField); }
        }
    }


    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualGroupDrawingShapeProps
    {

        private CT_GroupLocking grpSpLocksField;

        private CT_OfficeArtExtensionList extLstField;

        public CT_NonVisualGroupDrawingShapeProps()
        {
            this.extLstField = new CT_OfficeArtExtensionList();
            this.grpSpLocksField = new CT_GroupLocking();
        }

        [XmlElement(Order = 0)]
        public CT_GroupLocking grpSpLocks
        {
            get
            {
                return this.grpSpLocksField;
            }
            set
            {
                this.grpSpLocksField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_OfficeArtExtensionList extLst
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
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main")]
    [XmlRoot(Namespace = "http://schemas.openxmlformats.org/drawingml/2006/main", IsNullable = true)]
    public partial class CT_NonVisualGraphicFrameProperties
    {

        private CT_GraphicalObjectFrameLocking graphicFrameLocksField;

        private CT_OfficeArtExtensionList extLstField;

        public CT_NonVisualGraphicFrameProperties()
        {
            //this.extLstField = new CT_OfficeArtExtensionList();
            //this.graphicFrameLocksField = new CT_GraphicalObjectFrameLocking();
        }

        [XmlElement(Order = 0)]
        public CT_GraphicalObjectFrameLocking graphicFrameLocks
        {
            get
            {
                return this.graphicFrameLocksField;
            }
            set
            {
                this.graphicFrameLocksField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_OfficeArtExtensionList extLst
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
}
