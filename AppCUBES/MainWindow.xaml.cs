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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppCUBES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Connect/connectemp?username={login.Text}&password={password.Password}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            if (json == "true")
            {
                win1 win1 = new win1();
                win1.Show();
                this.Close();
                return;
            }
            res.Text = "Mauvais identifiant ou mot de passe, réessayez";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
