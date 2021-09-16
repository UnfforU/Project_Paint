using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class MyLine : Figure
    {
        public Point secondPoint;
        public Point firstPoint;
        
        [NonSerialized]
        private bool isFirst = true;


        public override void SetLeftClickPoint(Point point)
        {
            if (isFirst)
                firstPoint = point;
            isFirst = false;
        }

        public override void SetCurrPoint(Point mousePos)
        {
            secondPoint = mousePos;
        }

        public override Shape GetSysShape(Canvas canvas)
        {
            Line fig = new Line();
            fig.X1 = firstPoint.X;
            fig.Y1 = firstPoint.Y;
            fig.X2 = secondPoint.X;
            fig.Y2 = secondPoint.Y;
            
            fig.Stroke = new SolidColorBrush(PenColor);
            fig.StrokeThickness = PenWidth;
            fig.IsHitTestVisible = false;

            return fig;
        }
    }
}
