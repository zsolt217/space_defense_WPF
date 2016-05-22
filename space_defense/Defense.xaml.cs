using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Defense.xaml
    /// </summary>
    public partial class Defense : Window
    {
        Jatek_BusinessLogic BS;
        Jatek_ViewModel VM;

        public Defense()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM = new Jatek_ViewModel(new BindingList<Oszlop>(), new BindingList<Urhajo>(), new BindingList<Lovedek>());
            BS = new Jatek_BusinessLogic(VM.Oszlopok, (int)(Content as Grid).ActualHeight, (int)(Content as Grid).ActualWidth, VM.Hajok, VM.Lovedekek);
            BS.Ablakbezaras += BS_Ablakbezaras;
            DataContext = VM;
        }

        void BS_Ablakbezaras(object source, ErtekMentesEventArgs vmi)
        {
            if (vmi.Eredmeny == 0)
            {
                BS.Oszlopmozgas.Stop();
                BS.Ellensegaktivitas.Stop();
                BS.Lovedekmozgas.Stop();
                DialogResult = true;
            }
            else
            {
                BS.Oszlopmozgas.Stop();
                BS.Ellensegaktivitas.Stop();
                BS.Lovedekmozgas.Stop();
                MessageBox.Show(vmi.Eredmeny.ToString());
                NevBeiras kiir = new NevBeiras(vmi.Eredmeny);
                kiir.ShowDialog();
                DialogResult = true;
            }
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int leptek = 5;
            switch (e.Key)
            {
                case Key.Down: BS.SajatHajoMozgatas(0, leptek); break;
                case Key.Up: BS.SajatHajoMozgatas(0, -leptek); break;
                case Key.Left: BS.SajatHajoMozgatas(-leptek, 0); break;
                case Key.Right: BS.SajatHajoMozgatas(leptek, 0); break;
                case Key.Space: BS.Loves(); break;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            BS.Oszlopmozgas.Stop();
            BS.Ellensegaktivitas.Stop();
            BS.Lovedekmozgas.Stop();
        }
    }
}
