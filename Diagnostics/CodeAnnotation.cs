// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeAnnotation.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the value analysis attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Diagnostics
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Daemon;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CodeAnnotations;
  using JetBrains.ReSharper.Psi.ControlFlow;
  using JetBrains.ReSharper.Psi.ControlFlow.CSharp;
  using JetBrains.ReSharper.Psi.ControlFlow.Impl;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Resolve;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Util;

  /// <summary>
  /// Defines the value analysis attribute.
  /// </summary>
  public enum CodeAnnotationAttribute
  {
    /// <summary>
    /// The undefined field.
    /// </summary>
    Undefined,

    /// <summary>
    /// The undefined field.
    /// </summary>
    NotSet,

    /// <summary>
    /// The not null field.
    /// </summary>
    NotNull,

    /// <summary>
    /// The can be null field.
    /// </summary>
    CanBeNull
  }

  /// <summary>
  /// Defines the <see cref="CodeAnnotation"/> class.
  /// </summary>
  public class CodeAnnotation
  {
    #region Fields

    /// <summary>
    /// The can be null field.
    /// </summary>
    private ITypeElement canBeNull;

    /// <summary>
    /// The not null field.
    /// </summary>
    private ITypeElement notNull;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="CodeAnnotation"/> class. 
    /// Codes the annotation.</summary>
    /// <param name="typeMember">The type member.</param>
    public CodeAnnotation([NotNull] ITypeMemberDeclaration typeMember)
    {
      if (typeMember == null)
      {
        throw new ArgumentNullException("typeMember");
      }

      typeMember.GetPsiServices();

      this.Initialize(typeMember);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether this instance is valid.
    /// </summary>
    public bool IsValid
    {
      get
      {
        return this.notNull != null && this.canBeNull != null;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Gets the annotation.</summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>Returns the annotation.</returns>
    public CodeAnnotationAttribute GetAnnotation([NotNull] IParameterDeclaration parameter)
    {
      if (parameter == null)
      {
        throw new ArgumentNullException("parameter");
      }

      var attributesOwner = parameter.DeclaredElement as IAttributesOwner;

      return attributesOwner != null ? this.GetAnnotation(attributesOwner) : CodeAnnotationAttribute.Undefined;
    }

    /// <summary>Gets the annotation.</summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>Returns the annotation.</returns>
    public CodeAnnotationAttribute GetAnnotation([NotNull] ITypeMemberDeclaration parameter)
    {
      if (parameter == null)
      {
        throw new ArgumentNullException("parameter");
      }

      var attributesOwner = parameter.DeclaredElement as IAttributesOwner;

      return attributesOwner != null ? this.GetAnnotation(attributesOwner) : CodeAnnotationAttribute.Undefined;
    }

    /// <summary>Gets the annotation.</summary>
    /// <param name="method">The method.</param>
    /// <returns>Returns the annotation.</returns>
    public CodeAnnotationAttribute GetAnnotation([NotNull] IMethodDeclaration method)
    {
      if (method == null)
      {
        throw new ArgumentNullException("method");
      }

      var attributesOwner = method.DeclaredElement as IAttributesOwner;

      return attributesOwner != null ? this.GetAnnotation(attributesOwner) : CodeAnnotationAttribute.Undefined;
    }

    /// <summary>Gets the annotation.</summary>
    /// <param name="property">The property.</param>
    /// <returns>Returns the annotation.</returns>
    public CodeAnnotationAttribute GetAnnotation([NotNull] IPropertyDeclaration property)
    {
      if (property == null)
      {
        throw new ArgumentNullException("property");
      }

      var attributesOwner = property.DeclaredElement as IAttributesOwner;

      return attributesOwner != null ? this.GetAnnotation(attributesOwner) : CodeAnnotationAttribute.Undefined;
    }

    /// <summary>Gets the annotation.</summary>
    /// <param name="indexer">The indexer.</param>
    /// <returns>Returns the annotation.</returns>
    public CodeAnnotationAttribute GetAnnotation([NotNull] IIndexerDeclaration indexer)
    {
      if (indexer == null)
      {
        throw new ArgumentNullException("indexer");
      }

      var attributesOwner = indexer.DeclaredElement as IAttributesOwner;

      return attributesOwner != null ? this.GetAnnotation(attributesOwner) : CodeAnnotationAttribute.Undefined;
    }

    /// <summary>
    /// Gets the state of the expression null reference.
    /// </summary>
    /// <param name="treeNode">The element.</param>
    /// <param name="declaredElement">The declared element.</param>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the expression null reference state.</returns>
    public CodeAnnotationAttribute GetExpressionNullReferenceState(ITreeNode treeNode, IDeclaredElement declaredElement, IDeclarationStatement anchor)
    {
      return this.GetNullReferenceState(treeNode, declaredElement, anchor);

      /*
      var state = CodeAnnotationAttribute.Undefined;

      const string CookieName = "CodeAnnotations";

      var solution = treeNode.GetSolution();

      var cookie = solution.CreateTransactionCookie(DefaultAction.Rollback, CookieName);
      try
      {
        var psiServices = solution.GetPsiServices();

        Shell.Instance.GetComponent<UITaskExecutor>().SingleThreaded.ExecuteTask(CookieName, TaskCancelable.Yes, delegate(IProgressIndicator progress1)
        {
          progress1.TaskName = CookieName;

          psiServices.PsiManager.DoTransaction(() => state = this.GetNullReferenceState(treeNode, name, anchorStatement), CookieName);

          cookie.Rollback();
        });
      }
      finally
      {
        if (cookie != null)
        {
          cookie.Dispose();
        }
      }

      return state;
      */
    }

    /// <summary>Inspects the control graf.</summary>
    /// <param name="method">The method.</param>
    /// <returns>Returns the control graf.</returns>
    public CodeAnnotationAttribute InspectControlGraf([NotNull] IMethodDeclaration method)
    {
      if (method == null)
      {
        throw new ArgumentNullException("method");
      }

      return Inspect(method);
    }

    /// <summary>Inspects the control graf.</summary>
    /// <param name="constructor">The constructor.</param>
    /// <returns>Returns the control graf.</returns>
    public CodeAnnotationAttribute InspectControlGraf([NotNull] IConstructorDeclaration constructor)
    {
      if (constructor == null)
      {
        throw new ArgumentNullException("constructor");
      }

      var function = constructor;
      if (!function.DeclaredElement.ReturnType.IsReferenceType())
      {
        return CodeAnnotationAttribute.Undefined;
      }

      return CodeAnnotationAttribute.NotNull;
    }

    /// <summary>Inspects the control graf.</summary>
    /// <param name="getter">The getter.</param>
    /// <returns>Returns the control graf.</returns>
    public CodeAnnotationAttribute InspectControlGraf([NotNull] IAccessorDeclaration getter)
    {
      if (getter == null)
      {
        throw new ArgumentNullException("getter");
      }

      return Inspect(getter);
    }

    #endregion

    #region Methods

    /// <summary>Inspects the specified function.</summary>
    /// <param name="function">The function.</param>
    /// <returns>Returns the code annotation attribute.</returns>
    private static CodeAnnotationAttribute Inspect([NotNull] ICSharpFunctionDeclaration function)
    {
      if (function == null)
      {
        throw new ArgumentNullException("function");
      }

      var project = function.GetProject();
      if (project == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var projectFile = project.ProjectFile;
      if (projectFile == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      if (!function.DeclaredElement.ReturnType.IsReferenceType())
      {
        return CodeAnnotationAttribute.Undefined;
      }

      // return CodeAnnotationAttribute.NotNull;
      AllNonQualifiedReferencesResolver.ProcessAll(function);
      var graf = CSharpControlFlowBuilder.Build(function);
      if (graf == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var result = graf.Inspect(HighlightingSettingsManager.Instance.GetValueAnalysisMode(projectFile));
      if (result == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      switch (result.SuggestReturnValueAnnotationAttribute)
      {
        case CSharpControlFlowNullReferenceState.NOT_NULL:
          return CodeAnnotationAttribute.NotNull;

        case CSharpControlFlowNullReferenceState.NULL:
          return CodeAnnotationAttribute.CanBeNull;

        case CSharpControlFlowNullReferenceState.MAY_BE_NULL:
          return CodeAnnotationAttribute.CanBeNull;
      }

      return CodeAnnotationAttribute.NotSet;
    }

    /// <summary>Codes the annotation attribute.</summary>
    /// <param name="attributesOwner">The attributes owner.</param>
    /// <returns>Returns the annotation attribute.</returns>
    private CodeAnnotationAttribute GetAnnotation([NotNull] IAttributesOwner attributesOwner)
    {
      if (attributesOwner == null)
      {
        throw new ArgumentNullException("attributesOwner");
      }

      var instances = attributesOwner.GetAttributeInstances(true);

      foreach (var attributeInstance in instances)
      {
        var shortName = attributeInstance.GetClrName().ShortName;
        if (shortName == "NotNullAttribute")
        {
          return CodeAnnotationAttribute.NotNull;
        }

        if (shortName == "CanBeNullAttribute")
        {
          return CodeAnnotationAttribute.CanBeNull;
        }
      }

      return CodeAnnotationAttribute.NotSet;
    }

    /// <summary>
    /// Gets the state of the expression null reference.
    /// </summary>
    /// <param name="treeNode">The element.</param>
    /// <param name="declaredElement">The declared element.</param>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the expression null reference state.</returns>
    private CodeAnnotationAttribute GetNullReferenceState(ITreeNode treeNode, IDeclaredElement declaredElement, IDeclarationStatement statement)
    {
      var project = treeNode.GetProject();
      if (project == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var projectFile = project.ProjectFile;
      if (projectFile == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var functionDeclaration = treeNode.GetContainingNode<ICSharpFunctionDeclaration>(true);
      if (functionDeclaration == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var graf = CSharpControlFlowBuilder.Build(functionDeclaration);
      if (graf == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var inspect = graf.Inspect(HighlightingSettingsManager.Instance.GetValueAnalysisMode(projectFile));
      if (inspect == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var position = this.FindPosition(graf.BodyElement.Children, statement);
      if (position == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      position = this.FindFollowing(position);
      if (position == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var result = inspect.GetVariableStateAt(position, declaredElement);
      switch (result)
      {
        case CSharpControlFlowNullReferenceState.NOT_NULL:
          return CodeAnnotationAttribute.NotNull;

        case CSharpControlFlowNullReferenceState.NULL:
          return CodeAnnotationAttribute.CanBeNull;

        case CSharpControlFlowNullReferenceState.MAY_BE_NULL:
          return CodeAnnotationAttribute.CanBeNull;
      }

      return CodeAnnotationAttribute.Undefined;

      /*
      var block = treeNode.GetContainingNode<IBlock>(true);
      if (block == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var project = treeNode.GetProject();
      if (project == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var projectFile = project.ProjectFile;
      if (projectFile == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var factory = CSharpElementFactory.GetInstance(treeNode.GetPsiModule());
      var statement = factory.CreateStatement("if(" + name + " == null) { }");

      var ifStatement = block.AddStatementAfter(statement, (ICSharpStatement)anchorStatement) as IIfStatement;
      if (ifStatement == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var equalityExpression = ifStatement.Condition as IEqualityExpression;
      if (equalityExpression == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var referenceExpression = equalityExpression.LeftOperand as IReferenceExpression;
      if (referenceExpression == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var functionDeclaration = ifStatement.GetContainingNode<ICSharpFunctionDeclaration>(true);
      if (functionDeclaration == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var graf = CSharpControlFlowBuilder.Build(functionDeclaration);
      if (graf == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var inspect = graf.Inspect(HighlightingSettingsManager.Instance.GetValueAnalysisMode(projectFile));
      if (inspect == null)
      {
        return CodeAnnotationAttribute.Undefined;
      }

      var position = graf.AllElements.FirstOrDefault(e => e.SourceElement == treeNode);

      var result = inspect.GetVariableStateAt(position, declaredElement);
        
      result = inspect.GetExpressionNullReferenceState(referenceExpression, false);
      switch (result)
      {
        case CSharpControlFlowNullReferenceState.NOT_NULL:
          return CodeAnnotationAttribute.NotNull;

        case CSharpControlFlowNullReferenceState.NULL:
          return CodeAnnotationAttribute.CanBeNull;

        case CSharpControlFlowNullReferenceState.MAY_BE_NULL:
          return CodeAnnotationAttribute.CanBeNull;
      }

      return CodeAnnotationAttribute.Undefined;
      */
    }

    /// <summary>
    /// Finds the following.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>Returns the I control flow element.</returns>
    private IControlFlowElement FindFollowing(IControlFlowElement element)
    {
      while (element is ControlFlowMultiplexor)
      {
        element = element.Parent;
      }

      return element;

      /*
      if (element == null)
      {
        return null;
      }

      do
      {
        var parent = element.Parent;
        if (parent != null)
        {
          var index = parent.Children.IndexOf(element) + 1;
          if (index < parent.Children.Count)
          {
            return parent.Children[index];
          }
        }

        element = parent;
      }
      while (element != null);

      return null;
      */
    }

    /// <summary>
    /// Finds the position.
    /// </summary>
    /// <param name="elements">The graf.</param>
    /// <param name="treeNode">The tree node.</param>
    /// <returns>Returns the I control flow element.</returns>
    private IControlFlowElement FindPosition(IList<IControlFlowElement> elements, ITreeNode treeNode)
    {
      foreach (var element in elements)
      {
        if (element.SourceElement == null)
        {
          continue;
        }

        if (!element.SourceElement.Contains(treeNode))
        {
          continue;
        }

        var result = this.FindPosition(element.Children, treeNode) ?? element;

        return result;
      }

      return null;
    }

    /// <summary>Initializes the specified type member.</summary>
    /// <param name="typeMember">The type member.</param>
    /// <exception cref="ArgumentNullException">The type member cannot be null.</exception>
    private void Initialize([NotNull] ITypeMemberDeclaration typeMember)
    {
      if (typeMember == null)
      {
        throw new ArgumentNullException("typeMember");
      }

      var sourceFile = typeMember.GetSourceFile();
      if (sourceFile == null)
      {
        return;
      }

      var psiServices = typeMember.GetPsiServices();
      if (psiServices == null)
      {
        throw new InvalidOperationException("psiServices");
      }

      var cache = psiServices.GetCodeAnnotationsCache();

      this.notNull = cache.GetAttributeTypeForElement(typeMember, CodeAnnotationsCache.NotNullAttributeShortName);
      this.canBeNull = cache.GetAttributeTypeForElement(typeMember, CodeAnnotationsCache.CanBeNullAttributeShortName);
    }

    #endregion
  }
}