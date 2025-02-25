using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace c_tutor
{
    public partial class Form1 : Form
    {

        SqlConnection con = new SqlConnection("Server=DESKTOP-QKNE475;Database=hr; Integrated Security=true");
        public Form1()
        { 
            InitializeComponent();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {

            //con.Open();
            //check validation 
            if (txtbi.Text == "" || txtbun.Text == "" || txtbup.Text == "" || txtbur.Text == "" || dtprd.Text == "" || txtbr.Text == "")
                MessageBox.Show("Complete the form first");
            else
                //check for duplication
            con.Open();
            SqlCommand sqlcmd = new SqlCommand("select * from usertbl where id=@uid", con);
            sqlcmd.Parameters.AddWithValue("@uid", txtbi.Text);
            SqlDataReader sqldr = sqlcmd.ExecuteReader(); 
            if (sqldr.HasRows)
            {
                MessageBox.Show("User id already exist please try again!");

            }
            else
            {
                 sqldr.Close(); 
                int status = 1;
                SqlCommand sqlcm = new SqlCommand("insert into usertbl values('" + txtbi.Text + "','" + txtbun.Text + "','" + txtbup.Text + "','" + txtbur.Text + "','" + dtprd.Value.ToShortDateString() + "','" + txtbur.Text + "', '" + status + "')", con);
                sqlcm.ExecuteNonQuery();
                MessageBox.Show("Data Successfully Added", "Data Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbi.Text = "";
                txtbun.Text = "";
                txtbup.Text = "";
                txtbur.Text = "";
                txtbr.Text = "";
                txtbi.Focus();
            }
            con.Close(); 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //view user data from usertbl during Form1_Load
            viwdata("");
        }
        #region "view user data from usertbl"
        public void viwdata(string svd)
        {
            try
            {
                String stv = ("select * from usertbl where concat(id,username) like '%" + svd + "%' and status = 1");
                SqlCommand sqlcd = new SqlCommand(stv, con);
                SqlDataAdapter sqlda = new SqlDataAdapter(sqlcd);
                DataTable dataTable = new DataTable();
                sqlda.Fill(dataTable);
                dataGridViewuserdata.DataSource = dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("Error try again");
            }
        }
        #endregion
        #region "Update user data"
        private void btnupdate_Click(object sender, EventArgs e)
        {
            //check validation 
            if (txtbi.Text == "")
                MessageBox.Show("Select user data from datagridview first");
            else
             //check for user data availablity
                con.Open();
            SqlCommand sqlcmd = new SqlCommand("select * from usertbl where id=@uid", con);
            sqlcmd.Parameters.AddWithValue("@uid", txtbi.Text);
            SqlDataReader sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                //update user data
                sqldr.Close();
                SqlCommand sqlcup = new SqlCommand("update usertbl set username='"+ txtbun.Text +"',userrole='"+ txtbur.Text +"', registerdate='"+ dtprd.Value.ToShortDateString()+"', remark='"+ txtbr.Text +"' where id='"+txtbi.Text+"'",con);
                sqlcup.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated","Data Updated",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //clear the form
                txtbi.Text = "";
                txtbun.Text = "";
                txtbup.Text = "";
                txtbur.Text = "";
                txtbr.Text = "";
                txtbi.Focus();
            }
            else
            {
                MessageBox.Show("Please try again");
            }
            con.Close();
        }

    private void dataGridViewuserdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedrow;
                selectedrow = dataGridViewuserdata.Rows[index];
                //0 to 5 indicate the index of field in user table
                txtbi.Text = selectedrow.Cells[0].Value.ToString();
                txtbun.Text=selectedrow.Cells[1].Value.ToString();
                txtbup.Text = selectedrow.Cells[2].Value.ToString();
                txtbur.Text = selectedrow.Cells[3].Value.ToString();
                dtprd.Text = selectedrow.Cells[4].Value.ToString();
                txtbr.Text = selectedrow.Cells[5].Value.ToString();
            }
            catch (Exception)
            {

                MessageBox.Show("Error please try again");
            }
        }
        #endregion
 
        private void btndelete_Click(object sender, EventArgs e)
        {
            //check feild
            if (txtbi.Text == "")
            {
                MessageBox.Show("Enter user id to delete");
            }
            //confirm delete
            else if (MessageBox.Show("Confirm to delete ?", "Data Deleted", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from usertbl where id = '" + txtbi.Text + "'", con);
                //check as data exist
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("User data successfully deleted");
                }
                else
                {
                    MessageBox.Show("No such user id exist!");
                }
                con.Close();
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtbi.Text = "";
            txtbun.Text = "";
            txtbup.Text = "";
            txtbur.Text = "";
            txtbr.Text = "";
            txtbi.Focus();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
            Refresh();
        }
    }
   }

