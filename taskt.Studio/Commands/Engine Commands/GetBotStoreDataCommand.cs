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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Server;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command allows you to get data from tasktServer.")]
    [UsesDescription("Use this command when you want to retrieve data from tasktServer")]
    [ImplementationDescription("")]
    public class GetBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate a name of the key to retrieve")]
        [InputSpecification("Select a variable or provide an input value")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate whether to retrieve the whole record or just the value")]
        [InputSpecification("Depending upon the option selected, the whole record with metadata may be required.")]
        [SampleUsage("Select one of the associated options")]
        [Remarks("")]
        [PropertyUISelectionOption("Retrieve Value")]
        [PropertyUISelectionOption("Retrieve Entire Record")]
        public string v_DataOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Select the variable to receive the output")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public GetBotStoreDataCommand()
        {
            CommandName = "GetBotStoreDataCommand";
            SelectionName = "Get BotStore Data";
            CommandEnabled = true;
            CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(engine);
            var dataOption = v_DataOption.ConvertToUserVariable(engine);

            BotStoreRequestType requestType;
            if (dataOption == "Retrieve Entire Record")
            {
                requestType = BotStoreRequestType.BotStoreModel;
            }
            else
            {
                requestType = BotStoreRequestType.BotStoreValue;
            }

            
            try
            {
                var result = HttpServerClient.GetData(keyName, requestType);

                if (requestType == BotStoreRequestType.BotStoreValue)
                {
                    result = JsonConvert.DeserializeObject<string>(result);
                }


                result.StoreInUserVariable(engine, v_applyToVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public override List<Control> Render(IfrmCommandEditor editor)
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







