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

namespace taskt.Core.Automation.Attributes.ClassAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class Group : System.Attribute
    {
        public string groupName;
        public Group(string name)
        {
            this.groupName = name;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class Description : System.Attribute
    {
        public string commandFunctionalDescription;
        public Description(string desc)
        {
            this.commandFunctionalDescription = desc;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ImplementationDescription : System.Attribute
    {
        public string commandImplementationDescription;
        public ImplementationDescription(string desc)
        {
            this.commandImplementationDescription = desc;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class UsesDescription : System.Attribute
    {
        public string usesDescription;
        public UsesDescription(string desc)
        {
            this.usesDescription = desc;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class SubGruop : System.Attribute
    {
        public string subGruopName = "";
        public SubGruop(string group)
        {
            this.subGruopName = group;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EnableAutomateDisplayText : System.Attribute
    {
        public bool enableAutomateDisplayText = false;
        public EnableAutomateDisplayText(bool enableAutomateDisplayText)
        {
            this.enableAutomateDisplayText = enableAutomateDisplayText;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EnableAutomateRender : System.Attribute
    {
        public bool enableAutomateRender = false;
        public bool forceRenderComment = false;
        public EnableAutomateRender(bool enableAutomateRender, bool forceRenderComment = false)
        {
            this.enableAutomateRender = enableAutomateRender;
            this.forceRenderComment = forceRenderComment;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CommandSettings : System.Attribute
    {
        public string selectionName = "";
        public bool commandEnable = true;
        public bool customeRender = true;
        
        public CommandSettings()
        {

        }
        public CommandSettings(string selectionName, bool commandEnable = true, bool customeRender = true)
        {
            this.selectionName = selectionName;
            this.commandEnable = commandEnable;
            this.customeRender = customeRender;
        }
    }
}
