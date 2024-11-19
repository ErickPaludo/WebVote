using System.Text.Json.Serialization;
using WebVote.Entidades;

namespace WebVote.Models
{
    public class EleicoesModelSend
    {
        [JsonPropertyName("nomeEleicao")]
        public string nomeEleicao { get; set; }  
        [JsonPropertyName("candidatos")]
        public List<string> candidatos { get; set; }
        [JsonPropertyName("zonasEleitorais")]
        public List<ZonasModelSend> zonasEleitorais { get; set; }
    }
}
