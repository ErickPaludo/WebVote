using System.Text.Json.Serialization;

namespace WebVote.Models
{
    public class ResponseModelSend
    {
        public int code { get; set; }
        public string message { get; set; }
    }
    public class ResponseSecoes
    {
        [JsonPropertyName("totalSecoes")]
        public int totalSecoes { get; set; }
        [JsonPropertyName("secoesImportadas")]
        public int secoesImportadas { get; set; }
        [JsonPropertyName("totalEleitoresPresentes")]
        public int totalEleitoresPresentes { get; set; }
        [JsonPropertyName("percentualPresentes")]
        public float percentualPresentes { get; set; }
        [JsonPropertyName("totalAbstencoes")]
        public int totalAbstencoes { get; set; }
        [JsonPropertyName("percentualAbstencoes")]
        public float percentualAbstencoes { get; set; }
    }
}
