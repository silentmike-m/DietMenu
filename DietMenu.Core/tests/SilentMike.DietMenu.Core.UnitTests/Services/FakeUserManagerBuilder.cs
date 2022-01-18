namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using Moq;

internal sealed class FakeUserManagerBuilder
{
    private readonly Mock<FakeUserManager> mock = new();

    public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> action)
    {
        action(this.mock);
        return this;
    }

    public Mock<FakeUserManager> Build() => this.mock;
}
