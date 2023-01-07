using Newtonsoft.Json.Linq;
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
    public partial class empadd : Window
    {
        public empadd()
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
            }
            jobempadd.ItemsSource = list;
        }

        private void empval_Click(object sender, RoutedEventArgs e)
        {
            string resjob = "";
            if (emplogin.Text ==""|| emppassword.Text == "" || empname.Text == "" || empfirstname.Text == "" || jobempadd.SelectedValue == null)
            {
                resempadd.Text = "Un ou plusieurs champs est vide";
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidjob?name={jobempadd.SelectedValue}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                resjob = response.Content.ReadAsStringAsync().Result;

            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Add/add_employer?login={emplogin.Text}&password={emppassword.Text}&name={empname.Text}&firstname={empfirstname.Text}&idjob={resjob}";
                HttpResponseMessage response = client.PostAsync(parameters, null).Result;
            }
            emp emp = new emp();
            emp.Refreshemp_Click(sender, e);

            this.Close();
            
        }

        private void precedentemp_Click(object sender, RoutedEventArgs e)
        {           
            this.Close();
        }
    }
}
