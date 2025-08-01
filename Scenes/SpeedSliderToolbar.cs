using Godot;

public partial class SpeedSliderToolbar : HBoxContainer
{
	private Label SpeedLabel => GetNode<Label>("Label");
	private HSlider SpeedSlider => GetNode<HSlider>("HSlider");

	public override void _Ready()
	{
		this.UpdateLabel();
	}

	private void UpdateLabel()
	{
		this.SpeedLabel.Text = $"{this.SpeedSlider.Value}x";
	}

	public void OnSpeedSliderChanged(float value)
	{
		this.UpdateLabel();
	}
}