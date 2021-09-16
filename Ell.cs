using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;

namespace Paint
{
    [Serializable]
    public class Ell : Figure
    {
        public Point TopLeft;
        public Point DownRight;
        
        [NonSerialized]
        private bool isFirst = true;


        public override void SetLeftClickPoint(Point point)
        {
            if (isFirst)
                TopLeft = point;
            isFirst = false;
        }

        public override void SetCurrPoint(Point mousePos)
        {
            DownRight = mousePos;
        }

        public override Shape GetSysShape(Canvas canvas)
        {
            Ellipse fig = new Ellipse();

            int Width = (int)(DownRight.X - TopLeft.X);
            int Height = (int)(DownRight.Y - TopLeft.Y);
            Point p = TopLeft;

            if (Width < 0) { p.X += Width; }
            if (Height < 0) { p.Y += Height; }

            fig.Width = Math.Abs(Width);
            fig.Height = Math.Abs(Height);
            fig.Stroke = new SolidColorBrush(PenColor);
            fig.Fill = new SolidColorBrush(BrushColor);
            fig.StrokeThickness = PenWidth;
            fig.IsHitTestVisible = false;

            Canvas.SetLeft(fig, p.X);
            Canvas.SetTop(fig, p.Y);

            return fig;
        }

    }
}
