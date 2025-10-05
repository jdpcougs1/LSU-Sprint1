using System.Windows.Forms;
using System;
using System.Linq;
using System.Drawing;

namespace LSU_App
{
    public partial class Form1 : Form
    {
        private readonly AuthService _auth = new();
        private readonly AdmissionsService _admissions = new();
        private UserAccount? _session;

        // This is optional for demo identities
        private readonly Administrator _adminModel = new(101, "Randall", "Hutton", "Registrar");
        private readonly Faculty _facultyModel = new(201, "John-Luke", "Peck", "Computer Science");

        public Form1()
        {
            InitializeComponent();

            // Create DB, apply migrations, seed users, courses, and completion
            DbInitializer.EnsureMigratedAndSeeded();

            // Wire events for constructor
            btnLogin.Click += btnLogin_Click;
            btnRegister.Click += btnRegister_Click;

            // Search box and refresh button go here
            // btnRefreshCourses.Click += (_, __) => RefreshCourses();
            // txtSearch.TextChanged += (_, __) => RefreshCourses();

            RefreshCourses(); // optional initial list
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            // We did this to make the Login button bigger
            btnLogin.Size = new Size(150, 40);

            // This is also to center it under the textboxes and drop it a bit below Password
            btnLogin.Location = new Point(
                txtUser.Left + (txtUser.Width - btnLogin.Width) / 2,
                txtPass.Bottom + 10
            );

            // Entirely optional but to ensure the Login group box is big enough to fit the larger button
            grpLogin.Width = Math.Max(grpLogin.Width, btnLogin.Right + 20);
            grpLogin.Height = Math.Max(grpLogin.Height, btnLogin.Bottom + 20);
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            var u = txtUser.Text.Trim();
            var p = txtPass.Text;

            var s = _auth.Login(u, p);
            if (_session is null)
            {
                MessageBox.Show("Invalid credentials", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _session = s;
            lblLoggedInAs.Text = $"Logged in as: {s.Username} ({s.Role})";

            RefreshCourses();
        }

        //Modified 05OCT25 Karima
        private void btnRegister_Click(object? sender, EventArgs e)
        {
            if (_session is null || _session.Role != RoleName.Student)
            {
                MessageBox.Show("Student login required.", "Register",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
            }
            if (lstCourses.SelectedItem is not Course c)
            {
                MessageBox.Show("Select a course first.", "Register",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var (ok, msg) = _registration.Register(_session.UserId, c.Code);
            MessageBox.Show(msg, ok ? "Registered" : "Registration Failed");
            if (ok) RefreshCourses();
        }

        private void RefreshCourses()
        {
            lstCourses.Items.Clear();

            // If you have a search box:
            // var items = _courses.Search(txtSearch.Text);
        var items = _courses.List();

        foreach (var c in items)
            lstCourses.Items.Add(c); // Course.ToString() shows code/title/seats
        }
        private void SwitchRoleUI(Role role)
        {
            grpStudent.Visible = (role == Role.Student);
            grpAdmin.Visible = (role == Role.Admin);

            if (role == Role.Faculty)
            {
                MessageBox.Show(
                    $"{_facultyModel}\n\nSprint 1: Faculty login verified.\nGrade/attendance features arrive in later sprints.",
                    "Faculty", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSubmitApp_Click(object? sender, EventArgs e)
        {
            if (_session is null || _session.Role != Role.Student)
            {
                MessageBox.Show("Student login required.", "Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var first = txtFirst.Text.Trim();
            var last = txtLast.Text.Trim();

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            {
                MessageBox.Show("Please enter first and last name.", "Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = _admissions.Submit(first, last, DateTime.Now);
            MessageBox.Show($"Application submitted (ID #{id}). Status: {AppStatus.Submitted}.", "Submit",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtFirst.Clear();
            txtLast.Clear();
            RefreshLists();
        }

        private void btnAccept_Click(object? sender, EventArgs e)
        {
            if (_session is null || _session.Role != Role.Admin)
            {
                MessageBox.Show("Admin login required.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lstAllApps.SelectedItem is not Applicant a)
            {
                MessageBox.Show("Select an application first.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_admissions.Decide(a.Id, AppStatus.Accepted))
            {
                MessageBox.Show($"{_adminModel.FirstName} {_adminModel.LastName} accepted application #{a.Id}.",
                    "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshLists();
            }
        }

        private void btnReject_Click(object? sender, EventArgs e)
        {
            if (_session is null || _session.Role != Role.Admin)
            {
                MessageBox.Show("Admin login required.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lstAllApps.SelectedItem is not Applicant a)
            {
                MessageBox.Show("Select an application first.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_admissions.Decide(a.Id, AppStatus.Rejected))
            {
                MessageBox.Show($"{_adminModel.FirstName} {_adminModel.LastName} rejected application #{a.Id}.",
                    "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshLists();
            }
        }

        private void RefreshLists()
        {
            if (_session?.Role == Role.Student)
            {
                lstMyApps.Items.Clear();
                foreach (var a in _admissions.ListAll())
                    lstMyApps.Items.Add(a);   // uses Applicant.ToString()
            }

            if (_session?.Role == Role.Admin)
            {
                lstAllApps.Items.Clear();
                foreach (var a in _admissions.ListAll())
                    lstAllApps.Items.Add(a);
            }
        }
    }
}
