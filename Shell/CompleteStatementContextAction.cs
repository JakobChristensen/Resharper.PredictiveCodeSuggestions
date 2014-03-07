// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteStatementContextAction.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="CompleteStatementContextAction" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using JetBrains.ActionManagement;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.Application.DataContext;
  using JetBrains.TextControl;
  using JetBrains.Util.Special;
  using PredictiveCodeSuggestions.Options;

  /// <summary>Defines the <see cref="CompleteStatementContextAction"/> class.</summary>
  public class CompleteStatementContextAction : IActionHandler
  {
    #region Constants and Fields

    /// <summary>The trim chars field.</summary>
    private static readonly char[] trimChars = new[]
    {
      ' ', '\t'
    };

    #endregion

    #region Public Methods

    /// <summary>Executes action. Called after Update, that set ActionPresentation.Enabled to true.</summary>
    /// <param name="context">The data context.</param>
    /// <param name="nextExecute">delegate to call</param>
    public void Execute(IDataContext context, [CanBeNull] DelegateExecute nextExecute)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      if (!AtLeadInWhitespace(context))
      {
        if (nextExecute != null)
        {
          nextExecute();
        }

        return;
      }

      var service = Shell.Instance.GetComponent<AutoTemplatesService>();
      var settings = service.GetSettings();
      if (!settings.GetUseCompleteStatement())
      {
        return;
      }

      var actionManager = Shell.Instance.GetComponent<ActionManager>();
      var action = actionManager.TryGetAction("Generate") as IExecutableAction;
      if (action != null)
      {
        action.Execute(context);
      }
    }

    /// <summary>Updates action visual presentation. If presentation.Enabled is set to false, Execute
    /// will not be called.</summary>
    /// <param name="context">The data context</param>
    /// <param name="presentation">presentation to update</param>
    /// <param name="nextUpdate">delegate to call</param>
    /// <returns>Returns <c>true</c>, if successful, otherwise <c>false</c>.</returns>
    public bool Update(IDataContext context, ActionPresentation presentation, [CanBeNull] DelegateUpdate nextUpdate)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      if (presentation == null)
      {
        throw new ArgumentNullException("presentation");
      }

      if (!AtLeadInWhitespace(context))
      {
        if (nextUpdate != null)
        {
          return nextUpdate();
        }

        return false;
      }

      return true;
    }

    #endregion

    #region Methods

    /// <summary>Checks for the lead in whitespace.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the boolean.</returns>
    private static bool AtLeadInWhitespace([NotNull] IDataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var textControl = context.GetData(JetBrains.TextControl.DataContext.DataConstants.TEXT_CONTROL);
      if (textControl == null)
      {
        return false;
      }

      var caretLine = textControl.Document.GetCoordsByOffset(textControl.Caret.Offset()).Line;
      var line = textControl.Document.GetLineText(caretLine);
      var prefix = line.Substring(0, textControl.Caret.Offset() - textControl.Document.GetLineStartOffset(caretLine));

      if (prefix.TrimStart(trimChars).Length != 0)
      {
        return false;
      }

      if ((prefix.Length != 0) && (line.Length != prefix.Length))
      {
        return line[prefix.Length].IsAnyOf(trimChars);
      }

      return true;
    }

    #endregion
  }
}