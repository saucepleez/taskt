using System;

namespace taskt.Core
{

    [Serializable]
    public class WhiteListIP
    {
        string _value;
        public WhiteListIP(string s)
        {
            _value = s;
        }
        public string Value { get { return _value; } set { _value = value; } }
    }
}
