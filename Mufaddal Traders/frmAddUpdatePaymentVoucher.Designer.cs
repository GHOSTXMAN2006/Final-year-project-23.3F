﻿using System.Drawing;
using System.Windows.Forms;
using System;

namespace Mufaddal_Traders
{
    partial class frmAddUpdatePaymentVoucher
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
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.picHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2RadioButton2 = new Guna.UI2.WinForms.Guna2RadioButton();
            this.guna2RadioButton1 = new Guna.UI2.WinForms.Guna2RadioButton();
            this.guna2CheckBox1 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(44, 388);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 24);
            this.label9.TabIndex = 280;
            this.label9.Text = "Price";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(48, 413);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 33);
            this.textBox1.TabIndex = 279;
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(48, 335);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(124, 33);
            this.comboBox2.TabIndex = 275;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 311);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 24);
            this.label3.TabIndex = 274;
            this.label3.Text = "ID";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(48, 207);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(145, 28);
            this.textBox3.TabIndex = 273;
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
            this.picHeader.TabIndex = 269;
            this.picHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(44, 181);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 24);
            this.label4.TabIndex = 267;
            this.label4.Text = "Payment ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(252, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 36);
            this.label1.TabIndex = 266;
            this.label1.Text = "Manage Payment Voucher";
            // 
            // guna2RadioButton2
            // 
            this.guna2RadioButton2.AutoSize = true;
            this.guna2RadioButton2.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2RadioButton2.CheckedState.BorderThickness = 0;
            this.guna2RadioButton2.CheckedState.FillColor = System.Drawing.Color.Black;
            this.guna2RadioButton2.CheckedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.guna2RadioButton2.CheckedState.InnerOffset = -9;
            this.guna2RadioButton2.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.guna2RadioButton2.ForeColor = System.Drawing.Color.Black;
            this.guna2RadioButton2.Location = new System.Drawing.Point(238, 260);
            this.guna2RadioButton2.Name = "guna2RadioButton2";
            this.guna2RadioButton2.Size = new System.Drawing.Size(184, 29);
            this.guna2RadioButton2.TabIndex = 288;
            this.guna2RadioButton2.Text = "Purchase Contract";
            this.guna2RadioButton2.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2RadioButton2.UncheckedState.BorderThickness = 2;
            this.guna2RadioButton2.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2RadioButton2.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // guna2RadioButton1
            // 
            this.guna2RadioButton1.AutoSize = true;
            this.guna2RadioButton1.Checked = true;
            this.guna2RadioButton1.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2RadioButton1.CheckedState.BorderThickness = 0;
            this.guna2RadioButton1.CheckedState.FillColor = System.Drawing.Color.Black;
            this.guna2RadioButton1.CheckedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.guna2RadioButton1.CheckedState.InnerOffset = -9;
            this.guna2RadioButton1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.guna2RadioButton1.ForeColor = System.Drawing.Color.Black;
            this.guna2RadioButton1.Location = new System.Drawing.Point(48, 260);
            this.guna2RadioButton1.Name = "guna2RadioButton1";
            this.guna2RadioButton1.Size = new System.Drawing.Size(161, 29);
            this.guna2RadioButton1.TabIndex = 287;
            this.guna2RadioButton1.Text = "Purchase Order";
            this.guna2RadioButton1.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2RadioButton1.UncheckedState.BorderThickness = 2;
            this.guna2RadioButton1.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2RadioButton1.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // guna2CheckBox1
            // 
            this.guna2CheckBox1.AutoSize = true;
            this.guna2CheckBox1.CheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2CheckBox1.CheckedState.BorderRadius = 0;
            this.guna2CheckBox1.CheckedState.BorderThickness = 0;
            this.guna2CheckBox1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(195)))), ((int)(((byte)(154)))));
            this.guna2CheckBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.guna2CheckBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2CheckBox1.Location = new System.Drawing.Point(48, 473);
            this.guna2CheckBox1.Name = "guna2CheckBox1";
            this.guna2CheckBox1.Size = new System.Drawing.Size(81, 29);
            this.guna2CheckBox1.TabIndex = 289;
            this.guna2CheckBox1.Text = "Status";
            this.guna2CheckBox1.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.guna2CheckBox1.UncheckedState.BorderRadius = 0;
            this.guna2CheckBox1.UncheckedState.BorderThickness = 0;
            this.guna2CheckBox1.UncheckedState.FillColor = System.Drawing.Color.Silver;
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
            this.btnBack.TabIndex = 272;
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
            this.btnSave.Location = new System.Drawing.Point(309, 464);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 38);
            this.btnSave.TabIndex = 271;
            this.btnSave.Text = " Save";
            // 
            // guna2TextBox2
            // 
            this.guna2TextBox2.Animated = true;
            this.guna2TextBox2.AutoRoundedCorners = true;
            this.guna2TextBox2.BorderColor = System.Drawing.Color.MediumAquamarine;
            this.guna2TextBox2.BorderRadius = 18;
            this.guna2TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox2.DefaultText = "";
            this.guna2TextBox2.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2TextBox2.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.guna2TextBox2.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.IconLeft = global::Mufaddal_Traders.Properties.Resources.Search;
            this.guna2TextBox2.Location = new System.Drawing.Point(161, 117);
            this.guna2TextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2TextBox2.Name = "guna2TextBox2";
            this.guna2TextBox2.PasswordChar = '\0';
            this.guna2TextBox2.PlaceholderForeColor = System.Drawing.Color.Black;
            this.guna2TextBox2.PlaceholderText = "     Search";
            this.guna2TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.guna2TextBox2.SelectedText = "";
            this.guna2TextBox2.Size = new System.Drawing.Size(218, 38);
            this.guna2TextBox2.TabIndex = 270;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::Mufaddal_Traders.Properties.Resources.Cash_Payment_bro__1__1;
            this.pictureBox1.Location = new System.Drawing.Point(456, 155);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(369, 320);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 268;
            this.pictureBox1.TabStop = false;
            // 
            // frmAddUpdatePaymentVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 579);
            this.ControlBox = false;
            this.Controls.Add(this.guna2CheckBox1);
            this.Controls.Add(this.guna2RadioButton2);
            this.Controls.Add(this.guna2RadioButton1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.guna2TextBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddUpdatePaymentVoucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddUpdatePaymentVoucher";
            this.picHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Label label9;
        private TextBox textBox1;
        private ComboBox comboBox2;
        private Label label3;
        private TextBox textBox3;
        private Guna.UI2.WinForms.Guna2Panel picHeader;
        private Label label4;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox2;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2RadioButton guna2RadioButton2;
        private Guna.UI2.WinForms.Guna2RadioButton guna2RadioButton1;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBox1;
    }
}