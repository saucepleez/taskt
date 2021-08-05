//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using taskt.Core.Server;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get data from tasktServer.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to retrieve data from tasktServer")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate a name of the key to retrieve")]
        [Attributes.PropertyAttributes.InputSpecification("Select a variable or provide an input value")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate whether to retrieve the whole record or just the value")]
        [Attributes.PropertyAttributes.InputSpecification("Depending upon the option selected, the whole record with metadata may be required.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the associated options")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Retrieve Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Retrieve Entire Record")]
        public string v_DataOption { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select the variable to receive the output")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public GetDataCommand()
        {
            this.CommandName = "GetDataCommand";
            this.SelectionName = "Get BotStore Data";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(sender);
            var dataOption = v_DataOption.ConvertToUserVariable(sender);

            BotStoreRequest.RequestType requestType;
            if (dataOption == "Retrieve Entire Record")
            {
                requestType = BotStoreRequest.RequestType.BotStoreModel;
            }
            else
            {
                requestType = BotStoreRequest.RequestType.BotStoreValue;
            }


            try
            {
                var result = HttpServerClient.GetData(keyName, requestType);

                if (requestType == BotStoreRequest.RequestType.BotStoreValue)
                {
                    result = JsonConvert.DeserializeObject<string>(result);
                }


                result.StoreInUserVariable(sender, v_applyToVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_KeyName", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_DataOption", this));
            var dropdown = CommandControls.CreateDropdownFor("v_DataOption", this);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_DataOption", this, new Control[] { dropdown }, editor));
            RenderedControls.Add(dropdown);


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Data from Key '" + v_KeyName + "' in tasktServer BotStore]";
        }
    }




}







