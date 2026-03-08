using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CalendarWinUI3.Models
{

    public class HuangLiDto
    {
        [JsonPropertyName("solar")]
        public string Solar { get; set; }

        [JsonPropertyName("lunar")]
        public string Lunar { get; set; }

        [JsonPropertyName("ganzhi")]
        public string Ganzhi { get; set; }

        [JsonPropertyName("yi")]
        public List<string> Yi { get; set; }

        [JsonPropertyName("ji")]
        public List<string> Ji { get; set; }

        [JsonPropertyName("jieqi")]
        public string Jieqi { get; set; }
    }
}
