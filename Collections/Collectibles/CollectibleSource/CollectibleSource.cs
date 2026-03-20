
namespace Collections;

public abstract class CollectibleSource : ICollectibleSource
{
    public abstract string GetName();
    public abstract List<SourceCategory> GetSourceCategories();
    public abstract bool GetIslocatable();
    public abstract void DisplayLocation();

    protected abstract int GetIconId();

    public CollectibleSource()
    {
    }

    public ISharedImmediateTexture GetIcon()
    {
        return IconHandler.GetIcon(GetIconId(), false);
    }

    public abstract CollectibleSource Clone();

    ICollectibleSource ICollectibleSource.Clone()
    {
        return Clone();
    }
}
