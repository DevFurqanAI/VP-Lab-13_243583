using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace VP_Lab_10
{
    // 1. Model Class
    public class Employee
    {
        public int EmpId { get; set; }          // Primary Key
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }
    }

    // 2. DbContext Class
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\ProjectModels;Database=EmployeeDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.EmpId);
        }
    }

    // 3. Main Window Backend
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        // Helper method to refresh DataGrid
        private void LoadEmployees()
        {
            using (var db = new EmployeeContext())
            {
                EmployeesDataGrid.ItemsSource = db.Employees.ToList();
            }
        }

        // Validation Method
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return false;
            }

            if (CNICTextBox.Text.Length != 13 || !CNICTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("CNIC must be exactly 13 digits.");
                return false;
            }

            if (!decimal.TryParse(SalaryTextBox.Text, out _))
            {
                MessageBox.Show("Salary must be numeric.");
                return false;
            }

            return true;
        }

        // Add Employee
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;
            
            if (!string.IsNullOrEmpty(EmpIdTextBox.ToString().Trim()))
                MessageBox.Show("Employee ID System Generated.");

            using (var db = new EmployeeContext())
            {
                var employee = new Employee
                {
                    Name = NameTextBox.Text,
                    FatherName = FatherNameTextBox.Text,
                    CNIC = CNICTextBox.Text,
                    Designation = DesignationTextBox.Text,
                    Salary = Convert.ToDecimal(SalaryTextBox.Text),
                    Department = (DepartmentComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString(),
                    HireDate = HireDatePicker.SelectedDate ?? DateTime.Now
                };

                db.Employees.Add(employee);
                db.SaveChanges();
            }
            LoadEmployees();
        }

        // Update Employee
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            using (var db = new EmployeeContext())
            {
                int empId = Convert.ToInt32(EmpIdTextBox.Text);
                var employee = db.Employees.FirstOrDefault(e => e.EmpId == empId);

                if (employee != null)
                {
                    employee.Name = NameTextBox.Text;
                    employee.FatherName = FatherNameTextBox.Text;
                    employee.CNIC = CNICTextBox.Text;
                    employee.Designation = DesignationTextBox.Text;
                    employee.Salary = Convert.ToDecimal(SalaryTextBox.Text);
                    employee.Department = (DepartmentComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();
                    employee.HireDate = HireDatePicker.SelectedDate ?? DateTime.Now;

                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Employee not found.");
                }
            }
            LoadEmployees();
        }

        // Delete Employee
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new EmployeeContext())
            {
                int empId = Convert.ToInt32(EmpIdTextBox.Text);
                var employee = db.Employees.FirstOrDefault(e => e.EmpId == empId);

                if (employee != null)
                {
                    db.Employees.Remove(employee);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Employee not found.");
                }
            }
            LoadEmployees();
        }

        // Load All Employees
        private void LoadAllButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }
    }
}
