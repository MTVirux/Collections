namespace Collections;

public class CurrencyDataGenerator
{
    // manual information for item categories
    // if there's an in-data identifier, use that instead.
    public Dictionary<uint, SourceCategory> ItemIdToSourceCategory = new()
    {
        { 00001, SourceCategory.Gil }, // Gil
        { 00020, SourceCategory.CompanySeals }, // Storm Seal (designated company seals)
        { 00025, SourceCategory.PvP }, // Wolf Mark
        { 21067, SourceCategory.PvP }, // Wolf Collar
        { 36656, SourceCategory.PvP }, // Trophy Crystals
        { 30341, SourceCategory.Duty }, // Faux Leaves
        
        { 00027, SourceCategory.TheHunt }, // Allied Seal
        { 10307, SourceCategory.TheHunt }, // Centurio Seal
        { 26533, SourceCategory.TheHunt }, // Sack of nuts
        { 26807, SourceCategory.Fate}, // Bicolor gems
        { 00029, SourceCategory.MGP }, // MGP
        { 41629, SourceCategory.MGP}, // MGF (fall guys)
        { 28063, SourceCategory.RestorationZone}, // Skybuilder Scrips
        { 47343, SourceCategory.RestorationZone}, // Phaenna token booklet
        { 47594, SourceCategory.RestorationZone}, // Phaenna exploration token 
        // Occult Crescent
        { 47868, SourceCategory.FieldOperations}, // Sanguinite
    };

    public CurrencyDataGenerator()
    {
        PopulateData();
    }

    private void PopulateData()
    {
        // Add Tomestones
        var TomestonesItemSheet = ExcelCache<TomestonesItem>.GetSheet();
        foreach (var tomestone in TomestonesItemSheet)
        {
            ItemIdToSourceCategory[tomestone.Item.RowId] = SourceCategory.Tomestones;
        }

        // Add Beast tribe currencies
        var beastTribeSheet = ExcelCache<BeastTribe>.GetSheet();
        foreach (var beastTribe in beastTribeSheet)
        {
            ItemIdToSourceCategory[beastTribe.CurrencyItem.RowId] = SourceCategory.BeastTribes;
        }

        // generate currency items where we know categories
        var ItemSheet = ExcelCache<Item>.GetSheet();
        foreach (var item in ItemSheet)
        {
            if(ItemIdToSourceCategory.ContainsKey(item.RowId)) continue;
            switch (item.ItemSortCategory.RowId)
            {
                case 0:
                    // Cosmic Fortune credits (not booklets)
                    if(item.FilterGroup == 55 || item.FilterGroup == 56)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.RestorationZone;
                    // Island Sanctuary Cowries
                    else if(item.FilterGroup == 47)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.IslandSanctuary;
                    break;
                // Deep Dungeon Currency items
                case 41:
                    ItemIdToSourceCategory[item.RowId] = SourceCategory.DeepDungeon;
                    break;
                // Exploration Zones
                case 44: // Eureka
                case 48: // Bozja
                case 86: // Occult Crescent
                    ItemIdToSourceCategory[item.RowId] = SourceCategory.FieldOperations;
                    break;
                // FATEs
                case 55: 
                    // FATE tokens
                    if(item.Unknown4 == 24000)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.Fate;
                    // EX Totems + Chaotic
                    else if(item.Unknown4 == 10000 || item.Unknown4 == 3000)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.Duty;
                    // Crafter/Gatherer tokens
                    else if(item.Unknown4 == 17000)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.Scrips;
                    // // Moogle Tomestone Events
                    // else if(item.Unknown4 == 22000 || item.Unknown4 == 22001)
                    //     ItemIdToSourceCategory[item.RowId] = SourceCategory.Event;
                    // Cosmic Exploration Booklets
                    else if((item.Unknown4 == 0 || item.Unknown4 == 1 || item.Unknown4 == 5) && item.Lot == false)
                        ItemIdToSourceCategory[item.RowId] = SourceCategory.RestorationZone;
                    // Arch
                    break;
                
            }
        }
    }
}
