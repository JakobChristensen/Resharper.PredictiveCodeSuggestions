// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryCatchTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="TryCatchTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="TryCatchTemplate"/> class.</summary>
  public class TryCatchTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      var tryStatement = statement as ITryStatement;
      if (tryStatement == null)
      {
        return false;
      }

      return tryStatement.FinallyBlock == null && tryStatement.Catches.Count > 0;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var tryStatement = statement as ITryStatement;
      if (tryStatement == null)
      {
        return null;
      }

      var result = new StatementDescriptor(scope)
      {
        Template = string.Format("try {{ $END$ }}")
      };

      foreach (var catchClause in tryStatement.Catches)
      {
        var type = catchClause.ExceptionType;
        if (type == null)
        {
          result.Template += " catch { }";
          continue;
        }

        var typeName = type.GetLongPresentableName(tryStatement.Language);
        result.Template += string.Format(" catch ({0}) {{ }}", typeName);
      }

      return result;
    }

    #endregion
  }
}