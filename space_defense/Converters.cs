using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace space_defense
{
    class OszlopToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BindingList<Oszlop> oszlopok = null;
            try
            {
                oszlopok = value as BindingList<Oszlop>;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            GeometryGroup g = new GeometryGroup();
            g.Children.Add(new RectangleGeometry(new Rect(0, 0, 1, 1)));
            foreach (Oszlop item in oszlopok)
            {
                g.Children.Add(new RectangleGeometry(item.Oszlop1));
            }
            return g;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class HajokToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                BindingList<Urhajo> hajo = (BindingList<Urhajo>)value;
                DrawingImage image = new DrawingImage();
                DrawingGroup dgroup = new DrawingGroup();
                GeometryDrawing pont = new GeometryDrawing();
                pont.Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1));
                pont.Brush = Brushes.Black;
                dgroup.Children.Add(pont);
                foreach (Urhajo item in hajo)
                {
                    switch (item.Tipus)
                    {
                        case hajok.friend:
                            GeometryDrawing geo = new GeometryDrawing();
                            geo.Geometry = new RectangleGeometry(item.Hajo);
                            geo.Brush = new ImageBrush(new BitmapImage(new Uri("player.png", UriKind.Relative)));
                            dgroup.Children.Add(geo);
                            break;
                        case hajok.low_enemy:
                            GeometryDrawing geo1 = new GeometryDrawing();
                            geo1.Geometry = new RectangleGeometry(item.Hajo);
                            geo1.Brush = new ImageBrush(new BitmapImage(new Uri("low.png", UriKind.Relative)));
                            dgroup.Children.Add(geo1);
                            break;
                        case hajok.medium_enemy:
                            GeometryDrawing geo2 = new GeometryDrawing();
                            geo2.Geometry = new RectangleGeometry(item.Hajo);
                            geo2.Brush = new ImageBrush(new BitmapImage(new Uri("medium.png", UriKind.Relative)));
                            dgroup.Children.Add(geo2);
                            break;
                        case hajok.high_enemy:
                            GeometryDrawing geo3 = new GeometryDrawing();
                            geo3.Geometry = new RectangleGeometry(item.Hajo);
                            geo3.Brush = new ImageBrush(new BitmapImage(new Uri("high.png", UriKind.Relative)));
                            dgroup.Children.Add(geo3);
                            break;
                    }
                }
                image.Drawing = dgroup;
                return image;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Binding.DoNothing; //ha probléma lenne
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    class LovedekekToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                BindingList<Lovedek> lovedek = (BindingList<Lovedek>)value;
                GeometryGroup g = new GeometryGroup();
                foreach (Lovedek item in lovedek)
                {
                    g.Children.Add(new EllipseGeometry(item.Golyo));
                }
                g.FillRule = FillRule.Nonzero;
                return g;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Binding.DoNothing; //ha probléma lenne
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    class EletToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                BindingList<Urhajo> hajo = (BindingList<Urhajo>)value;
                DrawingImage image = new DrawingImage();
                DrawingGroup dgroup = new DrawingGroup();
                GeometryDrawing pont = new GeometryDrawing();
                pont.Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1));
                pont.Brush = Brushes.Black;
                dgroup.Children.Add(pont);
                for (int i = 0; i < hajo.ElementAt(0).Eletek; i++)
                {
                    GeometryDrawing geo = new GeometryDrawing();
                    geo.Geometry = new RectangleGeometry(new Rect((10 + i * 20), 10, 15, 15));
                    geo.Brush = new ImageBrush(new BitmapImage(new Uri("elet.png", UriKind.Relative)));
                    dgroup.Children.Add(geo);
                }
                image.Drawing = dgroup;
                return image;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    class PontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int pont = (value as BindingList<Urhajo>).ElementAt(0).Ertek;
                return pont;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
