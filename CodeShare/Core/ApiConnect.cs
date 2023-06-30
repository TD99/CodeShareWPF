using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CodeShare.MVVM.Model;
using CodeShare.Properties;
using Newtonsoft.Json;

namespace CodeShare.Core
{
    public static class ApiConnect
    {
        public static readonly string ApiUrl = Settings.Default.ServerUrl;
        public static readonly string UserUrl = @$"{ApiUrl}/User";

        public static async Task<List<User>?> GetUsers()
        {
            try
            {
                using var client = new HttpClient();

                var response = await client.GetAsync(UserUrl);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User?> GetUser(string id)
        {
            try
            {
                using var client = new HttpClient();

                var response = await client.GetAsync($"{UserUrl}/{id}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(json);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User?> GetUserByUsername(string username)
        {
            try
            {
                var users = await ApiConnect.GetUsers();
                var specificUser = users?.FirstOrDefault(u => u.Username == username);
                return specificUser;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> DeleteUser(string id)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.DeleteAsync($"{UserUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> PostUser(User user)
        {
            try
            {
                using var client = new HttpClient();
                string json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(UserUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> PutUser(User user)
        {
            try
            {
                using var client = new HttpClient();
                string json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{UserUrl}/{user.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<Snippet>?> GetSnippets()
        {
            try
            {
                using var client = new HttpClient();

                var response = await client.GetAsync($"{UserUrl}/{AccountManager.GetCurrentUser().Id}/snippets");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Snippet>>(json);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Snippet?> GetSnippetById(string id)
        {
            try
            {
                var snippets = await ApiConnect.GetSnippets();
                var specificSnippet = snippets?.FirstOrDefault(u => u.InternalId == id);
                return specificSnippet;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> PostSnippet(Snippet snippet)
        {
            try
            {
                Clipboard.SetText(JsonConvert.SerializeObject(snippet));
                using var client = new HttpClient();
                string json = JsonConvert.SerializeObject(snippet);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{UserUrl}/{snippet.UserId}/snippets", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false; 
            }
        }

        public static async Task<bool> DeleteSnippet(Snippet snippet)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.DeleteAsync($"{UserUrl}/{snippet.UserId}/snippets/{snippet.InternalId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> PutSnippet(Snippet snippet)
        {
            try
            {
                using var client = new HttpClient();
                string json = JsonConvert.SerializeObject(snippet);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response =
                    await client.PutAsync($"{UserUrl}/{snippet.UserId}/snippets/{snippet.InternalId}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
