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

namespace AppCUBES.command
{
    static class VarCommand
    {
        public static string refcom ;
        public static int idarticlecom;

    }
    /// <summary>
    /// Logique d'interaction pour dispcommand.xaml
    /// </summary>
    public partial class dispcommand : Window
    {
        List<int> list = new List<int>();
        List<string> refe = new List<string>();
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

        public dispcommand()
        {
            InitializeComponent();
            if (Connect.idjobuser == 1)
            {
                retdispcommand.Content = "Menu";
            }
            else
            {
                retdispcommand.Content = "Déconnecter";
            }
            display_command();
        }

        public void display_command()
        {
            List<Command> comms = new List<Command>();
            List<int> listuser = new List<int>();
            List<string> listnameuser = new List<string>();
            List<int> liststatus = new List<int>();
            List<string> listnamestatus = new List<string>();


            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Commands/displaycommands";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);

                for (int i = 0; i < detail.Count; i++)
                {
                    list.Add(Convert.ToInt32(detail[i]["id_Command"]));
                    listuser.Add(Convert.ToInt32(detail[i]["id_User"]));
                    liststatus.Add(Convert.ToInt32(detail[i]["status_Comman"]));
                    refe.Add(detail[i]["refCommand"].ToString());


                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnameuser?{createparameter(listuser)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listnameuser.Add((detail[i]).ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamestatus?{createparameter(liststatus)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listnamestatus.Add((detail[i]).ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Commands/displaycommands";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    comms.Add(new Command(detail[i]["refCommand"].ToString(), detail[i]["price_Command"].ToString(), detail[i]["date_Command"].ToString(), listnameuser[i], listnamestatus[i]));
                }
            }
                
            gridcommand.ItemsSource = comms;
        }

        public class Command
        {
            public string Reference { get; set; }
            public string Prix { get; set; }
            public string Date { get; set; }
            public string Nom { get; set; }
            public string Etat { get; set; }

            public Command(string reference, string prix,string date, string nom, string etat)
            {
                Reference = reference;
                Prix = prix;
                Date = date;
                Nom = nom;
                Etat = etat;
            }   
        }

        private void retdispcommand_Click(object sender, RoutedEventArgs e)
        {
            if (Connect.idjobuser == 1)
            {
                win1 win2 = new win1();
                win2.Show();
                this.Close();
                return;
            }
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win1 = new MainWindow();
            win1.Show();
            this.Close();
        }

        private void cominfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridcommand.SelectedValue == null)
            {
                rescommand.Text = "Aucune valeur séléctionnée";
                return;
            }
            VarCommand.refcom = refe[gridcommand.SelectedIndex];
            var win = new commandinfo();
            
            win.Show();
        }

        private void validcommand_Click(object sender, RoutedEventArgs e)
        {
            if (gridcommand.SelectedValue == null) 
            {
                rescommand.Text = "Aucune valeur séléctionnée";
                return;
            }
            string json = "";
            int a = list[gridcommand.SelectedIndex];
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Check/checkstatuscommand?id={a}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;

            }

            if ( json == "false")
            {
                rescommand.Text = "La commande est déjà validée";
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Commands/validatecommand?id={a}";
                HttpResponseMessage response = client.PostAsync(parameters,null).Result;
                
            }
            Refresh_comm_Click(sender, e);
        }
        private void Refresh_comm_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_command();
        }

        private void addcommand_Click(object sender, RoutedEventArgs e)
        {
            newcommand win = new newcommand();
            win.Show();
            this.Close();
        }
    }
}
