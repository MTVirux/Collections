namespace Collections;

public class IconHandler
{
    private int iconId { get; init; }
    public IconHandler(int iconId)
    {
        this.iconId = iconId;
    }

    public ISharedImmediateTexture GetIcon()
    {
        // TODO attempt reload after some time
        return GetIcon(iconId, false);
    }

    public static ISharedImmediateTexture GetIcon(int iconId, bool hq = false)
    {
        var lookup = new GameIconLookup(iconId: (uint)iconId, itemHq: hq);
        return Services.TextureProvider.GetFromGameIcon(lookup);
    }

    private static string getIconPath(int iconId, bool hq)
    {
        var lang = "";
        return string.Format("ui/icon/{0:D3}000/{1}{2:D6}_hr1.tex",
            iconId / 1000, (hq ? "hq/" : "") + lang, iconId);
    }
}
