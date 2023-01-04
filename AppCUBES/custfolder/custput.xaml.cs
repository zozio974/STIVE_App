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
    /// Logique d'interaction pour custput.xaml
    /// </summary>
    public partial class custput : Window
    {
        public custput()
        {
            InitializeComponent();
        }

        public void custval_Click(object sender, RoutedEventArgs e)
        {
            cust custo = new cust();    
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"PutTable/putcustomer?ID={inv.Text}&login={custloginput.Text}&password={custpasswordput.Text}&name={custnameput.Text}&firstname={custfirstnameput.Text}";
            HttpResponseMessage response = client.PutAsync(parameters,null).Result;
            
            this.Close();
        }

        private void returncust2_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
