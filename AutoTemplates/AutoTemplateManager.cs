// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateManager" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Xml.Linq;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.AutoTemplates.Analyzers;
  using PredictiveCodeSuggestions.AutoTemplates.Templates;
  using PredictiveCodeSuggestions.Extensions.XElementExtensions;

  /// <summary>Defines the <see cref="AutoTemplateManager"/> class.</summary>
  public static class AutoTemplateManager
  {
    #region Static Fields

    /// <summary>
    /// The file lock
    /// </summary>
    public static readonly object FileLock = new object();

    /// <summary>The templates field.</summary>
    [NotNull]
    private static readonly List<AutoTemplate> autoTemplates = new List<AutoTemplate>();

    /// <summary>The handlers field.</summary>
    [NotNull]
    private static readonly List<StatementAnalyzer> statementAnalyzers = new List<StatementAnalyzer>();

    /// <summary>The templates field.</summary>
    [NotNull]
    private static readonly List<StatementTemplate> statementTemplates = new List<StatementTemplate>();

    /// <summary>
    /// The sync root
    /// </summary>
    private static readonly object syncRoot = new object();

    /// <summary>The is loaded field.</summary>
    private static bool isLoaded;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the templates.
    /// </summary>
    [NotNull]
    public static IEnumerable<AutoTemplate> AutoTemplates
    {
      get
      {
        if (!isLoaded)
        {
          lock (syncRoot)
          {
            if (!isLoaded)
            {
              Load();
            }
          }
        }

        return autoTemplates;
      }
    }

    /// <summary>
    /// Gets the data folder.
    /// </summary>
    [NotNull]
    public static string DataFolder
    {
      get
      {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        folder = Path.Combine(folder, @"ReSharper.PredictiveCodeSuggestions");

        try
        {
          Directory.CreateDirectory(folder);
        }
        catch
        {
          return folder;
        }

        return folder;
      }
    }

    /// <summary>
    /// Gets the statement analyzers.
    /// </summary>
    [NotNull]
    public static IEnumerable<StatementAnalyzer> StatementAnalyzers
    {
      get
      {
        return statementAnalyzers;
      }
    }

    /// <summary>
    /// Gets the statement templates.
    /// </summary>
    [NotNull]
    public static IEnumerable<StatementTemplate> StatementTemplates
    {
      get
      {
        return statementTemplates;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Gets the name of the file.</summary>
    /// <returns>Returns the file name.</returns>
    [NotNull]
    public static string GetAutoTemplatesFileName()
    {
      return Path.Combine(DataFolder, "_Suggestions.xml");
    }

    /// <summary>
    /// Gets the name of the temp file.
    /// </summary>
    /// <returns>Returns the string.</returns>
    [NotNull]
    public static string GetTempFileName()
    {
      return Path.Combine(DataFolder, "_Suggestions.tmp");
    }

    /// <summary>Invalidates this instance.</summary>
    public static void Invalidate()
    {
      lock (syncRoot)
      {
        isLoaded = false;
        autoTemplates.Clear();
      }
    }

    /// <summary>Registers the specified analyzer.</summary>
    /// <param name="analyzer">The analyzer.</param>
    public static void Register([NotNull] StatementAnalyzer analyzer)
    {
      if (analyzer == null)
      {
        throw new ArgumentNullException("analyzer");
      }

      statementAnalyzers.Add(analyzer);
    }

    /// <summary>Registers the specified template.</summary>
    /// <param name="template">The template.</param>
    public static void Register([NotNull] StatementTemplate template)
    {
      if (template == null)
      {
        throw new ArgumentNullException("template");
      }

      statementTemplates.Add(template);
    }

    #endregion

    #region Methods

    /// <summary>Loads this instance.</summary>
    private static void Load()
    {
      isLoaded = true;

      autoTemplates.Clear();

      XDocument doc;
      try
      {
        lock (FileLock)
        {
          doc = XDocument.Load(GetAutoTemplatesFileName());
        }
      }
      catch
      {
        return;
      }

      var root = doc.Root;
      if (root == null)
      {
        return;
      }

      foreach (var element in root.Elements())
      {
        var template = new AutoTemplate
        {
          Key = element.GetAttributeValue("k"),
          Template = element.Value,
          Count = element.GetAttributeInt("c", 0),
          Percentage = element.GetAttributeInt("p", 0)
        };

        var variables = element.GetAttributeValue("v").Split('|');
        for (var index = 0; index < variables.Length - 1; index += 2)
        {
          template.TemplateVariables[variables[index]] = variables[index + 1];
        }

        autoTemplates.Add(template);
      }
    }

    #endregion
  }
}