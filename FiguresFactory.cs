using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    public abstract class FiguresFactory
    {
        public abstract Figure GetFigure();
    }

    public class RectangleFactory : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new Rect();
        }
    }

    public class EllipseFactory : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new Ell();
        }
    }

    public class MyLineFactory : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new MyLine();
        }
    }

    public class MyPolylineFactory : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new MyPolyline();
        }
    }

    public class MyPolygonFactory : FiguresFactory
    {
        public override Figure GetFigure()
        {
            return new MyPolygon();
        }
    }

    //class MyPenFactory : FiguresFactory
    //{
    //    public override Figure GetFigure()
    //    {
    //        return new MyPen();
    //    }
    //}
}
