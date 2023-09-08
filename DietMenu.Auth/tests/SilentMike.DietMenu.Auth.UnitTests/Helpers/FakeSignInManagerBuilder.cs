namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

internal sealed class FakeSignInManagerBuilder
{
    private readonly FakeSignInManager mock;

    public FakeSignInManagerBuilder(FakeUserManager userManager)
        => this.mock = Substitute.For<FakeSignInManager>(userManager);

    public FakeSignInManager Build() => this.mock;

    public FakeSignInManagerBuilder With(Action<FakeSignInManager> action)
    {
        action(this.mock);

        return this;
    }
}
