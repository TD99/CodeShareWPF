using System.Linq;
using Newtonsoft.Json;

namespace CodeShare.MVVM.Model
{
    public class Language
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string? DisplayName => Aliases?.First();

        [JsonProperty("extensions")]
        public string[]? Extensions { get; set; }

        [JsonProperty("aliases")]
        public string[]? Aliases { get; set; }

        [JsonProperty("mimetypes")]
        public string[]? MimeTypes { get; set; }

        public Language(string id, string[]? extensions = null, string[]? aliases = null, string[]? mimeTypes = null)
        {
            Id = id;
            Extensions = extensions;
            Aliases = aliases;
            MimeTypes = mimeTypes;
        }
    }
}
