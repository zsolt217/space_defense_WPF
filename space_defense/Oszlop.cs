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
    class Oszlop : INotifyPropertyChanged
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
        Rect oszlop1;
      
        public Rect Oszlop1
        {
            get { return oszlop1; }
        }

        public Oszlop(int x, int y, int w,int h)
        {
            
            oszlop1 = new Rect(x,y, w, h);
        }
        public void ChangeX(int diff)
        {
            oszlop1.X += diff;
            OnPropertyChanged("Oszlop1");
        }

        public void ChangeXYH(int x, int y, int h)
        {
            oszlop1.X = x;
            oszlop1.Y = y;
            oszlop1.Height = h;
            OnPropertyChanged("Oszlop1");
        }
    }
}
