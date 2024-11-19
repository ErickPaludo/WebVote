using System.Text.Json.Serialization;

namespace WebVote.Models
{
    public class ZonasModelSend
    {
        [JsonPropertyName("idZona")]
        public string? idZona { get; set; } 
        [JsonPropertyName("secoes")]
        public List<Secoes> secoes { get; set; }
    }
    public class Secoes
    {
        [JsonPropertyName("idSecao")]
        public string? idSecao { get; set; }
        [JsonPropertyName("quantidadeEleitores")]
        public int quantidadeEleitoresZona { get; set; }
    }
}
