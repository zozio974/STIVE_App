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
using System.Windows.Shapes;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour win2.xaml
    /// </summary>
    public partial class gestwin : Window
    {
        public gestwin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var custo = new cust();
            custo.Show();
            this.Close();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void article_Click(object sender, RoutedEventArgs e)
        {
            article win = new article();
            win.Show();
            this.Close();
        }

        private void familybutton_Click(object sender, RoutedEventArgs e)
        {
            family win = new family();
            win.Show();
            this.Close();
        }

        private void supplierbutton_Click(object sender, RoutedEventArgs e)
        {
            supplier win = new supplier();
            win.Show();
            this.Close();
        }

        private void buttonjob_Click(object sender, RoutedEventArgs e)
        {
            job win = new job();
            win.Show();
            this.Close();
        }

        private void buttonemp_Click(object sender, RoutedEventArgs e)
        {
            emp win = new emp();
            win.Show();
            this.Close();
        }
    }
}
