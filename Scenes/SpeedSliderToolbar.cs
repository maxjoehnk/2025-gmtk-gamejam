using Godot;

public partial class SpeedSliderToolbar : HBoxContainer
{
    private Label SpeedLabel => GetNode<Label>("Label");
    private HSlider SpeedSlider => GetNode<HSlider>("HSlider");


    public override void _Ready()
    {
        SpeedLabel.Text = SpeedSlider.Value.ToString() + "x";
    }

    public void OnSpeedSliderChanged(float value)
    {
        SpeedLabel.Text = SpeedSlider.Value.ToString() + "x";
    }
}
