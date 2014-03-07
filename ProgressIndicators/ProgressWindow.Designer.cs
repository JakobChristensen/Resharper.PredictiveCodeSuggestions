namespace PredictiveCodeSuggestions.ProgressIndicators
{
  partial class ProgressWindow
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.ProjectName = new System.Windows.Forms.Label();
      this.FileName = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // ProjectName
      // 
      this.ProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProjectName.Location = new System.Drawing.Point(7, 9);
      this.ProjectName.Name = "ProjectName";
      this.ProjectName.Size = new System.Drawing.Size(446, 17);
      this.ProjectName.TabIndex = 0;
      this.ProjectName.Text = "Project";
      // 
      // FileName
      // 
      this.FileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FileName.Location = new System.Drawing.Point(7, 36);
      this.FileName.Name = "FileName";
      this.FileName.Size = new System.Drawing.Size(446, 17);
      this.FileName.TabIndex = 1;
      this.FileName.Text = "File";
      // 
      // ProgressWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(465, 61);
      this.Controls.Add(this.FileName);
      this.Controls.Add(this.ProjectName);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProgressWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Progress";
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.Label ProjectName;
    public System.Windows.Forms.Label FileName;

  }
}