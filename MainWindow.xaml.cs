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
using System.Windows.Threading;

namespace UgraloGomb_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int eredmeny;
        private Random veletlen;
        private DateTime kezdoIdo;
        private int maxJaAtekido;
        private DispatcherTimer dtIdozito;
        private bool ervenyes;

        public MainWindow()
        {
            InitializeComponent();
            dtIdozito = new DispatcherTimer {
                Interval = new TimeSpan(0, 0, 0, 0, 500),
                IsEnabled = false
            };
            dtIdozito.Tick += DtIdozito_Tick;

            slCsuszka.Minimum = 100;
            slCsuszka.Maximum = 1500;

            slCsuszka.TickFrequency = 200;
            slCsuszka.SmallChange = 100;
            slCsuszka.LargeChange = 500;

            slCsuszka.Value = 500;

            llMin.Content = slCsuszka.Minimum + " ms";
            llMax.Content = slCsuszka.Maximum + " ms";

            btKapjEl.IsEnabled = false;

            maxJaAtekido = 10;
            pbVegrehajtasjelzo.Minimum = 0;
            pbVegrehajtasjelzo.Maximum = maxJaAtekido;
            pbVegrehajtasjelzo.Value = 0;
            veletlen = new Random();
        }


        private void FeliratKiir()
        {
            Title = string.Format("Találatok: {0}, Időztés: {1,7:F2} ms, Még hátravan: {2, 5:F2} s",
                eredmeny, slCsuszka.Value, Math.Max(0, maxJaAtekido - ElteltIdo()));
        }

        double ElteltIdo()
        {
            DateTime most = DateTime.Now;
            return most.Subtract(kezdoIdo).TotalSeconds;
        }

        private void DtIdozito_Tick(object sender, EventArgs e)
        {
            FeliratKiir();
            pbVegrehajtasjelzo.Value = ElteltIdo();
            if (ElteltIdo() < maxJaAtekido)
            {
                btKapjEl.SetValue(LeftProperty, veletlen.NextDouble() *
                    (cvLap.ActualWidth - btKapjEl.ActualWidth));
                btKapjEl.SetValue(TopProperty, veletlen.NextDouble() *
                    (cvLap.ActualHeight - btKapjEl.ActualHeight));
            }
            else
            {
                dtIdozito.IsEnabled = false;
                btStart.IsEnabled = true;
                btKapjEl.IsEnabled = false;
            }
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            eredmeny = 0;
            kezdoIdo = DateTime.Now;
            pbVegrehajtasjelzo.Value = 0;
            dtIdozito.Interval = new TimeSpan(0, 0, 0, 0, (int)slCsuszka.Value);
            btStart.IsEnabled = false;
            dtIdozito.IsEnabled = true;
            FeliratKiir();
            btKapjEl.IsEnabled = true;
        }

        private void btKapjEl_Click(object sender, RoutedEventArgs e)
        {
            if (!ervenyes) return;
            eredmeny++;
            FeliratKiir();
        }

        private void btKapjEl_MouseEnter(object sender, MouseEventArgs e)
        {
           ervenyes = true;
        }

        private void btKapjEl_MouseLeave(object sender, MouseEventArgs e)
        {
            ervenyes = false;
        }

        private void slCsuszka_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bool vanJatek = dtIdozito.IsEnabled;
            dtIdozito.IsEnabled = false;

            dtIdozito.Interval = new TimeSpan(0, 0, 0, 0, (int)slCsuszka.Value);

            if (vanJatek) dtIdozito.IsEnabled = true;
            FeliratKiir();
        }
    }
}
