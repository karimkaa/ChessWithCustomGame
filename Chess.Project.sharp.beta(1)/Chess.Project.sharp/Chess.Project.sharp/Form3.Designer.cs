namespace Chess.Project.sharp
{
    partial class Form3
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip6 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip7 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip8 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox7);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 141);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ивент";
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(6, 111);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(132, 17);
            this.checkBox7.TabIndex = 6;
            this.checkBox7.Text = "Заморозка (Freezing)";
            this.toolTip5.SetToolTip(this.checkBox7, "Во время срабатывания этого ивента, некоторые фигуры на поле замораживаются.\r\nКог" +
        "да фигуры замораживаются, они не могут двигаться.\r\nДействует \"Заморозка\" несколь" +
        "ко ходов.\r\n");
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(6, 88);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(118, 17);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "Торнадо (Tornado)";
            this.toolTip4.SetToolTip(this.checkBox5, "При запуске этого ивента, фигуры перемешиваются по полю.\r\nТорнадо исчезает сразу " +
        "после того, как фигуры были перемешаны.\r\n");
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(6, 65);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(182, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Извержение вулкана (Eruption)";
            this.toolTip3.SetToolTip(this.checkBox4, "Когда срабатывает этот ивент, на поле появляется \"Лава\",\r\nкоторая не дает пройти " +
        "другим фигурам (искл. фигуры, которые двигаются через клетки).\r\n\"Лава\" исчезает " +
        "спустя несколько ходов.\r\n");
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged_1);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(6, 42);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(170, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Землетрясение (Earthquake)";
            this.toolTip2.SetToolTip(this.checkBox3, "Когда срабатывает этот ивент, на поле появляется \"Скала\",\r\nкоторая не дает пройти" +
        " другим фигурам (искл. фигуры, которые двигаются через клетки).\r\nИсчезает \"Скала" +
        "\" спустя несколько ходов.");
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(153, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Красная зона (Red Zone)";
            this.toolTip1.SetToolTip(this.checkBox1, "Несколько клеток будут подсвечиваться красным цветом.\r\nЕсли игроки не уберут фигу" +
        "ры в течении нескольких ходов, то фигуры,\r\nкоторые стояли в на красной клетке ср" +
        "убаются.");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox9);
            this.groupBox3.Controls.Add(this.checkBox8);
            this.groupBox3.Cursor = System.Windows.Forms.Cursors.No;
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(12, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(138, 79);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Добавочные фигуры";
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(7, 44);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(76, 17);
            this.checkBox9.TabIndex = 1;
            this.checkBox9.Text = "Шпион (⫛)";
            this.toolTip7.SetToolTip(this.checkBox9, resources.GetString("checkBox9.ToolTip"));
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(7, 20);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(78, 17);
            this.checkBox8.TabIndex = 0;
            this.checkBox8.Text = "Страж (⛨)";
            this.toolTip6.SetToolTip(this.checkBox8, "Находится перед \"Слонами\".\r\nОни могут защищать другие фигуры методом \"Рокировки\"." +
        "\r\nОсобенность : подставляют себя, а защитившую фигуру ставят позади себя (см. по" +
        " траектории).");
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RosyBrown;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(0, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(542, 82);
            this.button1.TabIndex = 3;
            this.button1.Text = "Начало игры";
            this.toolTip8.SetToolTip(this.button1, "Начало CustomGame\r\nНЕ СТАБИЛЬНО, ЛУЧШЕ НЕ НАЖИМАТЬ. \r\nДВА РАЗА ПРЕДУПРЕЖДАТЬ НИКТ" +
        "О НЕ БУДЕТ.");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Красная зона";
            // 
            // toolTip2
            // 
            this.toolTip2.ToolTipTitle = "Землетрясение";
            // 
            // toolTip3
            // 
            this.toolTip3.ToolTipTitle = "Извержение вулкана";
            // 
            // toolTip4
            // 
            this.toolTip4.ToolTipTitle = "Торнадо";
            // 
            // toolTip5
            // 
            this.toolTip5.ToolTipTitle = "Заморозка";
            // 
            // toolTip6
            // 
            this.toolTip6.ToolTipTitle = "Добавочные фигуры : Страж";
            // 
            // toolTip7
            // 
            this.toolTip7.ToolTipTitle = "Добавочные фигуры : Шпион";
            // 
            // toolTip8
            // 
            this.toolTip8.ToolTipTitle = "Начало игры : удачи!";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImage = global::Chess.Project.sharp.Properties.Resources.icebox_valo_preview_1_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(542, 541);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "Шахматы: Custom Game Настройка";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip5;
        private System.Windows.Forms.ToolTip toolTip4;
        private System.Windows.Forms.ToolTip toolTip6;
        private System.Windows.Forms.ToolTip toolTip7;
        private System.Windows.Forms.ToolTip toolTip8;
    }
}