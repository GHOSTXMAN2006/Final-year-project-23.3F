using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmAddUpdateGRN
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
            this.label8 = new System.Windows.Forms.Label();
            this.txtSupplierName = new System.Windows.Forms.TextBox();
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGRN_Type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGRN_ID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtWarehouseName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbWarehouseID = new System.Windows.Forms.ComboBox();
            this.rbPurchaseContract = new Guna.UI2.WinForms.Guna2RadioButton();
            this.rbPurchaseOrder = new Guna.UI2.WinForms.Guna2RadioButton();
            this.txtSupplierID = new System.Windows.Forms.TextBox();
            this.cmbPurchaseOrGIN_ID = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtItemQtys = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtItemIDs = new System.Windows.Forms.TextBox();
            this.txtItemNames = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.rbGIN = new Guna.UI2.WinForms.Guna2RadioButton();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(203, 455);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 24);
            this.label8.TabIndex = 256;
            this.label8.Text = "Supplier Name";
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.BackColor = System.Drawing.Color.BurlyWood;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierName.Location = new System.Drawing.Point(206, 484);
            this.txtSupplierName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.Size = new System.Drawing.Size(196, 33);
            this.txtSupplierName.TabIndex = 254;
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
            this.picHeader.Size = new System.Drawing.Size(865, 38);
            this.picHeader.TabIndex = 247;
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 455);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 24);
            this.label4.TabIndex = 244;
            this.label4.Text = "Supplier ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(343, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 36);
            this.label1.TabIndex = 243;
            this.label1.Text = "Manage GRN";
            // 
            // cmbGRN_Type
            // 
            this.cmbGRN_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGRN_Type.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGRN_Type.FormattingEnabled = true;
            this.cmbGRN_Type.Items.AddRange(new object[] {
            "Purchased goods",
            "Finished goods GRN ",
            "Packaging materials GRN ",
            "Other GRN"});
            this.cmbGRN_Type.Location = new System.Drawing.Point(55, 234);
            this.cmbGRN_Type.Margin = new System.Windows.Forms.Padding(2);
            this.cmbGRN_Type.Name = "cmbGRN_Type";
            this.cmbGRN_Type.Size = new System.Drawing.Size(284, 33);
            this.cmbGRN_Type.TabIndex = 260;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 204);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 24);
            this.label3.TabIndex = 259;
            this.label3.Text = "GRN Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(51, 132);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 24);
            this.label5.TabIndex = 261;
            this.label5.Text = "GRN ID";
            // 
            // txtGRN_ID
            // 
            this.txtGRN_ID.BackColor = System.Drawing.Color.BurlyWood;
            this.txtGRN_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGRN_ID.Location = new System.Drawing.Point(54, 158);
            this.txtGRN_ID.Margin = new System.Windows.Forms.Padding(2);
            this.txtGRN_ID.Name = "txtGRN_ID";
            this.txtGRN_ID.ReadOnly = true;
            this.txtGRN_ID.Size = new System.Drawing.Size(114, 28);
            this.txtGRN_ID.TabIndex = 262;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(203, 661);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(164, 24);
            this.label7.TabIndex = 266;
            this.label7.Text = "Warehouse Name";
            // 
            // txtWarehouseName
            // 
            this.txtWarehouseName.BackColor = System.Drawing.Color.BurlyWood;
            this.txtWarehouseName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWarehouseName.Location = new System.Drawing.Point(208, 687);
            this.txtWarehouseName.Margin = new System.Windows.Forms.Padding(2);
            this.txtWarehouseName.Name = "txtWarehouseName";
            this.txtWarehouseName.ReadOnly = true;
            this.txtWarehouseName.Size = new System.Drawing.Size(196, 33);
            this.txtWarehouseName.TabIndex = 265;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(51, 661);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 24);
            this.label10.TabIndex = 264;
            this.label10.Text = "Warehouse ID";
            // 
            // cmbWarehouseID
            // 
            this.cmbWarehouseID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouseID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWarehouseID.FormattingEnabled = true;
            this.cmbWarehouseID.Location = new System.Drawing.Point(57, 687);
            this.cmbWarehouseID.Margin = new System.Windows.Forms.Padding(2);
            this.cmbWarehouseID.Name = "cmbWarehouseID";
            this.cmbWarehouseID.Size = new System.Drawing.Size(113, 33);
            this.cmbWarehouseID.TabIndex = 263;
            this.cmbWarehouseID.SelectedIndexChanged += new System.EventHandler(this.cmbWarehouseID_SelectedIndexChanged);
            // 
            // rbPurchaseContract
            // 
            this.rbPurchaseContract.AutoSize = true;
            this.rbPurchaseContract.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbPurchaseContract.CheckedState.BorderThickness = 0;
            this.rbPurchaseContract.CheckedState.FillColor = System.Drawing.Color.Black;
            this.rbPurchaseContract.CheckedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.rbPurchaseContract.CheckedState.InnerOffset = -9;
            this.rbPurchaseContract.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.rbPurchaseContract.ForeColor = System.Drawing.Color.Black;
            this.rbPurchaseContract.Location = new System.Drawing.Point(246, 286);
            this.rbPurchaseContract.Name = "rbPurchaseContract";
            this.rbPurchaseContract.Size = new System.Drawing.Size(184, 29);
            this.rbPurchaseContract.TabIndex = 286;
            this.rbPurchaseContract.Text = "Purchase Contract";
            this.rbPurchaseContract.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbPurchaseContract.UncheckedState.BorderThickness = 2;
            this.rbPurchaseContract.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rbPurchaseContract.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rbPurchaseContract.CheckedChanged += new System.EventHandler(this.rbPurchaseContract_CheckedChanged);
            // 
            // rbPurchaseOrder
            // 
            this.rbPurchaseOrder.AutoSize = true;
            this.rbPurchaseOrder.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbPurchaseOrder.CheckedState.BorderThickness = 0;
            this.rbPurchaseOrder.CheckedState.FillColor = System.Drawing.Color.Black;
            this.rbPurchaseOrder.CheckedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.rbPurchaseOrder.CheckedState.InnerOffset = -9;
            this.rbPurchaseOrder.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.rbPurchaseOrder.ForeColor = System.Drawing.Color.Black;
            this.rbPurchaseOrder.Location = new System.Drawing.Point(56, 286);
            this.rbPurchaseOrder.Name = "rbPurchaseOrder";
            this.rbPurchaseOrder.Size = new System.Drawing.Size(161, 29);
            this.rbPurchaseOrder.TabIndex = 285;
            this.rbPurchaseOrder.Text = "Purchase Order";
            this.rbPurchaseOrder.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbPurchaseOrder.UncheckedState.BorderThickness = 2;
            this.rbPurchaseOrder.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rbPurchaseOrder.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rbPurchaseOrder.CheckedChanged += new System.EventHandler(this.rbPurchaseOrder_CheckedChanged);
            // 
            // txtSupplierID
            // 
            this.txtSupplierID.BackColor = System.Drawing.Color.BurlyWood;
            this.txtSupplierID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierID.Location = new System.Drawing.Point(56, 484);
            this.txtSupplierID.Margin = new System.Windows.Forms.Padding(2);
            this.txtSupplierID.Name = "txtSupplierID";
            this.txtSupplierID.ReadOnly = true;
            this.txtSupplierID.Size = new System.Drawing.Size(115, 33);
            this.txtSupplierID.TabIndex = 287;
            // 
            // cmbPurchaseOrGIN_ID
            // 
            this.cmbPurchaseOrGIN_ID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchaseOrGIN_ID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPurchaseOrGIN_ID.FormattingEnabled = true;
            this.cmbPurchaseOrGIN_ID.Location = new System.Drawing.Point(56, 405);
            this.cmbPurchaseOrGIN_ID.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPurchaseOrGIN_ID.Name = "cmbPurchaseOrGIN_ID";
            this.cmbPurchaseOrGIN_ID.Size = new System.Drawing.Size(113, 33);
            this.cmbPurchaseOrGIN_ID.TabIndex = 291;
            this.cmbPurchaseOrGIN_ID.SelectedIndexChanged += new System.EventHandler(this.cmbPurchaseOrGIN_ID_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(52, 375);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 24);
            this.label11.TabIndex = 290;
            this.label11.Text = "Purchase ID";
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
            this.btnBack.Location = new System.Drawing.Point(55, 66);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(45, 45);
            this.btnBack.TabIndex = 250;
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
            this.btnSave.Location = new System.Drawing.Point(452, 682);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 38);
            this.btnSave.TabIndex = 249;
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
            this.txtSearch.Location = new System.Drawing.Point(209, 99);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.Black;
            this.txtSearch.PlaceholderText = "     Search";
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(219, 38);
            this.txtSearch.TabIndex = 248;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::Mufaddal_Traders.Properties.Resources.Receipt_bro_1;
            this.pictureBox1.Location = new System.Drawing.Point(464, 139);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(363, 344);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 245;
            this.pictureBox1.TabStop = false;
            // 
            // txtItemQtys
            // 
            this.txtItemQtys.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemQtys.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemQtys.Location = new System.Drawing.Point(450, 556);
            this.txtItemQtys.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemQtys.Multiline = true;
            this.txtItemQtys.Name = "txtItemQtys";
            this.txtItemQtys.ReadOnly = true;
            this.txtItemQtys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtItemQtys.Size = new System.Drawing.Size(255, 88);
            this.txtItemQtys.TabIndex = 253;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(447, 531);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 24);
            this.label6.TabIndex = 252;
            this.label6.Text = "Item Quantity";
            // 
            // txtItemIDs
            // 
            this.txtItemIDs.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemIDs.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemIDs.Location = new System.Drawing.Point(56, 556);
            this.txtItemIDs.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemIDs.Multiline = true;
            this.txtItemIDs.Name = "txtItemIDs";
            this.txtItemIDs.ReadOnly = true;
            this.txtItemIDs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtItemIDs.Size = new System.Drawing.Size(115, 88);
            this.txtItemIDs.TabIndex = 289;
            // 
            // txtItemNames
            // 
            this.txtItemNames.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemNames.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemNames.Location = new System.Drawing.Point(207, 556);
            this.txtItemNames.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemNames.Multiline = true;
            this.txtItemNames.Name = "txtItemNames";
            this.txtItemNames.ReadOnly = true;
            this.txtItemNames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtItemNames.Size = new System.Drawing.Size(196, 88);
            this.txtItemNames.TabIndex = 257;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(203, 531);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 24);
            this.label9.TabIndex = 258;
            this.label9.Text = "Item Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 530);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 24);
            this.label2.TabIndex = 246;
            this.label2.Text = "Item ID";
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
            this.btnClear.Image = global::Mufaddal_Traders.Properties.Resources.Edit;
            this.btnClear.Location = new System.Drawing.Point(696, 682);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(99, 38);
            this.btnClear.TabIndex = 293;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Animated = true;
            this.btnUpdate.AutoRoundedCorners = true;
            this.btnUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdate.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnUpdate.BorderRadius = 18;
            this.btnUpdate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUpdate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUpdate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(124)))), ((int)(((byte)(44)))));
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Image = global::Mufaddal_Traders.Properties.Resources.Edit;
            this.btnUpdate.Location = new System.Drawing.Point(581, 682);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(99, 38);
            this.btnUpdate.TabIndex = 292;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // rbGIN
            // 
            this.rbGIN.AutoSize = true;
            this.rbGIN.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbGIN.CheckedState.BorderThickness = 0;
            this.rbGIN.CheckedState.FillColor = System.Drawing.Color.Black;
            this.rbGIN.CheckedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.rbGIN.CheckedState.InnerOffset = -9;
            this.rbGIN.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.rbGIN.ForeColor = System.Drawing.Color.Black;
            this.rbGIN.Location = new System.Drawing.Point(56, 330);
            this.rbGIN.Name = "rbGIN";
            this.rbGIN.Size = new System.Drawing.Size(62, 29);
            this.rbGIN.TabIndex = 294;
            this.rbGIN.Text = "GIN";
            this.rbGIN.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.rbGIN.UncheckedState.BorderThickness = 2;
            this.rbGIN.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rbGIN.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rbGIN.CheckedChanged += new System.EventHandler(this.rbGIN_CheckedChanged);
            // 
            // frmAddUpdateGRN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 751);
            this.ControlBox = false;
            this.Controls.Add(this.rbGIN);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cmbPurchaseOrGIN_ID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtItemIDs);
            this.Controls.Add(this.txtSupplierID);
            this.Controls.Add(this.rbPurchaseContract);
            this.Controls.Add(this.rbPurchaseOrder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtWarehouseName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbWarehouseID);
            this.Controls.Add(this.txtGRN_ID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbGRN_Type);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtItemNames);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSupplierName);
            this.Controls.Add(this.txtItemQtys);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddUpdateGRN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "z";
            this.Load += new System.EventHandler(this.frmAddUpdateGRN_Load);
            this.picHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSupplierName;
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbGRN_Type;
        private System.Windows.Forms.Label label3;
        private Label label5;
        private TextBox txtGRN_ID;
        private Label label7;
        private TextBox txtWarehouseName;
        private Label label10;
        private ComboBox cmbWarehouseID;
        private Guna.UI2.WinForms.Guna2RadioButton rbPurchaseContract;
        private Guna.UI2.WinForms.Guna2RadioButton rbPurchaseOrder;
        private TextBox txtSupplierID;
        private ComboBox cmbPurchaseOrGIN_ID;
        private Label label11;
        private TextBox txtItemQtys;
        private Label label6;
        private TextBox txtItemIDs;
        private TextBox txtItemNames;
        private Label label9;
        private Label label2;
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2RadioButton rbGIN;
    }
}