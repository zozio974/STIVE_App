using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
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
    public partial class supplier : Window
    {
        List<int> list = new List<int>();
        public int c = 0;
        public supplier()
        {
            InitializeComponent();
            display_family();
        }
        public void display_family()
        {
            List<Supplier> suppliers = new List<Supplier>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = "Display/displaysup";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            JArray detail = JArray.Parse(json);

            for (int i = 0; i < detail.Count; i++)
            {
                suppliers.Add(new Supplier(detail[i]["name"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["id"]));
            }
            datasupplier.ItemsSource = suppliers;

        }

        private void refreshsupplier_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_family();
        }

        private void precedentsupplier_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        private void delsupplier_Click(object sender, RoutedEventArgs e)
        {
            if (datasupplier.SelectedItem == null)
            {
                supplierconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datasupplier.SelectedIndex];
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Check/checksupplierexist?a={a}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;
                
            }
            if (json == "true")
            {
                supplierconditionselect.Text = "Impossible de supprimer la valeur elle est assignée a des articles existants";
                return;
            }
            using (HttpClient client = new HttpClient()) 
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Delete/delete_sup?ID={a}";
                HttpResponseMessage response = client.DeleteAsync(parameters).Result;
                supplierconditionselect.Text = string.Empty;
            }
            

           
            refreshsupplier_Click(sender, e);
        }

        private void putsupplier_Click(object sender, RoutedEventArgs e)
        {
            if (datasupplier.SelectedItem == null)
            {
                supplierconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datasupplier.SelectedIndex];
            var custo = new supplierput();
            List<Family> fams = new List<Family>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displaysupbyid?ID={a}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            custo.nameputsupplier.Text = detail[0]["name"].ToString();   
            custo.invputsupplier.Text = a.ToString();
            custo.ShowDialog();
            custo.validateputsupplier.Click += refreshsupplier_Click;
            supplierconditionselect.Text = string.Empty;
        }

        private void addsupplier_Click(object sender, RoutedEventArgs e)
        {
            var family = new supplieradd();
            family.validatesupplier.Click += refreshsupplier_Click;
            family.ShowDialog();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    public class Supplier
    {

        public string Nom { get; set; }


        public Supplier(string nom)
        {
            Nom = nom;
        }
    }
}
