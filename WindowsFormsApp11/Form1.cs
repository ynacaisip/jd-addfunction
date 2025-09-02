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
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = 40;

            dataGridView.SelectionChanged += dataGridView_SelectionChanged;

 


            // Load existing employees into the grid
            LoadEmployees();

            txtSearch.KeyDown += txtSearch_KeyDown;
            txtSearch.Leave += txtSearch_Leave;   // ✅ this ensures DataGridView resets when leaving search
            txtSearch.TextChanged += txtSearch_TextChanged;  // ✅ live search

            dataGridView.KeyDown += dataGridView_KeyDown; //press delete in the DataGridView to DELETE record

        

        }

        


        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) // if user presses Delete key
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get employee_id of the selected row
                string empID = dataGridView.SelectedRows[0].Cells["employee_id"].Value.ToString();

                // Ask for confirmation
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

                            ClearFields();    // clear form fields only if deleted
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // ✅ Always refresh grid, whether YES or NO
                LoadEmployees();
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadEmployees(); // show all if search is empty
            }
            else
            {
                SearchEmployees(); // filter as you type
            }
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

                SaveEmployee();   // save record (INSERT or UPDATE)
                LoadEmployees();  // refresh DataGridView immediately
                ClearFields();    // clear fields right after saving
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
        {   // ✅ Check if all fields are filled
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
                    string sql;

                    if (IsEditing)
                    {
                        // 🔹 UPDATE existing record
                        sql = @"UPDATE employees SET 
                            last_name=@last_name, 
                            first_name=@first_name, 
                            middle_name=@middle_name, 
                            gender=@gender, 
                            dob=@dob, 
                            email=@email, 
                            department=@department, 
                            position=@position 
                        WHERE employee_id=@employee_id";
                    }
                    else
                    {
                        // 🔹 INSERT new record
                        sql = @"INSERT INTO employees 
                            (employee_id, last_name, first_name, middle_name, gender, dob, email, department, position) 
                        VALUES 
                            (@employee_id, @last_name, @first_name, @middle_name, @gender, @dob, @email, @department, @position)";
                    }

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

                    // ✅ Show message based on mode
                    if (IsEditing)
                    {
                        MessageBox.Show("Employee updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Employee added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // ✅ Reload grid first, then clear form
                    LoadEmployees();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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

                    // Temporarily disable SelectionChanged so it won't refill fields
                    dataGridView.SelectionChanged -= dataGridView_SelectionChanged;
                    dataGridView.DataSource = dt;
                    dataGridView.ClearSelection(); // ensure no row is selected
                    dataGridView.SelectionChanged += dataGridView_SelectionChanged;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employees: " + ex.Message);
                }
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


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    // If search box is empty, just reload all employees
                    LoadEmployees();
                }
                else
                {
                    // Show only filtered results
                    SearchEmployees();
                }
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            txtSearch.Clear();    // clear the search box
            LoadEmployees();      // reset DataGridView to show all records
        }
    }
}
