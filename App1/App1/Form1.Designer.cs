namespace App1
{
    partial class Form1
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ParameterNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonGetDefault = new System.Windows.Forms.Button();
            this.buttonGeneralReset = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.initButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterNumber,
            this.SubIndex,
            this.ParameterValue});
            this.dataGridView1.Location = new System.Drawing.Point(83, 133);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(805, 462);
            this.dataGridView1.TabIndex = 9;
            // 
            // ParameterNumber
            // 
            this.ParameterNumber.HeaderText = "Parameter Number";
            this.ParameterNumber.Name = "ParameterNumber";
            this.ParameterNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // SubIndex
            // 
            this.SubIndex.HeaderText = "Sub Index";
            this.SubIndex.Name = "SubIndex";
            // 
            // ParameterValue
            // 
            this.ParameterValue.HeaderText = "Parameter Value";
            this.ParameterValue.Name = "ParameterValue";
            // 
            // buttonGetDefault
            // 
            this.buttonGetDefault.Location = new System.Drawing.Point(472, 76);
            this.buttonGetDefault.Name = "buttonGetDefault";
            this.buttonGetDefault.Size = new System.Drawing.Size(75, 36);
            this.buttonGetDefault.TabIndex = 8;
            this.buttonGetDefault.Text = "Get Default File";
            this.buttonGetDefault.UseVisualStyleBackColor = true;
            this.buttonGetDefault.Click += new System.EventHandler(this.buttonGetDefault_Click);
            // 
            // buttonGeneralReset
            // 
            this.buttonGeneralReset.Location = new System.Drawing.Point(355, 77);
            this.buttonGeneralReset.Name = "buttonGeneralReset";
            this.buttonGeneralReset.Size = new System.Drawing.Size(75, 34);
            this.buttonGeneralReset.TabIndex = 7;
            this.buttonGeneralReset.Text = "General Reset";
            this.buttonGeneralReset.UseVisualStyleBackColor = true;
            this.buttonGeneralReset.Click += new System.EventHandler(this.buttonGeneralReset_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(83, 83);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // initButton
            // 
            this.initButton.Location = new System.Drawing.Point(232, 83);
            this.initButton.Name = "initButton";
            this.initButton.Size = new System.Drawing.Size(75, 23);
            this.initButton.TabIndex = 5;
            this.initButton.Text = "Init";
            this.initButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 670);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonGetDefault);
            this.Controls.Add(this.buttonGeneralReset);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.initButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonGetDefault;
        private System.Windows.Forms.Button buttonGeneralReset;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button initButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterValue;
    }
}

