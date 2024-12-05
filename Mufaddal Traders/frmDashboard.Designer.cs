using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmDashboard
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
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMainDashboard = new System.Windows.Forms.Panel();
            this.pnlChat = new RoundedPanel();
            this.btnSend = new Guna.UI2.WinForms.Guna2Button();
            this.txtChat = new Guna.UI2.WinForms.Guna2TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbChooseUser = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pnlAlerts = new RoundedPanel();
            this.pnlNotifications = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.pnlSecurityAlerts = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.lblDateDisplay = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSettings = new Guna.UI2.WinForms.Guna2Button();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.btnMenu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHistory = new Guna.UI2.WinForms.Guna2Button();
            this.btnDashboard = new Guna.UI2.WinForms.Guna2Button();
            this.btnAccount = new Guna.UI2.WinForms.Guna2Button();
            this.picHeader.SuspendLayout();
            this.pnlMainDashboard.SuspendLayout();
            this.pnlChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.pnlAlerts.SuspendLayout();
            this.pnlNotifications.SuspendLayout();
            this.pnlSecurityAlerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picHeader
            // 
            this.picHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHeader.BorderColor = System.Drawing.Color.Black;
            this.picHeader.BorderThickness = 2;
            this.picHeader.Controls.Add(this.btnMinimize);
            this.picHeader.Controls.Add(this.btnClose);
            this.picHeader.Location = new System.Drawing.Point(5, 5);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(1395, 38);
            this.picHeader.TabIndex = 38;
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
            this.btnMinimize.Location = new System.Drawing.Point(41, 5);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(26, 26);
            this.btnMinimize.TabIndex = 40;
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
            this.btnClose.Location = new System.Drawing.Point(9, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 26);
            this.btnClose.TabIndex = 24;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlMainDashboard
            // 
            this.pnlMainDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.pnlMainDashboard.Controls.Add(this.pnlChat);
            this.pnlMainDashboard.Controls.Add(this.pnlAlerts);
            this.pnlMainDashboard.Location = new System.Drawing.Point(123, 177);
            this.pnlMainDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.pnlMainDashboard.Name = "pnlMainDashboard";
            this.pnlMainDashboard.Size = new System.Drawing.Size(1234, 654);
            this.pnlMainDashboard.TabIndex = 37;
            // 
            // pnlChat
            // 
            this.pnlChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlChat.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.pnlChat.BorderRadius = 10;
            this.pnlChat.Controls.Add(this.btnSend);
            this.pnlChat.Controls.Add(this.txtChat);
            this.pnlChat.Controls.Add(this.label17);
            this.pnlChat.Controls.Add(this.cmbChooseUser);
            this.pnlChat.Controls.Add(this.pictureBox8);
            this.pnlChat.Location = new System.Drawing.Point(23, 393);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(516, 245);
            this.pnlChat.TabIndex = 17;
            // 
            // btnSend
            // 
            this.btnSend.Animated = true;
            this.btnSend.AutoRoundedCorners = true;
            this.btnSend.BorderRadius = 14;
            this.btnSend.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSend.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSend.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSend.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSend.FillColor = System.Drawing.Color.Transparent;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Image = global::Mufaddal_Traders.Properties.Resources.content_send_icon_1320087227200139227_1;
            this.btnSend.ImageSize = new System.Drawing.Size(30, 30);
            this.btnSend.Location = new System.Drawing.Point(466, 205);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(30, 30);
            this.btnSend.TabIndex = 41;
            // 
            // txtChat
            // 
            this.txtChat.BorderColor = System.Drawing.Color.LightGray;
            this.txtChat.BorderRadius = 8;
            this.txtChat.BorderThickness = 3;
            this.txtChat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChat.DefaultText = "";
            this.txtChat.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtChat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtChat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChat.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(224)))), ((int)(((byte)(221)))));
            this.txtChat.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChat.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtChat.ForeColor = System.Drawing.Color.Black;
            this.txtChat.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChat.Location = new System.Drawing.Point(12, 74);
            this.txtChat.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.PasswordChar = '\0';
            this.txtChat.PlaceholderText = "";
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.SelectedText = "";
            this.txtChat.Size = new System.Drawing.Size(484, 123);
            this.txtChat.TabIndex = 20;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label17.Location = new System.Drawing.Point(7, 11);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 32);
            this.label17.TabIndex = 12;
            this.label17.Text = "Chat";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbChooseUser
            // 
            this.cmbChooseUser.BackColor = System.Drawing.Color.Transparent;
            this.cmbChooseUser.BorderColor = System.Drawing.Color.DarkGreen;
            this.cmbChooseUser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChooseUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChooseUser.FillColor = System.Drawing.Color.Honeydew;
            this.cmbChooseUser.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbChooseUser.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbChooseUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbChooseUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbChooseUser.ItemHeight = 30;
            this.cmbChooseUser.Items.AddRange(new object[] {
            "Select User"});
            this.cmbChooseUser.Location = new System.Drawing.Point(332, 23);
            this.cmbChooseUser.Margin = new System.Windows.Forms.Padding(2);
            this.cmbChooseUser.Name = "cmbChooseUser";
            this.cmbChooseUser.Size = new System.Drawing.Size(164, 36);
            this.cmbChooseUser.TabIndex = 18;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox8.Image = global::Mufaddal_Traders.Properties.Resources.chat_bubble;
            this.pictureBox8.Location = new System.Drawing.Point(73, 11);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(33, 32);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox8.TabIndex = 15;
            this.pictureBox8.TabStop = false;
            // 
            // pnlAlerts
            // 
            this.pnlAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlAlerts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.pnlAlerts.BorderRadius = 10;
            this.pnlAlerts.Controls.Add(this.pnlNotifications);
            this.pnlAlerts.Controls.Add(this.pnlSecurityAlerts);
            this.pnlAlerts.Controls.Add(this.label7);
            this.pnlAlerts.Controls.Add(this.pictureBox7);
            this.pnlAlerts.Location = new System.Drawing.Point(23, 17);
            this.pnlAlerts.Name = "pnlAlerts";
            this.pnlAlerts.Size = new System.Drawing.Size(587, 358);
            this.pnlAlerts.TabIndex = 16;
            // 
            // pnlNotifications
            // 
            this.pnlNotifications.AutoScroll = true;
            this.pnlNotifications.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(228)))), ((int)(((byte)(224)))));
            this.pnlNotifications.Controls.Add(this.label13);
            this.pnlNotifications.Location = new System.Drawing.Point(11, 208);
            this.pnlNotifications.Margin = new System.Windows.Forms.Padding(2);
            this.pnlNotifications.Name = "pnlNotifications";
            this.pnlNotifications.Size = new System.Drawing.Size(565, 138);
            this.pnlNotifications.TabIndex = 14;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label13.Location = new System.Drawing.Point(4, 7);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(152, 32);
            this.label13.TabIndex = 12;
            this.label13.Text = "Notifications";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlSecurityAlerts
            // 
            this.pnlSecurityAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(228)))), ((int)(((byte)(224)))));
            this.pnlSecurityAlerts.Controls.Add(this.label11);
            this.pnlSecurityAlerts.Location = new System.Drawing.Point(11, 56);
            this.pnlSecurityAlerts.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSecurityAlerts.Name = "pnlSecurityAlerts";
            this.pnlSecurityAlerts.Size = new System.Drawing.Size(565, 138);
            this.pnlSecurityAlerts.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label11.Location = new System.Drawing.Point(4, 7);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(172, 32);
            this.label11.TabIndex = 12;
            this.label11.Text = "Security Alerts";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(4, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 32);
            this.label7.TabIndex = 8;
            this.label7.Text = "Alerts";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Image = global::Mufaddal_Traders.Properties.Resources.notifications1;
            this.pictureBox7.Location = new System.Drawing.Point(82, 12);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(33, 32);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 7;
            this.pictureBox7.TabStop = false;
            // 
            // lblDateDisplay
            // 
            this.lblDateDisplay.AutoSize = true;
            this.lblDateDisplay.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateDisplay.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDateDisplay.Location = new System.Drawing.Point(124, 140);
            this.lblDateDisplay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDateDisplay.Name = "lblDateDisplay";
            this.lblDateDisplay.Size = new System.Drawing.Size(95, 20);
            this.lblDateDisplay.TabIndex = 36;
            this.lblDateDisplay.Text = "11/26/2024";
            this.lblDateDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(235, 110);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 32);
            this.label4.TabIndex = 35;
            this.label4.Text = "John!";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(119, 103);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 32);
            this.label3.TabIndex = 34;
            this.label3.Text = "Welcome,";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(118, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 32);
            this.label1.TabIndex = 32;
            this.label1.Text = "Dashboard";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(118, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1246, 31);
            this.label2.TabIndex = 33;
            this.label2.Text = "_____________________________________________________________________________";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panel1.BackColor = System.Drawing.Color.SeaGreen;
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Controls.Add(this.btnHome);
            this.panel1.Controls.Add(this.btnMenu);
            this.panel1.Controls.Add(this.btnHistory);
            this.panel1.Controls.Add(this.btnDashboard);
            this.panel1.Controls.Add(this.btnAccount);
            this.panel1.Location = new System.Drawing.Point(5, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 805);
            this.panel1.TabIndex = 31;
            // 
            // btnSettings
            // 
            this.btnSettings.Animated = true;
            this.btnSettings.BorderRadius = 3;
            this.btnSettings.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSettings.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSettings.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSettings.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSettings.FillColor = System.Drawing.Color.Transparent;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Image = global::Mufaddal_Traders.Properties.Resources.Settings;
            this.btnSettings.ImageSize = new System.Drawing.Size(44, 44);
            this.btnSettings.Location = new System.Drawing.Point(7, 733);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(60, 56);
            this.btnSettings.TabIndex = 23;
            // 
            // btnHome
            // 
            this.btnHome.Animated = true;
            this.btnHome.BorderRadius = 3;
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.FillColor = System.Drawing.Color.Transparent;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = global::Mufaddal_Traders.Properties.Resources.Home;
            this.btnHome.ImageSize = new System.Drawing.Size(44, 44);
            this.btnHome.Location = new System.Drawing.Point(7, 658);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(60, 56);
            this.btnHome.TabIndex = 22;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Animated = true;
            this.btnMenu.BorderRadius = 3;
            this.btnMenu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMenu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMenu.FillColor = System.Drawing.Color.Transparent;
            this.btnMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Image = global::Mufaddal_Traders.Properties.Resources.images__1__5;
            this.btnMenu.ImageSize = new System.Drawing.Size(44, 44);
            this.btnMenu.Location = new System.Drawing.Point(7, 237);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(60, 56);
            this.btnMenu.TabIndex = 21;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Animated = true;
            this.btnHistory.BorderRadius = 3;
            this.btnHistory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHistory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHistory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHistory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHistory.FillColor = System.Drawing.Color.Transparent;
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnHistory.ForeColor = System.Drawing.Color.White;
            this.btnHistory.Image = global::Mufaddal_Traders.Properties.Resources.recent_3;
            this.btnHistory.ImageSize = new System.Drawing.Size(44, 44);
            this.btnHistory.Location = new System.Drawing.Point(7, 155);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(60, 56);
            this.btnHistory.TabIndex = 20;
            // 
            // btnDashboard
            // 
            this.btnDashboard.Animated = true;
            this.btnDashboard.BorderRadius = 3;
            this.btnDashboard.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDashboard.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDashboard.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDashboard.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDashboard.FillColor = System.Drawing.Color.Transparent;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Image = global::Mufaddal_Traders.Properties.Resources.Menu;
            this.btnDashboard.ImageSize = new System.Drawing.Size(44, 44);
            this.btnDashboard.Location = new System.Drawing.Point(7, 87);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(60, 56);
            this.btnDashboard.TabIndex = 19;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnAccount
            // 
            this.btnAccount.Animated = true;
            this.btnAccount.BorderRadius = 3;
            this.btnAccount.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAccount.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAccount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAccount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAccount.FillColor = System.Drawing.Color.Transparent;
            this.btnAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAccount.ForeColor = System.Drawing.Color.White;
            this.btnAccount.Image = global::Mufaddal_Traders.Properties.Resources.User;
            this.btnAccount.ImageSize = new System.Drawing.Size(44, 44);
            this.btnAccount.Location = new System.Drawing.Point(7, 14);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(60, 56);
            this.btnAccount.TabIndex = 18;
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 851);
            this.ControlBox = false;
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.pnlMainDashboard);
            this.Controls.Add(this.lblDateDisplay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDashboard";
            this.Load += new System.EventHandler(this.frmDashboard_Load);
            this.picHeader.ResumeLayout(false);
            this.pnlMainDashboard.ResumeLayout(false);
            this.pnlChat.ResumeLayout(false);
            this.pnlChat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.pnlAlerts.ResumeLayout(false);
            this.pnlAlerts.PerformLayout();
            this.pnlNotifications.ResumeLayout(false);
            this.pnlNotifications.PerformLayout();
            this.pnlSecurityAlerts.ResumeLayout(false);
            this.pnlSecurityAlerts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RoundedPanel pnlChat;
        private Guna.UI2.WinForms.Guna2TextBox txtChat;
        private Label label17;
        private Guna.UI2.WinForms.Guna2ComboBox cmbChooseUser;
        private PictureBox pictureBox8;
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private Label label7;
        private PictureBox pictureBox7;
        private Label label11;
        private Panel pnlSecurityAlerts;
        private RoundedPanel pnlAlerts;
        private Panel pnlNotifications;
        private Label label13;
        private Panel pnlMainDashboard;
        private Label lblDateDisplay;
        private Label label4;
        private Label label3;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnAccount;
        private Guna.UI2.WinForms.Guna2Button btnHome;
        private Guna.UI2.WinForms.Guna2Button btnMenu;
        private Guna.UI2.WinForms.Guna2Button btnHistory;
        private Guna.UI2.WinForms.Guna2Button btnDashboard;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSettings;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnSend;
    }
}