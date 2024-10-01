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

namespace practicalDBMS
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter daClient;
        SqlDataAdapter daImage;
        DataSet dset;
        BindingSource bsClient;
        BindingSource bsImage;

        SqlCommandBuilder cmdBuilder1;
        SqlCommandBuilder cmdBuilder2;

        string queryClient;
        string queryImage;

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

            table1 = "Client";
            table2 = "MedicalImage";



            queryClient = "SELECT * FROM " + table1;
            queryImage = "SELECT * FROM " + table2;

            daClient = new SqlDataAdapter(queryClient, conn);
            daImage = new SqlDataAdapter(queryImage, conn);

            dset = new DataSet();

            daClient.Fill(dset, table1);
            daImage.Fill(dset, table2);

            cmdBuilder1 = new SqlCommandBuilder(daClient);
            cmdBuilder2 = new SqlCommandBuilder(daImage);

            dset.Relations.Add("ClientImage",
                dset.Tables[table1].Columns["c_id"],
                dset.Tables[table2].Columns["fk_c"]);

            bsClient = new BindingSource();
            bsClient.DataSource = dset.Tables[table1];
            bsImage = new BindingSource(bsClient, "ClientImage");

            this.dgvClients.DataSource = bsClient;
            this.dgvMedicalImages.DataSource = bsImage;


            cmdBuilder1.GetUpdateCommand();
            cmdBuilder2.GetUpdateCommand();
        }

        string getConnectionString()
        {
            return "Data Source=DBSB;Initial Catalog=PracticalDBMS;" +
                "Integrated Security=true;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //daClient.Update(dset, table1);
            daImage.Update(dset, table2);
        }
    }
}
