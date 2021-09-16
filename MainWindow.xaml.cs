using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Reflection;

namespace Paint
{

    public partial class MainWindow : Window
    {
        private FiguresFactory FiguresFactory = new RectangleFactory();
        private Figure currFigure = new RectangleFactory().GetFigure();
        private bool isDraw = false;
        private ListOfFigures listFigures = new ListOfFigures();
        private static int plaginCounter = 0;
        List<Type> allTypesList = new List<Type>();

        public MainWindow()
        {  
            InitializeComponent();
            Rectangle_Button.IsChecked = true;
            FiguresFactory = new RectangleFactory();
            currFigure = FiguresFactory.GetFigure();


            SetCanvasAttributes();
        }

        private void pColor_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if(dlg.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                penColor.Fill = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                SetCanvasAttributes();
            }
        }

        private void bColor_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                brushColor.Fill = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                SetCanvasAttributes();
            }
        }

        private void ToggleButtons_Checked(object sender, RoutedEventArgs e)
        {
            foreach(UIElement el in FiguresList.Children)
                if ((el is ToggleButton button) && el != sender)
                    button.IsChecked = false;

            foreach (UIElement el in ToolsList.Children)
                if ((el is ToggleButton button) && el != sender)
                    button.IsChecked = false;
        }

        private void Pen_Button_Checked(object sender, RoutedEventArgs e) {
            
            ToggleButtons_Checked(sender, e);
        }

        public void SetCanvasAttributes()
        {
            currFigure = FiguresFactory.GetFigure();
            currFigure.SetAttributes(penColor.Fill, brushColor.Fill, (int)penSize.Value + 1);
        }

        private void penSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetCanvasAttributes();
        }

        private void Rectangle_Button_Click(object sender, RoutedEventArgs e)
        {
            FiguresFactory = new RectangleFactory();
            SetCanvasAttributes();
        }

        private void Ellipse_Button_Click(object sender, RoutedEventArgs e)
        {
            FiguresFactory = new EllipseFactory();
            SetCanvasAttributes();
        }

        private void Line_Button_Click(object sender, RoutedEventArgs e)
        {
            FiguresFactory = new MyLineFactory();
            SetCanvasAttributes();
        }

        private void Polyline_Button_Click(object sender, RoutedEventArgs e)
        {
            FiguresFactory = new MyPolylineFactory();
            SetCanvasAttributes();
        }

        private void Polygon_Button_Click(object sender, RoutedEventArgs e)
        {
            FiguresFactory = new MyPolygonFactory();
            SetCanvasAttributes();
        }


        private void Add_plagin_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            string path = "";

            openFileDialog.Filter = "DLL files (*.dll)|*.dll";

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
            }
            else { return; }

            Assembly asm = Assembly.LoadFrom(path);
            Type[] factory_plagin_types;
            try
            {
                factory_plagin_types = asm.GetTypes();
            }
            catch
            {
                System.Windows.MessageBox.Show("Выберите поддерживаемую dll");
                return;
            }

            int currPlaginCounter = 0;
            foreach(Type factory_plagin_type in factory_plagin_types)
            {
                if (typeof(FiguresFactory).IsAssignableFrom(factory_plagin_type)) 
                {
                    allTypesList.Add(factory_plagin_type);
                    currPlaginCounter++;
                    plaginCounter++;

                    ToggleButton tgl = new ToggleButton();
                    tgl.Name = "plagin_" + plaginCounter.ToString() + "_button";
                    tgl.Width = 20;
                    tgl.Height = 20;
                    tgl.BorderBrush = Brushes.White;
                    tgl.Checked += ToggleButtons_Checked;
                    tgl.Click += delegate (object sender_1, RoutedEventArgs e_1)
                    {
                        FiguresFactory = (FiguresFactory)Activator.CreateInstance(factory_plagin_type);
                        SetCanvasAttributes();
                    };

                    Image img = new Image();
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("Resources\\plagin.png", UriKind.Relative);
                    bi3.EndInit();
                    img.Source = bi3;
                    tgl.Content = img;

                    FiguresList.Children.Insert(FiguresList.Children.Count - 1, tgl);
                }
            }

            if (currPlaginCounter == 0)
            {
                System.Windows.MessageBox.Show("Выберите поддерживаемую dll");
                return;
            }
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                listFigures.list.Add(currFigure);

                SetCanvasAttributes();
                isDraw = false;
            }
            else
            {
                currFigure.SetLeftClickPoint(e.GetPosition(MyCanvas));
                isDraw = true;
            }
        }

        private void MyCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDraw)
            {
                int leng = MyCanvas.Children.Count;
                if (leng > listFigures.list.Count)
                {
                    MyCanvas.Children.RemoveAt(leng - 1);
                }

                Point p = e.MouseDevice.GetPosition(MyCanvas);
                currFigure.Draw(MyCanvas, p);
            }
        }

        private void MyCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isDraw) { return; }
            isDraw = false;
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            SetCanvasAttributes();
        }

        private void UnDo_Button_Click(object sender, RoutedEventArgs e)
        {
            listFigures.UnDo(MyCanvas);
        }

        private void ReDo_Button_Click(object sender, RoutedEventArgs e)
        {
            listFigures.ReDo(MyCanvas);
        }

        private void OpenCanvas_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Filter = "XML files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
                listFigures = listFigures.DeSerialize(openFileDialog.FileName, allTypesList);

            listFigures.DrawAll(MyCanvas);
        }

        private void SaveCanvas_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            saveFileDialog.Filter = "XML files (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == true)
                listFigures.Serialize(saveFileDialog.FileName, allTypesList);
        }
    }
}
