
// This file has been generated by the GUI designer. Do not modify.
namespace GDScript_Shell
{
	public partial class EditorWindow
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.Button button1;

		private global::Gtk.Notebook views;

		private global::Gtk.Label label3;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget GDScript_Shell.EditorWindow
			this.Name = "GDScript_Shell.EditorWindow";
			this.Title = global::Mono.Unix.Catalog.GetString("EditorWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child GDScript_Shell.EditorWindow.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.button1 = new global::Gtk.Button();
			this.button1.CanFocus = true;
			this.button1.Name = "button1";
			this.button1.UseUnderline = true;
			this.button1.Label = global::Mono.Unix.Catalog.GetString("preferences");
			this.vbox1.Add(this.button1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.button1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.views = new global::Gtk.Notebook();
			this.views.CanFocus = true;
			this.views.Name = "views";
			this.views.CurrentPage = 0;
			this.vbox1.Add(this.views);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.views]));
			w2.Position = 1;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.vbox1.Add(this.label3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.label3]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
			this.button1.Clicked += new global::System.EventHandler(this.OnButton1Clicked);
		}
	}
}
