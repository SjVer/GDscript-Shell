// shortcut actions
using System;

namespace GDScript_Shell.Shortcut
{
	public class Shell
	{
		// shortcuts for mainwindow
		public static void Shortcut(MainWindow window, string key)
		{
			Console.WriteLine(key);
			switch (key)
			{
				case "w":
					FileMethods.Popup(window, window, new EventArgs(), FileMethods.Trigger.Close);
					break;
				case "q":
					FileMethods.Popup(window, window, new EventArgs(), FileMethods.Trigger.Exit);
					break;
				case "o":
					FileMethods.Popup(window, window, new EventArgs(), FileMethods.Trigger.Open);
					break;
				case "n":
					EditMethods.Popup(window, window, new EventArgs(), EditMethods.Trigger.New);
					break;
				case "b":
					window.FullTextToCommand(window.mainShell.Buffer.Text);
					break;
				case "l":
					ShellMethods.Popup(window, window, new EventArgs(), ShellMethods.Trigger.Clear);
					break;
				case "r":
					window.Message("Restart");
					break;

				case "p":
					Console.WriteLine("Getting tags per character:");
					Gtk.TextIter iter = window.mainShell.Buffer.StartIter.Copy();
					for (int i = 0; i < window.mainShell.Buffer.CharCount; i++)
					{
						iter.ForwardChar();
						foreach (var tag in iter.Tags)
							Console.WriteLine(iter.Char + ": " + tag.Name);
					}
					break;
			}
		}
	}

	public class Editor
	{
		// shortcuts for editor
		public static void Shortcut(EditorWindow window, string key)
		{
			if (key == "w")
			{
				window.ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
			}
			else if (key == "q")
			{
				MainClass.windowCount = 0;
				window.ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
			}
		}
	}
}