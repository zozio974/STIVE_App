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
using System.Net.Http;

namespace AppCUBES
{
    /// <summary>
    /// Interaction logic for familyput.xaml
    /// </summary>
    public partial class jobput : Window
    {
        public jobput()
        {
            InitializeComponent();
        }      
        private void returnjob_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void validateputjob_Click(object sender, RoutedEventArgs e)
        {
            supplier sup = new supplier();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"PutTable/putjob?ID={invputjob.Text}&name={nameputjob.Text}";
            HttpResponseMessage response = client.PutAsync(parameters, null).Result;

            this.Close();
        }
    }
}
