
    using System.Windows.Forms;
    using System.Xml.Linq;
    using static System.Net.Mime.MediaTypeNames;

    namespace LSU_App
    {
        partial class Form1
        {
            private System.ComponentModel.IContainer components = null;

            // Login controls
            private GroupBox grpLogin;
            private TextBox txtUser;
            private TextBox txtPass;
            private Button btnLogin;
            private Label lblUser;
            private Label lblPass;
            private Label lblLoggedInAs;

            // Student panel
            private GroupBox grpStudent;
            private TextBox txtFirst;
            private TextBox txtLast;
            private Button btnSubmitApp;
            private Label lblFirst;
            private Label lblLast;
            private ListBox lstMyApps;

            // Admin panel
            private GroupBox grpAdmin;
            private ListBox lstAllApps;
            private Button btnAccept;
            private Button btnReject;

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null)) components.Dispose();
                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                components = new System.ComponentModel.Container();

                // ----- Login group -----
                grpLogin = new GroupBox();
                grpLogin.Text = "Login";
                grpLogin.SetBounds(12, 12, 280, 140);

                lblUser = new Label { Text = "Username:", AutoSize = true, Left = 12, Top = 28 };
                txtUser = new TextBox { Left = 90, Top = 25, Width = 170 };

                lblPass = new Label { Text = "Password:", AutoSize = true, Left = 12, Top = 60 };
                txtPass = new TextBox { Left = 90, Top = 57, Width = 170, PasswordChar = '•' };

                btnLogin = new Button { Text = "Login", Left = 90, Top = 90, Width = 100 };
                btnLogin.Click += btnLogin_Click;

                lblLoggedInAs = new Label { Text = "Not logged in", AutoSize = true, Left = 12, Top = 115, ForeColor = System.Drawing.Color.DimGray };

                grpLogin.Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, btnLogin, lblLoggedInAs });

                // ----- Student panel -----
                grpStudent = new GroupBox();
                grpStudent.Text = "Student";
                grpStudent.SetBounds(12, 165, 380, 240);
                grpStudent.Visible = false;

                lblFirst = new Label { Text = "First:", AutoSize = true, Left = 12, Top = 28 };
                txtFirst = new TextBox { Left = 60, Top = 25, Width = 140 };
                lblLast = new Label { Text = "Last:", AutoSize = true, Left = 210, Top = 28 };
                txtLast = new TextBox { Left = 250, Top = 25, Width = 110 };

                btnSubmitApp = new Button { Text = "Submit Application", Left = 12, Top = 60, Width = 170 };
                btnSubmitApp.Click += btnSubmitApp_Click;

                lstMyApps = new ListBox { Left = 12, Top = 95, Width = 348, Height = 130 };

                grpStudent.Controls.AddRange(new Control[] { lblFirst, txtFirst, lblLast, txtLast, btnSubmitApp, lstMyApps });

                // ----- Admin panel -----
                grpAdmin = new GroupBox();
                grpAdmin.Text = "Admin";
                grpAdmin.SetBounds(410, 12, 380, 393);
                grpAdmin.Visible = false;

                lstAllApps = new ListBox { Left = 12, Top = 25, Width = 348, Height = 300 };
                btnAccept = new Button { Text = "Accept", Left = 12, Top = 335, Width = 100 };
                btnReject = new Button { Text = "Reject", Left = 120, Top = 335, Width = 100 };

                btnAccept.Click += btnAccept_Click;
                btnReject.Click += btnReject_Click;

                grpAdmin.Controls.AddRange(new Control[] { lstAllApps, btnAccept, btnReject });

                // ----- Form -----
                AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new System.Drawing.Size(800, 420);
                Controls.AddRange(new Control[] { grpLogin, grpStudent, grpAdmin });
                Name = "Form1";
                Text = "LSU – Sprint 1 (WinForms MVP)";
                Load += Form1_Load;
            }
        }
    }