using Godot;

namespace gmtkgamejam.Core;

public abstract partial class Action : GodotObject
{
    public int Ticks { get; set; }
    
    public abstract string Title { get; }
}