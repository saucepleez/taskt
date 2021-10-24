using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class Theme
    {

        Color _BgGradientStartColor = Color.FromArgb(20, 136, 204);
        public Color BgGradientStartColor
        {
            get { return _BgGradientStartColor; }
            set { _BgGradientStartColor = value; }
        }

        Color _BgGradientEndColor = Color.FromArgb(43, 50, 178);
        public Color BgGradientEndColor
        {
            get { return _BgGradientEndColor; }
            set { _BgGradientEndColor = value; }
        }

        public LinearGradientBrush CreateGradient(Rectangle rect)
        {
            return new LinearGradientBrush(rect, _BgGradientStartColor, _BgGradientEndColor, 180);
        }

        // label style
        private UIFont _label = new UIFont("Segoe UI Light", 12, FontStyle.Regular, Color.White, Color.Transparent);
        public UIFont Label
        {
            get
            {
                return _label;
            }
        }

        // input,textbox style
        private UIFont _input = new UIFont();
        public UIFont Input
        {
            get
            {
                return _input;
            }
        }

        // dropdown style
        private UIFont _dropdown = new UIFont();
        public UIFont Dropdown
        {
            get
            {
                return _dropdown;
            }
        }

        // combobox
        private UIFont _combobox = new UIFont();
        public UIFont Combobox
        {
            get
            {
                return _combobox;
            }
        }

        // datagridview
        private UIFont _datagridview = new UIFont();
        public UIFont Datagridview
        {
            get
            {
                return _datagridview;
            }
        }

        // uiHelper
        private UIFont _uihelper = new UIFont("Segoe UI Semilight", 10, FontStyle.Regular, Color.AliceBlue, Color.Transparent);
        public UIFont UIHelper
        {
            get
            {
                return _uihelper;
            }
        }

        // error label AutomationCommand
        private UIFont _errorLabel = new UIFont("Segoe UI", 18, FontStyle.Bold, Color.Red, Color.Transparent);
        public UIFont ErrorLabel
        {
            get
            {
                return _errorLabel;
            }
        }

        // script charactor
        public static Dictionary<string, UIFont> scriptTexts = new Dictionary<string, UIFont>() {
            { "normal", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.SteelBlue, Color.WhiteSmoke)},
            { "invalid", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.Crimson, Color.WhiteSmoke)},
            { "comment", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.ForestGreen, Color.WhiteSmoke)},
            { "pause", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.MediumPurple, Color.Lavender)},
            { "debug", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.White, Color.OrangeRed)},
            { "current-match", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.Black, Color.Goldenrod)},
            { "match", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.Black, Color.LightYellow)},
            { "selected-normal", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.White, Color.DodgerBlue)},
            { "selected-invalid", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.Crimson, Color.DodgerBlue)},
            { "selected-comment", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.LightGreen, Color.DodgerBlue)},
            { "selected-pause", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.Plum, Color.DodgerBlue)},

            { "number-normal", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.LightSlateGray, Color.Transparent)},
            { "number-dontsaved", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.LightSlateGray, Color.Yellow)},
            { "number-newline", new UIFont("Segoe UI", 12, FontStyle.Bold, Color.White, Color.LimeGreen)},
        };

        public class UIFont
        {
            private string _font = "Segoe UI";
            private float _size = 12;
            private FontStyle _style = FontStyle.Regular;
            private Color _fontColor = Color.Black;
            private Color _backColor = Color.White;

            public UIFont()
            {

            }

            public UIFont(string font)
            {
                this.Font = font;
            }

            public UIFont(float size)
            {
                this.FontSize = size;
            }

            public UIFont(Color color)
            {
                this._fontColor = color;
            }

            public UIFont(string font, int size, FontStyle style, Color fontColor, Color backColor)
            {
                this.Font = font;
                this.FontSize = size;
                _style = style;
                _fontColor = fontColor;
                _backColor = backColor;
            }

            public string Font
            {
                get
                {
                    return _font;
                }
                set
                {
                    if (value.Length > 0)
                    {
                        _font = value;
                    }
                }
            }
            public float FontSize
            {
                get
                {
                    return _size;
                }
                set
                {
                    if (value >= 8)
                    {
                        _size = value;
                    }
                }
            }
            public FontStyle Style
            {
                get
                {
                    return _style;
                }
                set
                {
                    _style = value;
                }
            }
            public Color FontColor
            {
                get
                {
                    return _fontColor;
                }
                set
                {
                    _fontColor = value;
                }
            }

            public Color BackColor
            {
                get
                {
                    return _backColor;
                }
                set
                {
                    _backColor = value;
                }
            }
        }

    }
}
