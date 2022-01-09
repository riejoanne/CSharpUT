using System;
using System.Collections.Generic;

namespace Lib
{
    public class AuthenticationService
    {
        private readonly IProfile _profile;
        private readonly IToken _token;
        private readonly INotification _notification;

        public AuthenticationService(IProfile profile, IToken token, INotification notification)
        {
            _profile = profile;
            _token = token;
            _notification = notification;
        }

        public bool IsValid(string account, string password)
        {
            // 根據 account 取得自訂密碼
            var passwordFromDao = _profile.GetPassword(account);

            // 根據 account 取得 RSA token 目前的亂數
            var randomCode = _token.GetRandom(account);

            // 驗證傳入的 password 是否等於自訂密碼 + RSA token亂數
            var validPassword = passwordFromDao + randomCode;
            var isValid = password == validPassword;

            if (isValid)
                return true;
            else
            {
                _notification.Send($"Account:{account} try to login failed.");
                return false;
            }
        }
    }

    public interface INotification
    {
        void Send(string msg);
    }

    internal class FakeProfileDao : IProfile
    {
        public string GetPassword(string account)
        {
            if (account == "joey")
                return "91";
            return "";
        }
    }

    internal class FakeTokenDao : IToken
    {
        public string GetRandom(string account)
        {
            return "000000";
        }
    }

    public class ProfileDao : IProfile
    {
        public string GetPassword(string account)
        {
            return Context.GetPassword(account);
        }
    }

    public static class Context
    {
        public static Dictionary<string, string> profiles;

        static Context()
        {
            profiles = new Dictionary<string, string>();
            profiles.Add("joey", "91");
            profiles.Add("mei", "99");
        }

        public static string GetPassword(string key)
        {
            return profiles[key];
        }
    }

    public class RsaTokenDao : IToken
    {
        public string GetRandom(string account)
        {
            var seed = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var result = seed.Next(0, 999999).ToString("000000");
            Console.WriteLine("randomCode:{0}", result);

            return result;
        }
    }
}