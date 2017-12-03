using System.Diagnostics;

namespace Engine.Utilities
{
    [DebuggerDisplay("Vector2 [{X},{Y}]")]
    public class Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2()
        {

        }

        public Vector2 (int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }
    }
}
