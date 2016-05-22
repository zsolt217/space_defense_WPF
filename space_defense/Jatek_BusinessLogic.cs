using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace space_defense
{
    public delegate void AblakBezar(object source, ErtekMentesEventArgs vmi);
    public class ErtekMentesEventArgs : EventArgs
    {
        int eredmeny;

        public int Eredmeny
        {
            get { return eredmeny; }
        }
        public ErtekMentesEventArgs(int ertek)
        {
            eredmeny = ertek;
        }
    }
    class Jatek_BusinessLogic
    {
        //oszlopok
        int canvasheight;
        int canvaswidth;
        BindingList<Oszlop> oszlopok;
        static Random R;
        DispatcherTimer oszlopmozgas;
        private const int minOszloptavolsag = 40;
        public DispatcherTimer Oszlopmozgas
        {
            get { return oszlopmozgas; }
        }
        //jatekosok
        BindingList<Urhajo> urhajok;
        Urhajo sajathajo;
        //lovedek
        BindingList<Lovedek> lovedekek;
        DispatcherTimer lovedekmozgas;

        public DispatcherTimer Lovedekmozgas//windowsclosing
        {
            get { return lovedekmozgas; }
        }
        //ellenseg
        DispatcherTimer ellensegaktivitas;

        public event AblakBezar Ablakbezaras;

        public DispatcherTimer Ellensegaktivitas//windowsclosing
        {
            get { return ellensegaktivitas; }
        }
        public Jatek_BusinessLogic(BindingList<Oszlop> oszlopok, int canvasheight, int canvaswidth, BindingList<Urhajo> ujhajok, BindingList<Lovedek> golyok)
        {
            R = new Random();
            this.canvasheight = canvasheight;
            this.canvaswidth = canvaswidth;
            this.oszlopok = oszlopok;
            int elozox = 0;
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)//felső oszlopok
                {
                    elozox = R.Next(elozox + minOszloptavolsag, (canvaswidth / 10) * (int)((i + 2) / 2));
                    oszlopok.Add(new Oszlop(elozox, 0, 20, R.Next(5, (int)(canvasheight * 0.2))));// 20 a szélessége
                }
                else//alsó oszlopok
                {
                    int y = R.Next((int)(canvasheight * 0.8), canvasheight);
                    oszlopok.Add(new Oszlop(elozox, y, 20, canvasheight - y));// 20 a szélessége
                }
            }
            oszlopmozgas = new DispatcherTimer();
            oszlopmozgas.Interval = TimeSpan.FromMilliseconds(100);
            oszlopmozgas.Tick += oszlopmozgas_Tick;
            oszlopmozgas.Start();
            //urhajok
            this.urhajok = ujhajok;
            urhajok.Add(new Urhajo(10, canvasheight / 2, 60, 40, hajok.friend, 3, 0));
            //sajathajo
            sajathajo = urhajok.ElementAt(0);
            sajathajo.meghalt += hajo_meghalt;
            sajathajo.kiment += sajathajo_kiment;
            //lovedekek
            lovedekek = golyok;
            lovedekmozgas = new DispatcherTimer();
            lovedekmozgas.Interval = TimeSpan.FromMilliseconds(100);
            lovedekmozgas.Tick += lovedekmozgas_Tick;
            //ellenseg
            ellensegaktivitas = new DispatcherTimer();
            ellensegaktivitas.Interval = TimeSpan.FromMilliseconds(100);
            ellensegaktivitas.Tick += ellensegaktivitas_Tick;
            ellensegperiodus = 0;
            ellensegaktivitas.Start();
            highmozgasirany = -1;
        }

        void sajathajo_kiment(object source)
        {
            MessageBox.Show("Hagyta, hogy a fal kitolja a pályáról, az eredménye nem kerül rögzítésre.");
            Ablakbezaras(this, new ErtekMentesEventArgs(0)); //mentes nélkül ha nulla
        }

        /*
         *10.-nél létrehoz low-kat--5%os eséllyel lő--random mozog --1 élet
         *50. nél mediumot--10%os eséllyel lő --kis lépéssel irányra tart --2 élet
         *300 nál high-t--15%os eséllyel lő -- nagy lépéssel irányra tart-- 5 élet
         */
        int ellensegperiodus;
        int highmozgasirany;
        void ellensegaktivitas_Tick(object sender, EventArgs e)
        {
            if (ellensegperiodus % 30 == 0)
            {
                Urhajo hajo = new Urhajo((canvaswidth - 70), R.Next((int)(canvasheight * 0.2), (int)(canvasheight * 0.8)), 60, 40, hajok.low_enemy, 1, 5);
                hajo.meghalt += hajo_meghalt;
                urhajok.Add(hajo);
            }
            else if (ellensegperiodus % 359 == 0) //primszam
            {
                Urhajo hajo = new Urhajo((canvaswidth - 70), R.Next((int)(canvasheight * 0.2), (int)(canvasheight * 0.8)), 60, 40, hajok.medium_enemy, 1, 5);
                hajo.meghalt += hajo_meghalt;
                urhajok.Add(hajo);
            }
            else if (ellensegperiodus % 1117 == 0)//primszam
            {
                Urhajo hajo = new Urhajo((canvaswidth - 70), R.Next((int)(canvasheight * 0.2), (int)(canvasheight * 0.8)), 60, 40, hajok.high_enemy, 1, 5);
                hajo.meghalt += hajo_meghalt;
                urhajok.Add(hajo);
                ellensegperiodus = 0;
            }
            if (urhajok.Count > 0)
            {
                for (int i = 1; i < urhajok.Count; i++)
                {
                    int elojel;
                    switch (urhajok.ElementAt(i).Tipus)
                    {
                        case hajok.low_enemy:
                            EllensegHajoMozgas(R.Next(-8, -3), R.Next(-5, 5), urhajok.ElementAt(i));
                            if (R.Next(20) < 1) EllensegLoves(urhajok.ElementAt(i));
                            break;
                        case hajok.medium_enemy:
                            elojel = (int)sajathajo.Hajo.Y - (int)urhajok.ElementAt(i).Hajo.Y; //negatív ha felfele, pozitív ha lefele
                            if (elojel < 0)
                            {
                                EllensegHajoMozgas(R.Next(-8, -3), R.Next(-5, 0), urhajok.ElementAt(i));
                            }
                            else
                            {
                                EllensegHajoMozgas(R.Next(-8, -3), R.Next(0, 5), urhajok.ElementAt(i));
                            }
                            if (R.Next(20) < 2) EllensegLoves(urhajok.ElementAt(i));
                            break;
                        case hajok.high_enemy:
                            elojel = (int)sajathajo.Hajo.Y - (int)urhajok.ElementAt(i).Hajo.Y; //negatív ha felfele, pozitív ha lefele
                            int utolsoharmados = (int)urhajok.ElementAt(i).Hajo.X - (int)(canvaswidth * 0.6);//negatív akk előrébb jött
                            if (utolsoharmados < 0) highmozgasirany *= highmozgasirany;
                            if ((int)urhajok.ElementAt(i).Hajo.X > canvaswidth) highmozgasirany = -1;
                            if (highmozgasirany == 1)
                            {
                                if (elojel < 0)
                                {
                                    EllensegHajoMozgas(R.Next(15), R.Next(-10, 0), urhajok.ElementAt(i));
                                }
                                else
                                {
                                    EllensegHajoMozgas(R.Next(15), R.Next(0, 10), urhajok.ElementAt(i));
                                }
                            }
                            else
                            {
                                if (elojel < 0)
                                {
                                    EllensegHajoMozgas(R.Next(-15, 0), R.Next(-10, 0), urhajok.ElementAt(i));
                                }
                                else
                                {
                                    EllensegHajoMozgas(R.Next(-15, 0), R.Next(0, 10), urhajok.ElementAt(i));
                                }
                            }
                            if (R.Next(20) < 3) EllensegLoves(urhajok.ElementAt(i));
                            break;
                    }
                }
            }
            ellensegperiodus++;
        }

        void hajo_meghalt(object source, MyUrhajoEventArgs e)
        {
            switch (e.Hajotipus)
            {
                case hajok.friend:
                    Ablakbezaras(this, new ErtekMentesEventArgs(sajathajo.Ertek));//jatek vege
                    break;
                case hajok.low_enemy: sajathajo.Ertek += e.Pont;
                    urhajok.Remove((source as Urhajo));
                    break;
                case hajok.medium_enemy: sajathajo.Ertek += e.Pont;
                    urhajok.Remove((source as Urhajo));
                    break;
                case hajok.high_enemy: sajathajo.Ertek += e.Pont;
                    urhajok.Remove((source as Urhajo));
                    break;
            }

        }

        void lovedekmozgas_Tick(object sender, EventArgs e)
        {
            List<Lovedek> torlendo = new List<Lovedek>();
            foreach (Lovedek item in lovedekek)
            {
                item.ChangeX();
                if (item.Golyo.X < 0 || item.Golyo.X > canvaswidth || OszlopElemUtkozesVizsgalat(item.Golyo) || LovedekHajoUtkozes(item))
                {
                    torlendo.Add(item);
                }
            }
            foreach (Lovedek item in torlendo)
            {
                lovedekek.Remove(item);
            }
            if (lovedekek.Count == 0) lovedekmozgas.Stop();
        }

        void oszlopmozgas_Tick(object sender, EventArgs e)
        {
            foreach (Oszlop item in oszlopok)
            {
                item.ChangeX(-5);
            }
            foreach (Oszlop item in oszlopok)
            {
                if (item.Oszlop1.IntersectsWith(sajathajo.Hajo))
                {
                    sajathajo.OverWriteX((int)item.Oszlop1.X - (int)sajathajo.Hajo.Width - 3);
                }
            }
            if (oszlopok.ElementAt(0).Oszlop1.X < 0)//CSAK ADDIG MŰKÖDIK AMÍG minOszloptavolsag-NÁL KISEBB LÉPÉSSEL MOZOGNAK!
            {
                int elozox = (int)oszlopok.ElementAt(oszlopok.Count - 1).Oszlop1.X;
                Oszlop felso = oszlopok.ElementAt(0);

                //felső oszlop
                elozox = R.Next(elozox + minOszloptavolsag, (canvaswidth / 10) * (int)((oszlopok.Count) / 2));
                oszlopok.RemoveAt(0);
                felso.ChangeXYH(elozox, 0, R.Next(5, (int)(canvasheight * 0.2)));// 20 a szélessége
                oszlopok.Add(felso);
                //alsó oszlopok
                Oszlop also = oszlopok.ElementAt(0);
                int y = R.Next((int)(canvasheight * 0.8), canvasheight);
                also.ChangeXYH(elozox, y, canvasheight - y);
                oszlopok.RemoveAt(0);
                oszlopok.Add(also);
            }
            Jatek_ViewModel.Oszlop_frissit = true;
        }

        public void SajatHajoMozgatas(int x, int y)
        {
            if (y == 0) //x irany
            {
                if ((sajathajo.Hajo.X + x) < (int)canvaswidth / 3 && (sajathajo.Hajo.X + x) > 0 && !OszlopElemUtkozesVizsgalat(sajathajo.Hajo)) sajathajo.ChangeX(x);
            }
            else //y irany
            {
                if ((sajathajo.Hajo.Y + y) > 0 && (sajathajo.Hajo.Bottom + y) < canvasheight && !OszlopElemUtkozesVizsgalat(sajathajo.Hajo))
                    sajathajo.ChangeY(y);
            }
        }
        private void EllensegHajoMozgas(int x, int y, Urhajo hajo)
        {
            if (!OszlopElemUtkozesVizsgalat(hajo.Hajo)) hajo.ChangeX(x);
            if ((hajo.Hajo.Y + y) > 0 && (hajo.Hajo.Bottom + y) < canvasheight && !OszlopElemUtkozesVizsgalat(hajo.Hajo)) hajo.ChangeY(y);
            if(hajo.Hajo.IntersectsWith(sajathajo.Hajo)) Ablakbezaras(this,new ErtekMentesEventArgs(sajathajo.Ertek));
        }
        private bool OszlopElemUtkozesVizsgalat(Rect elem)
        {
            foreach (Oszlop item in oszlopok)
            {
                if (item.Oszlop1.IntersectsWith(elem))
                {
                    return true;
                }
            }
            return false;
        }
        private bool LovedekHajoUtkozes(Lovedek lovedek)
        {
            foreach (Urhajo item in urhajok)
            {
                if (item.Hajo.IntersectsWith(lovedek.Golyo))
                {
                    item.Eletek--;
                    return true;
                }
            }
            return false;
        }
        public void Loves()
        {
            int i = lovedekek.Count;
            lovedekek.Add(new Lovedek((int)(sajathajo.Hajo.X + sajathajo.Hajo.Width), (int)((sajathajo.Hajo.Y + sajathajo.Hajo.Bottom) / 2), 10)); //tizesével mozog
            if (i == 0) lovedekmozgas.Start();

        }
        private void EllensegLoves(Urhajo hajo)
        {
            int i = lovedekek.Count;
            lovedekek.Add(new Lovedek((int)(hajo.Hajo.X - 20), (int)((hajo.Hajo.Y + hajo.Hajo.Bottom) / 2), -10));//negatív tizesével mozog, -20 a lövedék nagysága- baloldal helyzetére van szükség
            if (i == 0) lovedekmozgas.Start();
        }
    }
}
