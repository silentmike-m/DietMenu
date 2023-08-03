namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Moq;

internal sealed class FakeSignInManagerBuilder
{
    private readonly Mock<FakeSignInManager> mock;

    public FakeSignInManagerBuilder(FakeUserManager userManager)
        => this.mock = new Mock<FakeSignInManager>(userManager);

    public Mock<FakeSignInManager> Build() => this.mock;

    public FakeSignInManagerBuilder With(Action<Mock<FakeSignInManager>> action)
    {
        action(this.mock);

        return this;
    }
}
