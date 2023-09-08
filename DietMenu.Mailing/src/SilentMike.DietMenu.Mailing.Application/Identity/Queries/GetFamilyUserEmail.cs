namespace SilentMike.DietMenu.Mailing.Application.Identity.Queries;

public sealed record GetFamilyUserEmail(Guid FamilyId) : IRequest<string>;
