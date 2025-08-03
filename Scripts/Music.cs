using Godot;
using System;

public partial class Music : AudioStreamPlayer
{
	private const int MenuAudioFileIndex = 0;
	private const int GameAudioFileIndex = 1;

	private const float VolumeOff = -60f;
	private const float VolumeOn = -5f;
	
	private const float TransitionDuration = 1f;
	
	public static Music Instance { get; private set; } = null!;

	private AudioStreamSynchronized AudioFiles => (this.Stream as AudioStreamSynchronized)!;

	private Tween? tween;

	[Export]
	public float MenuVolume
	{
		get => this.AudioFiles.GetSyncStreamVolume(MenuAudioFileIndex);
		set => this.AudioFiles.SetSyncStreamVolume(MenuAudioFileIndex, value);
	}

	[Export]
	public float GameVolume
	{
		get => this.AudioFiles.GetSyncStreamVolume(GameAudioFileIndex);
		set => this.AudioFiles.SetSyncStreamVolume(GameAudioFileIndex, value);
	}

	public override void _Ready()
	{
		Instance = this;
		this.tween = this.CreateTween();
		this.tween.TweenProperty(this, "volume_linear", 1, TransitionDuration);
	}

	public override void _EnterTree()
	{
		this.VolumeLinear = 0;
	}

	public void TransitionToMenuMusic()
	{
		this.tween = this.CreateTween();
		this.tween.TweenProperty(this, nameof(this.GameVolume), VolumeOff, 1f);
	}
	
	public void TransitionToGameMusic()
	{
		this.tween = this.CreateTween();
		this.tween.TweenProperty(this, nameof(this.GameVolume), VolumeOn, 0.5f).SetEase(Tween.EaseType.Out);
	}
}
