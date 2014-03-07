// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The auto template analyzer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Xml;
  using System.Xml.Linq;
  using JetBrains.Annotations;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.Util.Logging;
  using PredictiveCodeSuggestions.Extensions.XElementExtensions;
  using PredictiveCodeSuggestions.ProgressIndicators;

  /// <summary>
  /// The auto template analyzer.
  /// </summary>
  public class AutoTemplateAnalyzer
  {
    #region Constants and Fields

    /// <summary>
    /// Gets or sets the output.
    /// </summary>
    /// <value>The output.</value>
    private XmlTextWriter output;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplateAnalyzer"/> class.</summary>
    /// <param name="solution">The solution.</param>
    /// <exception cref="System.ArgumentNullException">Solution is null.</exception>
    public AutoTemplateAnalyzer([NotNull] ISolution solution)
    {
      if (solution == null)
      {
        throw new ArgumentNullException("solution");
      }

      this.Solution = solution;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    [NotNull]
    public string CurrentFileName { get; set; }

    /// <summary>
    /// Gets or sets the window.
    /// </summary>
    /// <value>The window.</value>
    [NotNull]
    public ProgressIndicator ProgressIndicator { get; set; }

    /// <summary>
    /// Gets the solution.
    /// </summary>
    [NotNull]
    public ISolution Solution { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Makes the candidate.</summary>
    /// <param name="statementDescriptor">The template.</param>
    /// <exception cref="ArgumentNullException">statementDescriptor</exception>
    public void Add([NotNull] StatementDescriptor statementDescriptor)
    {
      if (statementDescriptor == null)
      {
        throw new ArgumentNullException("statementDescriptor");
      }

      statementDescriptor.FileName = this.CurrentFileName;
      statementDescriptor.Write(this.output);
    }

    /// <summary>Runs this instance.</summary>
    /// <param name="progressIndicator">The progress Indicator.</param>
    /// <param name="fileNames">The file Names.</param>
    public void Process(ProgressIndicator progressIndicator, IEnumerable<string> fileNames)
    {
      this.ProgressIndicator = progressIndicator;

      var fileName = Path.Combine(AutoTemplateManager.DataFolder, this.Solution.Name + ".xml");

      if (fileNames == null)
      {
        this.Process(fileName);
      }
      else
      {
        this.Process(fileName, fileNames);
      }

      this.ProgressIndicator = null;
    }

    #endregion

    #region Methods

    /// <summary>Processes the specified file name.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void Process(string fileName)
    {
      using (this.output = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        this.output.Formatting = Formatting.Indented;
        this.output.WriteStartElement("i");

        foreach (var project in this.Solution.GetAllProjects())
        {
          this.ProgressIndicator.Task = project.Name;

          this.Process(project);
        }

        this.output.WriteEndElement();
        this.output.Flush();
      }
    }

    /// <summary>Processes the specified file names.</summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="fileNames">The file names.</param>
    private void Process(string fileName, IEnumerable<string> fileNames)
    {
      var targetFileName = Path.ChangeExtension(fileName, ".tmp");

      XDocument doc = null;
      if (File.Exists(fileName))
      {
        doc = XDocument.Load(fileName);
      }

      using (this.output = new XmlTextWriter(targetFileName, Encoding.UTF8))
      {
        this.output.Formatting = Formatting.Indented;
        this.output.WriteStartElement("i");

        if (doc != null)
        {
          foreach (var element in doc.Root.Elements())
          {
            var f = element.GetAttributeValue("f");
            if (fileNames.Contains(f))
            {
              continue;
            }

            var k = element.GetAttributeValue("k");
            var v = element.GetAttributeValue("v");
            var value = element.Value;

            this.output.WriteStartElement("i");
            this.output.WriteAttributeString("k", k);
            this.output.WriteAttributeString("f", f);
            this.output.WriteAttributeString("v", v);
            this.output.WriteValue(value);
            this.output.WriteEndElement();
          }
        }

        foreach (var fname in fileNames)
        {
          var n = fname.IndexOf(">", StringComparison.Ordinal);
          if (n < 0)
          {
            continue;
          }

          var projectName = fname.Substring(1, n - 1);
          var projectFileName = fname.Substring(n + 1);

          var project = this.Solution.GetProjectByName(projectName);
          if (project == null)
          {
            continue;
          }

          Predicate<IProjectFile> predicate = delegate(IProjectFile file)
          {
            return file.Location.ToString().EndsWith(projectFileName);
          };

          var projectFile = project.GetAllProjectFiles(predicate).FirstOrDefault();

          if (projectFile == null)
          {
            continue;
          }

          this.Process(project, projectFile);
        }

        this.output.WriteEndElement();
        this.output.Flush();
      }

      File.Copy(targetFileName, fileName, true);
      File.Delete(targetFileName);
    }

    /// <summary>Processes the specified project.</summary>
    /// <param name="project">The project.</param>
    /// <exception cref="ArgumentNullException">project</exception>
    private void Process([NotNull] IProject project)
    {
      if (project == null)
      {
        throw new ArgumentNullException("project");
      }

      foreach (var file in project.GetAllProjectFiles())
      {
        try
        {
          this.Process(project, file);
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }
      }
    }

    /// <summary>Processes the specified file.</summary>
    /// <param name="project">The project.</param>
    /// <param name="file">The file.</param>
    /// <exception cref="ArgumentNullException">project</exception>
    private void Process([NotNull] IProject project, [NotNull] IProjectFile file)
    {
      if (project == null)
      {
        throw new ArgumentNullException("project");
      }

      if (file == null)
      {
        throw new ArgumentNullException("file");
      }

      var psiFile = file.GetPrimaryPsiFile();
      if (psiFile == null)
      {
        return;
      }

      var fileName = file.Location.ToString();
      if (project.Location != null)
      {
        var root = project.Location.ToString();
        if (fileName.StartsWith(root, StringComparison.InvariantCultureIgnoreCase))
        {
          fileName = "<" + project.Name + ">" + fileName.Substring(root.Length);
        }
      }

      this.ProgressIndicator.Text = fileName;
      this.CurrentFileName = fileName;

      var processor = new RecursiveElementProcessor(this.ProcessTreeNode);
      processor.Process(psiFile.Children());
    }

    /// <summary>Processes the tree node.</summary>
    /// <param name="treeNode">The tree node.</param>
    /// <exception cref="System.ArgumentNullException">Tree node is null.</exception>
    private void ProcessTreeNode([NotNull] ITreeNode treeNode)
    {
      if (treeNode == null)
      {
        throw new ArgumentNullException("treeNode");
      }

      var statement = treeNode as IStatement;
      if (statement == null)
      {
        return;
      }

      foreach (var handler in AutoTemplateManager.StatementAnalyzers)
      {
        if (!handler.CanHandle(statement))
        {
          continue;
        }

        var scope = handler.Handle(statement);
        if (scope == null)
        {
          continue;
        }

        foreach (var statementDescriptor in handler.GetStatementDescriptors(scope))
        {
          this.Add(statementDescriptor);
        }
      }
    }

    #endregion
  }
}