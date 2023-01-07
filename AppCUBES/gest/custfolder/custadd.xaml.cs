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
    /// Logique d'interaction pour custadd.xaml
    /// </summary>
    public partial class custadd : Window
    {
        public custadd()
        {
            InitializeComponent();
        }

        private void custval_Click(object sender, RoutedEventArgs e)
        {
            if(custlogin.Text ==""|| custpassword.Text == "" || custname.Text == "" || custfirstname.Text == "")
            {
                rescustadd.Text = "Un ou plusieurs champs est vide";
                return;
            }
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Add/add_customer?login={custlogin.Text}&password={custpassword.Text}&name={custname.Text}&firstname={custfirstname.Text}";
            HttpResponseMessage response = client.PostAsync(parameters,null).Result;
            cust win = new cust();
            
            
            this.Close();
            
        }

        private void retourcust_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
