using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace model_practic
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        DataSet ds;
        SqlDataAdapter daUsers, daPosts;
        SqlCommandBuilder cbPosts;
        BindingSource bsUsers, bsPosts;
        DataRelation dr;


        public Form1()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            daPosts.Update(ds, "Posts");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(getConnectionString());
            ds = new DataSet();
            daUsers = new SqlDataAdapter("SELECT * FROM Users", conn);
            daPosts = new SqlDataAdapter("SELECT * FROM Posts", conn);

            cbPosts = new SqlCommandBuilder(daPosts);

            daUsers.Fill(ds, "Users");
            daPosts.Fill(ds, "Posts");

            dr = new DataRelation("FK_Posts_Users", ds.Tables["Users"].Columns["userID"], ds.Tables["Posts"].Columns["userID"]);
            
            ds.Relations.Add(dr);

            bsUsers = new BindingSource();
            bsUsers.DataSource = ds;
            bsUsers.DataMember = "Users";

            bsPosts = new BindingSource();
            bsPosts.DataSource = bsUsers;
            bsPosts.DataMember = "FK_Posts_Users";

            dgvUsers.DataSource = bsUsers;
            dgvPosts.DataSource = bsPosts;

        }

        string getConnectionString()
        {
            return "Data Source=DBSB\\MS_SQLSERVER;Initial Catalog=model_practic;" +
                "Integrated Security=true;";
        }
    }
}
