// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateAction.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateAction" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Actions
{
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Feature.Services.Bulbs;
  using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.ReSharper.Intentions.Extensibility.Menu;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.Util;
  using PredictiveCodeSuggestions.AutoTemplates;

  /// <summary>Defines the <see cref="AutoTemplateAction"/> class.</summary>
  [ContextAction(Description = "Insert AutoTemplate", Group = "C#", Name = "Insert AutoTemplate", Priority = 100)]
  public sealed class AutoTemplateAction : IContextAction
  {
    #region Constants and Fields

    /// <summary>The provider field.</summary>
    private readonly ICSharpContextActionDataProvider provider;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplateAction"/> class. For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
    /// be injected in this constructor.</summary>
    /// <param name="provider">The provider.</param>
    public AutoTemplateAction(ICSharpContextActionDataProvider provider)
    {
      this.provider = provider;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the items.
    /// </summary>
    [NotNull]
    private IBulbAction[] Items
    {
      get
      {
        var items = new List<IBulbAction>();
        var statement = this.provider.GetSelectedElement<IStatement>(true, true);
        if (statement == null)
        {
          return items.ToArray();
        }

        var scopes = new List<AutoTemplateScope>();
        foreach (var handler in AutoTemplateManager.StatementAnalyzers)
        {
          if (!handler.CanHandle(statement))
          {
            continue;
          }

          var scope = handler.Handle(statement);
          if (scope != null)
          {
            scopes.Add(scope);
          }
        }
         
        if (!scopes.Any())
        {
          return items.ToArray();
        }

        var expectedTemplates = new List<AutoTemplate>();
        foreach (var s in scopes.Distinct(t => t.Key).ToList())
        {
          var scope = s;
          var insertPosition = scope.InsertPosition;
          if (insertPosition == DocumentRange.InvalidRange)
          {
            continue;
          }

          var autoTemplates = AutoTemplateManager.AutoTemplates.Where(t => t.Key == scope.Key).ToList();

          foreach (var template in autoTemplates)
          {
            if (expectedTemplates.Any(e => e.Template == template.Template))
            {
              continue;
            }

            var bulbItem = new AutoTemplateBulb(scope.ScopeParameters, template, insertPosition.TextRange);
            items.Add(bulbItem);
            expectedTemplates.Add(template);
          }
        }

        return items.ToArray();
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates the bulb items.
    /// </summary>
    /// <returns>Returns the I enumerable{ intention action}.</returns>
    [NotNull]
    public IEnumerable<IntentionAction> CreateBulbItems()
    {
      foreach (var item in this.Items)
      {
        var action = new IntentionAction(item, item.Text, null, IntentionsAnchors.ContextActionsAnchor);

        yield return action;
      }
    }

    /// <summary>Check if this action is available at the constructed context.
    /// Actions could store precalculated info in <paramref name="cache"/> to share it between different actions</summary>
    /// <param name="cache">The cache.</param>
    /// <returns>true if this bulb action is available, false otherwise.</returns>
    public bool IsAvailable(IUserDataHolder cache)
    {
      return this.Items.Length > 0;
    }

    #endregion
  }
}