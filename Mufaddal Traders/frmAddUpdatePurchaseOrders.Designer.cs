using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmAddUpdatePurchaseOrders
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
            this.label10 = new System.Windows.Forms.Label();
            this.cmbSupplierID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSupplierName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlAlerts = new Mufaddal_Traders.RoundedPanel();
            this.txtPO_ID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQty3 = new System.Windows.Forms.TextBox();
            this.cmbItemID3 = new System.Windows.Forms.ComboBox();
            this.txtItemName3 = new System.Windows.Forms.TextBox();
            this.cmbItemID4 = new System.Windows.Forms.ComboBox();
            this.txtItemName4 = new System.Windows.Forms.TextBox();
            this.txtQty2 = new System.Windows.Forms.TextBox();
            this.txtItemName1 = new System.Windows.Forms.TextBox();
            this.txtQty4 = new System.Windows.Forms.TextBox();
            this.cmbItemID2 = new System.Windows.Forms.ComboBox();
            this.cmbItemID1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemName2 = new System.Windows.Forms.TextBox();
            this.txtQty5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtQty1 = new System.Windows.Forms.TextBox();
            this.cmbItemID5 = new System.Windows.Forms.ComboBox();
            this.txtItemName5 = new System.Windows.Forms.TextBox();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlAlerts.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(36, 473);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 24);
            this.label10.TabIndex = 103;
            this.label10.Text = "Supplier ID";
            // 
            // cmbSupplierID
            // 
            this.cmbSupplierID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplierID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSupplierID.FormattingEnabled = true;
            this.cmbSupplierID.Location = new System.Drawing.Point(48, 505);
            this.cmbSupplierID.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSupplierID.Name = "cmbSupplierID";
            this.cmbSupplierID.Size = new System.Drawing.Size(97, 33);
            this.cmbSupplierID.TabIndex = 104;
            this.cmbSupplierID.SelectedIndexChanged += new System.EventHandler(this.cmbSupplierID_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(165, 473);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 24);
            this.label2.TabIndex = 105;
            this.label2.Text = "Supplier Name";
            // 
            // picHeader
            // 
            this.picHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHeader.BorderColor = System.Drawing.Color.Black;
            this.picHeader.BorderThickness = 2;
            this.picHeader.Controls.Add(this.btnMinimize);
            this.picHeader.Controls.Add(this.btnClose);
            this.picHeader.Location = new System.Drawing.Point(4, 5);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(1067, 38);
            this.picHeader.TabIndex = 195;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(359, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 36);
            this.label1.TabIndex = 198;
            this.label1.Text = "Manage Purchase Orders";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.BackColor = System.Drawing.Color.BurlyWood;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierName.Location = new System.Drawing.Point(177, 505);
            this.txtSupplierName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.Size = new System.Drawing.Size(167, 33);
            this.txtSupplierName.TabIndex = 103;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::Mufaddal_Traders.Properties.Resources.Order_ahead_rafiki_1;
            this.pictureBox1.Location = new System.Drawing.Point(579, 125);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(475, 463);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 203;
            this.pictureBox1.TabStop = false;
            // 
            // btnBack
            // 
            this.btnBack.Animated = true;
            this.btnBack.BorderRadius = 7;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.FillColor = System.Drawing.Color.Transparent;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Image = global::Mufaddal_Traders.Properties.Resources.Arrow_Left_512_ezgif_com_webp_to_png_converter;
            this.btnBack.ImageSize = new System.Drawing.Size(40, 32);
            this.btnBack.Location = new System.Drawing.Point(55, 64);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(45, 45);
            this.btnBack.TabIndex = 201;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSave
            // 
            this.btnSave.Animated = true;
            this.btnSave.AutoRoundedCorners = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.BorderRadius = 18;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.MediumAquamarine;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Image = global::Mufaddal_Traders.Properties.Resources.save_icon_2048x2048_iovw4qr4;
            this.btnSave.ImageSize = new System.Drawing.Size(17, 17);
            this.btnSave.Location = new System.Drawing.Point(443, 550);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 38);
            this.btnSave.TabIndex = 200;
            this.btnSave.Text = " Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Animated = true;
            this.txtSearch.AutoRoundedCorners = true;
            this.txtSearch.BorderColor = System.Drawing.Color.MediumAquamarine;
            this.txtSearch.BorderRadius = 18;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FillColor = System.Drawing.Color.Gainsboro;
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.IconLeft = global::Mufaddal_Traders.Properties.Resources.Search;
            this.txtSearch.Location = new System.Drawing.Point(177, 130);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.Black;
            this.txtSearch.PlaceholderText = "     Search";
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(218, 38);
            this.txtSearch.TabIndex = 199;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // pnlAlerts
            // 
            this.pnlAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlAlerts.BorderColor = System.Drawing.Color.DarkGray;
            this.pnlAlerts.BorderRadius = 10;
            this.pnlAlerts.BorderThickness = 3;
            this.pnlAlerts.Controls.Add(this.txtPO_ID);
            this.pnlAlerts.Controls.Add(this.label4);
            this.pnlAlerts.Controls.Add(this.txtQty3);
            this.pnlAlerts.Controls.Add(this.cmbItemID3);
            this.pnlAlerts.Controls.Add(this.txtItemName3);
            this.pnlAlerts.Controls.Add(this.cmbItemID4);
            this.pnlAlerts.Controls.Add(this.txtItemName4);
            this.pnlAlerts.Controls.Add(this.txtQty2);
            this.pnlAlerts.Controls.Add(this.txtItemName1);
            this.pnlAlerts.Controls.Add(this.txtQty4);
            this.pnlAlerts.Controls.Add(this.cmbItemID2);
            this.pnlAlerts.Controls.Add(this.cmbItemID1);
            this.pnlAlerts.Controls.Add(this.label9);
            this.pnlAlerts.Controls.Add(this.label3);
            this.pnlAlerts.Controls.Add(this.txtItemName2);
            this.pnlAlerts.Controls.Add(this.txtQty5);
            this.pnlAlerts.Controls.Add(this.label5);
            this.pnlAlerts.Controls.Add(this.txtQty1);
            this.pnlAlerts.Controls.Add(this.cmbItemID5);
            this.pnlAlerts.Controls.Add(this.txtItemName5);
            this.pnlAlerts.Location = new System.Drawing.Point(23, 192);
            this.pnlAlerts.Name = "pnlAlerts";
            this.pnlAlerts.Size = new System.Drawing.Size(533, 261);
            this.pnlAlerts.TabIndex = 107;
            // 
            // txtPO_ID
            // 
            this.txtPO_ID.BackColor = System.Drawing.Color.BurlyWood;
            this.txtPO_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPO_ID.Location = new System.Drawing.Point(17, 49);
            this.txtPO_ID.Margin = new System.Windows.Forms.Padding(2);
            this.txtPO_ID.Name = "txtPO_ID";
            this.txtPO_ID.ReadOnly = true;
            this.txtPO_ID.Size = new System.Drawing.Size(108, 28);
            this.txtPO_ID.TabIndex = 217;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(149, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 24);
            this.label4.TabIndex = 83;
            this.label4.Text = "Item ID";
            // 
            // txtQty3
            // 
            this.txtQty3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty3.Location = new System.Drawing.Point(413, 132);
            this.txtQty3.Margin = new System.Windows.Forms.Padding(2);
            this.txtQty3.Name = "txtQty3";
            this.txtQty3.Size = new System.Drawing.Size(104, 33);
            this.txtQty3.TabIndex = 94;
            // 
            // cmbItemID3
            // 
            this.cmbItemID3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID3.FormattingEnabled = true;
            this.cmbItemID3.Location = new System.Drawing.Point(142, 132);
            this.cmbItemID3.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID3.Name = "cmbItemID3";
            this.cmbItemID3.Size = new System.Drawing.Size(79, 33);
            this.cmbItemID3.TabIndex = 93;
            this.cmbItemID3.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // txtItemName3
            // 
            this.txtItemName3.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName3.Location = new System.Drawing.Point(233, 132);
            this.txtItemName3.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName3.Name = "txtItemName3";
            this.txtItemName3.ReadOnly = true;
            this.txtItemName3.Size = new System.Drawing.Size(167, 33);
            this.txtItemName3.TabIndex = 92;
            // 
            // cmbItemID4
            // 
            this.cmbItemID4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID4.FormattingEnabled = true;
            this.cmbItemID4.Location = new System.Drawing.Point(142, 173);
            this.cmbItemID4.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID4.Name = "cmbItemID4";
            this.cmbItemID4.Size = new System.Drawing.Size(79, 33);
            this.cmbItemID4.TabIndex = 96;
            this.cmbItemID4.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // txtItemName4
            // 
            this.txtItemName4.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName4.Location = new System.Drawing.Point(233, 173);
            this.txtItemName4.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName4.Name = "txtItemName4";
            this.txtItemName4.ReadOnly = true;
            this.txtItemName4.Size = new System.Drawing.Size(167, 33);
            this.txtItemName4.TabIndex = 95;
            // 
            // txtQty2
            // 
            this.txtQty2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty2.Location = new System.Drawing.Point(413, 89);
            this.txtQty2.Margin = new System.Windows.Forms.Padding(2);
            this.txtQty2.Name = "txtQty2";
            this.txtQty2.Size = new System.Drawing.Size(104, 33);
            this.txtQty2.TabIndex = 91;
            // 
            // txtItemName1
            // 
            this.txtItemName1.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName1.Location = new System.Drawing.Point(233, 49);
            this.txtItemName1.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName1.Name = "txtItemName1";
            this.txtItemName1.ReadOnly = true;
            this.txtItemName1.Size = new System.Drawing.Size(167, 33);
            this.txtItemName1.TabIndex = 82;
            // 
            // txtQty4
            // 
            this.txtQty4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty4.Location = new System.Drawing.Point(413, 173);
            this.txtQty4.Margin = new System.Windows.Forms.Padding(2);
            this.txtQty4.Name = "txtQty4";
            this.txtQty4.Size = new System.Drawing.Size(104, 33);
            this.txtQty4.TabIndex = 97;
            // 
            // cmbItemID2
            // 
            this.cmbItemID2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID2.FormattingEnabled = true;
            this.cmbItemID2.Location = new System.Drawing.Point(142, 89);
            this.cmbItemID2.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID2.Name = "cmbItemID2";
            this.cmbItemID2.Size = new System.Drawing.Size(79, 33);
            this.cmbItemID2.TabIndex = 90;
            this.cmbItemID2.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // cmbItemID1
            // 
            this.cmbItemID1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID1.FormattingEnabled = true;
            this.cmbItemID1.Location = new System.Drawing.Point(142, 49);
            this.cmbItemID1.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID1.Name = "cmbItemID1";
            this.cmbItemID1.Size = new System.Drawing.Size(79, 33);
            this.cmbItemID1.TabIndex = 84;
            this.cmbItemID1.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 24);
            this.label9.TabIndex = 98;
            this.label9.Text = "Purchase ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(264, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 24);
            this.label3.TabIndex = 85;
            this.label3.Text = "Item Name";
            // 
            // txtItemName2
            // 
            this.txtItemName2.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName2.Location = new System.Drawing.Point(233, 89);
            this.txtItemName2.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName2.Name = "txtItemName2";
            this.txtItemName2.ReadOnly = true;
            this.txtItemName2.Size = new System.Drawing.Size(167, 33);
            this.txtItemName2.TabIndex = 89;
            // 
            // txtQty5
            // 
            this.txtQty5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty5.Location = new System.Drawing.Point(413, 215);
            this.txtQty5.Margin = new System.Windows.Forms.Padding(2);
            this.txtQty5.Name = "txtQty5";
            this.txtQty5.Size = new System.Drawing.Size(104, 33);
            this.txtQty5.TabIndex = 102;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(427, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 24);
            this.label5.TabIndex = 86;
            this.label5.Text = "Item Qty";
            // 
            // txtQty1
            // 
            this.txtQty1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty1.Location = new System.Drawing.Point(413, 49);
            this.txtQty1.Margin = new System.Windows.Forms.Padding(2);
            this.txtQty1.Name = "txtQty1";
            this.txtQty1.Size = new System.Drawing.Size(104, 33);
            this.txtQty1.TabIndex = 88;
            // 
            // cmbItemID5
            // 
            this.cmbItemID5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID5.FormattingEnabled = true;
            this.cmbItemID5.Location = new System.Drawing.Point(142, 215);
            this.cmbItemID5.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID5.Name = "cmbItemID5";
            this.cmbItemID5.Size = new System.Drawing.Size(79, 33);
            this.cmbItemID5.TabIndex = 101;
            this.cmbItemID5.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // txtItemName5
            // 
            this.txtItemName5.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName5.Location = new System.Drawing.Point(233, 215);
            this.txtItemName5.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName5.Name = "txtItemName5";
            this.txtItemName5.ReadOnly = true;
            this.txtItemName5.Size = new System.Drawing.Size(167, 33);
            this.txtItemName5.TabIndex = 100;
            // 
            // btnClear
            // 
            this.btnClear.Animated = true;
            this.btnClear.AutoRoundedCorners = true;
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnClear.BorderRadius = 18;
            this.btnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClear.FillColor = System.Drawing.Color.Gray;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = global::Mufaddal_Traders.Properties.Resources._254686;
            this.btnClear.Location = new System.Drawing.Point(328, 550);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(99, 38);
            this.btnClear.TabIndex = 231;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmAddUpdatePurchaseOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 615);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtSupplierName);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.pnlAlerts);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSupplierID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddUpdatePurchaseOrders";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddUpdatePurchaseOrders";
            this.Load += new System.EventHandler(this.frmAddUpdatePurchaseOrders_Load);
            this.picHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlAlerts.ResumeLayout(false);
            this.pnlAlerts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbSupplierID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtItemName4;
        private System.Windows.Forms.TextBox txtItemName1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbItemID1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtQty5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbItemID5;
        private System.Windows.Forms.TextBox txtItemName5;
        private System.Windows.Forms.TextBox txtQty1;
        private System.Windows.Forms.TextBox txtItemName2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbItemID2;
        private System.Windows.Forms.TextBox txtQty4;
        private System.Windows.Forms.TextBox txtQty2;
        private System.Windows.Forms.ComboBox cmbItemID4;
        private System.Windows.Forms.TextBox txtItemName3;
        private System.Windows.Forms.ComboBox cmbItemID3;
        private System.Windows.Forms.TextBox txtQty3;
        private RoundedPanel pnlAlerts;
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSupplierName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private TextBox txtPO_ID;
        private Guna.UI2.WinForms.Guna2Button btnClear;
    }
}