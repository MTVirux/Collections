using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace Collections;

public class MountCollectible : Collectible<Mount>, ICreateable<MountCollectible, Mount>
{
    public static string CollectionName => "Mounts";

    public MountCollectible(Mount excelRow) : base(excelRow)
    {
        SortOptions.Add(new CollectibleSortOption("Seats", (c) => c is MountCollectible ? ((MountCollectible)c).ExcelRow.ExtraSeats : -1));
    }

    public static MountCollectible Create(Mount excelRow)
    {
        return new(excelRow);
    }

    protected override string GetCollectionName()
    {
        return CollectionName;
    }

    protected override string GetName()
    {
        return ExcelRow.Singular.ToString();
    }

    protected override uint GetId()
    {
        return ExcelRow.RowId;
    }

    protected override string GetDescription()
    {
        return ExcelCache<MountTransient>.GetSheet().GetRow(ExcelRow.RowId)?.Description.ToString() ?? "";
    }

    protected override HintModule GetSecondaryHint()
    {
        return new HintModule((ExcelRow.ExtraSeats + 1).ToString(), FontAwesomeIcon.PeopleGroup);
    }

    public override void UpdateObtainedState()
    {
        isObtained = Services.UnlockState.IsMountUnlocked(ExcelRow);
    }

    protected override int GetIconId()
    {
        return ExcelRow.Icon;
    }

    private int GetImageId()
    {
        return 64000 + ExcelRow.Icon; 
    }

    public override unsafe void Interact()
    {
        if (isObtained)
            ActionManager.Instance()->UseAction(ActionType.Mount, ExcelRow.RowId);
    }

    public override void DrawAdditionalTooltip()
    {
        // ImGui.Text(ExcelCache<MountTransient>.GetSheet().GetRow(ExcelRow.RowId).Value.DescriptionEnhanced.ToString());
        var pic = Services.TextureProvider.GetFromGameIcon(new GameIconLookup((uint)GetImageId()));
        ImGui.Image(pic.GetWrapOrEmpty().Handle, pic.GetWrapOrEmpty().Size * 0.75f);
    }

    public override void OpenGamerEscape()
    {
        WikiOpener.OpenGamerEscape(GetDisplayName() + "_(Mount)");
    }
}
