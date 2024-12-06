using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmStorekeeperMenu
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSettings = new Guna.UI2.WinForms.Guna2Button();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.btnMenu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHistory = new Guna.UI2.WinForms.Guna2Button();
            this.btnDashboard = new Guna.UI2.WinForms.Guna2Button();
            this.btnAccount = new Guna.UI2.WinForms.Guna2Button();
            this.pnlChat = new RoundedPanel();
            this.tilePaymentVouchers = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileWarehouse = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileDiscardedGoods = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileStockDamage = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileStock = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileStockTransfer = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileSRN = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileGRN = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileGIN = new Guna.UI2.WinForms.Guna2TileButton();
            this.tilePC = new Guna.UI2.WinForms.Guna2TileButton();
            this.tilePO = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileSuppliers = new Guna.UI2.WinForms.Guna2TileButton();
            this.tileItems = new Guna.UI2.WinForms.Guna2TileButton();
            this.picHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlChat.SuspendLayout();
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
            this.picHeader.Location = new System.Drawing.Point(4, 4);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(1396, 38);
            this.picHeader.TabIndex = 42;
            this.picHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.picHeader_Paint);
            this.picHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
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
            this.panel1.Location = new System.Drawing.Point(4, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 805);
            this.panel1.TabIndex = 41;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(92, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 32);
            this.label1.TabIndex = 44;
            this.label1.Text = "Menu";
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
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
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
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
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
            // pnlChat
            // 
            this.pnlChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlChat.BorderColor = System.Drawing.Color.Empty;
            this.pnlChat.BorderRadius = 10;
            this.pnlChat.Controls.Add(this.tilePaymentVouchers);
            this.pnlChat.Controls.Add(this.tileWarehouse);
            this.pnlChat.Controls.Add(this.tileDiscardedGoods);
            this.pnlChat.Controls.Add(this.tileStockDamage);
            this.pnlChat.Controls.Add(this.tileStock);
            this.pnlChat.Controls.Add(this.tileStockTransfer);
            this.pnlChat.Controls.Add(this.tileSRN);
            this.pnlChat.Controls.Add(this.tileGRN);
            this.pnlChat.Controls.Add(this.tileGIN);
            this.pnlChat.Controls.Add(this.tilePC);
            this.pnlChat.Controls.Add(this.tilePO);
            this.pnlChat.Controls.Add(this.tileSuppliers);
            this.pnlChat.Controls.Add(this.tileItems);
            this.pnlChat.Location = new System.Drawing.Point(98, 114);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(1257, 716);
            this.pnlChat.TabIndex = 43;
            this.pnlChat.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChat_Paint);
            // 
            // tilePaymentVouchers
            // 
            this.tilePaymentVouchers.Animated = true;
            this.tilePaymentVouchers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePaymentVouchers.BorderRadius = 5;
            this.tilePaymentVouchers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tilePaymentVouchers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tilePaymentVouchers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tilePaymentVouchers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tilePaymentVouchers.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePaymentVouchers.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tilePaymentVouchers.ForeColor = System.Drawing.Color.DimGray;
            this.tilePaymentVouchers.Image = global::Mufaddal_Traders.Properties.Resources.Money_order_512;
            this.tilePaymentVouchers.ImageSize = new System.Drawing.Size(80, 75);
            this.tilePaymentVouchers.Location = new System.Drawing.Point(37, 427);
            this.tilePaymentVouchers.Name = "tilePaymentVouchers";
            this.tilePaymentVouchers.Size = new System.Drawing.Size(151, 149);
            this.tilePaymentVouchers.TabIndex = 37;
            this.tilePaymentVouchers.Text = "Payment Vouchers";
            // 
            // tileWarehouse
            // 
            this.tileWarehouse.Animated = true;
            this.tileWarehouse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileWarehouse.BorderRadius = 5;
            this.tileWarehouse.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileWarehouse.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileWarehouse.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileWarehouse.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileWarehouse.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileWarehouse.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileWarehouse.ForeColor = System.Drawing.Color.DimGray;
            this.tileWarehouse.Image = global::Mufaddal_Traders.Properties.Resources._6528651;
            this.tileWarehouse.ImageSize = new System.Drawing.Size(80, 75);
            this.tileWarehouse.Location = new System.Drawing.Point(443, 40);
            this.tileWarehouse.Name = "tileWarehouse";
            this.tileWarehouse.Size = new System.Drawing.Size(165, 149);
            this.tileWarehouse.TabIndex = 36;
            this.tileWarehouse.Text = "Warehouse";
            this.tileWarehouse.Click += new System.EventHandler(this.tileWarehouse_Click);
            // 
            // tileDiscardedGoods
            // 
            this.tileDiscardedGoods.Animated = true;
            this.tileDiscardedGoods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileDiscardedGoods.BorderRadius = 5;
            this.tileDiscardedGoods.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileDiscardedGoods.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileDiscardedGoods.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileDiscardedGoods.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileDiscardedGoods.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileDiscardedGoods.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileDiscardedGoods.ForeColor = System.Drawing.Color.DimGray;
            this.tileDiscardedGoods.Image = global::Mufaddal_Traders.Properties.Resources.WhatsApp_Image_2024_12_05_at_13_53_17_7db47e1d;
            this.tileDiscardedGoods.ImageSize = new System.Drawing.Size(80, 75);
            this.tileDiscardedGoods.Location = new System.Drawing.Point(1067, 235);
            this.tileDiscardedGoods.Name = "tileDiscardedGoods";
            this.tileDiscardedGoods.Size = new System.Drawing.Size(156, 149);
            this.tileDiscardedGoods.TabIndex = 35;
            this.tileDiscardedGoods.Text = "Discarded Goods";
            // 
            // tileStockDamage
            // 
            this.tileStockDamage.Animated = true;
            this.tileStockDamage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStockDamage.BorderRadius = 5;
            this.tileStockDamage.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileStockDamage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileStockDamage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileStockDamage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileStockDamage.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStockDamage.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileStockDamage.ForeColor = System.Drawing.Color.DimGray;
            this.tileStockDamage.Image = global::Mufaddal_Traders.Properties.Resources._6179914;
            this.tileStockDamage.ImageSize = new System.Drawing.Size(80, 70);
            this.tileStockDamage.Location = new System.Drawing.Point(869, 235);
            this.tileStockDamage.Name = "tileStockDamage";
            this.tileStockDamage.Size = new System.Drawing.Size(151, 149);
            this.tileStockDamage.TabIndex = 34;
            this.tileStockDamage.Text = "Stock Damage";
            // 
            // tileStock
            // 
            this.tileStock.Animated = true;
            this.tileStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStock.BorderRadius = 5;
            this.tileStock.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileStock.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileStock.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileStock.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileStock.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStock.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileStock.ForeColor = System.Drawing.Color.DimGray;
            this.tileStock.Image = global::Mufaddal_Traders.Properties.Resources.stock_1;
            this.tileStock.ImageSize = new System.Drawing.Size(80, 75);
            this.tileStock.Location = new System.Drawing.Point(240, 40);
            this.tileStock.Name = "tileStock";
            this.tileStock.Size = new System.Drawing.Size(151, 149);
            this.tileStock.TabIndex = 33;
            this.tileStock.Text = "Stock";
            this.tileStock.Click += new System.EventHandler(this.tileStock_Click);
            // 
            // tileStockTransfer
            // 
            this.tileStockTransfer.Animated = true;
            this.tileStockTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStockTransfer.BorderRadius = 5;
            this.tileStockTransfer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileStockTransfer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileStockTransfer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileStockTransfer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileStockTransfer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileStockTransfer.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileStockTransfer.ForeColor = System.Drawing.Color.DimGray;
            this.tileStockTransfer.Image = global::Mufaddal_Traders.Properties.Resources.warehouse_transfer;
            this.tileStockTransfer.ImageSize = new System.Drawing.Size(80, 75);
            this.tileStockTransfer.Location = new System.Drawing.Point(663, 40);
            this.tileStockTransfer.Name = "tileStockTransfer";
            this.tileStockTransfer.Size = new System.Drawing.Size(151, 149);
            this.tileStockTransfer.TabIndex = 32;
            this.tileStockTransfer.Text = "Stock Transfer";
            this.tileStockTransfer.Click += new System.EventHandler(this.tileStockTransfer_Click);
            // 
            // tileSRN
            // 
            this.tileSRN.Animated = true;
            this.tileSRN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileSRN.BorderRadius = 5;
            this.tileSRN.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileSRN.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileSRN.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileSRN.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileSRN.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileSRN.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.tileSRN.ForeColor = System.Drawing.Color.DimGray;
            this.tileSRN.Image = global::Mufaddal_Traders.Properties.Resources.return_icon_logo_illustration_order_delivery_and_reverse_logistics_symbol_template_for_graphic_and_web_design_collection_free_vector_modified_1_;
            this.tileSRN.ImageSize = new System.Drawing.Size(80, 75);
            this.tileSRN.Location = new System.Drawing.Point(663, 235);
            this.tileSRN.Name = "tileSRN";
            this.tileSRN.Size = new System.Drawing.Size(151, 149);
            this.tileSRN.TabIndex = 31;
            this.tileSRN.Text = "SRN";
            // 
            // tileGRN
            // 
            this.tileGRN.Animated = true;
            this.tileGRN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileGRN.BorderRadius = 5;
            this.tileGRN.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileGRN.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileGRN.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileGRN.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileGRN.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileGRN.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.tileGRN.ForeColor = System.Drawing.Color.DimGray;
            this.tileGRN.Image = global::Mufaddal_Traders.Properties.Resources.deliver_check1;
            this.tileGRN.ImageSize = new System.Drawing.Size(80, 75);
            this.tileGRN.Location = new System.Drawing.Point(451, 235);
            this.tileGRN.Name = "tileGRN";
            this.tileGRN.Size = new System.Drawing.Size(151, 149);
            this.tileGRN.TabIndex = 30;
            this.tileGRN.Text = "GRN";
            this.tileGRN.Click += new System.EventHandler(this.tileGRN_Click);
            // 
            // tileGIN
            // 
            this.tileGIN.Animated = true;
            this.tileGIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileGIN.BorderRadius = 5;
            this.tileGIN.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileGIN.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileGIN.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileGIN.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileGIN.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileGIN.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileGIN.ForeColor = System.Drawing.Color.DimGray;
            this.tileGIN.Image = global::Mufaddal_Traders.Properties.Resources.ico_warehouse_management_please_collect_and_issue_modified;
            this.tileGIN.ImageSize = new System.Drawing.Size(80, 75);
            this.tileGIN.Location = new System.Drawing.Point(240, 235);
            this.tileGIN.Name = "tileGIN";
            this.tileGIN.Size = new System.Drawing.Size(151, 149);
            this.tileGIN.TabIndex = 29;
            this.tileGIN.Text = "GIN";
            this.tileGIN.Click += new System.EventHandler(this.tileGIN_Click);
            // 
            // tilePC
            // 
            this.tilePC.Animated = true;
            this.tilePC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePC.BorderRadius = 5;
            this.tilePC.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tilePC.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tilePC.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tilePC.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tilePC.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePC.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tilePC.ForeColor = System.Drawing.Color.DimGray;
            this.tilePC.Image = global::Mufaddal_Traders.Properties.Resources.pc_2;
            this.tilePC.ImageSize = new System.Drawing.Size(80, 70);
            this.tilePC.Location = new System.Drawing.Point(37, 235);
            this.tilePC.Name = "tilePC";
            this.tilePC.Size = new System.Drawing.Size(151, 149);
            this.tilePC.TabIndex = 28;
            this.tilePC.Text = "Purchase Contract";
            this.tilePC.Click += new System.EventHandler(this.tilePC_Click);
            // 
            // tilePO
            // 
            this.tilePO.Animated = true;
            this.tilePO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePO.BorderRadius = 5;
            this.tilePO.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tilePO.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tilePO.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tilePO.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tilePO.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tilePO.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tilePO.ForeColor = System.Drawing.Color.DimGray;
            this.tilePO.Image = global::Mufaddal_Traders.Properties.Resources._9402144;
            this.tilePO.ImageSize = new System.Drawing.Size(80, 70);
            this.tilePO.Location = new System.Drawing.Point(1070, 40);
            this.tilePO.Name = "tilePO";
            this.tilePO.Size = new System.Drawing.Size(151, 149);
            this.tilePO.TabIndex = 27;
            this.tilePO.Text = "Purchase Orders";
            this.tilePO.Click += new System.EventHandler(this.tilePO_Click);
            // 
            // tileSuppliers
            // 
            this.tileSuppliers.Animated = true;
            this.tileSuppliers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileSuppliers.BorderRadius = 5;
            this.tileSuppliers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileSuppliers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileSuppliers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileSuppliers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileSuppliers.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileSuppliers.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileSuppliers.ForeColor = System.Drawing.Color.DimGray;
            this.tileSuppliers.Image = global::Mufaddal_Traders.Properties.Resources._819438;
            this.tileSuppliers.ImageSize = new System.Drawing.Size(80, 75);
            this.tileSuppliers.Location = new System.Drawing.Point(869, 40);
            this.tileSuppliers.Name = "tileSuppliers";
            this.tileSuppliers.Size = new System.Drawing.Size(151, 149);
            this.tileSuppliers.TabIndex = 26;
            this.tileSuppliers.Text = "Suppliers";
            this.tileSuppliers.Click += new System.EventHandler(this.tileSuppliers_Click);
            // 
            // tileItems
            // 
            this.tileItems.Animated = true;
            this.tileItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileItems.BorderRadius = 5;
            this.tileItems.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.tileItems.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.tileItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tileItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.tileItems.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(210)))), ((int)(((byte)(148)))));
            this.tileItems.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.tileItems.ForeColor = System.Drawing.Color.DimGray;
            this.tileItems.Image = global::Mufaddal_Traders.Properties.Resources._3486568;
            this.tileItems.ImageSize = new System.Drawing.Size(80, 70);
            this.tileItems.Location = new System.Drawing.Point(37, 40);
            this.tileItems.Name = "tileItems";
            this.tileItems.Size = new System.Drawing.Size(151, 149);
            this.tileItems.TabIndex = 25;
            this.tileItems.Text = "Items";
            this.tileItems.Click += new System.EventHandler(this.tileItems_Click);
            // 
            // frmStorekeeperMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 851);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlChat);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmStorekeeperMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmStorekeeperMenu";
            this.Load += new System.EventHandler(this.frmStorekeeperMenu_Load);
            this.picHeader.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlChat.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnSettings;
        private Guna.UI2.WinForms.Guna2Button btnHome;
        private Guna.UI2.WinForms.Guna2Button btnMenu;
        private Guna.UI2.WinForms.Guna2Button btnHistory;
        private Guna.UI2.WinForms.Guna2Button btnDashboard;
        private Guna.UI2.WinForms.Guna2Button btnAccount;
        private RoundedPanel pnlChat;
        private Guna.UI2.WinForms.Guna2TileButton tileItems;
        private Guna.UI2.WinForms.Guna2TileButton tileSuppliers;
        private Guna.UI2.WinForms.Guna2TileButton tilePO;
        private Guna.UI2.WinForms.Guna2TileButton tilePC;
        private Guna.UI2.WinForms.Guna2TileButton tileGIN;
        private Guna.UI2.WinForms.Guna2TileButton tileGRN;
        private Guna.UI2.WinForms.Guna2TileButton tileSRN;
        private Guna.UI2.WinForms.Guna2TileButton tileStockTransfer;
        private Guna.UI2.WinForms.Guna2TileButton tileStock;
        private Guna.UI2.WinForms.Guna2TileButton tileStockDamage;
        private Guna.UI2.WinForms.Guna2TileButton tileDiscardedGoods;
        private Guna.UI2.WinForms.Guna2TileButton tileWarehouse;
        private Guna.UI2.WinForms.Guna2TileButton tilePaymentVouchers;
        private Label label1;
    }
}