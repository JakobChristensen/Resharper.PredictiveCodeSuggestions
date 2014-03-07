// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionDescriptor.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ExpressionDescriptor" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="ExpressionDescriptor"/> class.</summary>
  public class ExpressionDescriptor
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ExpressionDescriptor"/> class.</summary>
    public ExpressionDescriptor()
    {
      this.TemplateVariables = new Dictionary<string, string>();
    }

    #endregion

    #region Public Properties

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
  }
}