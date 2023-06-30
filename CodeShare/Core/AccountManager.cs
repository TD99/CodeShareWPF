using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CodeShare.MVVM.Model;
using CodeShare.Properties;
using CodeShare.Windows;
using Newtonsoft.Json;

namespace CodeShare.Core
{
    public static class AccountManager
    {
        private static readonly Dictionary<string, (string Message, bool IsSuccessful)> Msg = new()
        {
            { "UserNotFound", ("The specified user could not be found.", false) },
            { "PasswordWrong", ("The password entered is incorrect.", false) },
            { "UnknownError", ("An unknown error occurred. Please try again later.", false) },
            { "Successful", ("You have successfully logged in.", true) }
        };

        public static async Task<(string, bool)> Login(string username, string password)
        {
            var user = await ApiConnect.GetUserByUsername(username);
            if (user == null) return Msg["UserNotFound"];
            if (password != user.Password) return Msg["PasswordWrong"];

            Settings.Default.CurrentUser = JsonConvert.SerializeObject(user);
            Settings.Default.Save();
            return Msg["Successful"];
        }

        public static void Logout()
        {
            Settings.Default.CurrentUser = string.Empty;
            Settings.Default.Save();

            if (App.ConfigWindow.IsLoaded)
                App.OpenConfigWindow(new ConfigWindow());
        }

        public static async Task<bool> Delete()
        {
            return await ApiConnect.DeleteUser(GetCurrentUser()?.Id);
        }

        public static async Task<bool> Create(User user)
        {
            return await ApiConnect.PostUser(user);
        }

        public static async Task<bool> Update(User user)
        {
            return await ApiConnect.PutUser(user);
        }

        public static User? GetCurrentUser()
        {
            return JsonConvert.DeserializeObject<User?>(Settings.Default.CurrentUser);
        }

        public static async Task<List<string>?> GetAllUserNames()
        {
            var users = await ApiConnect.GetUsers();
            return users?.Select(user => user.Username).ToList();
        }
    }
}
