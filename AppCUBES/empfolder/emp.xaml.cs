using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
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
using static System.Net.WebRequestMethods;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour cust.xaml
    /// </summary>
    public partial class emp : Window
    {
        List<int> list = new List<int>();
        public int c = 0;
        public emp()
        {
            InitializeComponent();
            display_emp();          
        }
        public string createparameter(List<int> list)
        {
            string a = "";
            foreach (int item in list)
            {
                a += $"listOfIds={item}&";
            }
            var output = a.Remove(a.Length - 1);
            return output;
        }
        public void display_emp()
        {
            List<Emp> emps = new List<Emp>();
            List<int> listidjob = new List<int>();
            List<string> listnamejob = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayemployer";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);

                for (int i = 0; i < detail.Count; i++)
                {
                    listidjob.Add(Convert.ToInt32(detail[i]["idjob"]));
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamejob?{createparameter(listidjob)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listnamejob.Add((detail[i]).ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayemployer";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);

                for (int i = 0; i < detail.Count; i++)
                {
                    emps.Add(new Emp(detail[i]["logInEmp"].ToString(), detail[i]["nameEmp"].ToString(), detail[i]["firstNameEmp"].ToString(), listnamejob[i]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Employer"]));
                }
            }
            
            empdata.ItemsSource = emps;
            
        }



        private void addemp_Click(object sender, RoutedEventArgs e)
        {
            var custo = new empadd();
            custo.empval.Click += Refreshemp_Click;
            custo.Show();
        }

        public void Refreshemp_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_emp();
            
        }

        private void Deleteemp_Click(object sender, RoutedEventArgs e)
        {
            if (empdata.SelectedItem == null) {
                empconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[empdata.SelectedIndex];
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Delete/delete_emp?ID={a}";
            HttpResponseMessage response = client.DeleteAsync(parameters).Result;
            empconditionselect.Text = string.Empty;
            Refreshemp_Click(sender, e);
        }

        private void putemp_Click(object sender, RoutedEventArgs e)
        {
            if (empdata.SelectedItem == null)
            {
                empconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[empdata.SelectedIndex];
            int b = 0;
            empput empo = new empput();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/displayemployersbyid?ID={a}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = $"[{response.Content.ReadAsStringAsync().Result}]";
                JArray detail = JArray.Parse(json);
                empo.emploginput.Text = detail[0]["logInEmp"].ToString();         
                empo.emppasswordput.Text = detail[0]["passWordEmp"].ToString();
                empo.empnameput.Text = detail[0]["nameEmp"].ToString();
                empo.empfirstnameput.Text = detail[0]["firstNameEmp"].ToString();
                b = Convert.ToInt32(detail[0]["idjob"]);

                empo.inv.Text = a.ToString();

            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getnamejob?ID={b}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                empo.empjobput.SelectedValue = json;
            }
            empo.Show();
            empo.empvalput.Click += Refreshemp_Click;
            empconditionselect.Text = string.Empty;
        }

        private void precedentemp_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        
    }

    public class Emp
    {
        public string Email{ get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Metier { get; set; } 

        public Emp (string login, string name, string firsname, string job)
        {
            Email = login;
            Nom = name;
            Prenom = firsname;
            Metier = job;
        }
    }
}
