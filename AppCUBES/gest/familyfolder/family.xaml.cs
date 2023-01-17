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
    /// Interaction logic for family.xaml
    /// </summary>
    public partial class family : Window
    {
        List<int> list = new List<int>();
        
        public family()
        {
            InitializeComponent();
            display_family();
        }
        public void display_family()
        {
            List<Family> families = new List<Family>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = "Display/displayfamily";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            JArray detail = JArray.Parse(json);

            for (int i = 0; i < detail.Count; i++)
            {
                families.Add(new Family(detail[i]["nameFamily"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["iD_Family"]));
            }
            datafamily.ItemsSource = families;

        }

        private void refreshfamily_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_family();
        }

        private void precedentfamily_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        private void delfamily_Click(object sender, RoutedEventArgs e)
        {
            if (datafamily.SelectedItem == null)
            {
                familyconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datafamily.SelectedIndex];
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters2 = $"Check/checkfamilyexist?a={a}";
                HttpResponseMessage response = client.GetAsync(parameters2).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                if (json == "true")
                {
                    familyconditionselect.Text = "Impossible de supprimer la valeur elle est assignée a des articles existants";
                    return;
                }
            }
            using (HttpClient client = new HttpClient()) 
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Delete/delete_family?ID={a}";
                HttpResponseMessage response = client.DeleteAsync(parameters).Result;
                familyconditionselect.Text = string.Empty;
            }
            

            refreshfamily_Click(sender, e);
        }

        private void putfamily_Click(object sender, RoutedEventArgs e)
        {
            if (datafamily.SelectedItem == null)
            {
                familyconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datafamily.SelectedIndex];
            var win = new familyput();
            List<Family> fams = new List<Family>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displayfamilybyid?ID={a}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            win.nameputfamily.Text = detail[0]["nameFamily"].ToString();   
            win.invputfamily.Text = a.ToString();
            win.validputfamily.Click += refreshfamily_Click;

            win.ShowDialog();
            familyconditionselect.Text = string.Empty;
        }

        private void addfamily_Click(object sender, RoutedEventArgs e)
        {
            var family = new familyadd();
            family.valfamefamily.Click += refreshfamily_Click;
            family.ShowDialog();
        }
    }

    public class Family
    {
        public string Nom { get; set; }
        public Family(string nom)
        {
            Nom = nom;
        }
    }
}
