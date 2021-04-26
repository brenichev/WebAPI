using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using NotebookAPI.Models;

namespace NotebookClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const string _baseAddress = "http://localhost:12101/";

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                response = client.GetAsync("api/People").Result;
                if (response.IsSuccessStatusCode)
                {
                    Person[] reports = response.Content.ReadAsAsync<Person[]>().Result;
                    foreach (var p in reports)
                    {
                        var item = new ListViewItem(new[] { p.Firstname, p.Secondname, p.BirthDay.ToShortDateString() });
                        item.Tag = p.Id;
                        listView.Items.Add(item);
                    }
                }
            }
        }

        private void Delete(int delete)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2728/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync("api/People/" + delete).Result;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                int id = (int)listView.SelectedItems[0].Tag;

                Delete(id);

                Update();
            }
        }

        private void Add()
        {
            Person newReport = new Person() { Firstname = "Коля", BirthDay = DateTime.Parse("01.02.2003") };


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("api/People", newReport).Result;

            }
        }

    }
}
