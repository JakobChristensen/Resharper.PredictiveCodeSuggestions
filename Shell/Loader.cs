// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Loader.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="Loader" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using System.Collections.Generic;
  using System.Timers;
  using System.Windows.Forms;
  using JetBrains.ActionManagement;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using PredictiveCodeSuggestions.AutoTemplates;
  using PredictiveCodeSuggestions.AutoTemplates.Analyzers;
  using PredictiveCodeSuggestions.AutoTemplates.Templates;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.Options;
  using PredictiveCodeSuggestions.SmartGenerators;
  using PredictiveCodeSuggestions.SmartGenerators.Scopes;

  /// <summary>Defines the <see cref="Loader"/> class.</summary>
  [ShellComponent]
  public class Loader
  {
    #region Constants and Fields

    /// <summary>
    /// The changed file names.
    /// </summary>
    private static readonly List<string> fileNames = new List<string>();

    /// <summary>
    /// The number of saves.
    /// </summary>
    private static int saves;

    /// <summary>
    /// The sync root
    /// </summary>
    private static readonly object SyncRoot = new object();

    /// <summary>
    /// The timer
    /// </summary>
    [CanBeNull]
    private static System.Timers.Timer timer;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="Loader"/> class.</summary>
    public Loader()
    {
      ShellManager.Instance.Init();

      GeneratorManager.Register(new SmartGeneratorPopulator());
      GeneratorManager.Register(new IteratorPopulator());
      GeneratorManager.Register(new AssignmentPopulator());
      GeneratorManager.Register(new FlowControlPopulator());
      GeneratorManager.Register(new ReturnPopulator());

      ScopeManager.Register(new EndOfBlockScope());
      ScopeManager.Register(new ClassMemberScope());
      ScopeManager.Register(new TypeScope());
      ScopeManager.Register(new SwitchCaseScope());
      ScopeManager.Register(new AfterInvocationScope());
      ScopeManager.Register(new BeforeInvocationScope());
      ScopeManager.Register(new AfterAssignmentExpressionScope());
      ScopeManager.Register(new BeforeAssignmentExpressionScope());
      ScopeManager.Register(new AfterLocalVariableDeclarationScope());
      ScopeManager.Register(new BeforeLocalVariableDeclarationScope());
      ScopeManager.Register(new InterfaceMemberScope());
      ScopeManager.Register(new StructMemberScope());
      ScopeManager.Register(new EmptyFileScope());
      ScopeManager.Register(new AfterEnumerableScope());
      ScopeManager.Register(new InsideMethodScope());
      ScopeManager.Register(new BeginningOfFunctionScope());

      AutoTemplateManager.Register(new InvocationAnalyzer());
      AutoTemplateManager.Register(new LocalVariableAnalyzer());
      AutoTemplateManager.Register(new AssignmentAnalyzer());
      AutoTemplateManager.Register(new ForeachAnalyzer());
      AutoTemplateManager.Register(new UsingAnalyzer());
      AutoTemplateManager.Register(new IfNullAnalyzer());
      AutoTemplateManager.Register(new IfInvocationAnalyzer());
      AutoTemplateManager.Register(new IfUnaryOperatorAnalyzer());

      AutoTemplateManager.Register(new InvocationTemplate());
      AutoTemplateManager.Register(new LocalVariableTemplate());
      AutoTemplateManager.Register(new IfTemplate());
      AutoTemplateManager.Register(new WhileTemplate());
      AutoTemplateManager.Register(new DoWhileTemplate());
      AutoTemplateManager.Register(new ForEachTemplate());
      AutoTemplateManager.Register(new TryFinallyTemplate());
      AutoTemplateManager.Register(new TryCatchTemplate());
      AutoTemplateManager.Register(new ReturnTemplate());
      AutoTemplateManager.Register(new BreakTemplate());
      AutoTemplateManager.Register(new ContinueTemplate());

      ShellManager.Instance.DocumentSaved += this.DocumentSaved;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the file names.
    /// </summary>
    /// <value>The file names.</value>
    [NotNull]
    public static IEnumerable<string> FileNames
    {
      get
      {
        return fileNames;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [update only].
    /// </summary>
    /// <value><c>true</c> if [update only]; otherwise, <c>false</c>.</value>
    public static bool UpdateOnly { get; set; }

    #endregion

    #region Methods

    /// <summary>Documents the saved.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void DocumentSaved(string fileName)
    {
      if (fileNames.Contains(fileName))
      {
        fileNames.Remove(fileName);
      }

      fileNames.Add(fileName);

      var service = Shell.Instance.GetComponent<AutoTemplatesService>();
      var settings = service.GetSettings();

      var fileSaves = settings.GetFileSaves();

      lock (SyncRoot)
      {
        saves++;
        if (saves < fileSaves)
        {
          return;
        }

        if (timer != null)
        {
          timer.Stop();
          timer.Dispose();
          timer = null;
        }

        timer = new System.Timers.Timer(1000);
        timer.Elapsed += this.DoUpdateFiles;

        timer.Enabled = true;
      }
    }

    /// <summary>
    /// Does the update files.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
    private void DoUpdateFiles(object sender, ElapsedEventArgs e)
    {
      if (timer != null)
      {
        lock (SyncRoot)
        {
          timer.Stop();
          timer.Dispose();
          timer = null;
        }
      }

      this.UpdateFiles();
    }

    /// <summary>
    /// Updates the files.
    /// </summary>
    private void UpdateFiles()
    {
      try
      {
        Action action = delegate
        {
          UpdateOnly = true;
          try
          {
            var actionManager = Shell.Instance.GetComponent<ActionManager>();
            actionManager.ExecuteAction("PredictiveCodeSuggestions.AnalyzeSolution");
          }
          finally
          {
            fileNames.Clear();
            saves = 0;
            UpdateOnly = false;
          }
        };

        ShellManager.ExecuteReentrancyGuard("PredictiveCodeSuggestions.AnalyzeSolution", action);
      }
      catch (Exception ex)
      {
        MessageBox.Show("A solution must be open.\n\n" + ex.Message);
      }
    }

    #endregion
  }
}