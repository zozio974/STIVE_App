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
    public partial class win2 : Window
    {
        public win2()
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
            win1 win = new win1();
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
    }
}
