using System;
using System.IO;
using Gtk;
using System.Collections.Generic;
using System.Diagnostics;

namespace GDScript_Shell
{
	public class MainClass 
	{
		public static string prompt;
		public static int windowCount;
		public static Dictionary<string, string[]> Messages;

		public static void Main(string[] args)
		{
			// Get preferences from config file
			Dictionary<string, string> preferences = ReadConfig();

			// set theme to preference if exists, else to "Godot"
			string theme;
			if (preferences.ContainsKey("theme"))
			{
				theme = preferences["theme"];
			}
			else
			{
				preferences["theme"] = "Godot";
				SaveConfig(preferences);
				theme = "Godot";
			}

			// set prompt to preference if exists else "GD>"
			if (preferences.ContainsKey("prompt"))
			{
				prompt = preferences["prompt"];
			}
			else
			{
				preferences["prompt"] = "GD>";
				SaveConfig(preferences);
				prompt = "GD>";
			}

			Messages = ReadMsgFromJson();

			// Get stuff rolling
			Application.Init();

			Settings.Default.ThemeName = theme;

			MainWindow win = new MainWindow();
			win.SetPosition(WindowPosition.Center);
			win.Show();

			Application.Run();
		}

		public static void SaveConfig(Dictionary<string, string> dict)
		{
			string config = "# GDScript_Shell preferences\n";
			foreach (KeyValuePair<string, string> kvp in dict)
			{
				config += kvp.Key;
				config += ":";
				config += kvp.Value;
				config += "\n"; //newline to represent new pair
			}

			File.WriteAllText(Directory.GetParent(Environment.CurrentDirectory)
			                  .Parent.FullName + "/data/preferences.config", config);
		}

		public static Dictionary<string, string> ReadConfig()
		{
			var reader = new StreamReader(File.OpenRead(Directory.GetParent(Environment.CurrentDirectory)
							  .Parent.FullName + "/data/preferences.config"));
			var dict = new Dictionary<string, string>();
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				if (line[0].ToString() == "#") 
				{
					continue;
				}
				var values = line.Split(':');
				dict.Add(values[0], values[1]);
			}
			reader.Close();
			return dict;
		}

