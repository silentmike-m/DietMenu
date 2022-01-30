namespace SilentMike.DietMenu.Auth.UnitTests.Services;

using System;
using Moq;

internal class FakeUserManagerBuilder
{
    private readonly Mock<FakeUserManager> mock = new();

    public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> action)
    {
        action(this.mock);
        return this;
    }

    public Mock<FakeUserManager> Build() => this.mock;
}
