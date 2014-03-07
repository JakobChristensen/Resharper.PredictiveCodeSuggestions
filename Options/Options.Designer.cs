namespace PredictiveCodeSuggestions.Options
{
  partial class Options
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
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.UsingLabel = new System.Windows.Forms.Label();
      this.linkLabel3 = new System.Windows.Forms.LinkLabel();
      this.label1 = new System.Windows.Forms.Label();
      this.MaxTemplates = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.Occurances = new System.Windows.Forms.TextBox();
      this.linkLabel2 = new System.Windows.Forms.LinkLabel();
      this.label3 = new System.Windows.Forms.Label();
      this.MinPercentage = new System.Windows.Forms.TextBox();
      this.listView1 = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.UseCompleteStatement = new System.Windows.Forms.CheckBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.FileSaves = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // linkLabel1
      // 
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new System.Drawing.Point(28, 309);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(208, 13);
      this.linkLabel1.TabIndex = 15;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "Open database folder in Windows Explorer";
      this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      // 
      // UsingLabel
      // 
      this.UsingLabel.AutoSize = true;
      this.UsingLabel.Location = new System.Drawing.Point(28, 332);
      this.UsingLabel.Name = "UsingLabel";
      this.UsingLabel.Size = new System.Drawing.Size(35, 13);
      this.UsingLabel.TabIndex = 16;
      this.UsingLabel.Text = "label1";
      // 
      // linkLabel3
      // 
      this.linkLabel3.AutoSize = true;
      this.linkLabel3.Location = new System.Drawing.Point(28, 170);
      this.linkLabel3.Name = "linkLabel3";
      this.linkLabel3.Size = new System.Drawing.Size(171, 13);
      this.linkLabel3.TabIndex = 9;
      this.linkLabel3.TabStop = true;
      this.linkLabel3.Text = "Update suggestions from database";
      this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(28, 95);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(206, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Max number of suggestions per statement:";
      // 
      // MaxTemplates
      // 
      this.MaxTemplates.Location = new System.Drawing.Point(300, 92);
      this.MaxTemplates.Name = "MaxTemplates";
      this.MaxTemplates.Size = new System.Drawing.Size(47, 20);
      this.MaxTemplates.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(28, 120);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(264, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Number of occurances needed to define a suggestion:";
      // 
      // Occurances
      // 
      this.Occurances.Location = new System.Drawing.Point(300, 117);
      this.Occurances.Name = "Occurances";
      this.Occurances.Size = new System.Drawing.Size(47, 20);
      this.Occurances.TabIndex = 6;
      // 
      // linkLabel2
      // 
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.Location = new System.Drawing.Point(28, 32);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new System.Drawing.Size(294, 13);
      this.linkLabel2.TabIndex = 1;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "Add current solution to the database and update suggestions";
      this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(28, 146);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(256, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Minimum percentage required to define a suggestion:";
      // 
      // MinPercentage
      // 
      this.MinPercentage.Location = new System.Drawing.Point(300, 143);
      this.MinPercentage.Name = "MinPercentage";
      this.MinPercentage.Size = new System.Drawing.Size(47, 20);
      this.MinPercentage.TabIndex = 8;
      // 
      // listView1
      // 
      this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new System.Drawing.Point(31, 348);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(370, 231);
      this.listView1.TabIndex = 17;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Key";
      this.columnHeader1.Width = 50;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Suggestion";
      this.columnHeader2.Width = 75;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Occurances";
      this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.columnHeader3.Width = 92;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "%";
      this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(14, 74);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(52, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Filtering";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(12, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Database";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(14, 287);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(65, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Templates";
      // 
      // UseCompleteStatement
      // 
      this.UseCompleteStatement.AutoSize = true;
      this.UseCompleteStatement.Location = new System.Drawing.Point(31, 227);
      this.UseCompleteStatement.Name = "UseCompleteStatement";
      this.UseCompleteStatement.Size = new System.Drawing.Size(220, 17);
      this.UseCompleteStatement.TabIndex = 11;
      this.UseCompleteStatement.Text = "Integrate with Complete Statement action";
      this.UseCompleteStatement.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(14, 203);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(68, 13);
      this.label7.TabIndex = 10;
      this.label7.Text = "Integration";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(28, 247);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(191, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Number of file saves between updates:";
      // 
      // FileSaves
      // 
      this.FileSaves.Location = new System.Drawing.Point(300, 240);
      this.FileSaves.Name = "FileSaves";
      this.FileSaves.Size = new System.Drawing.Size(47, 20);
      this.FileSaves.TabIndex = 13;
      // 
      // Options
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.FileSaves);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.UseCompleteStatement);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.MinPercentage);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.linkLabel2);
      this.Controls.Add(this.Occurances);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.MaxTemplates);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.linkLabel3);
      this.Controls.Add(this.linkLabel1);
      this.Controls.Add(this.UsingLabel);
      this.Name = "Options";
      this.Size = new System.Drawing.Size(433, 603);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label UsingLabel;
    private System.Windows.Forms.LinkLabel linkLabel3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox MaxTemplates;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox Occurances;
    private System.Windows.Forms.LinkLabel linkLabel2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox MinPercentage;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox UseCompleteStatement;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox FileSaves;
  }
}
