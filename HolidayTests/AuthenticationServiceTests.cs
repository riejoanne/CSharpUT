using Lib;
using NSubstitute;
using NUnit.Framework;

namespace HolidayTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [SetUp]
        public void Setup()
        {
            _profile = Substitute.For<IProfile>();
            _token = Substitute.For<IToken>();
            _notification = Substitute.For<INotification>();
            _authenticationService = new AuthenticationService(_profile, _token, _notification);
        }

        private IProfile _profile;
        private IToken _token;
        private INotification _notification;
        private AuthenticationService _authenticationService;

        [Test]
        public void should_not_notify_when_valid()
        {
            WhenValid("joey");
            ShouldNotNotifyUser();
        }


        [Test]
        public void is_invalid()
        {
            WhenValid("joey");
            ShouldBeInvalid("joey", "wrong password");
        }

        [Test]
        public void should_notify_user_when_invalid()
        {
            WhenInvalid("joey");
            ShouldNotifyUser("joey", "login failed");
        }

        private void ShouldNotifyUser(string account, string status)
        {
            _notification.Received(1).Send(Arg.Is<string>(s => s.Contains(account) && s.Contains(status)));
        }

        private void ShouldNotNotifyUser()
        {
            _notification.DidNotReceiveWithAnyArgs().Send("");
        }

        private void WhenInvalid(string account)
        {
            GivenPassword(account, "91");
            GivenToken("000000");
            _authenticationService.IsValid(account, "wrong password");
        }


        private void WhenValid(string account)
        {
            GivenPassword(account, "91");
            GivenToken("000000");
            _authenticationService.IsValid(account, "91000000");
        }

        private void ShouldBeInvalid(string account, string password)
        {
            var actual = _authenticationService.IsValid(account, password);
            //always failed
            Assert.IsFalse(actual);
        }

        private void ShouldBeValid(string account, string password)
        {
            var actual = _authenticationService.IsValid(account, password);
            Assert.IsTrue(actual);
        }

        private void GivenToken(string token)
        {
            _token.GetRandom("").ReturnsForAnyArgs(token);
        }

        private void GivenPassword(string account, string password)
        {
            _profile.GetPassword(account).Returns(password);
        }
    }
}