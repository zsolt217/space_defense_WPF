using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace space_defense
{
    class Lovedek : INotifyPropertyChanged
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
        Rect golyo;
        int mozgas;
        public Rect Golyo
        {
            get { return golyo; }
        }
        public Lovedek(int x, int y, int irany)
        {
            golyo = new Rect(x, y, 20, 20);
            mozgas = irany;
        }
        public void ChangeX()
        {
            golyo.X += mozgas;
            OnPropertyChanged("Golyo");
        }
    }
}
