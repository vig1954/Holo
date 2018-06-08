using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Processing
{
    /// <summary>
    /// Прямоугольная область выделения на изображении
    /// </summary>
    public struct ImageSelection
    {
        public Vector2 Start;
        public Vector2 End;

        public float Width
        {
            get => End.X - Start.X;
            set => End.X = Start.X + value;
        }

        public float Height
        {
            get => End.Y - Start.Y;
            set => End.Y = Start.Y + value;
        }

        public static ImageSelection Empty => new ImageSelection {Start = Vector2.Zero, End = Vector2.Zero};
    }
}
