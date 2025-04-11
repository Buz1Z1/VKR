namespace restaur.forms.POS
{
    partial class product
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.image = new System.Windows.Forms.PictureBox();
            this.Pname = new System.Windows.Forms.Label();
            this.Pprice = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2ShadowPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.image);
            this.guna2ShadowPanel1.Controls.Add(this.guna2Panel1);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(227, 222);
            this.guna2ShadowPanel1.TabIndex = 0;
            // 
            // image
            // 
            this.image.Location = new System.Drawing.Point(30, 12);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(167, 129);
            this.image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.image.TabIndex = 0;
            this.image.TabStop = false;
            this.image.Click += new System.EventHandler(this.image_Click);
            // 
            // Pname
            // 
            this.Pname.AutoSize = true;
            this.Pname.Location = new System.Drawing.Point(80, 10);
            this.Pname.Name = "Pname";
            this.Pname.Size = new System.Drawing.Size(47, 20);
            this.Pname.TabIndex = 2;
            this.Pname.Text = "name";
            // 
            // Pprice
            // 
            this.Pprice.AutoSize = true;
            this.Pprice.Location = new System.Drawing.Point(80, 42);
            this.Pprice.Name = "Pprice";
            this.Pprice.Size = new System.Drawing.Size(43, 20);
            this.Pprice.TabIndex = 3;
            this.Pprice.Text = "price";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderRadius = 2;
            this.guna2Panel1.BorderThickness = 2;
            this.guna2Panel1.Controls.Add(this.Pprice);
            this.guna2Panel1.Controls.Add(this.Pname);
            this.guna2Panel1.CustomBorderThickness = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 147);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(227, 75);
            this.guna2Panel1.TabIndex = 4;
            // 
            // product
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "product";
            this.Size = new System.Drawing.Size(227, 222);
            this.guna2ShadowPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private System.Windows.Forms.PictureBox image;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        public System.Windows.Forms.Label Pname;
        public System.Windows.Forms.Label Pprice;
    }
}
