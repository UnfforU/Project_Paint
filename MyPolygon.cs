using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
namespace Paint
{
    [Serializable]
    public class MyPolygon : Figure
    {
        public PointCollection linePoints = new PointCollection();
        private Point leftClickPoint;
        private Point currPoint;

        [NonSerialized]
        private bool firstDraw = false;


        public override void SetLeftClickPoint(Point point)
        {
            leftClickPoint = point;
            firstDraw = true;

        }

        public override void SetCurrPoint(Point mousePos)
        {
            currPoint = mousePos;
            firstDraw = true;

        }

        public override Shape GetSysShape(Canvas canvas)
        {
            Polygon pll = new Polygon();
            pll.Points = new PointCollection();

            if ((linePoints.Count != 0) && (firstDraw)) { linePoints.RemoveAt(linePoints.Count - 1); } // Удалит последнюю добавленную точку,
                                                                                      // если она не последняя точка ломаной

            //Список точек ломаной
            if (!linePoints.Contains(leftClickPoint) && (firstDraw))
            {
                linePoints.Add(leftClickPoint);
            }

            //Для динамической отрисовки всгеда перед добавлением "возможной новой" точки доавляем уже сохраненные "вершины" ломаной
            foreach (Point tmp in linePoints)
            {
                if (!pll.Points.Contains(tmp)) { pll.Points.Add(tmp); }
            }

            if (firstDraw) { pll.Points.Add(currPoint); }

            pll.Stroke = new SolidColorBrush(PenColor);
            pll.Fill = new SolidColorBrush(BrushColor);
            pll.StrokeThickness = PenWidth;
            pll.IsHitTestVisible = false;

            //Сохраняем последнюю добавленную, если в метод Draw обльше не вызовется, значит p была последней точкой ломаной 
            if (firstDraw) { linePoints.Add(currPoint); }

            return pll;
        }

    }
}
