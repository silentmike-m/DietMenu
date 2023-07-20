namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Moq;

internal class FakeUserManagerBuilder
{
    private readonly Mock<FakeUserManager> mock = new();

    public Mock<FakeUserManager> Build() => this.mock;

    public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> action)
    {
        action(this.mock);

        return this;
    }
}
