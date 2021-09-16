using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using System.IO;

namespace Paint
{
    [Serializable]
    public class ListOfFigures
    {
        public List<Figure> list;

        [NonSerialized]
        private List<Figure> buffList;

        public ListOfFigures()
        {
            list = new List<Figure>();
            buffList = new List<Figure>();
        }

        public void DrawAll(Canvas canvas)
        {
            canvas.Children.Clear();
            foreach (Figure tmp in list) { canvas.Children.Add(tmp.GetSysShape(canvas)); }
        }

        public void UnDo(Canvas canvas)
        {
            if (list.Count != 0) {
                buffList.Add(list.ElementAt(list.Count - 1));
                list.RemoveAt(list.Count - 1);
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
        }
        
        public void ReDo(Canvas canvas)
        {
            if (buffList.Count != 0)
            {
                list.Add(buffList.ElementAt(buffList.Count - 1));
                canvas.Children.Add(buffList.ElementAt(buffList.Count - 1).GetSysShape(canvas));
                buffList.RemoveAt(buffList.Count - 1);
            }
        }

        public void ClearBuffList()
        {
            buffList = new List<Figure>();
        }

        public void Serialize(string path, List<Type> allTypes)
        {


            foreach (Figure fig in list)
            {
                if (!allTypes.Contains(fig.GetType())) { allTypes.Add(fig.GetType()); }
            }

            Type[] allTypesArr = allTypes.ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(ListOfFigures), allTypesArr);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, this);
            }
        }

        public ListOfFigures DeSerialize(string path, List<Type> allTypes)
        {
            ListOfFigures result;

            foreach (Figure fig in list)
            {
                if (!allTypes.Contains(fig.GetType())) { allTypes.Add(fig.GetType()); }
            }

            Type[] allTypesArr = allTypes.ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(ListOfFigures), allTypesArr);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                result = (ListOfFigures)xml.Deserialize(fs);
            }
            return result;
        }
    }
}
