using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace space_defense
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Defense jatek;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            jatek = new Defense();
            jatek.ShowDialog();

            //NevBeiras uj = new NevBeiras(35);
            //uj.ShowDialog();
        }

        private void Eredmeny_Click(object sender, RoutedEventArgs e)
        {
            EredmenyLista vmi = new EredmenyLista();
            vmi.ShowDialog();
        }

        private void Bezar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
