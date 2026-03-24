
using Collections;

public class CollectibleListFilterOption<T> : CollectibleFilterOption where T : struct
{
    /// <summary>
    /// Represents a dropdown list for filtering collectibles
    /// </summary>
    /// <param name="Name"> name of the filter option </param>
    /// <param name="Property"> function to determine the field that should be used for comparison
    /// <param name="Options"> items that can be selected from
    /// <param name="ItemLabels"> custom labels for each option. Discarded if 1 + length does not match options
    /// <param name="Description"> optional description of the filter option. </param>
    /// <param name="Enabled"> whether this filter should be used.
    /// <param name="Selection"> Default option for the first comparison</param>
    public CollectibleListFilterOption(
        string Name,
        Func<(ICollectible, T), bool> Filter,
        List<T> Options,
        List<string>? ItemLabels = null,
        string Description = "",
        bool Enabled = false) : base(Name, c => false, Description, Enabled)
    {
        this.Options = Options;
        this.Filter = c => Filter((c, Options[selectedOpt]));
        if(ItemLabels != null && ItemLabels.Count + 1 != Options.Count)
        {
            itemLabels = ItemLabels;
        }
        else
        {
            itemLabels = Options.Select(o => o.ToString() ?? "").ToList();
            itemLabels.Insert(0, $"All {Name}");
        }
    }

    public List<T> Options { get; set; }
    private List<string> itemLabels;
    private int selectedOpt = -1;

    public override void Draw(EventService service)
    {
        if (ImGui.BeginCombo($"##filterOptionEnabled{Name}", itemLabels[selectedOpt + 1]))
        {
            // Generic 'all' option
            if(ImGui.MenuItem(itemLabels[0], !Enabled))
            {
                Enabled = false;
                selectedOpt = -1;
                service.Publish<FilterChangeEvent, FilterChangeEventArgs>(new FilterChangeEventArgs());
            }
            for(int i = 0; i < Options.Count; i++)
            {
                if(ImGui.MenuItem(itemLabels[i + 1], Enabled && i == selectedOpt))
                {
                    Enabled = true;
                    selectedOpt = i;
                    service.Publish<FilterChangeEvent, FilterChangeEventArgs>(new FilterChangeEventArgs());
                }
            }
            ImGui.EndCombo();
        }
    }
}