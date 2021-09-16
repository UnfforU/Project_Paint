using Paint;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace trapezoid_plagin
{
    public class MyTrapezoid : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new Trapezoid();
        }
    }

    public class Trapezoid : Figure
    {
        public Point TopLeft;
        public Point DownRight;

        [NonSerialized]
        public bool isFirst = true;

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
            Polygon trp = new Polygon();
            trp.Points = new PointCollection();

            trp.Points.Add(TopLeft);
            trp.Points.Add(new Point(DownRight.X - (DownRight.X - TopLeft.X) / 5, TopLeft.Y));
            trp.Points.Add(DownRight);
            trp.Points.Add(new Point(TopLeft.X - (DownRight.X - TopLeft.X) / 5, DownRight.Y));

            trp.Stroke = new SolidColorBrush(PenColor);
            trp.Fill = new SolidColorBrush(BrushColor);
            trp.StrokeThickness = PenWidth;
            trp.IsHitTestVisible = false;

            return trp;
        }
    }
}
