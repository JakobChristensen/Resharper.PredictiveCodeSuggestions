// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyzeSolutionAction.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AnalyzeSolutionAction" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Actions
{
  using System;
  using System.IO;
  using System.Text;
  using System.Threading;
  using JetBrains.ActionManagement;
  using JetBrains.Application;
  using JetBrains.Application.DataContext;
  using JetBrains.Util;
  using PredictiveCodeSuggestions.AutoTemplates;
  using PredictiveCodeSuggestions.Options;
  using PredictiveCodeSuggestions.ProgressIndicators;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="AnalyzeSolutionAction"/> class.</summary>
  [ActionHandler("PredictiveCodeSuggestions.AnalyzeSolution")]
  public class AnalyzeSolutionAction : IActionHandler
  {
    #region Public Methods and Operators

    /// <summary>Executes action. Called after Update, that set ActionPresentation.Enabled to true.</summary>
    /// <param name="context">The data context</param>
    /// <param name="nextExecute">delegate to call</param>
    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      try
      {
        var solution = context.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
        if (solution == null)
        {
          return;
        }

        if (Loader.UpdateOnly)
        {
          var fileName = Path.Combine(AutoTemplateManager.DataFolder, solution.Name + ".config");
          if (!File.Exists(fileName))
          {
            this.FirstTime(fileName);
          }

          if (File.Exists(fileName))
          {
            var config = File.ReadAllText(fileName);
            if (config == "disabled")
            {
              return;
            }
          }
        }

        var window = new ProgressWindow();
        try
        {
          if (!Loader.UpdateOnly)
          {
            window.Show();
          }

          var analyzer = new AutoTemplateAnalyzer(solution);
          analyzer.Process(window.ProgressIndicator, Loader.UpdateOnly ? Loader.FileNames : null);

          if (Loader.UpdateOnly)
          {
            var thread = new Thread(this.BuildTemplates);
            thread.Start();
          }
          else
          {
            this.BuildTemplates(window.ProgressIndicator);
          }
        }
        finally
        {
          if (window.Visible)
          {
            window.Close();
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.ShowError(ex.Message, "Predictive Code Suggestions");
      }

      if (nextExecute != null)
      {
        nextExecute();
      }
    }

    /// <summary>
    /// Firsts the time.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private void FirstTime(string fileName)
    {
      string contents;

      if (!MessageBox.ShowYesNo("Predictive Code Suggestions can analyze changed files in the background.\n\nDo you want to enable background analysis?\n\n(Not recommended for huge solutions.)", "Predictive Code Suggestions"))
      {
        contents = "disabled";
      }
      else
      {
        contents = "enabled";
      }

      File.WriteAllText(fileName, contents, Encoding.UTF8);
    }

    /// <summary>
    /// Updates action visual presentation. If presentation.Enabled is set to false, Execute
    /// will not be called.
    /// </summary>
    /// <param name="context">The DataContext</param>
    /// <param name="presentation">presentation to update</param>
    /// <param name="nextUpdate">delegate to call</param>
    /// <returns>The update result.</returns>
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      var solution = context.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
      return solution != null;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Builds the templates.
    /// </summary>
    private void BuildTemplates()
    {
      this.BuildTemplates(null);
    }

    /// <summary>
    /// Builds the templates.
    /// </summary>
    /// <param name="progressIndicator">The progress indicator.</param>
    private void BuildTemplates(ProgressIndicator progressIndicator)
    {
      var service = Shell.Instance.GetComponent<AutoTemplatesService>();
      var settings = service.GetSettings();

      var builder = new AutoTemplateBuilder();
      builder.Process(progressIndicator, settings.GetMaxTemplates(), settings.GetOccurances(), settings.GetMinPercentage());

      AutoTemplateManager.Invalidate();
    }

    #endregion
  }
}