using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderThickness = 7;
            Color borderColor = Color.DarkGray;

            // Draw border
            using (Pen pen = new Pen(borderColor, borderThickness))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate(); // Forces the form to redraw the border on resize
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.pnlForgotPassword = new Guna.UI2.WinForms.Guna2Panel();
            this.picBackToLogin = new System.Windows.Forms.PictureBox();
            this.pnlNewPassword = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.guna2TextBox3 = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2TextBox4 = new Guna.UI2.WinForms.Guna2TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.guna2TextBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnForgotPasswordSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBox5 = new Guna.UI2.WinForms.Guna2TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCRTel = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCREmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCRConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlLoginInterface1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblForgotpassword = new System.Windows.Forms.Label();
            this.lblCreateAccount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoginPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLoginInterfaceLoginButton = new Guna.UI2.WinForms.Guna2Button();
            this.txtLoginUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlCreateAcc = new Guna.UI2.WinForms.Guna2Panel();
            this.cbCRUserType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCRPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnCreateAccountinterfaceLoginButton = new Guna.UI2.WinForms.Guna2Button();
            this.txtCAUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLogin = new Guna.UI2.WinForms.Guna2Button();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.picHeader2 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlForgotPassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackToLogin)).BeginInit();
            this.pnlNewPassword.SuspendLayout();
            this.pnlLoginInterface1.SuspendLayout();
            this.pnlCreateAcc.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlForgotPassword
            // 
            this.pnlForgotPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlForgotPassword.Controls.Add(this.picBackToLogin);
            this.pnlForgotPassword.Controls.Add(this.pnlNewPassword);
            this.pnlForgotPassword.Controls.Add(this.label13);
            this.pnlForgotPassword.Controls.Add(this.guna2TextBox2);
            this.pnlForgotPassword.Controls.Add(this.label15);
            this.pnlForgotPassword.Controls.Add(this.label18);
            this.pnlForgotPassword.Controls.Add(this.btnForgotPasswordSave);
            this.pnlForgotPassword.Controls.Add(this.guna2TextBox5);
            this.pnlForgotPassword.Location = new System.Drawing.Point(28, 174);
            this.pnlForgotPassword.Margin = new System.Windows.Forms.Padding(2);
            this.pnlForgotPassword.Name = "pnlForgotPassword";
            this.pnlForgotPassword.Size = new System.Drawing.Size(492, 657);
            this.pnlForgotPassword.TabIndex = 27;
            // 
            // picBackToLogin
            // 
            this.picBackToLogin.BackgroundImage = global::Mufaddal_Traders.Properties.Resources._3114815;
            this.picBackToLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBackToLogin.Location = new System.Drawing.Point(454, 10);
            this.picBackToLogin.Margin = new System.Windows.Forms.Padding(2);
            this.picBackToLogin.Name = "picBackToLogin";
            this.picBackToLogin.Size = new System.Drawing.Size(28, 27);
            this.picBackToLogin.TabIndex = 20;
            this.picBackToLogin.TabStop = false;
            this.picBackToLogin.Click += new System.EventHandler(this.picBackToLogin_Click);
            // 
            // pnlNewPassword
            // 
            this.pnlNewPassword.Controls.Add(this.label5);
            this.pnlNewPassword.Controls.Add(this.guna2TextBox3);
            this.pnlNewPassword.Controls.Add(this.guna2TextBox4);
            this.pnlNewPassword.Controls.Add(this.label14);
            this.pnlNewPassword.Controls.Add(this.label17);
            this.pnlNewPassword.Location = new System.Drawing.Point(54, 308);
            this.pnlNewPassword.Margin = new System.Windows.Forms.Padding(2);
            this.pnlNewPassword.Name = "pnlNewPassword";
            this.pnlNewPassword.Size = new System.Drawing.Size(383, 192);
            this.pnlNewPassword.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(234, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Please provide a new password!";
            // 
            // guna2TextBox3
            // 
            this.guna2TextBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox3.DefaultText = "";
            this.guna2TextBox3.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox3.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox3.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox3.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox3.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox3.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox3.Location = new System.Drawing.Point(4, 154);
            this.guna2TextBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox3.Name = "guna2TextBox3";
            this.guna2TextBox3.PasswordChar = '\0';
            this.guna2TextBox3.PlaceholderText = "";
            this.guna2TextBox3.SelectedText = "";
            this.guna2TextBox3.Size = new System.Drawing.Size(379, 35);
            this.guna2TextBox3.TabIndex = 15;
            // 
            // guna2TextBox4
            // 
            this.guna2TextBox4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox4.DefaultText = "";
            this.guna2TextBox4.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox4.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox4.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox4.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox4.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox4.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox4.Location = new System.Drawing.Point(4, 76);
            this.guna2TextBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox4.Name = "guna2TextBox4";
            this.guna2TextBox4.PasswordChar = '\0';
            this.guna2TextBox4.PlaceholderText = "";
            this.guna2TextBox4.SelectedText = "";
            this.guna2TextBox4.Size = new System.Drawing.Size(379, 35);
            this.guna2TextBox4.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(4, 131);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(137, 20);
            this.label14.TabIndex = 17;
            this.label14.Text = "Confirm Password";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(4, 53);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(78, 20);
            this.label17.TabIndex = 12;
            this.label17.Text = "Password";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(54, 223);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 20);
            this.label13.TabIndex = 18;
            this.label13.Text = "Email";
            // 
            // guna2TextBox2
            // 
            this.guna2TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox2.DefaultText = "";
            this.guna2TextBox2.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox2.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Location = new System.Drawing.Point(54, 246);
            this.guna2TextBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox2.Name = "guna2TextBox2";
            this.guna2TextBox2.PasswordChar = '\0';
            this.guna2TextBox2.PlaceholderText = "";
            this.guna2TextBox2.SelectedText = "";
            this.guna2TextBox2.Size = new System.Drawing.Size(379, 35);
            this.guna2TextBox2.TabIndex = 16;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.SeaGreen;
            this.label15.Location = new System.Drawing.Point(146, 50);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(204, 26);
            this.label15.TabIndex = 14;
            this.label15.Text = "Forgot Password?";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(54, 149);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 20);
            this.label18.TabIndex = 11;
            this.label18.Text = "Username";
            // 
            // btnForgotPasswordSave
            // 
            this.btnForgotPasswordSave.Animated = true;
            this.btnForgotPasswordSave.AutoRoundedCorners = true;
            this.btnForgotPasswordSave.BackColor = System.Drawing.Color.Transparent;
            this.btnForgotPasswordSave.BorderColor = System.Drawing.Color.SeaGreen;
            this.btnForgotPasswordSave.BorderRadius = 18;
            this.btnForgotPasswordSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnForgotPasswordSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnForgotPasswordSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnForgotPasswordSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnForgotPasswordSave.FillColor = System.Drawing.Color.SeaGreen;
            this.btnForgotPasswordSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnForgotPasswordSave.ForeColor = System.Drawing.Color.White;
            this.btnForgotPasswordSave.Location = new System.Drawing.Point(325, 538);
            this.btnForgotPasswordSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnForgotPasswordSave.Name = "btnForgotPasswordSave";
            this.btnForgotPasswordSave.Size = new System.Drawing.Size(108, 38);
            this.btnForgotPasswordSave.TabIndex = 7;
            this.btnForgotPasswordSave.Text = "Save";
            // 
            // guna2TextBox5
            // 
            this.guna2TextBox5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox5.DefaultText = "";
            this.guna2TextBox5.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox5.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox5.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox5.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox5.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox5.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox5.Location = new System.Drawing.Point(54, 173);
            this.guna2TextBox5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox5.Name = "guna2TextBox5";
            this.guna2TextBox5.PasswordChar = '\0';
            this.guna2TextBox5.PlaceholderText = "";
            this.guna2TextBox5.SelectedText = "";
            this.guna2TextBox5.Size = new System.Drawing.Size(379, 35);
            this.guna2TextBox5.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(44, 462);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 20);
            this.label12.TabIndex = 20;
            this.label12.Text = "Telephone";
            // 
            // txtCRTel
            // 
            this.txtCRTel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCRTel.DefaultText = "";
            this.txtCRTel.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCRTel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCRTel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRTel.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRTel.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRTel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCRTel.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRTel.Location = new System.Drawing.Point(44, 485);
            this.txtCRTel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCRTel.Name = "txtCRTel";
            this.txtCRTel.PasswordChar = '\0';
            this.txtCRTel.PlaceholderText = "";
            this.txtCRTel.SelectedText = "";
            this.txtCRTel.Size = new System.Drawing.Size(379, 35);
            this.txtCRTel.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(44, 385);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Email";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(44, 306);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(137, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "Confirm Password";
            // 
            // txtCREmail
            // 
            this.txtCREmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCREmail.DefaultText = "";
            this.txtCREmail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCREmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCREmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCREmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCREmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCREmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCREmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCREmail.Location = new System.Drawing.Point(44, 409);
            this.txtCREmail.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCREmail.Name = "txtCREmail";
            this.txtCREmail.PasswordChar = '\0';
            this.txtCREmail.PlaceholderText = "";
            this.txtCREmail.SelectedText = "";
            this.txtCREmail.Size = new System.Drawing.Size(379, 35);
            this.txtCREmail.TabIndex = 16;
            // 
            // txtCRConfirmPassword
            // 
            this.txtCRConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCRConfirmPassword.DefaultText = "";
            this.txtCRConfirmPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCRConfirmPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCRConfirmPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRConfirmPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRConfirmPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCRConfirmPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRConfirmPassword.Location = new System.Drawing.Point(44, 330);
            this.txtCRConfirmPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCRConfirmPassword.Name = "txtCRConfirmPassword";
            this.txtCRConfirmPassword.PasswordChar = '\0';
            this.txtCRConfirmPassword.PlaceholderText = "";
            this.txtCRConfirmPassword.SelectedText = "";
            this.txtCRConfirmPassword.Size = new System.Drawing.Size(379, 35);
            this.txtCRConfirmPassword.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SeaGreen;
            this.label7.Location = new System.Drawing.Point(240, 50);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 26);
            this.label7.TabIndex = 14;
            this.label7.Text = "Create Account";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(86, 50);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(65, 26);
            this.lblLogin.TabIndex = 13;
            this.lblLogin.Text = "Login";
            this.lblLogin.Click += new System.EventHandler(this.lblLogin_Click);
            // 
            // pnlLoginInterface1
            // 
            this.pnlLoginInterface1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlLoginInterface1.Controls.Add(this.lblForgotpassword);
            this.pnlLoginInterface1.Controls.Add(this.lblCreateAccount);
            this.pnlLoginInterface1.Controls.Add(this.label3);
            this.pnlLoginInterface1.Controls.Add(this.label2);
            this.pnlLoginInterface1.Controls.Add(this.label1);
            this.pnlLoginInterface1.Controls.Add(this.txtLoginPassword);
            this.pnlLoginInterface1.Controls.Add(this.btnLoginInterfaceLoginButton);
            this.pnlLoginInterface1.Controls.Add(this.txtLoginUsername);
            this.pnlLoginInterface1.Location = new System.Drawing.Point(27, 281);
            this.pnlLoginInterface1.Margin = new System.Windows.Forms.Padding(2);
            this.pnlLoginInterface1.Name = "pnlLoginInterface1";
            this.pnlLoginInterface1.Size = new System.Drawing.Size(492, 488);
            this.pnlLoginInterface1.TabIndex = 24;
            // 
            // lblForgotpassword
            // 
            this.lblForgotpassword.AutoSize = true;
            this.lblForgotpassword.BackColor = System.Drawing.Color.Transparent;
            this.lblForgotpassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForgotpassword.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblForgotpassword.Location = new System.Drawing.Point(44, 322);
            this.lblForgotpassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblForgotpassword.Name = "lblForgotpassword";
            this.lblForgotpassword.Size = new System.Drawing.Size(156, 17);
            this.lblForgotpassword.TabIndex = 15;
            this.lblForgotpassword.Text = "Forgot Your Password?";
            this.lblForgotpassword.Click += new System.EventHandler(this.lblForgotpassword_Click);
            // 
            // lblCreateAccount
            // 
            this.lblCreateAccount.AutoSize = true;
            this.lblCreateAccount.BackColor = System.Drawing.Color.Transparent;
            this.lblCreateAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreateAccount.Location = new System.Drawing.Point(247, 63);
            this.lblCreateAccount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCreateAccount.Name = "lblCreateAccount";
            this.lblCreateAccount.Size = new System.Drawing.Size(162, 26);
            this.lblCreateAccount.TabIndex = 14;
            this.lblCreateAccount.Text = "Create Account";
            this.lblCreateAccount.Click += new System.EventHandler(this.lblCreateAccount_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SeaGreen;
            this.label3.Location = new System.Drawing.Point(94, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "Login";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 252);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 172);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Username";
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLoginPassword.DefaultText = "";
            this.txtLoginPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtLoginPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtLoginPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLoginPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLoginPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoginPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtLoginPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoginPassword.Location = new System.Drawing.Point(44, 276);
            this.txtLoginPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '\0';
            this.txtLoginPassword.PlaceholderText = "";
            this.txtLoginPassword.SelectedText = "";
            this.txtLoginPassword.Size = new System.Drawing.Size(379, 35);
            this.txtLoginPassword.TabIndex = 10;
            // 
            // btnLoginInterfaceLoginButton
            // 
            this.btnLoginInterfaceLoginButton.Animated = true;
            this.btnLoginInterfaceLoginButton.AutoRoundedCorners = true;
            this.btnLoginInterfaceLoginButton.BackColor = System.Drawing.Color.Transparent;
            this.btnLoginInterfaceLoginButton.BorderColor = System.Drawing.Color.SeaGreen;
            this.btnLoginInterfaceLoginButton.BorderRadius = 18;
            this.btnLoginInterfaceLoginButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLoginInterfaceLoginButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLoginInterfaceLoginButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLoginInterfaceLoginButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLoginInterfaceLoginButton.FillColor = System.Drawing.Color.SeaGreen;
            this.btnLoginInterfaceLoginButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLoginInterfaceLoginButton.ForeColor = System.Drawing.Color.White;
            this.btnLoginInterfaceLoginButton.Location = new System.Drawing.Point(325, 394);
            this.btnLoginInterfaceLoginButton.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoginInterfaceLoginButton.Name = "btnLoginInterfaceLoginButton";
            this.btnLoginInterfaceLoginButton.Size = new System.Drawing.Size(108, 38);
            this.btnLoginInterfaceLoginButton.TabIndex = 7;
            this.btnLoginInterfaceLoginButton.Text = "Login";
            this.btnLoginInterfaceLoginButton.Click += new System.EventHandler(this.btnLoginInterfaceLoginButton_Click);
            // 
            // txtLoginUsername
            // 
            this.txtLoginUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLoginUsername.DefaultText = "";
            this.txtLoginUsername.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtLoginUsername.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtLoginUsername.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLoginUsername.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLoginUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoginUsername.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtLoginUsername.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoginUsername.Location = new System.Drawing.Point(44, 197);
            this.txtLoginUsername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtLoginUsername.Name = "txtLoginUsername";
            this.txtLoginUsername.PasswordChar = '\0';
            this.txtLoginUsername.PlaceholderText = "";
            this.txtLoginUsername.SelectedText = "";
            this.txtLoginUsername.Size = new System.Drawing.Size(379, 35);
            this.txtLoginUsername.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(44, 149);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 20);
            this.label10.TabIndex = 11;
            this.label10.Text = "Username";
            // 
            // pnlCreateAcc
            // 
            this.pnlCreateAcc.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlCreateAcc.Controls.Add(this.cbCRUserType);
            this.pnlCreateAcc.Controls.Add(this.label16);
            this.pnlCreateAcc.Controls.Add(this.label12);
            this.pnlCreateAcc.Controls.Add(this.txtCRTel);
            this.pnlCreateAcc.Controls.Add(this.label6);
            this.pnlCreateAcc.Controls.Add(this.label11);
            this.pnlCreateAcc.Controls.Add(this.txtCREmail);
            this.pnlCreateAcc.Controls.Add(this.txtCRConfirmPassword);
            this.pnlCreateAcc.Controls.Add(this.label7);
            this.pnlCreateAcc.Controls.Add(this.lblLogin);
            this.pnlCreateAcc.Controls.Add(this.label9);
            this.pnlCreateAcc.Controls.Add(this.label10);
            this.pnlCreateAcc.Controls.Add(this.txtCRPassword);
            this.pnlCreateAcc.Controls.Add(this.btnCreateAccountinterfaceLoginButton);
            this.pnlCreateAcc.Controls.Add(this.txtCAUsername);
            this.pnlCreateAcc.Location = new System.Drawing.Point(29, 174);
            this.pnlCreateAcc.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCreateAcc.Name = "pnlCreateAcc";
            this.pnlCreateAcc.Size = new System.Drawing.Size(492, 657);
            this.pnlCreateAcc.TabIndex = 26;
            // 
            // cbCRUserType
            // 
            this.cbCRUserType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCRUserType.FormattingEnabled = true;
            this.cbCRUserType.Items.AddRange(new object[] {
            "Storekeeper",
            "Shipping Manager",
            "Accountants",
            "Marketing and Sales Department"});
            this.cbCRUserType.Location = new System.Drawing.Point(44, 562);
            this.cbCRUserType.Name = "cbCRUserType";
            this.cbCRUserType.Size = new System.Drawing.Size(250, 33);
            this.cbCRUserType.TabIndex = 22;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(44, 539);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(81, 20);
            this.label16.TabIndex = 21;
            this.label16.Text = "User Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(44, 228);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Password";
            // 
            // txtCRPassword
            // 
            this.txtCRPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCRPassword.DefaultText = "";
            this.txtCRPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCRPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCRPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCRPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCRPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCRPassword.Location = new System.Drawing.Point(44, 252);
            this.txtCRPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCRPassword.Name = "txtCRPassword";
            this.txtCRPassword.PasswordChar = '\0';
            this.txtCRPassword.PlaceholderText = "";
            this.txtCRPassword.SelectedText = "";
            this.txtCRPassword.Size = new System.Drawing.Size(379, 35);
            this.txtCRPassword.TabIndex = 10;
            // 
            // btnCreateAccountinterfaceLoginButton
            // 
            this.btnCreateAccountinterfaceLoginButton.Animated = true;
            this.btnCreateAccountinterfaceLoginButton.AutoRoundedCorners = true;
            this.btnCreateAccountinterfaceLoginButton.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateAccountinterfaceLoginButton.BorderColor = System.Drawing.Color.SeaGreen;
            this.btnCreateAccountinterfaceLoginButton.BorderRadius = 18;
            this.btnCreateAccountinterfaceLoginButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateAccountinterfaceLoginButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateAccountinterfaceLoginButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateAccountinterfaceLoginButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateAccountinterfaceLoginButton.FillColor = System.Drawing.Color.SeaGreen;
            this.btnCreateAccountinterfaceLoginButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCreateAccountinterfaceLoginButton.ForeColor = System.Drawing.Color.White;
            this.btnCreateAccountinterfaceLoginButton.Location = new System.Drawing.Point(325, 611);
            this.btnCreateAccountinterfaceLoginButton.Margin = new System.Windows.Forms.Padding(2);
            this.btnCreateAccountinterfaceLoginButton.Name = "btnCreateAccountinterfaceLoginButton";
            this.btnCreateAccountinterfaceLoginButton.Size = new System.Drawing.Size(108, 38);
            this.btnCreateAccountinterfaceLoginButton.TabIndex = 7;
            this.btnCreateAccountinterfaceLoginButton.Text = "Login";
            this.btnCreateAccountinterfaceLoginButton.Click += new System.EventHandler(this.btnCreateAccountinterfaceLoginButton_Click);
            // 
            // txtCAUsername
            // 
            this.txtCAUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCAUsername.DefaultText = "";
            this.txtCAUsername.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCAUsername.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCAUsername.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCAUsername.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCAUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCAUsername.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCAUsername.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCAUsername.Location = new System.Drawing.Point(44, 173);
            this.txtCAUsername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCAUsername.Name = "txtCAUsername";
            this.txtCAUsername.PasswordChar = '\0';
            this.txtCAUsername.PlaceholderText = "";
            this.txtCAUsername.SelectedText = "";
            this.txtCAUsername.Size = new System.Drawing.Size(379, 35);
            this.txtCAUsername.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnLogin);
            this.panel1.Controls.Add(this.btnHome);
            this.panel1.Location = new System.Drawing.Point(4, 42);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1413, 89);
            this.panel1.TabIndex = 28;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Mufaddal_Traders.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogin.BorderColor = System.Drawing.Color.SpringGreen;
            this.btnLogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogin.FillColor = System.Drawing.Color.Transparent;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.SystemColors.GrayText;
            this.btnLogin.HoverState.BorderColor = System.Drawing.Color.MediumSpringGreen;
            this.btnLogin.HoverState.FillColor = System.Drawing.Color.LightGreen;
            this.btnLogin.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(321, 0);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(90, 90);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "Login";
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BorderColor = System.Drawing.Color.SpringGreen;
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.FillColor = System.Drawing.Color.Transparent;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold);
            this.btnHome.ForeColor = System.Drawing.SystemColors.GrayText;
            this.btnHome.HoverState.BorderColor = System.Drawing.Color.MediumSpringGreen;
            this.btnHome.HoverState.FillColor = System.Drawing.Color.LightGreen;
            this.btnHome.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(203, 0);
            this.btnHome.Margin = new System.Windows.Forms.Padding(2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(90, 89);
            this.btnHome.TabIndex = 8;
            this.btnHome.Text = "Home";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // picHeader
            // 
            this.picHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHeader.BorderColor = System.Drawing.Color.Black;
            this.picHeader.BorderThickness = 2;
            this.picHeader.Controls.Add(this.btnMinimize);
            this.picHeader.Controls.Add(this.btnClose);
            this.picHeader.Location = new System.Drawing.Point(4, 3);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(547, 38);
            this.picHeader.TabIndex = 29;
            this.picHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Animated = true;
            this.btnMinimize.AutoRoundedCorners = true;
            this.btnMinimize.BorderRadius = 12;
            this.btnMinimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMinimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMinimize.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Image = global::Mufaddal_Traders.Properties.Resources.orange_circle_png_3;
            this.btnMinimize.ImageSize = new System.Drawing.Size(23, 23);
            this.btnMinimize.Location = new System.Drawing.Point(46, 5);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(26, 26);
            this.btnMinimize.TabIndex = 42;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Animated = true;
            this.btnClose.AutoRoundedCorners = true;
            this.btnClose.BorderRadius = 12;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = global::Mufaddal_Traders.Properties.Resources.red_circle_round_3d_button_3__1_;
            this.btnClose.ImageSize = new System.Drawing.Size(23, 23);
            this.btnClose.Location = new System.Drawing.Point(14, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 26);
            this.btnClose.TabIndex = 41;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(630, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(351, 47);
            this.label4.TabIndex = 31;
            this.label4.Text = "Welcome to MTSMS,";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(633, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(648, 150);
            this.label8.TabIndex = 32;
            this.label8.Text = resources.GetString("label8.Text");
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Mufaddal_Traders.Properties.Resources.conifers_18365821;
            this.pictureBox2.Location = new System.Drawing.Point(551, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(866, 882);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            // 
            // picHeader2
            // 
            this.picHeader2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHeader2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(202)))), ((int)(((byte)(212)))));
            this.picHeader2.BorderColor = System.Drawing.Color.Transparent;
            this.picHeader2.Location = new System.Drawing.Point(551, 3);
            this.picHeader2.Name = "picHeader2";
            this.picHeader2.Size = new System.Drawing.Size(866, 38);
            this.picHeader2.TabIndex = 43;
            this.picHeader2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader2_MouseDown);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1421, 890);
            this.ControlBox = false;
            this.Controls.Add(this.picHeader2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlForgotPassword);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlLoginInterface1);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.pnlCreateAcc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.pnlForgotPassword.ResumeLayout(false);
            this.pnlForgotPassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackToLogin)).EndInit();
            this.pnlNewPassword.ResumeLayout(false);
            this.pnlNewPassword.PerformLayout();
            this.pnlLoginInterface1.ResumeLayout(false);
            this.pnlLoginInterface1.PerformLayout();
            this.pnlCreateAcc.ResumeLayout(false);
            this.pnlCreateAcc.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.picHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlForgotPassword;
        private System.Windows.Forms.PictureBox picBackToLogin;
        private System.Windows.Forms.Panel pnlNewPassword;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox3;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label13;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private Guna.UI2.WinForms.Guna2Button btnForgotPasswordSave;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox5;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2TextBox txtCRTel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox txtCREmail;
        private Guna.UI2.WinForms.Guna2TextBox txtCRConfirmPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLogin;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Guna.UI2.WinForms.Guna2Panel pnlLoginInterface1;
        private System.Windows.Forms.Label lblForgotpassword;
        private System.Windows.Forms.Label lblCreateAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtLoginPassword;
        private Guna.UI2.WinForms.Guna2Button btnLoginInterfaceLoginButton;
        private Guna.UI2.WinForms.Guna2TextBox txtLoginUsername;
        private System.Windows.Forms.Label label10;
        private Guna.UI2.WinForms.Guna2Panel pnlCreateAcc;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox txtCRPassword;
        private Guna.UI2.WinForms.Guna2Button btnCreateAccountinterfaceLoginButton;
        private Guna.UI2.WinForms.Guna2TextBox txtCAUsername;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Guna.UI2.WinForms.Guna2Button btnHome;
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private PictureBox pictureBox2;
        private Label label4;
        private Label label8;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Panel picHeader2;
        private ComboBox cbCRUserType;
        private Label label16;
    }
}