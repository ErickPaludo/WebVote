namespace WebVote.Entidades;

public class Candidatos
{
    public List<People> candidatos { get; set; }
}
public class People
{
    public string? nome { get; set; }
    public int votos { get; set; }
}
