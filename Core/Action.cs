using Godot;

namespace gmtkgamejam.Core;

public abstract partial class Action : Control
{
    public int Ticks { get; set; }
    
    public abstract string Title { get; }
    
    public abstract void Act();
}