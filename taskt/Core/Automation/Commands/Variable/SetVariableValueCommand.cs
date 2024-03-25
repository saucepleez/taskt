﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Variable Value")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetVariableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableValue))]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Convert Variables in Input Text Above")]
        [Remarks("If **{{{vNum}}}** has **'1'** and You select **'Yes'**, Variable will be Assigned **'1'**. If You Select **'No'**, Variable will be assigned **'{{{vNum}}}'**.")]
        [PropertyIsOptional(true, "Yes")]
        public string v_ReplaceInputVariables { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Create New Variable when that Variable Does not Exist")]
        [PropertyIsOptional(true, "No")]
        [Remarks("This command ignores the 'Create Missing Variable During Execution' value in the Settings")]
        public string v_CreateNewVariable { get; set; }

        public SetVariableValueCommand()
        {
            //this.CommandName = "VariableCommand";
            //this.SelectionName = "Set Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ReplaceInputVariables = "Yes";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var isRepalce = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ReplaceInputVariables), engine);
            //string variableValue;
            //if (isRepalce == "yes")
            //{
            //    variableValue = v_Input.ExpandValueOrUserVariable(engine);
            //}
            //else
            //{
            //    variableValue = v_Input;
            //}

            string variableValue;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReplaceInputVariables), engine))
            {
                variableValue = v_Input.ExpandValueOrUserVariable(engine);
            }
            else
            {
                variableValue = v_Input;
            }

            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            if (VariableNameControls.IsVariableExists(variableName, engine) ||
                    this.ExpandValueOrUserVariableAsYesNo(nameof(v_CreateNewVariable), engine))
            {
                variableValue.StoreInUserVariable(engine, variableName);
            }
            else
            {
                throw new Exception("Variable Name '" + variableName + "' does not exists.");
            }
        }
    }
}