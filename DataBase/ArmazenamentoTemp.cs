using System.Net.NetworkInformation;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.DataBase;

public static class ArmazenamentoTemp
{

    private static string? nomeEleicao;

    private static List<string>? candidatos = new List<string>();

    private static List<ZonasModelSend> zonasEleitorais = new List<ZonasModelSend>();

    private static List<CandidatoModelSendList> dadosSecoes = new List<CandidatoModelSendList>();


    public static string NomeEleicao { get { return nomeEleicao; } set { nomeEleicao = value; } }
    public static List<string>? Candidatos { get { return candidatos; } set { candidatos = value; } }
    public static List<ZonasModelSend> ZonasEleitorais { get { return zonasEleitorais; } set { zonasEleitorais = value; } }
    public static List<CandidatoModelSendList> DadosSecoes { get { return dadosSecoes; } set { dadosSecoes = value; } }

}
