// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatementDescriptor.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="StatementDescriptor" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Xml;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="StatementDescriptor"/> class.</summary>
  public class StatementDescriptor
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="StatementDescriptor"/> class.</summary>
    /// <param name="scope">The scope.</param>
    public StatementDescriptor(AutoTemplateScope scope)
    {
      this.Scope = scope;
      this.TemplateVariables = new Dictionary<string, string>();
    }

    /// <summary>Initializes a new instance of the <see cref="StatementDescriptor"/> class.</summary>
    /// <param name="scope">The scope.</param>
    /// <param name="template">The template.</param>
    /// <param name="templateVariables">The template variables.</param>
    public StatementDescriptor(AutoTemplateScope scope, string template, Dictionary<string, string> templateVariables)
    {
      this.Scope = scope;
      this.Template = template;
      this.TemplateVariables = new Dictionary<string, string>(templateVariables);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the scope.
    /// </summary>
    /// <value>The scope.</value>
    public AutoTemplateScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    [NotNull]
    public string Template { get; set; }

    /// <summary>
    /// Gets the variables.
    /// </summary>
    [NotNull]
    public Dictionary<string, string> TemplateVariables { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Writes the specified output.</summary>
    /// <param name="output">The output.</param>
    public void Write([NotNull] XmlTextWriter output)
    {
      if (output == null)
      {
        throw new ArgumentNullException("output");
      }

      var variables = new StringBuilder();

      foreach (var variable in this.TemplateVariables)
      {
        if (variables.Length != 0)
        {
          variables.Append('|');
        }

        variables.Append(variable.Key);
        variables.Append('|');
        variables.Append(variable.Value);
      }

      var v = variables.ToString();

      output.WriteStartElement("i");
      output.WriteAttributeString("k", this.Scope.Key);
      output.WriteAttributeString("f", this.FileName);
      if (!string.IsNullOrEmpty(v))
      {
        output.WriteAttributeString("v", v);
      }

      output.WriteValue(this.Template);
      output.WriteEndElement();
    }

    #endregion
  }
}