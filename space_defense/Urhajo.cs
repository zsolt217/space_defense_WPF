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
    public enum hajok
    {
        friend, low_enemy, medium_enemy, high_enemy
    }

    class Urhajo : INotifyPropertyChanged
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
        public event MyUrhajoHalalHandler meghalt;
        public event MyUrhajoKimentHandler kiment;
        public hajok Tipus { get; private set; }
        Rect hajo;
        int eletek;
        int ertek;
        

        public int Ertek
        {
            get { return ertek; }
            set { ertek = value; OnPropertyChanged(); }
        }
        public int Eletek
        {
            get { return eletek; }
            set
            {
                if (value == 0 && meghalt != null) { meghalt(this, new MyUrhajoEventArgs(ertek, Tipus)); }
                eletek = value;
            }
        }
        public Rect Hajo
        {
            get { return hajo; }
        }
        public Urhajo(int x, int y, int w, int h, hajok tipus, int kezdoelet, int ujertek)
        {
            hajo = new Rect(x, y, w, h);
            Tipus = tipus;
            eletek = kezdoelet;
            ertek = ujertek;
        }
        public void ChangeX(int diff)
        {
            hajo.X += diff;
            if (meghalt!=null&&Tipus!=hajok.friend&&hajo.X<=0)
            {
                meghalt(this, new MyUrhajoEventArgs(0,Tipus));
            }
           
            OnPropertyChanged("Hajo");
        }
        public void ChangeY(int diff)
        {
            hajo.Y += diff;
            OnPropertyChanged("Hajo");
        }
        public void OverWriteX(int x)
        {
            hajo.X = x;
            if (kiment != null && hajo.X < 0)
            {
                kiment(this);
            }
            OnPropertyChanged("Hajo");
        }
    }
    public delegate void MyUrhajoKimentHandler(object source);
    public delegate void MyUrhajoHalalHandler(object source, MyUrhajoEventArgs e);
    public class MyUrhajoEventArgs : EventArgs
    {
        public int Pont { get; private set; }
        public hajok Hajotipus { get; private set; }
        public MyUrhajoEventArgs(int pont, hajok hajo)
        {
            Pont = pont;
            Hajotipus = hajo;
        }
    }
}
