using FFXIVClientStructs.FFXIV.Client.Game;

namespace Collections;

public class QuestExecutor
{
    // wrapper for QuestManager
    public static unsafe bool IsQuestComplete(uint questId)
    {
        return QuestManager.IsQuestComplete(questId);
    }

    // returns whether a quest is an event quest or not
    public static bool IsEventQuest(Quest quest)
    {
        // Okay to keep in english as quest ID's are consistent across all language versions
        return quest.Id.ToString().StartsWith("Fes");
    }
}