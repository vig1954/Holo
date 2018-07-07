namespace rab1.Forms
{
    partial class Model
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Model));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(207, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "Записать\r\nв zComplex[0] модель до деформации\r\nв zComplex[1] модель после деформац" +
    "ии";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(434, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "сдвиг в длинах волн\r\nпосле деформации";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "длина волны в мкм";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(160, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "0,5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Шероховатость в мкм";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(160, 60);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "0,02";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(253, 179);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(473, 96);
            this.button3.TabIndex = 8;
            this.button3.Text = resources.GetString("button3.Text");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 26);
            this.label4.TabIndex = 10;
            this.label4.Text = "сдвиг в длинах волн\r\nдо деформации";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(160, 29);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "0";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(542, 132);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(56, 20);
            this.textBox5.TabIndex = 12;
            this.textBox5.Text = "0";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(604, 132);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(51, 20);
            this.textBox6.TabIndex = 13;
            this.textBox6.Text = "90";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(661, 132);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(46, 20);
            this.textBox7.TabIndex = 14;
            this.textBox7.Text = "180";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(713, 132);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(59, 20);
            this.textBox8.TabIndex = 15;
            this.textBox8.Text = "270";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(749, 179);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(390, 89);
            this.button6.TabIndex = 17;
            this.button6.Text = "Формирование 4 интерферограмм (PSI)-> \r\n1 состояние 8,9,10,11\r\n2 состояние 5,6,7," +
    "8\r\nОбласть Френеля -> результат  в zComplex[1]\r\n";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(296, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Размер апертуры в мм";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(434, 64);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 20);
            this.textBox9.TabIndex = 19;
            this.textBox9.Text = "7";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(296, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Расстояние в мм";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(434, 93);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(100, 20);
            this.textBox10.TabIndex = 21;
            this.textBox10.Text = "135";
            // 
            // button7
            // 
            this.button7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button7.Location = new System.Drawing.Point(253, 281);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(473, 139);
            this.button7.TabIndex = 22;
            this.button7.Text = resources.GetString("button7.Text");
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(383, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(151, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Фазовые сдвиги в градусах";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(30, 260);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(207, 77);
            this.button5.TabIndex = 24;
            this.button5.Text = "Записать в zComplex[regComplex]\r\nпреобразование Френеля от модели\r\nдо деформации " +
    "      => zComplex[1]\r\nпосле деформации => zComplex[2]";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Угол по X";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(106, 136);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(61, 20);
            this.textBox11.TabIndex = 26;
            this.textBox11.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(203, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Угол по Y";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(282, 136);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(61, 20);
            this.textBox12.TabIndex = 28;
            this.textBox12.Text = "0,7";
            // 
            // Model
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 464);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Model";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "Моделирование объекта";
            this.Load += new System.EventHandler(this.Model_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox12;
    }
}