using System;
using Gtk;

namespace GDScript_Shell
{
	public static class EditMethods
	{
		[Flags]
		public enum Trigger
		{
			None,
			Open,
			New,
		}

		// creates the popup menu and adds all items
		public static void Popup(MainWindow parent, object sender, EventArgs e, Trigger trigger = Trigger.None)
		{
			Menu mBox = new Menu();

			// edit file
			MenuItem mEdit = new MenuItem("Edit file...");
			mEdit.Activated += delegate (object _sender, EventArgs _e)
			{
				string filePath = MainClass.GetFileContents(parent, "Select script to edit...")[1];
				if (parent.childWindow == null)
				{
					EditorWindow edWin = new EditorWindow(parent);
					parent.childWindow = edWin;
					edWin.AddCodeTab(filePath);
				}
				else
				{
					parent.childWindow.AddCodeTab(filePath);
				}
			};
			mBox.Append(mEdit);

			// new file
			MenuItem mNew = new MenuItem("New file...");
			mNew.Activated += delegate (object _sender, EventArgs _e)
			{
				if (parent.childWindow == null)
				{
					EditorWindow edWin = new EditorWindow(parent);
					parent.childWindow = edWin;
					edWin.AddEmptyCodeTab();
				}
				else
				{
					parent.childWindow.AddEmptyCodeTab();
				}
			};
			mBox.Append(mNew);

			// open editor
			MenuItem mOpen = new MenuItem("New editor window...");
			mOpen.Activated += delegate
			{
				EditorWindow edWin = new EditorWindow(parent);
				parent.childWindow = edWin;
				edWin.AddEmptyCodeTab();
			};
			mBox.Append(mOpen);

			// separator
			SeparatorMenuItem sep = new SeparatorMenuItem();
			mBox.Append(sep);

			// preferences
			MenuItem mPref = new MenuItem("Preferences...");
			mPref.Activated += delegate {
				PreferencesWindow prefWin = new PreferencesWindow(parent);
			};
			mBox.Append(mPref);

			mBox.ShowAll();
			mBox.Popup();

			// Triggers used by shortcuts
			switch (trigger)
			{
				case Trigger.Open:
					{
						mBox.ActivateItem(mEdit, true);
						break;
					}
				case Trigger.New:
					{
						mBox.ActivateItem(mNew, true);
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