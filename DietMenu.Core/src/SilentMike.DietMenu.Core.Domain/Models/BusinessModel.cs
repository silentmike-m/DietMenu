namespace SilentMike.DietMenu.Core.Domain.Models;

public abstract class BusinessModel
{
    public bool IsDeleted { get; private set; } = default;
    public bool IsDirty { get; private set; } = default;
    public bool IsNew { get; private set; } = default;

    public void MarkDeleted()
    {
        this.IsDeleted = true;
        this.IsDirty = false;
        this.IsNew = false;
    }

    public void MarkDirty()
    {
        this.IsDirty = true;
        this.IsNew = false;
        this.IsDeleted = false;
    }

    public void MarkNew()
    {
        this.IsDirty = false;
        this.IsDeleted = false;
        this.IsNew = true;
    }

    public void MarkOld()
    {
        this.IsDirty = false;
        this.IsNew = false;
        this.IsDeleted = false;
    }
}
