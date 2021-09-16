using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace Paint
{
    [Serializable]
    public abstract class Figure
    {
        public Color PenColor;
        public Color BrushColor;
        public int PenWidth;

        public abstract void SetLeftClickPoint(Point point);
        public abstract void SetCurrPoint(Point mousePos);
        public abstract Shape GetSysShape(Canvas canvas);

        public void Draw(Canvas canvas, Point mousePos)
        {
            SetCurrPoint(mousePos);
            canvas.Children.Add(GetSysShape(canvas));
        }   
        

        public void SetAttributes(Brush _penColor, Brush _brushColor, int _penWidth)
        {
            PenColor = ((SolidColorBrush)_penColor).Color;
            BrushColor = ((SolidColorBrush)_brushColor).Color;
            PenWidth = _penWidth;
        }
    }
}
