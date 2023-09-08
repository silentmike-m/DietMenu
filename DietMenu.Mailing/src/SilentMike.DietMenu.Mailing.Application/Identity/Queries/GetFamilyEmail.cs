namespace SilentMike.DietMenu.Mailing.Application.Identity.Queries;

public sealed record GetFamilyEmail(Guid FamilyId) : IRequest<string>;
