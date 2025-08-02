using Godot;
using System;

public partial class LevelButton : Button
{
	public string LevelName
	{
		set => this.Text = value;
	}

	public bool IsTest
	{
		set
		{
			this.AddThemeColorOverride("font_color", value ? Colors.Red : Colors.White);
			this.AddThemeColorOverride("font_hover_color", value ? Colors.Red : Colors.White);
		}
	}
}
