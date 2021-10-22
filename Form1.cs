using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FoleyCommaDelimter
{
    public partial class Foley : Form
    {
        public class Customer
        {
            //basic get sets for the Customer class data
            public string customerID    { get; set; }
            public string firstName     { get; set; }
            public string lastName      { get; set; }
            public string address       { get; set; }
            public void Output()
            {
                //output to table, table not made yet
            }
        }

        //Object list for table output using foreach
        public List<Customer> Customers = new List<Customer>();

        public Foley()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                if (Path.GetExtension(file) != ".csv")
                {
                    MessageBox.Show("The file you have selected must be in .csv format", "Error with file format");
                    return;
                }
                var fileContents = File.ReadAllLines(file);
                //Removing first element (Headers)
                fileContents = fileContents.Skip(0).ToArray();
                try
                {
                    foreach(string i in fileContents)
                    {
                        //Regex to remove all commas that exist outside quotes
                        List<string> customerInfo = Regex.Split(i, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))").ToList();
                        Customer Customer = new Customer();

                        //Substrings for all except ID in order to trim leading and trailing quotes (checks if there is a quote first)
                        Customer.customerID = customerInfo[0];

                        //Firstname
                        if(customerInfo[1].FirstOrDefault() == '"' && customerInfo[1].EndsWith("\""))
                        {
                            Customer.firstName = customerInfo[1].Substring(1, customerInfo[1].Length - 2);
                        }
                        else
                        {
                            Customer.firstName = customerInfo[1];
                        }

                        //Lastname
                        if (customerInfo[2].FirstOrDefault() == '"' && customerInfo[2].EndsWith("\""))
                        {
                            Customer.lastName = customerInfo[2].Substring(1, customerInfo[2].Length - 2);
                        }
                        else
                        {
                            Customer.lastName = customerInfo[2];
                        }

                        //Address
                        if (customerInfo[3].FirstOrDefault() == '"' && customerInfo[3].EndsWith("\""))
                        {
                            Customer.address = customerInfo[3].Substring(1, customerInfo[3].Length - 2);
                        }
                        else
                        {
                            Customer.address = customerInfo[3];
                        }

                        Customers.Add(Customer);
                    }
                    //Remove headers from object list
                    Customers.RemoveAt(0);
                    //Display list in data grid view
                    dgvCustomers.DataSource = Customers;
                    //Rename column headers
                    dgvCustomers.Columns[0].HeaderText = "Customer ID";
                    dgvCustomers.Columns[0].HeaderText = "First Name";
                    dgvCustomers.Columns[0].HeaderText = "Last Name";
                    dgvCustomers.Columns[0].HeaderText = "Address";

                }
                catch (IOException)
                {
                    MessageBox.Show("There was an unexpected error, please try again", "Unexpected Error");
                    return;
                }
            }
        }
    }
}
