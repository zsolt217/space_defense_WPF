using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace space_defense
{
    /// <summary>
    /// Interaction logic for EredmenyLista.xaml
    /// </summary>
    public partial class EredmenyLista : Window
    {
        List<EredmenyElem> elemek;
        public EredmenyLista()
        {
            elemek = new List<EredmenyElem>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] osszes;
            osszes = File.ReadAllLines("eredmeny.txt");
            for (int i = 0; i < osszes.Length; i++)
            {
                string[] adatok = osszes[i].Split(':');
                EredmenyElem elem = new EredmenyElem(adatok[0], adatok[1]);
                elemek.Add(elem);
            }
            elemek.Sort();
            lista.ItemsSource = elemek;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
    public class EredmenyElem:IComparable
    {
        string nev;

        public string Nev
        {
            get { return nev; }
        }
        string pontok;

        public string Pontok
        {
            get { return pontok; }
        }
        public EredmenyElem(string ujnev, string ujpont)
        {
            nev = ujnev; pontok = ujpont;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            EredmenyElem elem = obj as EredmenyElem;
            int szam = int.Parse(elem.Pontok);
            int masikszam = int.Parse(pontok);
            return szam.CompareTo(masikszam);
        }
    }
}
