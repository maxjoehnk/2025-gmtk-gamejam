namespace gmtkgamejam.Scripts.Core;

public interface ISwitchable
{
	public void Toggle();
	
	public bool State { get; set; }
}