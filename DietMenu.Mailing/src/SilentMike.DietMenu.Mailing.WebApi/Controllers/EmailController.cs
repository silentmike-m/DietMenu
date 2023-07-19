namespace SilentMike.DietMenu.Mailing.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;

[ApiController, Route("[controller]/[action]")]
public sealed class EmailController : ApiControllerBase
{
    private readonly EmailFactory emailFactory;

    public EmailController(EmailFactory emailFactory)
        => this.emailFactory = emailFactory;

    [HttpPost(Name = "SendEmail")]
    public async Task<ActionResult> SendEmail(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        var email = await this.emailFactory.CreateEmailAsync(request.Receiver, request.Subject, request.Text, cancellationToken);

        var newRequest = new SendEmail
        {
            Email = email,
        };

        await this.Mediator.Send(newRequest, cancellationToken);

        return await Task.FromResult(this.Ok());
    }
}
