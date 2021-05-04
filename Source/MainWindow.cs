// Code by Sjoerd Vermeulen
// License: MIT

using System;
using System.IO;
using System.Collections.Generic;
using Gtk;
using GDScript_Shell;
using GDScript_Shell.Shortcut;

public partial class MainWindow : Window
{
	public bool isAlive;
	public EditorWindow childWindow;
	public bool controlPressed;
	public TextView mainShell;
	public Dictionary<string, TextTag> shellTags;
 
	public MainWindow() : base(WindowType.Toplevel)
	{
		Build();
		SetTags();
		MainClass.OpenWindow(this, "MainWindow");
		isAlive = true;
		shell.HasFocus = false;
		shell.Buffer.Changed += new global::System.EventHandler(OnShellChanged);

		// duplicate some private objects publically for easy access
		mainShell = shell;
		shellTags = new Dictionary<string, TextTag>();

		shell.Buffer.TagTable.Foreach((TextTag tag) => shellTags[tag.Name] = tag);
		InputBegin = shell.Buffer.CreateMark("InputBegin", shell.Buffer.EndIter, true);

		noEdit = new TextTag("noEdit");
		noEdit.Editable = false;

		//Message("Restart");
		Prompt();
	}

	public void SetTags()
	{
		string json = File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/data/tags.json");
		var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
		foreach (var item in dict)
		{
			TextTag tag = new TextTag(item.Key);
			tag.Editable = item.Value["Editable"];
			tag.Foreground = item.Value["Foreground"];
			tag.Background = item.Value["Background"];
			shell.Buffer.TagTable.Add(tag);
			//Console.WriteLine("Tag '" + tag.Name + "': Fg: " + item.Value["Foreground"] + 
			        //" Bg: " + item.Value["Background"] +" Ed: " + item.Value["Editable"]);
		}
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		isAlive = false;
		MainClass.CloseWindow(sender, a, "MainWindow");
		Destroy();
	}

	public void OnButtonFileClicked(object sender, EventArgs e)
	{
		FileMethods.Popup(this, sender, e);
	}

	protected void OnButtonEditClicked(object sender, EventArgs e)
	{
		EditMethods.Popup(this, sender, e);
	}

	protected void OnButtonShellClicked(object sender, EventArgs e)
	{
		ShellMethods.Popup(this, sender, e);
	}

	public bool cancontinue = true; //this bool makes sure the output of a command wont trigger a new one
	protected void OnShellChanged(object sender, EventArgs e)
	{
		// Check for newline, if so execute
		if (cancontinue && shell.Buffer.Text.EndsWith("\n"))// && 
		    //CheckIfEditable(shell.Buffer.EndIter, true))
		{
			cancontinue = false;
			ExecuteInput();
			Prompt();
			cancontinue = true;
		}

		//shell.Buffer.EndIter.
		//Console.WriteLine(shell.GetIterLocation(shell.Buffer.EndIter).Location.Y);
		int col = shell.Buffer.EndIter.LineOffset;
		int line = shell.Buffer.EndIter.Line + 1;

		labelPos.Text = String.Format("Ln: {0} Col: {1}", line, col);
	}

	public void SetFocusOn() { Present(); }

	protected void OnKeyPressEvent(object o, KeyPressEventArgs args)
	{
		if (shell.HasFocus) 
		{
			// shell has focus
		}
		string pressedKey = args.Event.Key.ToString();
		if (pressedKey.ToLower().Contains("control"))
		{
			controlPressed = true;
		}
		else if (controlPressed)
		{
			Shell.Shortcut(this, args.Event.Key.ToString().ToLower());
			controlPressed = false;
		}
	}

	public void FullTextToCommand(string text)
	{
		text = text.Trim();
		Console.WriteLine("\ninput: " + text);
		string output = "";
		if (!(text == "" || text == null))
		{
			//string first = System.Text.RegularExpressions.Regex.Match(text, @"^([\w\-]+)").Value;
			//string args = text.Replace(first, "").Trim();
			//output = MainClass.RunCommand(first, args).Replace("\n", "").Trim();
			output = MainClass.RunCommand(text).Replace("\n", "").Trim();
			if (!(output == "")) output += "\n";
		}
		Console.WriteLine("output: " + output + "\n");
		InsertText(output, shellTags["Output"]);
	}

}
