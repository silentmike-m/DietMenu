namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class CoreEntity
{
    public CoreEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public Dictionary<string, string> Versions { get; set; } = new();

    public string this[string name]
    {
        get
        {
            if (!this.Versions.ContainsKey(name))
            {
                this.Versions.Add(name, string.Empty);
            }

            return this.Versions[name];
        }
        set => this.Versions[name] = value;
    }
}
