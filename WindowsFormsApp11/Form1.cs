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
        private bool IsEditing = false;

        public Form1()
        {
            InitializeComponent();
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
            cmbGender.SelectedIndex = 0; // set default to first item

            // ✅ Set DataGridView columns to auto-size
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.SelectionChanged += dataGridView_SelectionChanged;


            // Load existing employees into the grid
            LoadEmployees();
        }

      

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                IsEditing = true; // now we're editing an existing record
                DataGridViewRow row = dataGridView.SelectedRows[0];
                txtEmployeeID.Text = row.Cells["employee_id"].Value.ToString();
                txtLname.Text = row.Cells["last_name"].Value.ToString();
                txtFname.Text = row.Cells["first_name"].Value.ToString();
                txtMname.Text = row.Cells["middle_name"].Value.ToString();
                cmbGender.Text = row.Cells["gender"].Value.ToString();
                dtpBirthday.Value = Convert.ToDateTime(row.Cells["dob"].Value);
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtDept.Text = row.Cells["department"].Value.ToString();
                txtPos.Text = row.Cells["position"].Value.ToString();
            }
        }
        private void ClearFields()
        {
            txtEmployeeID.Clear();
            txtLname.Clear();
            txtFname.Clear();
            txtMname.Clear();
            cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            txtEmail.Clear();
            txtDept.Clear();
            txtPos.Clear();
            txtEmployeeID.Focus();
            IsEditing = false; // back to adding new record
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
                txtMname.Focus();
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
                // ✅ Validate Email
                if (!txtEmail.Text.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Please enter a valid Gmail address (must end with @gmail.com).",
                                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus(); // stay on Email field
                    txtEmail.SelectAll(); // highlight for correction
                    return;
                }
                // If valid → go to next field
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
                txtPos.Focus();
                SaveEmployee(); // call save function directly
            }
        }

        private void txtPos_Leave(object sender, EventArgs e)
        {
            // ✅ Only auto-save if adding a new record (employee ID not in DB)
            if (!IsEditing) // flag to track editing mode
            {
                SaveEmployee();
            }
        }

        // 🔹 Centralized Save Logic
        private void SaveEmployee()
        {
            // ✅ Check if all fields are filled
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
                string.IsNullOrWhiteSpace(txtLname.Text) ||
                string.IsNullOrWhiteSpace(txtFname.Text) ||
                string.IsNullOrWhiteSpace(txtMname.Text) ||
                string.IsNullOrWhiteSpace(cmbGender.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDept.Text) ||
                string.IsNullOrWhiteSpace(txtPos.Text))
            {
                return; // don’t save if incomplete
            }

            // ✅ Email validation
            if (!txtEmail.Text.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please enter a valid Gmail address (must end with @gmail.com).",
                                "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

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

                    // ✅ Clear fields after save
                    txtEmployeeID.Clear();
                    txtLname.Clear();
                    txtFname.Clear();
                    txtMname.Clear();
                    cmbGender.SelectedIndex = -1;
                    dtpBirthday.Value = DateTime.Now;
                    txtEmail.Clear();
                    txtDept.Clear();
                    txtPos.Clear();

                    txtEmployeeID.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                // Refresh DataGridView immediately
                LoadEmployees();  // <-- THIS IS THE KEY
            }
        }
        private void LoadEmployees()
        {
            string connStr = "server=localhost;user=root;password=;database=employee_db;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT employee_id, last_name, first_name, middle_name, gender, dob, email, department, position FROM employees";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No records found in the database.");
                    }

                    dataGridView.DataSource = dt; // bind to grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employees: " + ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchEmployees();
        }

        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;   // prevent ding sound
                btnSearch.PerformClick();    // trigger the Search button click
            }
        }

        private void SearchEmployees()
        {
            string searchText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadEmployees(); // show all if search box is empty
                return;
            }

            string connStr = "server=localhost;user=root;password=;database=employee_db;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT employee_id, last_name, first_name, middle_name, gender, dob, email, department, position " +
                                 "FROM employees " +
                                 "WHERE last_name LIKE @search OR CONCAT(first_name, ' ', last_name) LIKE @search";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching employees: " + ex.Message);
                }
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            string empID = dataGridView.SelectedRows[0].Cells["employee_id"].Value.ToString();

            string connStr = "server=localhost;user=root;password=;database=employee_db;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE employees SET last_name=@last_name, first_name=@first_name, middle_name=@middle_name, " +
                                 "gender=@gender, dob=@dob, email=@email, department=@department, position=@position " +
                                 "WHERE employee_id=@employee_id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@employee_id", empID);
                    cmd.Parameters.AddWithValue("@last_name", txtLname.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFname.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMname.Text);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@dob", dtpBirthday.Value);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@department", txtDept.Text);
                    cmd.Parameters.AddWithValue("@position", txtPos.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Employee updated successfully!");

                    // 🔹 Update only the selected row in the DataGridView
                    DataGridViewRow row = dataGridView.SelectedRows[0];
                    row.Cells["last_name"].Value = txtLname.Text;
                    row.Cells["first_name"].Value = txtFname.Text;
                    row.Cells["middle_name"].Value = txtMname.Text;
                    row.Cells["gender"].Value = cmbGender.Text;
                    row.Cells["dob"].Value = dtpBirthday.Value;
                    row.Cells["email"].Value = txtEmail.Text;
                    row.Cells["department"].Value = txtDept.Text;
                    row.Cells["position"].Value = txtPos.Text;

                    // ✅ No need to reload the whole DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating employee: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Make sure a row is selected
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected employee's ID
            string empID = dataGridView.SelectedRows[0].Cells["employee_id"].Value.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string connStr = "server=localhost;user=root;password=;database=employee_db;";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();

                        string sql = "DELETE FROM employees WHERE employee_id=@employee_id";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@employee_id", empID);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Employee deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the DataGridView
                        LoadEmployees();

                        // Clear input fields
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadEmployees(); // reload data into DataGridView
        }
    }
}
