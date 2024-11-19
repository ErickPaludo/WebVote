using System.Text.Json.Serialization;

namespace WebVote.Entidades;

public class Candidatos
{
    [JsonPropertyName("nomeCandidato")]
    public string? nomeCandidato { get; set; }
    [JsonPropertyName("quantidadeVotos")]
    public int quantidadeVotos { get; set; }
}

public class CandidatoModelSendList
{
    [JsonPropertyName("idZona")]
    public string? idZona { get; set; }
    [JsonPropertyName("idSecao")]
    public int idSecao { get; set; }
    [JsonPropertyName("quantidadePresentes")]
    public int quantidadePresentes { get; set; }
    [JsonPropertyName("votosValidos")]
    public int votosValidos { get; set; }
    [JsonPropertyName("nomeCandidato")]
    public List<Candidatos>? candidatos { get; set; }
}