		public static Dictionary<string, string[]> ReadMsgFromJson()
		{
			string text = File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory)
														.Parent.FullName + "/data/messages.json");
			Dictionary<string, string[]> dict = Newtonsoft.Json.JsonConvert.DeserializeObject
														  <Dictionary<string, string[]>>(text);

			return dict;
		}

		public static void OpenWindow(Window window, string name)
		{
			windowCount += 1;
			try
			{
				window.SetIconFromFile(Directory.GetParent(Environment.CurrentDirectory)
				                       .Parent.Parent.FullName + "/Assets/GDShell.ico");
			}
			catch
			{
				window.SetIconFromFile(Directory.GetParent(Environment.CurrentDirectory).FullName + "/Assets/GDShell.ico");
			}
			Console.WriteLine(name + " built. window count: " + windowCount);
		}

		public static void CloseWindow(object sender, DeleteEventArgs a, string name)
		{
			windowCount -= 1;
			Console.WriteLine(name + " destroyed. window count: " + MainClass.windowCount);
			if (MainClass.windowCount <= 0)
			{
				Console.WriteLine("zero windows left. terminating...");
				a.RetVal = true;
				Environment.Exit(0);
				Application.Quit();
			}
		}

		public static string[] GetFileContents(Window parent, string title)
		{
			string[] returnval = new string[3];

			// start filechooser dialog and add filters
			Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Choose script to execute", parent,
			    FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

			FileFilter gdfilter = new FileFilter
			{
				Name = "GDscript"
			};
			gdfilter.AddPattern("*.gd");
			filechooser.AddFilter(gdfilter);

			FileFilter allfilter = new FileFilter
			{
				Name = "All files"
			};
			allfilter.AddPattern("*.*");
			filechooser.AddFilter(allfilter);

			if (filechooser.Run() == (int)ResponseType.Accept)
			{
				string FileNamePath = filechooser.Filename;
				string FileName = Path.GetFileName(FileNamePath);
				try
				{
					// success. returning contents
					string FileLines = File.ReadAllText(FileNamePath);
					filechooser.Destroy();

					returnval[0] = FileName;
					returnval[1] = FileNamePath;
					returnval[2] = FileLines;
					return returnval;
				}
				catch (Exception e)
				{
					return returnval;
				}
			}
			// something went wrong. returning null
			filechooser.Destroy();
			return returnval;
		}

		public static string[] GetThemes(string path = "/usr/share/themes", bool sort = true)
		{
			string[] dirs = Directory.GetDirectories("/usr/share/themes");
			for (int i = 0; i < dirs.Length; i++)
			{
				dirs[i] = dirs[i].Replace(path + "/", "");
			}
			if (sort) { Array.Sort(dirs); }
			return dirs;
		}

		public static string RunCommand(string command)
		{
			string cliOut = "error";
			string headless = Directory.GetParent(Environment.CurrentDirectory)
                   .Parent.Parent.FullName + "/Assets/gdscript-cli/gdscript/gdscript.py";

			string[] cmd = new string[2];
			cmd[0] = "python3";
			cmd[1] = " " + headless + " \"" + command.Replace("\"", "\\\"") + "\"";

			var cliProcess = new Process()
			{
				StartInfo = new ProcessStartInfo(cmd[0], cmd[1])
				{
					UseShellExecute = false,
					RedirectStandardOutput = true
				}
			};
			try
			{
				Console.WriteLine("full command: '" + cliProcess.StartInfo.FileName 
							                  + cliProcess.StartInfo.Arguments + "'");
				cliProcess.Start();
				cliOut = cliProcess.StandardOutput.ReadToEnd();
				cliProcess.WaitForExit();
				cliProcess.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("error: " + ex);
			}

			return cliOut;
		}
	}
}

/*
using Mono.Terminal;
using System;

class Demo
{
	static void Main()
	{
		// Creates a line editor, and sets the name of the editing session to
		// "foo".  This is used to save the history of input entered
		LineEditor le = new LineEditor("foo")
		{
			HeuristicsMode = "csharp"
		};

		// Configures auto-completion, in this example, the result
		// is always to offer the numbers as completions
		le.AutoCompleteEvent += delegate (string text, int pos) {
			string prefix = "";
			var completions = new string[] {
				"One", "Two", "Three", "Four", "Five",
				"Six", "Seven", "Eight", "Nine", "Ten" };
			return new Mono.Terminal.LineEditor.Completion(prefix, completions);
		};

		string s;

		// Prompts the user for input
		while ((s = le.Edit("shell> ", "")) != null)
		{
			Console.WriteLine("Your Input: [{0}]", s);
		}
	}
}
*/

/*
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace ProcessStandardInputSample
{
	class StandardInputTest
	{
		static void Main()
		{
			Console.WriteLine(">>> Process started!");

			using (Process myProcess = new Process())
			{
				myProcess.StartInfo.FileName = "csharp";
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.RedirectStandardOutput = true;

				myProcess.Start();

				StreamWriter myStreamWriter = myProcess.StandardInput;
				StreamReader myStreamReader = myProcess.StandardOutput;

				String inputText;

				do // execute the next code...
				{
					Console.WriteLine(">>> Enter input (or press the Enter key to stop):");

					inputText = Console.ReadLine();
					if (inputText.Length > 0) 
					{
						myStreamWriter.WriteLine(inputText);
					}

				// ...while this is true
				} while (inputText.Length > 0);

				Console.WriteLine(">>> Process done!");
				myStreamWriter.Close();
				myProcess.WaitForExit();
			}
		}
	}
}
*/
