namespace SilentMike.DietMenu.Shared.MassTransit;

public interface ISendCreatedUserMessage
{
    public string Email { get; set; }
    public string FamilyName { get; set; }
    public string LoginUrl { get; set; }
    public string UserName { get; set; }
}
