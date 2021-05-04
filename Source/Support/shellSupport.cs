using System;
using Gtk;
using GDScript_Shell;

public partial class MainWindow : Window
{
	public TextMark InputBegin;
	TextTag noEdit;

	public void InsertText(string text, TextTag tag = null)
	{
		//shell.Buffer.Text += text;
		shell.Buffer.InsertWithTags(shell.Buffer.EndIter, text, tag);
	}

	public void Prompt(bool newline = false) {
		if (newline)
			InsertText("\n");
		InsertText(MainClass.prompt + "  ", shellTags["Prompt"]);

		shell.Buffer.PlaceCursor(shell.Buffer.EndIter);
		shell.Buffer.MoveMark(InputBegin, shell.Buffer.EndIter);
		//shell.Buffer.ApplyTag(noEdit, shell.Buffer.StartIter, shell.Buffer.EndIter);
	}

	public void ExecuteInput() {
		TextIter begin = shell.Buffer.GetIterAtMark(InputBegin);
		TextIter end = shell.Buffer.EndIter;

		string input = shell.Buffer.GetText(begin, end, false);

		shell.Buffer.Delete(ref begin, ref end);
		InsertText(input, shellTags["FakeUser"]);

		FullTextToCommand(input);
	}

	public void Message(string title, bool ignoreNoNewline = false, bool noPrompt = false)
	{
		ignoringShellChange = true;
		if (!shell.Buffer.Text.EndsWith("\n", StringComparison.Ordinal) && !ignoreNoNewline)
			InsertText("\n", shellTags["Message"]);
		string[] lines = MainClass.Messages[title];
		foreach (string line in lines)
		{
			InsertText(line + "\n", shellTags["Message"]);
		}
		ignoringShellChange = false;
		if (!noPrompt)
			Prompt();
	}


	/* NOTES:
	this method is supposed to check if a iter is editable but it's shit.
	as of right now it just picks the last actually typed char and checks if it has tags and shit
	for some reason it doesnt see some tags while it sees others (try running an empty command)
	it is pure fucking shit
	no fukcing clue how to fix this
	i'm at the fucking edge man
	this pain
	can't take it any longer man
	fucken hell
	
	PS: WHY THE FUCK DOES THE THING I ASSIGNED TO CTRL+P WORK THEN WHAT THE FUCK
	*/
	public bool CheckIfEditable(TextIter givenIter, bool verbose = false)
	{
		TextIter iter = givenIter.Copy();

		iter.BackwardChars(2);

		if (verbose) Console.WriteLine("\n\nchar: '" + iter.Char + "'");
		if (verbose) Console.WriteLine("tag count: " + iter.Tags.Length);
		if (verbose) Console.WriteLine("editable: " + iter.Editable(true));
		if (verbose) Console.WriteLine("---");

		bool editable = true;

		foreach (var tag in iter.Tags)
			if (verbose) Console.WriteLine(tag.Name + ": " + tag.Editable);

		if (verbose) Console.WriteLine("---");

		return editable;
	}
}