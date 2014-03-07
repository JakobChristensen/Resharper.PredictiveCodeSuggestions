// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateScope.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateScope" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="AutoTemplateScope"/> class.</summary>
  public class AutoTemplateScope
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplateScope"/> class.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="key">The key.</param>
    /// <param name="scopeParameters">The scope parameters.</param>
    public AutoTemplateScope([NotNull] IStatement statement, [NotNull] string key, [NotNull] Dictionary<string, string> scopeParameters)
    {
      this.Statement = statement;
      this.Key = key;
      this.ScopeParameters = scopeParameters;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the get insert position.
    /// </summary>
    /// <value>The get insert position.</value>
    [CanBeNull]
    public Func<IStatement, DocumentRange> GetInsertPosition { get; set; }

    /// <summary>
    /// Gets or sets the get next statement.
    /// </summary>
    /// <value>The get next statement.</value>
    [CanBeNull]
    public Func<IStatement, IStatement> GetNextStatement { get; set; }

    /// <summary>
    /// Gets the document range.
    /// </summary>
    /// <value>The document range.</value>
    public DocumentRange InsertPosition
    {
      get
      {
        if (this.GetInsertPosition != null)
        {
          return this.GetInsertPosition(this.Statement);
        }

        return this.Statement.GetDocumentRange().EndOffsetRange();
      }
    }

    /// <summary>
    /// Gets the shortcut.
    /// </summary>
    /// <value>The shortcut.</value>
    [NotNull]
    public string Key { get; private set; }

    /// <summary>
    /// Gets the next statement.
    /// </summary>
    [CanBeNull]
    public IStatement NextStatement
    {
      get
      {
        if (this.GetNextStatement != null)
        {
          return this.GetNextStatement(this.Statement);
        }

        return this.GetNext(this.Statement);
      }
    }

    /// <summary>
    /// Gets the handler parameters.
    /// </summary>
    /// <value>The handler parameters.</value>
    [NotNull]
    public Dictionary<string, string> ScopeParameters { get; private set; }

    /// <summary>
    /// Gets the statement.
    /// </summary>
    [NotNull]
    public IStatement Statement { get; private set; }

    #endregion

    #region Methods

    /// <summary>Gets the next statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the next statement.</returns>
    [CanBeNull]
    private IStatement GetNext([NotNull] IStatement statement)
    {
      if (statement == null)
      {
        throw new ArgumentNullException("statement");
      }

      var block = statement.GetContainingNode<IBlock>();
      if (block == null)
      {
        return null;
      }

      var s = statement as ICSharpStatement;
      if (s == null)
      {
        return null;
      }

      var index = block.Statements.IndexOf(s);
      if (index < 0)
      {
        return null;
      }

      if (index == block.Statements.Count - 1)
      {
        return null;
      }

      return block.Statements[index + 1];
    }

    #endregion
  }
}