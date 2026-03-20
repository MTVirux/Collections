using Collections;

public class CollectibleBooleanFilterOption : CollectibleFilterOption
{
    /// <summary>
    /// Represents an boolean option for filtering collectibles
    /// </summary>
    /// <param name="Name"> name of the filter option </param>
    /// <param name="Filter"> function to determine the field that should be used for comparison
    /// this should return true if the collectible should be filtered. </param>
    /// <param name="Description"> optional description of the filter option. </param>
    /// <param name="Selection"> Default option for the first comparison</param>
    public CollectibleBooleanFilterOption(string Name, Func<ICollectible, bool> Filter, string Description = "", bool Enabled = false) : base(Name, Filter, Description, Enabled)
    {
    }

    // Try to assume everything is on the same line
    public override unsafe void Draw(EventService service)
    {
        bool ready = Enabled;
        if (ImGui.Checkbox(Name, ref ready))
        {
            Enabled = ready;
            service.Publish<FilterChangeEvent, FilterChangeEventArgs>(new FilterChangeEventArgs());
        }
    }
}