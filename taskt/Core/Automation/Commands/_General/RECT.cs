namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Rect Struct
    /// </summary>
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int GetWidth()
        {
            return (right - left);
        }

        public int GetHeight()
        {
            return (bottom - top);
        }

        public (int, int) GetWidthAndHeight()
        {
            return (GetWidth(), GetHeight());
        }

        public (int, int) GetCenterPosition()
        {
            return (
                    left + (GetWidth() / 2),
                    top + (GetHeight() / 2)
                );
        }
    }
}