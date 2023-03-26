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
using System.Xml.Serialization;
using Newtonsoft.Json;
using taskt.Core.Server;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get BotStore Data")]
    [Attributes.ClassAttributes.Description("This command allows you to get data from tasktServer.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to retrieve data from tasktServer")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Name of the Key to Retrieve")]
        [InputSpecification("Key", true)]
        [PropertyDetailSampleUsage("**Key**", PropertyDetailSampleUsage.ValueType.Value, "Key")]
        [PropertyDetailSampleUsage("**{{{vKey}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Key")]
        [PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key")]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Retrieve the whole Record or just the Value")]
        [PropertyUISelectionOption("Retrieve Value")]
        [PropertyUISelectionOption("Retrieve Entire Record")]
        [PropertyValidationRule("Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_DataOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_applyToVariableName { get; set; }

        public GetBotStoreDataCommand()
        {
            //this.CommandName = "GetDataCommand";
            //this.SelectionName = "Get BotStore Data";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(engine);

            var dataOption = this.GetUISelectionValue(nameof(v_DataOption), engine);
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

                if (!String.IsNullOrEmpty(v_applyToVariableName))
                {
                    if (requestType == BotStoreRequest.RequestType.BotStoreValue)
                    {
                        result = JsonConvert.DeserializeObject<string>(result);
                    }

                    result.StoreInUserVariable(engine, v_applyToVariableName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}







