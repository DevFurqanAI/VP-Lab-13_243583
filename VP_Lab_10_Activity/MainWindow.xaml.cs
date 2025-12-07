using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace VP_Lab_10_Activity
{
    // 1. Model Class
    public class Student
    {
        public int RollNo { get; set; }
        public string Name { get; set; }
    }

    // 2. DbContext Class
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\ProjectModels;Database=StudentDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(s => s.RollNo);
            modelBuilder.Entity<Student>().Property(s => s.RollNo).ValueGeneratedNever();
        }
    }

    // 3. Main Window Backend
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadStudents();
        }
        private void LoadStudents()
        {
            using (var db = new StudentContext())
            {
                StudentsDataGrid.ItemsSource = db.Students.ToList();
            }
        }
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StudentContext())
            {
                var student = new Student
                {
                    RollNo = Convert.ToInt32(RollNoTextBox.Text),
                    Name = NameTextBox.Text
                };

                db.Students.Add(student);
                db.SaveChanges();
            }
            LoadStudents();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StudentContext())
            {
                var rollNo = Convert.ToInt32(RollNoTextBox.Text);
                var student = db.Students.FirstOrDefault(s => s.RollNo == rollNo);

                if (student != null)
                {
                    student.Name = NameTextBox.Text;
                    db.SaveChanges();
                }
            }
            LoadStudents();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StudentContext())
            {
                var rollNo = Convert.ToInt32(RollNoTextBox.Text);
                var student = db.Students.FirstOrDefault(s => s.RollNo == rollNo);

                if (student != null)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                }
            }
            LoadStudents();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StudentContext())
            {
                var rollNo = Convert.ToInt32(RollNoTextBox.Text);
                var found = db.Students.FirstOrDefault(s => s.RollNo == rollNo);

                if (found != null)
                {
                    StudentsDataGrid.ItemsSource = new List<Student> { found };
                }
                else
                {
                    MessageBox.Show("Student not found.");
                }
            }
        }
        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStudents();
        }
    }
}
