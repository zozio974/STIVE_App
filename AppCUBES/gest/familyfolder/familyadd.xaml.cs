using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for familyadd.xaml
    /// </summary>
    public partial class familyadd : Window
    {
        public familyadd()
        {
            InitializeComponent();
        }

        private void returnfamily_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void valfamefamily_Click(object sender, RoutedEventArgs e)
        {
            if (namefamily.Text == ""  )
            {
                rescustadd.Text = "Le champ est vide";
                return;
            }
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Add/add_family?name={namefamily.Text}";
            HttpResponseMessage response = client.PostAsync(parameters, null).Result;
            this.Close();
        }
    }
}
