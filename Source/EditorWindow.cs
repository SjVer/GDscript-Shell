using System;
using Gtk;

namespace GDScript_Shell
{
	public partial class EditorWindow : Gtk.Window
	{
		MainWindow parentWindow;
		public bool isAlive;

		public EditorWindow(MainWindow parent) : base(Gtk.WindowType.Toplevel)
		{
			Build();
			isAlive = true;
			MainClass.OpenWindow(this, "EditorWindow");
			parentWindow = parent;
			Console.WriteLine("Parent window: " + parentWindow.Name);
			Title = "GDScript Editor";

		}

		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs a)
		{
			isAlive = false;
			if (parentWindow.isAlive)
			{
				if (parentWindow.childWindow == this)
				{
					parentWindow.childWindow = null;
				}
				parentWindow.SetFocusOn();
			}
			MainClass.CloseWindow(o, a, "EditorWindow");
		}

		//public void RenderText(string text) { textview1.Buffer.Text = text; }

		public void SetFocusOn() { Present(); }

		protected void OnButton1Clicked(object sender, EventArgs e)
		{
			PreferencesWindow prefWin = new PreferencesWindow(edparent : this);
		}

		public void AddCodeTab(string filePath)
		{
			// put textview in vbox with 1 spacing so that the path of the
			// opened file can be stored as the name of the vbox
			VBox container = new VBox();
			container.Name = filePath;
			container.Spacing = 1;

			TextView text = new TextView();
			text.Buffer.Text = System.IO.File.ReadAllText(filePath);

			container.Add(text);

			Label label = new Label();
			label.Text = System.IO.Path.GetFileName(filePath);


			views.AppendPage(container, label);
			views.SetTabDetachable(container, true);
			views.ShowAll();
		}

		public void AddEmptyCodeTab(string name = "Untitled")
		{
			TextView text = new TextView();
			Label label = new Label();
			label.Text = name;

			views.AppendPage(text, label);
			views.SetTabDetachable(text, true);
			views.ShowAll();
		}

		protected void OnButtonRunClicked(object sender, EventArgs e)
		{
			//Widget current = views.CurrentPageWidget;
			VBox container = (VBox)views.CurrentPageWidget;
			Label label = (Label)views.GetTabLabel(container);
			if (!(container.Name == "") && !(container.Name == null))
			{
				parentWindow.ignoringShellChange = true;
				parentWindow.InsertText("\nRunning script: " + label.Text
				                        + "\n", parentWindow.shellTags["Message"]);
				parentWindow.FullTextToCommand(container.Name);
			}
		}
	}
}
