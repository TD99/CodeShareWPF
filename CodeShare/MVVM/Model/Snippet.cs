using Newtonsoft.Json;

namespace CodeShare.MVVM.Model
{
    public class Snippet
    {
        [JsonProperty ("id")]
        public string InternalId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        public Snippet(string internalId, string userId, string? content, string? title, string? language, string? createdAt)
        {
            InternalId = internalId;
            UserId = userId;
            Content = content;
            Title = title;
            Language = language;
            CreatedAt = createdAt;
        }
    }
}
