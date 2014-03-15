Predictive Code Suggestions
===========================
Predictive Code Suggestions is a plugin for JetBrains ReSharper, that helps you produce code by suggestion the next line of code.

<h3>Idea</h3>

When writing code there are often certain patterns to how the code is constructed. Some statements are often followed by certain other statements, functions called followed by checks.

For instance, if a function can return null, you often want to check for null.

<pre>
var text = Path.GetDirectoryName(fileName);
if (text == null) 
{
  ...
}
</pre>

When you write a call to `Path.GetDirectoryName(...)` wouldn't it be nice, if ReSharper suggested that the next line should be a check for null.

Predictive Code Suggestions attempts to do this.

<img src="http://vsplugins.sitecore.net/downloads/github/pcs1.gif" alt="" />

<h3>Features</h3>

<h4>Predefined suggestions</h4>
Predictive Code Suggestions comes with a number of predefined suggestions.

* Suggests a `return` statement, if the caret is at the end of a block.
* Suggests a `continue` or `break` statement, if the caret is at the end of a block inside a `for` or `foreach` loop.
* Suggests a `foreach` statement, if the caret is after a variable declaration of type Enumerable.
* Suggests a check for null, if the caret is a after a variable declaration where the variable might be null, typically if a variable is initialized by a function call. This works well with Code Annotation attributes.

<h4>Live templates</h4>
Predictive Code Suggestions allows you to create your own suggestions by providing highly context-aware Live Templates.

When you open the the ReSharper Generate popup inside a text editor, you will see a new option at the very bottom: "Create live template". Selecting this option opens a new menu, where you select the context you want to use.

Predictive Code Suggestions offers these contexts:
* After variable of type <type name>
* After enumerable of type <type name>
* After call to <method name>
* Before variable of type <type name>
* Before call to <method name>
* Beginning of function
* Class member
* Empty file
* End of statement block
* Inside method
* Interface member
* Struct member
* Switch case
* Class, struct, interface or enum

After selecting the context, the ReSharper Live Template editor opens. You will notice that the Shortcut field starts with "Do not change". This is so that Predictive Code Suggestions may recognize your context-aware Live Templates from standard Live Templates. As the text suggests, you should not change the Shortcut.

You may use some of the predefined macros offered by Predictive Code Suggestions, but remember to delete the comments before saving the Live Template.

When you open Generate popup menu, you will see your context-aware Live Template at the very top.

<h4>Suggestions analysis</h4>
Predictive Code Suggestions also analyzes your code in the background to determine common patterns. When a pattern is detected, Predictive Code Suggestions offers the suggestions as a Live Template in the Generate popup menu.

Over time Predictive Code Suggestions "learn" how you write code and is able to suggest Live Templates, that are automatically generated.

The image below shows a part of Options page for Predictive Code Suggestions. The highlighted line shows that in my code, there are 74 call to `System.Collections.Generics.List.Add` and of these 74 calls, 26% are followed by a `return` statement. If another call to the `Add` is made, there is a good chance, it should be followed by a `return` statement.


