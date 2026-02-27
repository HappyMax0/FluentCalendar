using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CalendarWinUI3.Models
{

    public class HolidayData
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        [JsonPropertyName("$id")]
        public string Id { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("papers")]
        public List<string> Papers { get; set; }

        [JsonPropertyName("days")]
        public List<HolidayDay> Days { get; set; }
    }

    public class HolidayDay
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("isOffDay")]
        public bool IsOffDay { get; set; }
    }
}
