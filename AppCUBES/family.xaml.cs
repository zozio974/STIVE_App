﻿using Newtonsoft.Json.Linq;
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
        public int c = 0;
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

        private void precedent_Click_1(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        private void delfamily_Click_1(object sender, RoutedEventArgs e)
        {
            if (datafamily.SelectedItem == null)
            {
                familyconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datafamily.SelectedIndex];
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Delete/delete_family?ID={a}";
            HttpResponseMessage response = client.DeleteAsync(parameters).Result;
            familyconditionselect.Text = string.Empty;
        }

        private void putfamily_Click_1(object sender, RoutedEventArgs e)
        {
            if (datafamily.SelectedItem == null)
            {
                familyconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datafamily.SelectedIndex];
            var custo = new custput();
            List<Family> fams = new List<Family>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displayfamilybyid?ID={a}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            custo.custnameput.Text = detail[0]["nameFamily"].ToString();   
            custo.inv.Text = a.ToString();
            custo.Show();
            familyconditionselect.Text = string.Empty;
        }

        private void addfamily_Click(object sender, RoutedEventArgs e)
        {
            var family = new familyadd();
            family.Show();
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
