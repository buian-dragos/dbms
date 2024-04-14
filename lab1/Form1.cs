using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace lab1
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter daOrder;
        SqlDataAdapter daCustomer;
        DataSet dset;
        BindingSource bsOrder;
        BindingSource bsCustomer;

        SqlCommandBuilder cmdBuilder1;
        SqlCommandBuilder cmdBuilder2;

        string queryOrder;
        string queryCustomer;

        string table1;
        string table2;

        public Form1()
        {
            InitializeComponent();
            FillData();
        }

        void FillData()
        {
            conn = new SqlConnection(getConnectionString());

            table1 = ConfigurationManager.AppSettings["table1"];
            table2 = ConfigurationManager.AppSettings["table2"];

            queryCustomer = "SELECT * FROM Customer";
            queryOrder = "SELECT * FROM Orders";

            daCustomer = new SqlDataAdapter(queryCustomer, conn);
            daOrder = new SqlDataAdapter(queryOrder, conn);

            dset = new DataSet();

            daCustomer.Fill(dset, "Customer");
            daOrder.Fill(dset, "Orders");

            cmdBuilder1 = new SqlCommandBuilder(daCustomer);
            cmdBuilder2 = new SqlCommandBuilder(daOrder);


            dset.Relations.Add("CustomerOrder",
                dset.Tables["Customer"].Columns["customer_id"],
                dset.Tables["Orders"].Columns["customer_id"]);
            /*
            this.dataGridView1.DataSource = dset.Tables["Customer"];
            this.dataGridView2.DataSource = this.dataGridView1.DataSource;
            this.dataGridView2.DataMember = "CustomerOrder";
            */

            bsCustomer = new BindingSource();
            bsCustomer.DataSource = dset.Tables["Customer"];
            bsOrder = new BindingSource(bsCustomer, "CustomerOrder");

            this.dataGridView1.DataSource = bsCustomer;
            this.dataGridView2.DataSource = bsOrder;


            cmdBuilder1.GetUpdateCommand();
            cmdBuilder2.GetUpdateCommand();
        }

        string getConnectionString()
        {
            return "Data Source=DBSB\\MS_SQLSERVER;Initial Catalog=Furniture Shop;" +
                "Integrated Security=true;";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*MessageBox.Show(ConfigurationManager.AppSettings["greeting"]);*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            daCustomer.Update(dset, "Customer");
            daOrder.Update(dset, "Orders");
        }
    }
}
