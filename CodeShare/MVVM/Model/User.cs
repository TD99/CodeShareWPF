using Newtonsoft.Json;

namespace CodeShare.MVVM.Model
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("githubName")]
        public string GithubName { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        public User(string id, string username, string githubName, string region, string password, string createdAt)
        {
            Id = id;
            Username = username;
            GithubName = githubName;
            Region = region;
            Password = password;
            CreatedAt = createdAt;
        }
    }
}
