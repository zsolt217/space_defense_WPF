using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace space_defense
{
    class Jatek_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        BindingList<Oszlop> oszlopok;
        static bool oszlop_frissit;
 
        public static bool Oszlop_frissit
        {
            get { return oszlop_frissit; }
            set { oszlop_frissit = value; }
        }

        public BindingList<Oszlop> Oszlopok
        {
            get { return oszlopok; }
        }

        BindingList<Urhajo> hajok;

        public BindingList<Urhajo> Hajok
        {
            get { return hajok; }
        }

        private BindingList<Lovedek> lovedekek;

        public BindingList<Lovedek> Lovedekek
        {
            get { return lovedekek; }
        }

        public Jatek_ViewModel(BindingList<Oszlop> oszlopok, BindingList<Urhajo> ujhajok, BindingList<Lovedek> golyok)
        {
            this.oszlopok = oszlopok;
            this.oszlopok.ListChanged += oszlopok_ListChanged;
            oszlop_frissit = false;
            hajok = ujhajok;
            hajok.ListChanged += hajok_ListChanged;
            lovedekek = golyok;
            lovedekek.ListChanged += lovedekek_ListChanged;
        }

        void lovedekek_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged("Lovedekek");
        }

        void hajok_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged("Hajok");
        }

        void oszlopok_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (oszlop_frissit)
            {
                OnPropertyChanged("Oszlopok");
            }
            oszlop_frissit = false;
        }


    }
}
