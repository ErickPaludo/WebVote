using System.Text.Json.Serialization;

namespace WebVote.Entidades;

public class CandidatosFinal : Candidatos
{

    [JsonPropertyName("percentualVotos")]
    public float percentualVotos { get; set; }
}
public class Candidatos
{
    [JsonPropertyName("nomeCandidato")]
    public string? nomeCandidato { get; set; }
    [JsonPropertyName("quantidadeVotos")]
    public int quantidadeVotos { get; set; }
    [JsonPropertyName("urlImg")]
    public string? urlImg { get; set; }
}

public class CadCandidato
{
    [JsonPropertyName("nomeCandidato")]
    public string? nomeCandidato { get; set; }
    [JsonPropertyName("urlImg")]
    public string? urlImg { get; set; }

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

public class CandidatosResult
{
    [JsonPropertyName("nomeEleicao")]
    public string nomeEleicao { get; set; }
    [JsonPropertyName("totalVotosValidos")]
    public int totalVotosValidos { get; set; }
    [JsonPropertyName("totalVopercentualVotosValidostosValidos")]
    public float percentualVotosValidos { get; set; }
    [JsonPropertyName("candidatos")]
    public List<CandidatosFinal>? candidatos { get; set; }
}

