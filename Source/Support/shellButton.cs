using System;
using Gtk;

namespace GDScript_Shell
{
	public static class ShellMethods
	{
		[Flags]
		public enum Trigger
		{
			None,
			Clear,
		}

		// creates the popup menu and adds all items
		public static void Popup(MainWindow parent, object sender, EventArgs e, Trigger trigger = Trigger.None)
		{
			Menu mBox = new Menu();

			// clear shell
			MenuItem mClear = new MenuItem("Clear Shell...");
			mClear.Activated += delegate (object _sender, EventArgs _e)
			{
				parent.mainShell.Buffer.Clear();
				parent.Prompt();
			};
			mBox.Append(mClear);

			mBox.ShowAll();
			mBox.Popup();

			// Triggers used by shortcuts
			switch (trigger)
			{
				case Trigger.Clear:
					{
						mBox.ActivateItem(mClear, true);
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