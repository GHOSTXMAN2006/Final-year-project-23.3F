﻿using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmAddDiscardedGoods
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.QtyDisplay = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWarehouseName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbWarehouseID = new System.Windows.Forms.ComboBox();
            this.cmbItemID = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDiscardedGoodID = new System.Windows.Forms.TextBox();
            this.txtItemQty = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.picHeader.Location = new System.Drawing.Point(4, 5);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(860, 38);
            this.picHeader.TabIndex = 225;
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
            this.btnMinimize.Location = new System.Drawing.Point(41, 6);
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
            this.btnClose.Location = new System.Drawing.Point(9, 6);
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
            this.label1.Location = new System.Drawing.Point(265, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 36);
            this.label1.TabIndex = 218;
            this.label1.Text = "Add Discarded Goods";
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
            this.btnBack.Location = new System.Drawing.Point(48, 59);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(45, 45);
            this.btnBack.TabIndex = 228;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
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
            this.txtSearch.Location = new System.Drawing.Point(161, 117);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.Black;
            this.txtSearch.PlaceholderText = "     Search";
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(218, 38);
            this.txtSearch.TabIndex = 226;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::Mufaddal_Traders.Properties.Resources.Discarded_idea_amico;
            this.pictureBox1.Location = new System.Drawing.Point(472, 136);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(369, 320);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 220;
            this.pictureBox1.TabStop = false;
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
            this.btnSave.Location = new System.Drawing.Point(224, 469);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 38);
            this.btnSave.TabIndex = 311;
            this.btnSave.Text = " Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // QtyDisplay
            // 
            this.QtyDisplay.BackColor = System.Drawing.Color.LightGray;
            this.QtyDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QtyDisplay.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.QtyDisplay.Location = new System.Drawing.Point(387, 424);
            this.QtyDisplay.Name = "QtyDisplay";
            this.QtyDisplay.Size = new System.Drawing.Size(69, 27);
            this.QtyDisplay.TabIndex = 310;
            this.QtyDisplay.Text = "000000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(165, 248);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 24);
            this.label8.TabIndex = 309;
            this.label8.Text = "Warehouse Name";
            // 
            // txtWarehouseName
            // 
            this.txtWarehouseName.BackColor = System.Drawing.Color.BurlyWood;
            this.txtWarehouseName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWarehouseName.Location = new System.Drawing.Point(169, 274);
            this.txtWarehouseName.Margin = new System.Windows.Forms.Padding(2);
            this.txtWarehouseName.Name = "txtWarehouseName";
            this.txtWarehouseName.ReadOnly = true;
            this.txtWarehouseName.Size = new System.Drawing.Size(232, 33);
            this.txtWarehouseName.TabIndex = 308;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 248);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 24);
            this.label6.TabIndex = 307;
            this.label6.Text = "Warehouse ID";
            // 
            // cmbWarehouseID
            // 
            this.cmbWarehouseID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouseID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWarehouseID.FormattingEnabled = true;
            this.cmbWarehouseID.Location = new System.Drawing.Point(25, 274);
            this.cmbWarehouseID.Margin = new System.Windows.Forms.Padding(2);
            this.cmbWarehouseID.Name = "cmbWarehouseID";
            this.cmbWarehouseID.Size = new System.Drawing.Size(125, 33);
            this.cmbWarehouseID.TabIndex = 306;
            this.cmbWarehouseID.SelectedIndexChanged += new System.EventHandler(this.cmbWarehouseID_SelectedIndexChanged);
            // 
            // cmbItemID
            // 
            this.cmbItemID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemID.FormattingEnabled = true;
            this.cmbItemID.Location = new System.Drawing.Point(26, 418);
            this.cmbItemID.Margin = new System.Windows.Forms.Padding(2);
            this.cmbItemID.Name = "cmbItemID";
            this.cmbItemID.Size = new System.Drawing.Size(124, 33);
            this.cmbItemID.TabIndex = 305;
            this.cmbItemID.SelectedIndexChanged += new System.EventHandler(this.cmbItemID_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 175);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 24);
            this.label7.TabIndex = 304;
            this.label7.Text = "Discarded Good ID";
            // 
            // txtDiscardedGoodID
            // 
            this.txtDiscardedGoodID.BackColor = System.Drawing.Color.BurlyWood;
            this.txtDiscardedGoodID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscardedGoodID.Location = new System.Drawing.Point(26, 201);
            this.txtDiscardedGoodID.Margin = new System.Windows.Forms.Padding(2);
            this.txtDiscardedGoodID.Name = "txtDiscardedGoodID";
            this.txtDiscardedGoodID.ReadOnly = true;
            this.txtDiscardedGoodID.Size = new System.Drawing.Size(126, 28);
            this.txtDiscardedGoodID.TabIndex = 303;
            // 
            // txtItemQty
            // 
            this.txtItemQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemQty.Location = new System.Drawing.Point(26, 346);
            this.txtItemQty.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemQty.Name = "txtItemQty";
            this.txtItemQty.Size = new System.Drawing.Size(126, 28);
            this.txtItemQty.TabIndex = 302;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 320);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 24);
            this.label5.TabIndex = 301;
            this.label5.Text = "Item Quantity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(165, 394);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 24);
            this.label3.TabIndex = 300;
            this.label3.Text = "Item Name";
            // 
            // txtItemName
            // 
            this.txtItemName.BackColor = System.Drawing.Color.BurlyWood;
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName.Location = new System.Drawing.Point(169, 422);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(195, 29);
            this.txtItemName.TabIndex = 299;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 394);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 24);
            this.label4.TabIndex = 298;
            this.label4.Text = "Item ID";
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
            this.btnClear.Location = new System.Drawing.Point(357, 469);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(99, 38);
            this.btnClear.TabIndex = 312;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmAddDiscardedGoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 535);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.QtyDisplay);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWarehouseName);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmbWarehouseID);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cmbItemID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.txtDiscardedGoodID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtItemQty);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddDiscardedGoods";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddDiscardedGoods";
            this.Load += new System.EventHandler(this.frmAddDiscardedGoods_Load);
            this.picHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2HtmlLabel QtyDisplay;
        private Label label8;
        private TextBox txtWarehouseName;
        private Label label6;
        private ComboBox cmbWarehouseID;
        private ComboBox cmbItemID;
        private Label label7;
        private TextBox txtDiscardedGoodID;
        private TextBox txtItemQty;
        private Label label5;
        private Label label3;
        private TextBox txtItemName;
        private Label label4;
        private Guna.UI2.WinForms.Guna2Button btnClear;
    }
}