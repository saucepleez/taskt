using System;

namespace taskt.Core.Settings
{
    [Serializable]
    public class WhiteListIPSettings
    {
        public string Setting { get; set; }
        public WhiteListIPSettings(string setting)
        {
            Setting = setting;
        }
    }
}
