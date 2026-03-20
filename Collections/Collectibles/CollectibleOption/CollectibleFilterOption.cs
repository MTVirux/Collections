using Collections;

public abstract class CollectibleFilterOption
{
    /// <summary>
    /// Represents an option for filtering collectibles
    /// </summary>
    /// <param name="Name"> name of the filter option </param>
    /// <param name="Filter"> function to determine the field that should be used for comparison
    /// this should return true if the collectible should be filtered. </param>
    /// <param name="Description"> optional description of the filter option. </param>
    /// <param name="Selection"> Default option for the first comparison</param>
    public CollectibleFilterOption(string Name, Func<ICollectible, bool> Filter, string Description = "", bool Enabled = false)
    {
        this.Name = Name;
        this.Description = Description;
        this.Filter = Filter;
        this.Enabled = Enabled;
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public Func<ICollectible, bool> Filter { get; set; }

    public bool Enabled { get; set; }
    public virtual bool IsFiltered(ICollectible collectible)
    {
        return Enabled && Filter(collectible);
    }

    public virtual IEnumerable<ICollectible> FilterCollection(IEnumerable<ICollectible> collection)
    {
        return collection.AsParallel().Where(c => !Filter(c));
    }

    // Try to assume everything is on the same line
    public virtual unsafe void Draw(EventService service)
    {
    }
}