// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateLiveTemplateGenerator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="CreateLiveTemplateGenerator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.PopupMenus;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="CreateLiveTemplateGenerator"/> class.</summary>
  public class CreateLiveTemplateGenerator : GeneratorBase
  {
    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns><c>true</c> if this instance can execute the specified data context; otherwise, <c>false</c>.</returns>
    public override bool CanExecute(DataContext context)
    {
      return true;
    }

    /// <summary>Executes the specified data context.</summary>
    /// <param name="context">The data context.</param>
    public override void Execute(DataContext context)
    {
      var menuItems = new List<PopupMenuItem>();

      foreach (var scope in ScopeManager.GetScopes(context))
      {
        var shortcut = scope.Scope.GetShortcut(scope.Parameters);

        var menuItem = new PopupMenuItem(shortcut)
        {
          Tag = scope
        };

        menuItem.Clicked += (sender, args) => this.CreateTemplate((PopupMenuItem)sender);
        menuItems.Add(menuItem);
      }

      var menu = new PopupMenu("Smart Generate Scope");

      menu.Show(menuItems);
    }

    /// <summary>
    /// Gets or sets the action group.
    /// </summary>
    /// <returns>The action group.</returns>
    public override ActionGroup GetActionGroup()
    {
      return ActionGroup.CreateLiveTemplate;
    }

    #endregion

    #region Methods

    /// <summary>Gets the action id.</summary>
    /// <returns>Returns the action id.</returns>
    protected override string GetActionId()
    {
      return "SmartGenerate.CreateLiveTemplate";
    }

    /// <summary>Gets the title.</summary>
    /// <returns>Returns the title.</returns>
    protected override string GetTitle()
    {
      return "Create live template";
    }

    /// <summary>Creates the template.</summary>
    /// <param name="sender">The sender.</param>
    private void CreateTemplate([NotNull] PopupMenuItem sender)
    {
      if (sender == null)
      {
        throw new ArgumentNullException("sender");
      }

      var scope = (ScopeInstance)sender.Tag;

      var text = string.Empty;

      if (scope.Parameters.Any())
      {
        text = "/*\r\n  Macros - delete before saving\r\n";

        foreach (var parameter in scope.Parameters)
        {
          text += "  " + parameter.Key + " = \"" + parameter.Value + "\"\r\n";
        }

        text += "*/\r\n";
      }

      LiveTemplateManager.CreateLiveTemplate(scope.Shortcut, scope.Shortcut, text);
    }

    #endregion
  }
}