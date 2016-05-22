using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace space_defense
{
    /// <summary>
    /// Interaction logic for NevBeiras.xaml
    /// </summary>
    public partial class NevBeiras : Window
    {
        int pont;
        public NevBeiras(int ujpont)
        {
            pont = ujpont;
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox.Text != String.Empty)
            {
                string nev = txtBox.Text;
                using (StreamWriter writer = new StreamWriter("eredmeny.txt", true))
                {
                    writer.WriteLine(nev + ":" + pont.ToString());
                }
                DialogResult = true;
            }
        }
    }
}
