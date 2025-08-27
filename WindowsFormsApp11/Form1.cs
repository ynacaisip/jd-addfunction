using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
        }

        private void txtEmployeeID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtLname.Focus();
            }
        }

        private void txtLname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtFname.Focus();
            }
        }

        private void txtFname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtFname.Focus();
            }
        }

        private void txtMname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                cmbGender.Focus();
            }
        }

        private void cmbGender_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                dtpBirthday.Focus();
            }
        }

        private void dtpBirthday_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtEmail.Focus();
            }

        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtDept.Focus();
            }
        }

        private void txtDept_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPos.Focus();
            }
        }

        private void txtPos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSave.PerformClick(); // simulate Save click
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string connStr = "server=localhost;user=root;password=;database=employee_db;"; 

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string sql = "INSERT INTO employees " +
                                 "(employee_id, last_name, first_name, middle_name, gender, dob, email, department, position) " +
                                 "VALUES (@employee_id, @last_name, @first_name, @middle_name, @gender, @dob, @email, @department, @position)";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@employee_id", txtEmployeeID.Text);
                    cmd.Parameters.AddWithValue("@last_name", txtLname.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFname.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMname.Text);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@dob", dtpBirthday.Value);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@department", txtDept.Text);
                    cmd.Parameters.AddWithValue("@position", txtPos.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear all fields
                    txtEmployeeID.Clear();
                    txtLname.Clear();
                    txtFname.Clear();
                    txtMname.Clear();
                    cmbGender.SelectedIndex = -1;
                    dtpBirthday.Value = DateTime.Now;
                    txtEmail.Clear();
                    txtDept.Clear();
                    txtPos.Clear();

                    txtEmployeeID.Focus(); // go back to first field
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
