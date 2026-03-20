using Dalamud.Utility;

namespace Collections;

public class JobSelectorWidget
{
    public Dictionary<ClassJob, bool> Filters;

    private const int JobIconScale = 7;
    private int iconSize = 25;
    private Vector2 overrideItemSpacing = new(2, 1);

    // Specific filter for "All Classes" items
    private bool allClasses = true;
    // Specific filter for "Job-Specific" items
    private bool jobSpecific = true;

    private EventService EventService { get; init; }
    public JobSelectorWidget(EventService eventService)
    {
        EventService = eventService;
        var classJobs = Services.DataProvider.SupportedClassJobs;
        Filters = classJobs.ToDictionary(entry => entry, entry => true);
        roles = classJobs.GroupBy(entry =>
        {
            // UI Priority to the rescue
            // specifically want to truncate anything below 10
            // This will probably have to be updated to be more complex
            // once they introduce a new melee class (Viper is at UI prio 39 atm)
            return (uint)entry.UIPriority / 10;
        }).OrderBy(entry => entry.Key).ToDictionary(entry => entry.Key, entry => entry.OrderBy(job => job.UIPriority).ToList());
    }

    public void Draw()
    {
        // Draw Buttons
        ImGui.PushStyleColor(ImGuiCol.Button, Services.WindowsInitializer.MainWindow.originalButtonColor);
        if (ImGui.Button("Enable All"))
        {
            SetAllState(true, true);
        }
        ImGui.SameLine();
        if (ImGui.Button("Disable All"))
        {
            SetAllState(false, true);
        }
        ImGui.SameLine();
        if (ImGui.Button("Current Job"))
        {
            SetCurrentJob();
        }
        ImGui.PopStyleColor();

        // Draw job icons
        JobSelector();
    }

    private Dictionary<uint, List<ClassJob>> roles;
    private void JobSelector()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, overrideItemSpacing);
        foreach (var classes in roles.Values)
        {
            foreach (var job in classes)
            {
                var icon = job.GetIcon();
                if (icon != null)
                {
                    // Button
                    UiHelper.ImageToggleButton(icon, new Vector2(iconSize, iconSize) * (ImGui.GetFontSize() / 14), Filters[job]);

                    // Left click - Switch to selection
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                    {
                        var newState = IsAllActive() ? true : !Filters[job];
                        SetAllState(false, false);
                        Filters[job] = newState;
                        PublishFilterChangeEvent();
                    }

                    // Right click - Toggle selection
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    {
                        Filters[job] = !Filters[job];
                        PublishFilterChangeEvent();
                    }
                    if(ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text(job.Name.ToString().FirstCharToUpper());
                        ImGui.EndTooltip();
                    }
                }
                ImGui.SameLine();
            }
            // lets us not have to do logic around ImGui.SameLine()
            ImGui.Text("");
        }
        // "All Classes" Icon Button
        UiHelper.ImageToggleButton(IconHandler.GetIcon(62522), new Vector2(iconSize, iconSize) * (ImGui.GetFontSize() / 14), allClasses);
        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
        {
            var newState = IsAllActive() ? true : !allClasses;
            SetAllState(false, false);
            allClasses = newState;
            PublishFilterChangeEvent();
        }
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        {
            allClasses = !allClasses;
            PublishFilterChangeEvent();
        }
        if(ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text("All Classes");
            ImGui.EndTooltip();
        }
        // Job-specific gear (Artifact basically)
        ImGui.SameLine();
        UiHelper.ImageToggleButton(IconHandler.GetIcon(62521), new Vector2(iconSize, iconSize) * (ImGui.GetFontSize() / 14), jobSpecific);
        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
        {
            var newState = IsAllActive() ? true : !jobSpecific;
            SetAllState(false, false);
            jobSpecific = newState;
            PublishFilterChangeEvent();
        }
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        {
            jobSpecific = !jobSpecific;
            PublishFilterChangeEvent();
        }
        if(ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text("Artifact Gear");
            ImGui.EndTooltip();
        }
        ImGui.PopStyleVar();
    }

    private void SetAllState(bool state, bool publishEvent)
    {
        Filters = Filters.ToDictionary(e => e.Key, e => state);
        allClasses = state;
        jobSpecific = state;
        if (publishEvent)
            PublishFilterChangeEvent();
    }

    private void SetCurrentJob()
    {
        var matchingClassJob = Filters.Where(e => e.Key.RowId == Services.PlayerState.ClassJob.RowId);
        if (matchingClassJob.Any())
        {
            SetAllState(false, false);
            Filters[matchingClassJob.First().Key] = true;
            PublishFilterChangeEvent();
        }
    }

    private bool IsAllActive()
    {
        return !Filters.Any(e => e.Value == false) && allClasses && jobSpecific;
    }

    public bool AllClasses()
    {
        return allClasses;
    }

    public bool JobSpecific()
    {
        return jobSpecific;
    }

    private void PublishFilterChangeEvent()
    {
        EventService.Publish<FilterChangeEvent, FilterChangeEventArgs>(new FilterChangeEventArgs());
    }
}

