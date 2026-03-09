using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace Collections;


public class EmoteCollectible : Collectible<Emote>, ICreateable<EmoteCollectible, Emote>
{
    public new static string CollectionName => "Emotes";

    public EmoteCollectible(Emote excelRow) : base(excelRow)
    {
    }

    public static EmoteCollectible Create(Emote excelRow)
    {
        return new(excelRow);
    }

    protected override string GetCollectionName()
    {
        return CollectionName;
    }

    protected override string GetName()
    {
        return ExcelRow.Name.ToString();
    }

    protected override uint GetId()
    {
        return ExcelRow.UnlockLink;
    }

    protected override string GetDescription()
    {
        return "";
    }

    public override unsafe void UpdateObtainedState()
    {
        isObtained = UIState.Instance()->IsEmoteUnlocked((ushort)ExcelRow.RowId);
    }

    protected override decimal GetPatchAdded()
    {
        // emotes now have patch data baked in! Yay!
        if(ExcelRow.Patch > 1)
        {
            int majorPatch = ExcelRow.Patch / 100; // truncating
            int minorPatch = ExcelRow.Patch - (majorPatch * 100);
            if(minorPatch % 10 == 0) minorPatch /= 10;
            if(decimal.TryParse($"{majorPatch}.{minorPatch}", out decimal temp))
                return temp;
        }
        return base.GetPatchAdded();
    }

    protected override int GetIconId()
    {
        return (int)ExcelRow.Icon;
    }

    public override unsafe void Interact()
    {
        if(isObtained)
            EmoteManager.Instance()->ExecuteEmote((ushort)ExcelRow.RowId);
    }
}
