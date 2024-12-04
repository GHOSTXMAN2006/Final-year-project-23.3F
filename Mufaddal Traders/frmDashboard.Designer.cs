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
            Color borderColor = Color.MediumSeaGreen;

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
            this.pnlChat = new RoundedPanel();
            this.txtChat = new Guna.UI2.WinForms.Guna2TextBox();
            this.picSend = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbChooseUser = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.picMinimize = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlSecurityAlerts = new System.Windows.Forms.Panel();
            this.pnlAlerts = new RoundedPanel();
            this.pnlNotifications = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pnlMainDashboard = new System.Windows.Forms.Panel();
            this.lblDateDisplay = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picHome = new System.Windows.Forms.PictureBox();
            this.picSettings = new System.Windows.Forms.PictureBox();
            this.picMenu = new System.Windows.Forms.PictureBox();
            this.picHistory = new System.Windows.Forms.PictureBox();
            this.picDashboard = new System.Windows.Forms.PictureBox();
            this.picAccount = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.pnlSecurityAlerts.SuspendLayout();
            this.pnlAlerts.SuspendLayout();
            this.pnlNotifications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.pnlMainDashboard.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDashboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlChat
            // 
            this.pnlChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlChat.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.pnlChat.BorderRadius = 10;
            this.pnlChat.Controls.Add(this.txtChat);
            this.pnlChat.Controls.Add(this.picSend);
            this.pnlChat.Controls.Add(this.label17);
            this.pnlChat.Controls.Add(this.cmbChooseUser);
            this.pnlChat.Controls.Add(this.pictureBox8);
            this.pnlChat.Location = new System.Drawing.Point(23, 393);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(516, 257);
            this.pnlChat.TabIndex = 17;
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
            // picSend
            // 
            this.picSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picSend.Image = global::Mufaddal_Traders.Properties.Resources.content_send_icon_1320087227200139227_1;
            this.picSend.Location = new System.Drawing.Point(463, 209);
            this.picSend.Margin = new System.Windows.Forms.Padding(2);
            this.picSend.Name = "picSend";
            this.picSend.Size = new System.Drawing.Size(33, 32);
            this.picSend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSend.TabIndex = 19;
            this.picSend.TabStop = false;
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
            // picHeader
            // 
            this.picHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHeader.Controls.Add(this.label5);
            this.picHeader.Controls.Add(this.picMinimize);
            this.picHeader.Controls.Add(this.picClose);
            this.picHeader.Location = new System.Drawing.Point(5, 5);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(1395, 38);
            this.picHeader.TabIndex = 38;
            // 
            // picMinimize
            // 
            this.picMinimize.Image = global::Mufaddal_Traders.Properties.Resources.orange_circle_png_3;
            this.picMinimize.Location = new System.Drawing.Point(41, 8);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Size = new System.Drawing.Size(23, 23);
            this.picMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMinimize.TabIndex = 5;
            this.picMinimize.TabStop = false;
            this.picMinimize.Click += new System.EventHandler(this.picMinimize_Click);
            // 
            // picClose
            // 
            this.picClose.Image = global::Mufaddal_Traders.Properties.Resources.red_circle_emoji_512x512_8xv6a7vo;
            this.picClose.Location = new System.Drawing.Point(12, 8);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(23, 23);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClose.TabIndex = 4;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
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
            // pnlMainDashboard
            // 
            this.pnlMainDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.pnlMainDashboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMainDashboard.Controls.Add(this.pnlChat);
            this.pnlMainDashboard.Controls.Add(this.pnlAlerts);
            this.pnlMainDashboard.Location = new System.Drawing.Point(123, 164);
            this.pnlMainDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.pnlMainDashboard.Name = "pnlMainDashboard";
            this.pnlMainDashboard.Size = new System.Drawing.Size(1234, 666);
            this.pnlMainDashboard.TabIndex = 37;
            // 
            // lblDateDisplay
            // 
            this.lblDateDisplay.AutoSize = true;
            this.lblDateDisplay.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateDisplay.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDateDisplay.Location = new System.Drawing.Point(124, 128);
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
            this.label4.Location = new System.Drawing.Point(247, 91);
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
            this.label3.Location = new System.Drawing.Point(119, 91);
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
            this.label1.Location = new System.Drawing.Point(118, 46);
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
            this.label2.Location = new System.Drawing.Point(118, 55);
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
            this.panel1.Controls.Add(this.picHome);
            this.panel1.Controls.Add(this.picSettings);
            this.panel1.Controls.Add(this.picMenu);
            this.panel1.Controls.Add(this.picHistory);
            this.panel1.Controls.Add(this.picDashboard);
            this.panel1.Controls.Add(this.picAccount);
            this.panel1.Location = new System.Drawing.Point(5, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 805);
            this.panel1.TabIndex = 31;
            // 
            // picHome
            // 
            this.picHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picHome.Image = global::Mufaddal_Traders.Properties.Resources.Home;
            this.picHome.Location = new System.Drawing.Point(14, 662);
            this.picHome.Margin = new System.Windows.Forms.Padding(2);
            this.picHome.Name = "picHome";
            this.picHome.Size = new System.Drawing.Size(44, 47);
            this.picHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHome.TabIndex = 6;
            this.picHome.TabStop = false;
            // 
            // picSettings
            // 
            this.picSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSettings.Image = global::Mufaddal_Traders.Properties.Resources.Settings;
            this.picSettings.Location = new System.Drawing.Point(14, 740);
            this.picSettings.Margin = new System.Windows.Forms.Padding(2);
            this.picSettings.Name = "picSettings";
            this.picSettings.Size = new System.Drawing.Size(44, 46);
            this.picSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSettings.TabIndex = 5;
            this.picSettings.TabStop = false;
            // 
            // picMenu
            // 
            this.picMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picMenu.Image = global::Mufaddal_Traders.Properties.Resources.images__1__5;
            this.picMenu.Location = new System.Drawing.Point(14, 216);
            this.picMenu.Margin = new System.Windows.Forms.Padding(2);
            this.picMenu.Name = "picMenu";
            this.picMenu.Size = new System.Drawing.Size(44, 47);
            this.picMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenu.TabIndex = 4;
            this.picMenu.TabStop = false;
            // 
            // picHistory
            // 
            this.picHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picHistory.Image = global::Mufaddal_Traders.Properties.Resources.recent_3;
            this.picHistory.Location = new System.Drawing.Point(14, 149);
            this.picHistory.Margin = new System.Windows.Forms.Padding(2);
            this.picHistory.Name = "picHistory";
            this.picHistory.Size = new System.Drawing.Size(44, 47);
            this.picHistory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHistory.TabIndex = 3;
            this.picHistory.TabStop = false;
            // 
            // picDashboard
            // 
            this.picDashboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picDashboard.Image = global::Mufaddal_Traders.Properties.Resources.Menu;
            this.picDashboard.Location = new System.Drawing.Point(14, 83);
            this.picDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.picDashboard.Name = "picDashboard";
            this.picDashboard.Size = new System.Drawing.Size(44, 43);
            this.picDashboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDashboard.TabIndex = 2;
            this.picDashboard.TabStop = false;
            // 
            // picAccount
            // 
            this.picAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picAccount.Image = global::Mufaddal_Traders.Properties.Resources.User;
            this.picAccount.Location = new System.Drawing.Point(14, 14);
            this.picAccount.Margin = new System.Windows.Forms.Padding(2);
            this.picAccount.Name = "picAccount";
            this.picAccount.Size = new System.Drawing.Size(44, 46);
            this.picAccount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAccount.TabIndex = 1;
            this.picAccount.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(67, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1342, 31);
            this.label5.TabIndex = 39;
            this.label5.Text = "_________________________________________________________________________________" +
    "__";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
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
            this.Text = "frmDashboard";
            this.pnlChat.ResumeLayout(false);
            this.pnlChat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.picHeader.ResumeLayout(false);
            this.picHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.pnlSecurityAlerts.ResumeLayout(false);
            this.pnlSecurityAlerts.PerformLayout();
            this.pnlAlerts.ResumeLayout(false);
            this.pnlAlerts.PerformLayout();
            this.pnlNotifications.ResumeLayout(false);
            this.pnlNotifications.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.pnlMainDashboard.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDashboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RoundedPanel pnlChat;
        private Guna.UI2.WinForms.Guna2TextBox txtChat;
        private PictureBox picSend;
        private Label label17;
        private Guna.UI2.WinForms.Guna2ComboBox cmbChooseUser;
        private PictureBox pictureBox8;
        private PictureBox picMinimize;
        private PictureBox picClose;
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
        private PictureBox picHome;
        private PictureBox picSettings;
        private PictureBox picMenu;
        private PictureBox picHistory;
        private PictureBox picDashboard;
        private PictureBox picAccount;
        private Panel panel1;
        private Label label5;
    }
}