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
using Newtonsoft.Json.Linq;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour custput.xaml
    /// </summary>
    public partial class empput : Window
    {
        public empput()
        {
            InitializeComponent();
            List<string> list = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayjob";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count(); i++)
                {
                    list.Add(detail[i]["jobName"].ToString());
                }
                empjobput.ItemsSource= list;
            }
        }

        public void empval_Click(object sender, RoutedEventArgs e)
        {
            cust custo = new cust();
            string resjob= "";

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidjob?name={empjobput.SelectedValue}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                resjob = response.Content.ReadAsStringAsync().Result;
            }

                using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutTable/putemployer?ID={inv.Text}&idjob={resjob}&login={emploginput.Text}&password={emppasswordput.Text}&name={empnameput.Text}&firstname={empfirstnameput.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
            }
            
            
            this.Close();
        }

        private void returnemp_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
