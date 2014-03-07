// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentEventHandler.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Class DocumentEventHandler
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using System.IO;
  using JetBrains.DocumentManagers;
  using JetBrains.DocumentManagers.impl;
  using JetBrains.ProjectModel;

  /// <summary>
  /// Class DocumentEventHandler
  /// </summary>
  [SolutionComponent]
  public class DocumentEventHandler
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="DocumentEventHandler"/> class.</summary>
    /// <param name="ops">The ops.</param>
    public DocumentEventHandler(DocumentManagerOperations ops)
    {
      ops.AfterDocumentSaved += this.AfterDocumentSaved;
    }

    #endregion

    #region Methods

    /// <summary>Called after the document is saved.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DocumentSavedEventArgs"/> instance containing the event data.</param>
    private void AfterDocumentSaved(object sender, DocumentSavedEventArgs e)
    {
      var data = e.Document.EnumerateData();

      foreach (var pair in data)
      {
        var projectItem = pair.Value as IProjectItem;
        if (projectItem == null)
        {
          continue;
        }

        var project = projectItem.GetProject();
        if (project == null)
        {
          return;
        }

        var fileName = projectItem.Location.ToString();
        if (string.IsNullOrEmpty(fileName))
        {
          return;
        }

        var folder = Path.GetDirectoryName(project.Location.ToString()) ?? string.Empty;
        if (fileName.StartsWith(folder, StringComparison.InvariantCultureIgnoreCase))
        {
          fileName = fileName.Substring(folder.Length);
        }

        fileName = "<" + project.Name + ">" + fileName;

        ShellManager.Instance.RaiseDocumentChanged(fileName);

        return;
      }
    }

    #endregion
  }
}