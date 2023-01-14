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
    public partial class job : Window
    {
        List<int> list = new List<int>();
        public int c = 0;
        public job()
        {
            InitializeComponent();
            display_job();
        }
        public void display_job()
        {
            List<Job> jobs = new List<Job>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = "Display/displayjob";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            JArray detail = JArray.Parse(json);

            for (int i = 0; i < detail.Count; i++)
            {
                jobs.Add(new Job(detail[i]["jobName"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["iD_Job"]));
            }
            datajob.ItemsSource = jobs;

        }

        private void refreshjob_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_job();
        }

        private void precedentjob_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        private void deljob_Click(object sender, RoutedEventArgs e)
        {
            if (datajob.SelectedItem == null)
            {
                jobconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datajob.SelectedIndex];
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Check/checkjobexist?a={a}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;
                
            }
            if (json == "true")
            {
                jobconditionselect.Text = "Impossible de supprimer la valeur elle est assignée a des employés existants";
                return;
            }
            using (HttpClient client = new HttpClient()) 
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Delete/delete_job?ID={a}";
                HttpResponseMessage response = client.DeleteAsync(parameters).Result;
                jobconditionselect.Text = string.Empty;
            }
            

           
            refreshjob_Click(sender, e);
        }

        private void putjob_Click(object sender, RoutedEventArgs e)
        {
            if (datajob.SelectedItem == null)
            {
                jobconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[datajob.SelectedIndex];
            var custo = new jobput();
            List<Job> jobs = new List<Job>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displayjobbyid?ID={a}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            custo.nameputjob.Text = detail[0]["jobName"].ToString();   
            custo.invputjob.Text = a.ToString();
            custo.ShowDialog();
            custo.validateputjob.Click += refreshjob_Click;
            jobconditionselect.Text = string.Empty;
        }

        private void addjob_Click(object sender, RoutedEventArgs e)
        {
            var family = new jobadd();
            family.validatejob.Click += refreshjob_Click;
            family.ShowDialog();
        }
    }

    public class Job
    {

        public string Nom { get; set; }


        public Job(string nom)
        {
            Nom = nom;
        }
    }
}
