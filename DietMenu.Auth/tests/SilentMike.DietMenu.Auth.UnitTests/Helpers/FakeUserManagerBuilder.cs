namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

internal class FakeUserManagerBuilder
{
    private readonly FakeUserManager mock = Substitute.For<FakeUserManager>();

    public FakeUserManager Build() => this.mock;

    public FakeUserManagerBuilder With(Action<FakeUserManager> action)
    {
        action(this.mock);

        return this;
    }
}
