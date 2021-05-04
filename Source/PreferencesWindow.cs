using System;
//using GtkSource;
namespace GDScript_Shell
{
	public partial class PreferencesWindow : Gtk.Window
	{
		MainWindow parentWindow;
		EditorWindow parentWindowEd;

		public PreferencesWindow(MainWindow parent = null, EditorWindow edparent = null) : base(Gtk.WindowType.Toplevel)
		{
			Build();
			if (parent != null)
			{
				parentWindow = parent;
				parentWindowEd = null;
			}
			else if (edparent != null)
			{
				parentWindow = null;
				parentWindowEd = edparent;
			}
			else
			{
				throw new Exception("parent window needed");
			}
			MainClass.OpenWindow(this, "PreferencesWindow");

			// test:
			string all = "";
			foreach (string item in MainClass.GetThemes())
			{
				all += "\n" + item;
			}
			textview1.Buffer.Text = all;
		}

		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs a)
		{
			if (parentWindow != null)
			{
				if (parentWindow.isAlive)
				{
					parentWindow.SetFocusOn();
				}
			}
			else if (parentWindowEd != null)
			{
				if (parentWindowEd.isAlive)
				{
					parentWindowEd.SetFocusOn();
				}
			}
			MainClass.CloseWindow(o, a, "PreferencesWindow");
		}
	}
}
