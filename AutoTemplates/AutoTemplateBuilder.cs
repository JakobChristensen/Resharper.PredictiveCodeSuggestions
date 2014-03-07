// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateBuilder.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateBuilder" /> class.
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
  using PredictiveCodeSuggestions.Extensions.XElementExtensions;
  using PredictiveCodeSuggestions.ProgressIndicators;

  /// <summary>Defines the <see cref="AutoTemplateBuilder"/> class.</summary>
  public class AutoTemplateBuilder
  {
    #region Constants and Fields

    /// <summary>The templates field.</summary>
    private readonly List<Record> templates = new List<Record>();

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the max templates.
    /// </summary>
    /// <value>The max templates.</value>
    protected int MaxSuggestions { get; set; }

    /// <summary>
    /// Gets or sets the min percentage.
    /// </summary>
    /// <value>The min percentage.</value>
    protected int MinPercentage { get; set; }

    /// <summary>
    /// Gets or sets the occurances.
    /// </summary>
    /// <value>The occurances.</value>
    protected int Occurances { get; set; }

    /// <summary>
    /// Gets or sets the progress window.
    /// </summary>
    /// <value>The progress window.</value>
    [CanBeNull]
    protected ProgressIndicator ProgressIndicator { get; set; }

    #endregion

    #region Public Methods

    /// <summary>Processes the specified progress window.</summary>
    /// <param name="progressIndicator">The progress window.</param>
    /// <param name="maxSuggestions">The max templates.</param>
    /// <param name="occurrances">The occurrances.</param>
    /// <param name="minPercentage">The min percentage.</param>
    public void Process([CanBeNull] ProgressIndicator progressIndicator, int maxSuggestions, int occurrances, int minPercentage)
    {
      this.ProgressIndicator = progressIndicator;
      this.MaxSuggestions = maxSuggestions;
      this.Occurances = occurrances;
      this.MinPercentage = minPercentage;

      if (this.MaxSuggestions < 1)
      {
        this.MaxSuggestions = 1;
      }

      var indicator = this.ProgressIndicator;
      if (indicator != null)
      {
        indicator.Task = "Loading data files...";
        indicator.Text = string.Empty;
      }

      this.LoadDataFiles();

      if (indicator != null)
      {
        indicator.Task = "Building suggestions...";
        indicator.Text = string.Format("0 of {0}", this.templates.Count);
      }

      this.Build();
    }

    #endregion

    #region Methods

    /// <summary>Saves the workset.</summary>
    private void Build()
    {
      var tempFileName = AutoTemplateManager.GetTempFileName();
      using (var output = new XmlTextWriter(tempFileName, Encoding.UTF8))
      {
        output.Formatting = Formatting.Indented;

        output.WriteStartElement("i");

        string key = null;
        var records = new List<Record>();

        var total = this.templates.Count;
        var count = 0;
        foreach (var record in this.templates.OrderBy(d => d.Key))
        {
          if (count % 25 == 0 || count == total - 1)
          {
            var progressIndicator = this.ProgressIndicator;
            if (progressIndicator != null)
            {
              progressIndicator.Text = string.Format("{0} of {1}", count, total);
            }
          }

          count++;

          if (record.Key != key)
          {
            key = record.Key;

            this.Build(output, records);
            records.Clear();
          }

          records.Add(record);
        }

        this.Build(output, records);

        output.WriteEndElement();
        output.Flush();
      }

      var fileName = AutoTemplateManager.GetAutoTemplatesFileName();

      lock (AutoTemplateManager.FileLock)
      {
        File.Delete(fileName);
        File.Move(tempFileName, fileName);
      }
    }

    /// <summary>Saves the workset.</summary>
    /// <param name="output">The output.</param>
    /// <param name="records">The descriptors.</param>
    private void Build(XmlTextWriter output, List<Record> records)
    {
      if (!records.Any())
      {
        return;
      }

      var descriptors = new List<Record>();
      Record current = null;
      string template = null;
      var total = 0;

      foreach (var record in records.OrderBy(d => d.Template))
      {
        if (record.Template != template)
        {
          template = record.Template;

          current = new Record
          {
            Key = record.Key, 
            Template = record.Template
          };

          foreach (var variable in record.TemplateVariables)
          {
            current.TemplateVariables[variable.Key] = variable.Value;
          }

          descriptors.Add(current);
        }
        else if (current != null)
        {
          this.MergeVariables(record.TemplateVariables, current.TemplateVariables);
        }

        if (current != null)
        {
          current.Count++;
          total++;
        }
      }

      this.Build(output, descriptors, total);
    }

    /// <summary>Builds the specified output.</summary>
    /// <param name="output">The output.</param>
    /// <param name="records">The descriptors.</param>
    /// <param name="total">The total.</param>
    private void Build(XmlTextWriter output, List<Record> records, int total)
    {
      foreach (var record in records.Where(d => d.Count >= this.Occurances && d.Count * 100.0 / total >= this.MinPercentage).OrderByDescending(d => d.Count).Take(this.MaxSuggestions))
      {
        var variables = new StringBuilder();
        foreach (var variable in record.TemplateVariables)
        {
          if (variables.Length != 0)
          {
            variables.Append('|');
          }

          var value = variable.Value;
          if (string.IsNullOrEmpty(value))
          {
            value = "completeSmart()";
          }
          else if (value.StartsWith("c:"))
          {
            value = "list(\"" + value.Substring(2) + "\")";
          }

          variables.Append(variable.Key);
          variables.Append('|');
          variables.Append(value);
        }

        var v = variables.ToString();

        output.WriteStartElement("i");
        output.WriteAttributeString("k", record.Key);
        output.WriteAttributeString("c", record.Count.ToString());
        if (!string.IsNullOrEmpty(v))
        {
          output.WriteAttributeString("v", variables.ToString());
        }

        output.WriteAttributeString("p", ((int)(record.Count * 100.0 / total)).ToString());
        output.WriteValue(record.Template);
        output.WriteEndElement();
      }
    }

    /// <summary>Loads the data file.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void LoadDataFile(string fileName)
    {
      XDocument doc;
      try
      {
        doc = XDocument.Load(fileName);
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
        try
        {
          var template = new Record
          {
            Key = element.GetAttributeValue("k"),
            Template = element.Value
          };

          var variables = element.GetAttributeValue("v").Split('|');
          for (var index = 0; index < variables.Length - 1; index += 2)
          {
            template.TemplateVariables[variables[index]] = variables[index + 1];
          }

          this.templates.Add(template);
        }
        catch
        {
          continue;
        }
      }
    }

    /// <summary>Loads the data files.</summary>
    private void LoadDataFiles()
    {
      var folder = AutoTemplateManager.DataFolder;
      if (!Directory.Exists(folder))
      {
        return;
      }

      foreach (var fileName in Directory.GetFiles(folder, "*.xml"))
      {
        var fileInfo = new FileInfo(fileName);
        if ((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
          continue;
        }

        if ((fileInfo.Attributes & FileAttributes.System) == FileAttributes.System)
        {
          continue;
        }

        var name = Path.GetFileName(fileName);
        if (string.Compare(name, "_AutoTemplates.xml", StringComparison.InvariantCultureIgnoreCase) == 0)
        {
          continue;
        }

        var progressIndicator = this.ProgressIndicator;
        if (progressIndicator != null)
        {
          progressIndicator.Text = name;
        }

        this.LoadDataFile(fileName);
      }
    }

    /// <summary>Merges the variables.</summary>
    /// <param name="variables0">The variables0.</param>
    /// <param name="variables1">The variables1.</param>
    private void MergeVariables(Dictionary<string, string> variables0, Dictionary<string, string> variables1)
    {
      foreach (var variable0 in variables0)
      {
        var key = variable0.Key;
        var value0 = variable0.Value;

        string value1;
        if (!variables1.TryGetValue(key, out value1))
        {
          variables1[key] = value0;
          continue;
        }

        if (value1.StartsWith("c:"))
        {
          if (value0.StartsWith("c:"))
          {
            var values = value1.Substring(2).Split(',');
            if (!values.Contains(value0.Substring(2)))
            {
              variables1[key] = value1 + "," + value0.Substring(2);
            }

            continue;
          }

          variables1[key] = string.Empty;
          continue;
        }

        if (value0 != value1)
        {
          variables1[key] = string.Empty;
        }
      }
    }

    #endregion

    /// <summary>Defines the <see cref="Record"/> class.</summary>
    public class Record
    {
      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="Record"/> class.</summary>
      public Record()
      {
        this.TemplateVariables = new Dictionary<string, string>();
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Gets or sets the count.
      /// </summary>
      /// <value>The count.</value>
      public int Count { get; set; }

      /// <summary>
      /// Gets or sets the key.
      /// </summary>
      /// <value>The key.</value>
      public string Key { get; set; }

      /// <summary>
      /// Gets or sets the template.
      /// </summary>
      /// <value>The template.</value>
      public string Template { get; set; }

      /// <summary>
      /// Gets the template variables.
      /// </summary>
      [NotNull]
      public Dictionary<string, string> TemplateVariables { get; private set; }

      #endregion
    }
  }
}