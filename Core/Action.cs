using Godot;

namespace gmtkgamejam.Core;

public abstract partial class Action : Control
{
	private int _ticks = 1;

	public int Ticks 
	{ 
		get => _ticks;
		set { _ticks = Mathf.Clamp(value, 1, 6);
			UpdateTickDisplay();
		}
	}
	
	public abstract string Title { get; }
	
	public abstract void Act(Player player);

	protected Button tickButton;

	public void SetTickButton(Button button)
	{
		tickButton = button;
		UpdateTickDisplay();
	}

	private void UpdateTickDisplay()
	{
		if (tickButton != null)
			tickButton.Text = _ticks.ToString();
	}
}
