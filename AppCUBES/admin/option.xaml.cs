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

namespace AppCUBES.admin
{
    /// <summary>
    /// Logique d'interaction pour option.xaml
    /// </summary>
    public partial class option : Window
    {
        public option()
        {
            InitializeComponent();
            List<string> list = new List<string>() { "Activer", "Desactiver" } ;
            commandautoselect.ItemsSource = list;
        }

        private void retouroption_Click(object sender, RoutedEventArgs e)
        {
            win1 win = new win1();
            win.Show();
            this.Close();
        }

        private void commandauto_Click(object sender, RoutedEventArgs e)
        {
            if(commandautoselect.SelectedValue =="Activer")
            {
                return;
            }
        }

        private void changestockminadd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
