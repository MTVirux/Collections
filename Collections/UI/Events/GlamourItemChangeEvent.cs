namespace Collections;

public class GlamourItemChangeEventArgs : EventArgs
{
    public GlamourCollectible Collectible { get; init; }
    public bool ApplyToSlot { get; init; }
    public GlamourItemChangeEventArgs(GlamourCollectible collectible, bool applyToSlot = false)
    {
        Collectible = collectible;
        ApplyToSlot = applyToSlot;
    }
}

public class GlamourItemChangeEvent : Event<GlamourItemChangeEventArgs>
{
}


