using Collections;
public static class SpecialShopExtensions
{
    public static Dictionary<uint, uint> BuildTomestones(this SpecialShop shop)
    {
        var tomestoneItems = ExcelCache<TomestonesItem>.GetSheet()
            .Where(t => t.Tomestones.RowId > 0)
            .OrderBy(t => t.Tomestones.RowId)
            .ToArray();

        var tomeStones = new Dictionary<uint, uint>();

        for (uint i = 0; i < tomestoneItems.Length; i++)
        {
            tomeStones[i + 1] = tomestoneItems[i].Item.RowId;
        }

        return tomeStones;
    }

    public static unsafe uint FixItemId(this SpecialShop shop, uint itemId, byte UseCurrencyType)
    {
        if (itemId == 0 || itemId > 7)
        {
            return itemId;
        }

        switch (UseCurrencyType)
        {
            // Scrips
            case 16:
                uint refId = FFXIVClientStructs.FFXIV.Client.Game.CurrencyManager.Instance()->GetItemIdBySpecialId((byte)itemId);
                switch (itemId)
                {
                    // Poetics
                    case 1: return 28;
                    // Last-latest Crafting Scrip
                    case 2:
                    // Last-latest Gathering Scrip
                    case 4:
                    // Latest Crafting Scrip
                    case 6:
                    // Latest Gathering Scrip
                    case 7:
                        return refId;
                    default: return itemId;
                }
            // Gil
            case 8:
                return 1;
            case 4:
                var tomestones = shop.BuildTomestones();
                return tomestones[itemId];
            case 2:
                if (itemId == 1)
                {
                    tomestones = shop.BuildTomestones();
                    return tomestones[itemId];
                }
                else
                {
                    return itemId;
                }
            default:
                return itemId;
                // Tomestones
                //case 4:
                //    if (TomeStones.ContainsKey(itemId))
                //    {
                //        return TomeStones[itemId];
                //    }

                //    return itemId;
        }
    }
}