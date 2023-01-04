using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public partial class cust : Window
    {
        List<int> list = new List<int>();
        public int c = 0;
        public cust()
        {
            InitializeComponent();
            display_cust();          
        }
        public void display_cust()
        {
            List<Cust> custs = new List<Cust>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = "Display/displaycustomers";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            JArray detail = JArray.Parse(json);

            for (int i = 0; i < detail.Count; i++)
            {
                custs.Add(new Cust(detail[i]["logInCus"].ToString(), detail[i]["nameCus"].ToString(), detail[i]["firstNameCus"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["iD_Customer"]));
            }
            custdata.ItemsSource = custs;
            
        }



        private void addcust_Click(object sender, RoutedEventArgs e)
        {
            var custo = new custadd();
            custo.custval.Click += Refreshcust_Click;
            custo.Show();
        }

        public void Refreshcust_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_cust();
            
        }

        private void Deletecust_Click(object sender, RoutedEventArgs e)
        {
            if (custdata.SelectedItem == null) {
                custconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[custdata.SelectedIndex];
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Delete/delete_cust?ID={a}";
            HttpResponseMessage response = client.DeleteAsync(parameters).Result;
            custconditionselect.Text = string.Empty;
            Refreshcust_Click(sender, e);
        }

        private void putcust_Click(object sender, RoutedEventArgs e)
        {
            if (custdata.SelectedItem == null)
            {
                custconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[custdata.SelectedIndex];
            var custo = new custput();
            List<Cust> custs = new List<Cust>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displaycustomersbyid?ID={a}";        
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{ response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            custo.custloginput.Text= detail[0]["logInCus"].ToString();
            custo.custpasswordput.Text = detail[0]["passWordCus"].ToString();
            custo.custnameput.Text = detail[0]["nameCus"].ToString();
            custo.custfirstnameput.Text = detail[0]["firstNameCus"].ToString();
            custo.inv.Text = a.ToString(); 
            custo.Show();
            custo.custvalput.Click += Refreshcust_Click;
            custconditionselect.Text = string.Empty;
        }

        private void precedentcust_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        
    }

    public class Cust
    {
        public string Email{ get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public Cust (string login, string name, string firsname)
        {
            Email = login;
            Nom = name;
            Prenom = firsname;
        }
    }
}
