// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="Options" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Options
{
  using System;
  using System.Diagnostics;
  using System.Linq;
  using System.Windows.Forms;
  using JetBrains.ActionManagement;
  using JetBrains.Application;
  using JetBrains.Application.Settings;
  using JetBrains.DataFlow;
  using JetBrains.UI.CrossFramework;
  using JetBrains.UI.Options;
  using JetBrains.UI.Resources;
  using PredictiveCodeSuggestions.AutoTemplates;
  using PredictiveCodeSuggestions.ProgressIndicators;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="Options"/> class.</summary>
  [OptionsPage(PageName, "Predictive Code Suggestions", typeof(OptionsThemedIcons.Plugins), ParentId = "CodeEditing", Sequence = 4.6)]
  public partial class Options : UserControl, IOptionsPage
  {
    #region Constants and Fields

    /// <summary>
    /// The page name.
    /// </summary>
    private const string PageName = "PredictiveCodeSuggestions.OptionsPage";

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="Options"/> class.</summary>
    /// <param name="lifetime">The lifetime.</param>
    /// <param name="settings">The settings.</param>
    public Options(Lifetime lifetime, OptionsSettingsSmartContext settings)
    {
      this.InitializeComponent();

      settings.SetBinding(lifetime, (AutoTemplateSettings s) => s.MaxTemplates, WinFormsProperty.Create(lifetime, this.MaxTemplates, box => box.Text, true));
      settings.SetBinding(lifetime, (AutoTemplateSettings s) => s.Occurances, WinFormsProperty.Create(lifetime, this.Occurances, box => box.Text, true));
      settings.SetBinding(lifetime, (AutoTemplateSettings s) => s.MinPercentage, WinFormsProperty.Create(lifetime, this.MinPercentage, box => box.Text, true));
      settings.SetBinding(lifetime, (AutoTemplateSettings s) => s.UseCompleteStatement, WinFormsProperty.Create(lifetime, this.UseCompleteStatement, box => box.Checked, true));
      settings.SetBinding(lifetime, (AutoTemplateSettings s) => s.FileSaves, WinFormsProperty.Create(lifetime, this.FileSaves, box => box.Text, true));

      this.listView1.Columns[0].Width = this.listView1.Width / 2 - 40;
      this.listView1.Columns[1].Width = this.listView1.Width / 2 - 40;
      this.listView1.Columns[2].Width = 40;
      this.listView1.Columns[3].Width = 40;

      this.RefreshInfo();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// <para>
    /// Control to be shown as page.
    /// </para>
    /// <para>
    /// May be <c>Null</c> if the page does not have any UI.
    /// </para>
    /// <para>
    /// Will NOT be disposed of automatically. Import a lifetime in page ctor and attach the control to that lifetime. If you're returning self (<see cref="T:JetBrains.UI.Options.IOptionsPage"/>-based class) as the control, then it's implementing <see cref="T:System.IDisposable"/> already and its <see cref="M:System.IDisposable.Dispose"/> will be called automatically.
    /// </para>
    /// </summary>
    public EitherControl Control
    {
      get
      {
        return this;
      }
    }

    /// <summary>
    /// Gets the ID of this option page.
    ///             <see cref="T:JetBrains.UI.Options.IOptionsDialog"/> or <see cref="T:JetBrains.UI.Options.OptionsPageDescriptor"/> could be used to retrieve the <see cref="T:JetBrains.UI.Options.OptionsManager"/> out of it.
    /// </summary>
    public string Id
    {
      get
      {
        return PageName;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>Invoked when OK button in the options dialog is pressed.
    ///             If the page returns <c>false</c>, the the options dialog won't be closed, and focus will be put into this page.</summary>
    /// <returns>The <see cref="bool"/>.</returns>
    public bool OnOk()
    {
      /*
      if (this.isChanged)
      {
        switch (MessageBox.Show("The filtering values have changed.\n\nDo you want to updated the Auto Templates?", "ReSharper", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Cancel:
            return false;
          case DialogResult.Yes:
            this.UpdateTemplates();
            break;
        }
      }
      */

      return true;
    }

    /// <summary>
    /// Check if the settings on the page are consistent, and page could be closed.
    /// </summary>
    /// <returns>
    /// <c>true</c> if page data is consistent.
    /// </returns>
    public bool ValidatePage()
    {
      return true;
    }

    #endregion

    #region Methods

    /// <summary>Refreshes the info.</summary>
    private void RefreshInfo()
    {
      this.UsingLabel.Text = string.Format("Currently using {0} suggestions.", AutoTemplateManager.AutoTemplates.Count());

      this.listView1.Items.Clear();

      if (!AutoTemplateManager.AutoTemplates.Any())
      {
        return;
      }

      foreach (var smartTemplate in AutoTemplateManager.AutoTemplates.OrderBy(t => t.Key).ThenByDescending(t => t.Count))
      {
        var listViewItem = new ListViewItem(new[]
        {
          smartTemplate.Key, smartTemplate.Template, smartTemplate.Count.ToString(), smartTemplate.Percentage + "%"
        });

        this.listView1.Items.Add(listViewItem);
      }
    }

    /// <summary>Updates the templates.</summary>
    private void UpdateTemplates()
    {
      var window = new ProgressWindow();
      try
      {
        int maxTemplates;
        if (!int.TryParse(this.MaxTemplates.Text, out maxTemplates))
        {
          MessageBox.Show("Max Suggestions must be a number.");
          return;
        }

        int occurances;
        if (!int.TryParse(this.Occurances.Text, out occurances))
        {
          MessageBox.Show("Occurances must be a number.");
          return;
        }

        int minPercentage;
        if (!int.TryParse(this.MinPercentage.Text, out minPercentage))
        {
          MessageBox.Show("Minimum Percentage must be a number.");
          return;
        }

        window.Show();

        var builder = new AutoTemplateBuilder();
        builder.Process(window.ProgressIndicator, maxTemplates, occurances, minPercentage);

        AutoTemplateManager.Invalidate();

        this.RefreshInfo();
      }
      finally
      {
        if (window.Visible)
        {
          window.Close();
        }
      }
    }

    /// <summary>Handles the LinkClicked event of the linkLabel1 control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      var dataFolder = AutoTemplateManager.DataFolder;

      Process.Start(@"explorer.exe", dataFolder);
    }

    /// <summary>Handles the LinkClicked event of the linkLabel2 control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        ShellManager.ExecuteReentrancyGuard("PredictiveCodeSuggestions.AnalyzeSolution", () =>
        {
          var actionManager = Shell.Instance.GetComponent<ActionManager>();
          actionManager.ExecuteAction("PredictiveCodeSuggestions.AnalyzeSolution");
          this.RefreshInfo();
        });
      }
      catch (Exception ex)
      {
        MessageBox.Show("A solution must be open.\n\n" + ex.Message);
      }
    }

    /// <summary>Handles the LinkClicked event of the linkLabel3 control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.UpdateTemplates();
    }

    #endregion
  }
}