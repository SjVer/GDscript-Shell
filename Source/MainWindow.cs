using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButtonFileClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonEditClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonShellClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonDebugClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonOptionsClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonWindowClicked(object sender, EventArgs e)
	{
	}

	protected void OnButtonHelpClicked(object sender, EventArgs e)
	{
	}
}
