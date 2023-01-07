using System;
using System.Security.RightsManagement;

namespace taskt.Core.Automation.Attributes.PropertyAttributes
{
    #region enum
    public enum MultiAttributesBehavior
    {
        Merge = 0,
        Overwrite,
    }
    #endregion

    #region Virtual Property
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyVirtualProperty : System.Attribute
    {
        public string className;
        public string propertyName;
        public PropertyVirtualProperty(string className, string propertyName)
        {
            this.className = className;
            this.propertyName = propertyName;
        }
    }
    #endregion

    #region to Label, Document
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class InputSpecification : System.Attribute
    {
        public string inputSpecification;
        public bool autoGenerate = false;

        public InputSpecification()
        {
            inputSpecification = "";
            autoGenerate = false;
        }

        public InputSpecification(string desc, bool autoGenerate = false)
        {
            this.inputSpecification = desc;
            this.autoGenerate = autoGenerate;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SampleUsage : System.Attribute
    {
        public string sampleUsage;
        public SampleUsage(string desc)
        {
            this.sampleUsage = desc;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyDetailSampleUsage : System.Attribute
    {
        public string sampleUsage = "";
        public string means = "";
        public bool showInDescription = true;
        public ValueType valueType = ValueType.Manual;
        public string target = "";

        public PropertyDetailSampleUsage(string sampleUsage, string means, bool showDescription = true)
        {
            this.sampleUsage = sampleUsage;
            this.means = means;
            this.showInDescription = showDescription;
        }
        public PropertyDetailSampleUsage(string sampleUsage, ValueType type, string target = "", bool showDescription = true)
        {
            this.sampleUsage = sampleUsage;
            this.valueType = type;
            this.target = target;
            this.showInDescription = showDescription;
        }

        public enum ValueType
        {
            Manual,
            Value,
            VariableValue,
            VariableName
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyDetailSampleUsageBehavior : System.Attribute
    {
        public MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;

        public PropertyDetailSampleUsageBehavior()
        {

        }
        public PropertyDetailSampleUsageBehavior(MultiAttributesBehavior behavior)
        {
            this.behavior = behavior;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Remarks : System.Attribute
    {
        public string remarks;
        public Remarks(string desc)
        {
            this.remarks = desc;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyDescription : System.Attribute
    {
        public string propertyDescription;
        public PropertyDescription(string description)
        {
            this.propertyDescription = description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyShowSampleUsageInDescription : System.Attribute
    {
        public bool showSampleUsage = false;
        public PropertyShowSampleUsageInDescription()
        {

        }
        public PropertyShowSampleUsageInDescription(bool opt)
        {
            this.showSampleUsage = opt;
        }
    }
    #endregion

    #region for 2ndLabel
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertySecondaryLabel : System.Attribute
    {
        public bool useSecondaryLabel = false;

        public PropertySecondaryLabel()
        {

        }
        public PropertySecondaryLabel(bool opt)
        {
            this.useSecondaryLabel = opt;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyAddtionalParameterInfo : System.Attribute
    {
        public string searchKey = "";
        public string description = "";
        public string sampleUsage = "";
        public string remarks = "";
        public PropertyAddtionalParameterInfo(string searchKey, string description, string sampleUsage = "", string remarks = "")
        {
            this.searchKey = searchKey;
            this.description = description;
            this.sampleUsage = sampleUsage;
            this.remarks = remarks;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyAddtionalParameterInfoBehavior : System.Attribute
    {
        public MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;

        public PropertyAddtionalParameterInfoBehavior()
        {

        }
        public PropertyAddtionalParameterInfoBehavior(MultiAttributesBehavior behavior)
        {
            this.behavior = behavior;
        }
    }
    #endregion

    #region for UIHelper/CustomUIHelper
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyUIHelper : System.Attribute
    {
        public UIAdditionalHelperType additionalHelper;
        public PropertyUIHelper(UIAdditionalHelperType helperType)
        {
            this.additionalHelper = helperType;
        }
        public enum UIAdditionalHelperType
        {
            ShowVariableHelper,
            ShowFileSelectionHelper,
            ShowFolderSelectionHelper,
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyUIHelperBehavior : System.Attribute
    {
        public MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;

        public PropertyUIHelperBehavior()
        {

        }
        public PropertyUIHelperBehavior(MultiAttributesBehavior behavior)
        {
            this.behavior = behavior;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyCustomUIHelper : System.Attribute
    {
        public string labelText;
        public string methodName;
        public string nameKey;
        public PropertyCustomUIHelper(string labelText, string methodName, string nameKey = "")
        {
            this.labelText = labelText;
            this.methodName = methodName;
            this.nameKey = nameKey;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyCustomUIHelperBehavior : System.Attribute
    {
        public MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;

        public PropertyCustomUIHelperBehavior()
        {

        }
        public PropertyCustomUIHelperBehavior(MultiAttributesBehavior behavior)
        {
            this.behavior = behavior;
        }
    }
    #endregion

    #region for All Controls
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyRecommendedUIControl : System.Attribute
    {
        public RecommendeUIControlType recommendedControl = RecommendeUIControlType.TextBox;
        public PropertyRecommendedUIControl(RecommendeUIControlType ctl)
        {
            this.recommendedControl = ctl;
        }
        public enum RecommendeUIControlType
        {
            TextBox,
            ComboBox,
            DataGridView,
            MultiLineTextBox,
            CheckBox,
            RadioButton,
            TextLink,
            Label
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyInstanceType : System.Attribute
    {
        public InstanceType instanceType = InstanceType.none;
        public bool autoWrapVariableMarker = false;

        public PropertyInstanceType()
        {

        }
        public PropertyInstanceType(InstanceType type, bool autoWrapVariableMarker = false)
        {
            this.instanceType = type;
            this.autoWrapVariableMarker = autoWrapVariableMarker;
        }

        public enum InstanceType
        {
            none,
            DataBase,
            Excel,
            IE,
            WebBrowser,
            StopWatch,
            Word,
            NLG,
            Dictionary,
            DataTable,
            JSON,
            List,
            Boolean,
            DateTime,
            AutomationElement,
            Color,
            MailKitEMail,
            MailKitEMailList
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyParameterDirection : System.Attribute
    {
        public ParameterDirection porpose = ParameterDirection.Unknown;
        public PropertyParameterDirection()
        {

        }
        public PropertyParameterDirection(ParameterDirection porpose)
        {
            this.porpose = porpose;
        }
        public enum ParameterDirection
        {
            Unknown,
            Input,
            Output,
            Both
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyControlIntoCommandField : System.Attribute
    {
        public string labelName = "";
        public string bodyName = "";
        public string secondLabelName = "";
        public PropertyControlIntoCommandField(string bodyName = "", string labelName = "", string secondLabelName = "")
        {
            this.labelName = labelName;
            this.bodyName = bodyName;
            this.secondLabelName = secondLabelName;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyValidationRule : System.Attribute
    {
        public string parameterName = "";
        public ValidationRuleFlags errorRule = 0;
        public ValidationRuleFlags warningRule = 0;

        public PropertyValidationRule()
        {

        }
        public PropertyValidationRule(string parameterName, ValidationRuleFlags errorRule = 0, ValidationRuleFlags warningRule = 0)
        {
            this.parameterName = parameterName;
            this.errorRule = errorRule;
            this.warningRule = warningRule;
        }

        public bool IsErrorFlag(ValidationRuleFlags err)
        {
            return ((this.errorRule & err) == err);
        }

        public bool IsWarningFlag(ValidationRuleFlags warn)
        {
            return ((this.warningRule & warn) == warn);
        }

        [System.Flags]
        public enum ValidationRuleFlags
        {
            None = 0,
            Empty = 1,
            LessThanZero = 2,
            GreaterThanZero = 4,
            EqualsZero = 8,
            NotEqualsZero = 16,
            NotSelectionOption = 32,
            Between = 64,
            NotBetween = 128,
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyValueRange : System.Attribute
    {
        public double min;
        public double max;
        public PropertyValueRange(double min, double max)
        {
            if (max > min)
            {
                this.min = min;
                this.max = max;
            }
            else
            {
                this.min = max;
                this.max = min;
            }
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyDisplayText : System.Attribute
    {
        public bool parameterDisplay = false;
        public string parameterName = "";
        public string afterText = "";

        public PropertyDisplayText()
        {
        }
        public PropertyDisplayText(bool show, string name, string afterText = "")
        {
            this.parameterDisplay = show;
            this.parameterName = name;
            this.afterText = afterText;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyIsOptional : System.Attribute
    {
        public bool isOptional = false;
        public string setBlankToValue = "";

        public PropertyIsOptional()
        {

        }
        public PropertyIsOptional(bool opt, string setBlankToValue = "")
        {
            this.isOptional = opt;
            this.setBlankToValue = setBlankToValue;
        }
    }
    #endregion

    #region for TextBox/ComboBox
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyFirstValue : System.Attribute
    {
        public string firstValue = "";
        public PropertyFirstValue()
        {

        }
        public PropertyFirstValue(string firstValue)
        {
            this.firstValue = firstValue;
        }
    }
    #endregion

    #region for TextBox
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyTextBoxSetting : System.Attribute
    {
        public int height = 1;
        public bool allowNewLine = true;
        public PropertyTextBoxSetting()
        {

        }
        public PropertyTextBoxSetting(int height = 1, bool allowNewLine = true)
        {
            if (height < 0)
            {
                this.height = 1;
            }
            else
            {
                this.height = height;
            }
            this.allowNewLine = allowNewLine;
        }
    }
    #endregion

    #region for ComboBox
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyUISelectionOption : System.Attribute
    {
        public string uiOption;
        public PropertyUISelectionOption(string description)
        {
            this.uiOption = description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyUISelectionOptionBehavior : System.Attribute
    {
        public MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;

        public PropertyUISelectionOptionBehavior()
        {

        }
        public PropertyUISelectionOptionBehavior(MultiAttributesBehavior behavior)
        {
            this.behavior = behavior;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertySelectionValueSensitive : System.Attribute
    {
        public bool caseSensitive = false;
        public bool whiteSpaceSensitive = true;
        
        public PropertySelectionValueSensitive()
        {
        }
        public PropertySelectionValueSensitive(bool caseSensitive, bool whiteSpaceSensitive = true)
        {
            this.caseSensitive = caseSensitive;
            this.whiteSpaceSensitive = whiteSpaceSensitive;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertySelectionChangeEvent : System.Attribute
    {
        public string methodName = "";
        public PropertySelectionChangeEvent()
        {

        }
        public PropertySelectionChangeEvent(string methodName)
        {
            this.methodName = methodName;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyIsWindowNamesList : System.Attribute
    {
        public bool isWindowNamesList = false;
        public bool allowCurrentWindow = true;
        public bool allowAllWindows = false;
        public bool allowDesktop = false;

        public PropertyIsWindowNamesList()
        {

        }
        public PropertyIsWindowNamesList(bool isWindowNameList = false, bool allowCurrent = true, bool allowAll = false, bool allowDesktop = false)
        {
            this.isWindowNamesList = isWindowNameList;
            this.allowCurrentWindow = allowCurrent;
            this.allowAllWindows = allowAll;
            this.allowDesktop = allowDesktop;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyIsVariablesList : System.Attribute
    {
        public bool isVariablesList = false;
        public PropertyIsVariablesList()
        {

        }
        public PropertyIsVariablesList(bool opt)
        {
            this.isVariablesList = opt;
        }
    }
    #endregion

    #region for DataGridView
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class PropertyDataGridViewSetting : System.Attribute
    {
        public bool allowAddRow = true;
        public bool allowDeleteRow = true;
        public bool allowResizeRow = true;
        public int width = 400;
        public int height = 250;
        public bool autoGenerateColumns = true;
        public int headerRowHeight = 1;

        public PropertyDataGridViewSetting()
        {

        }
        public PropertyDataGridViewSetting(bool allowAddRow = true, bool allowDeleteRow = true, bool allowResizeRow = true, int width = 400, int height = 250, bool autoGenerateColumns = true, int headerRowHeight = 1)
        {
            this.allowAddRow = allowAddRow;
            this.allowDeleteRow = allowDeleteRow;
            this.allowResizeRow = allowResizeRow;
            this.width = width;
            this.height = height;
            this.autoGenerateColumns = autoGenerateColumns;
            this.headerRowHeight = headerRowHeight;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyDataGridViewColumnSettings : System.Attribute
    {
        public string columnName = "";
        public string headerText = "";
        public bool readOnly = false;
        public DataGridViewColumnType type = DataGridViewColumnType.TextBox;
        public string comboBoxItems = "";
        public string defaultValue = "";
        public PropertyDataGridViewColumnSettings(string columnName, string headerText, bool readOnly = false, DataGridViewColumnType type = DataGridViewColumnType.TextBox, string comboBoxItems = "", string defaultValue = null)
        {
            this.columnName = columnName;
            this.headerText = headerText;
            this.readOnly = readOnly;
            this.type = type;
            this.comboBoxItems = comboBoxItems;
            this.defaultValue = defaultValue;
        }
        public enum DataGridViewColumnType
        {
            TextBox,
            ComboBox,
            CheckBox,
            All
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyDataGridViewCellEditEvent : System.Attribute
    {
        public string methodName;
        public DataGridViewCellEvent eventRaise;
        public PropertyDataGridViewCellEditEvent(string methodName, DataGridViewCellEvent eventRaise)
        {
            this.methodName = methodName;
            this.eventRaise = eventRaise;
        }
        public enum DataGridViewCellEvent
        {
            CellClick,
            CellBeginEdit
        }
    }
    #endregion
}