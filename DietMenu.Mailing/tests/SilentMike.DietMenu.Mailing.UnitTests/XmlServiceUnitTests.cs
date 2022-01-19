namespace SilentMike.DietMenu.Mailing.UnitTests;

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Services;

[TestClass]
public sealed class XmlServiceUnitTests
{
    [TestMethod]
    public void ShouldThrowResourceNotFoundWhenInvalidResourceName()
    {
        //GIVEN
        var resourceName = "Test";
        var service = new XmlService();

        //WHEN
        Func<string> action = () => service.GetXsltString(resourceName);

        //THEN
        action.Should()
            .ThrowExactly<ResourceNotFoundException>()
            .WithMessage($"*{resourceName}*")
            ;
    }
}
