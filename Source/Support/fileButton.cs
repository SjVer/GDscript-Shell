using System;
using Gtk;

namespace GDScript_Shell
{
	public static class FileMethods
	{
		[Flags]
		public enum Trigger
		{
			None,
			Open,
			Close,
			Exit
		}

		// creates the popup menu and adds all items
		public static void Popup(MainWindow parent, object sender, EventArgs e, Trigger trigger = Trigger.None)
		{
			//parent = newparent;
			Menu mBox = new Menu();

			// open
			MenuItem mRun = new MenuItem("Run script...");
			mRun.Activated += delegate (object _sender, EventArgs _e)
			{
				// returns string[name, path, contents]
				string[] file = MainClass.GetFileContents(parent, "Select script to run...");
				try {
					parent.ignoringShellChange = true;
					parent.InsertText(MainClass.RunCommand(file[1]), parent.shellTags["Output"]);
					parent.ignoringShellChange = false;
					parent.Prompt();
				} catch {
					parent.InsertText("Error reading file '" + file[1] + "'");
				}
			};
			mBox.Append(mRun);

			// separator
			SeparatorMenuItem sep = new SeparatorMenuItem();
			mBox.Append(sep);

			// close
			MenuItem mClose = new MenuItem("Close...");
			mClose.Activated += delegate (object _sender, EventArgs _e)
			{
				parent.ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
			};
			mBox.Append(mClose);

			// exit
			MenuItem mExit = new MenuItem("Exit...");
			mExit.Activated += delegate (object _sender, EventArgs _e)
			{
				Console.WriteLine("Program exited. Terminating...");
				Environment.Exit(0);
			};
			mBox.Append(mExit);

			mBox.ShowAll();
			mBox.Popup();

			// Triggers used by shortcuts
			switch (trigger)
			{
				case Trigger.Open:
					{
						mBox.ActivateItem(mRun, true);
						break;
					}
				case Trigger.Close:
					{
 						mBox.ActivateItem(mClose, true);
						break;
					}
				case Trigger.Exit:
					{
						mBox.ActivateItem(mExit, true);
						break;
					}
				case Trigger.None:
					{
						break;
					}
			}
		}
	}
}