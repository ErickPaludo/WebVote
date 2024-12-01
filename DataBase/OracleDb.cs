using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.DataBase;

public static class OracleDb
{
    private static string endereco_banco = "Data Source=25.49.76.159:1521/freepdb1;User Id =vote;Password=vote;";
    public static ResponseModelSend ImportaConfig(EleicoesModelSend model)
    {
        using (OracleConnection connection = new OracleConnection(endereco_banco))
        {
            connection.Open();
            using (OracleTransaction transaction = connection.BeginTransaction())
            using (OracleCommand cmd = new OracleCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;

                    List<string> comandos = new List<string>();
                    foreach (var candidatos in model.candidatos)
                    {
                        comandos.Add($"INSERT INTO candidatos (nome,url) VALUES ('{candidatos.nomeCandidato}','{candidatos.urlImg}')");
                    }

                    comandos.Add(
                        $"INSERT INTO titulo_eleicao (nomeeleicao) VALUES ('{model.nomeEleicao}')"
                    );
                    foreach (var zonas in model.zonasEleitorais)
                    {
                        foreach (var secao in zonas.secoes)
                        {
                            comandos.Add($"insert into zonas(id, secao, qtde_eleitores) values({zonas.idZona}, {secao.idSecao}, {secao.quantidadeEleitoresZona})");
                        }
                    }

                    foreach (var comando in comandos)
                    {
                        cmd.CommandText = comando;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return new ResponseModelSend { code = 200, message = "Cadastrado com sucesso!" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ResponseModelSend { code = 500, message = ex.Message };
                }
            }

        }
    }
    public static ResponseModelSend ImportaSecoes(CandidatoModelSendList model)
    {
        using (OracleConnection connection = new OracleConnection(endereco_banco))
        {
            connection.Open();
            using (OracleTransaction transaction = connection.BeginTransaction())
            using (OracleCommand cmd = new OracleCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;

                    List<string> comandos = new List<string>();

                    foreach (var candidatos in model.candidatos)
                    {
                        comandos.Add($"insert into secoes (id,qtde_presentes,votos_validos,candidato,qtde_votos,id_zona)values ({model.idSecao},{model.quantidadePresentes},{model.votosValidos},'{candidatos.nomeCandidato}',{candidatos.quantidadeVotos},{model.idZona})");
                    }


                    foreach (var comando in comandos)
                    {
                        cmd.CommandText = comando;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return new ResponseModelSend { code = 200, message = "Cadastrado com sucesso!" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ResponseModelSend { code = 500, message = ex.Message };
                }
            }

        }
    }
    public static ResponseSecoes RetornoSecoes(int zona, int secao)
    {
        using (OracleConnection connection = new OracleConnection(endereco_banco))
        {
            connection.Open();
            int vostos_valido = 0;
            int qtdepresentes = 0;
            int totalsecoes = 0;
            int secoesimportadas = 0;
            int supostototal = 0;
            float percetualPresenca = 0;

            string? nomeEleicao = "";

            using (OracleCommand cmd = new OracleCommand($@"
             select *
                    from titulo_eleicao t", connection))
            {
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nomeEleicao = reader["nomeeleicao"].ToString();
                    }
                }
            }

            using (OracleCommand cmd = new OracleCommand($@"
            select s.votos_validos as votosvalidos,
                s.qtde_presentes as qtdepresentes
              from secoes s  
             where ({secao} = 0 or s.id = {secao})
               and ({zona} = 0 or s.id_zona = {zona})
             group by s.votos_validos,
                   s.qtde_presentes", connection))
            {
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        qtdepresentes += Convert.ToInt32(reader["qtdepresentes"]);
                        vostos_valido += Convert.ToInt32(reader["votosvalidos"]);
                             }
                }
            }
            using (OracleCommand cmd = new OracleCommand($@"
 select count(secao) as secaoestotais,sum(qtde_eleitores) as supostototal
  from zonas z where ({secao} = 0 or z.secao = {secao})
               and ({zona} = 0 or z.id = {zona})", connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalsecoes = Convert.ToInt32(reader["secaoestotais"]);
                        supostototal = Convert.ToInt32(reader["supostototal"]);
                    }
                }

            }
            using (OracleCommand cmd = new OracleCommand($@"
select count(distinct id) as secaoesimportadas
  from secoes  s  
             where ({secao} = 0 or s.id = {secao})
               and ({zona} = 0 or s.id_zona = {zona})", connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        secoesimportadas = Convert.ToInt32(reader["secaoesimportadas"]);
                    }
                }
            }
            percetualPresenca = (qtdepresentes / (float)supostototal) * 100;
            return new ResponseSecoes
            {
                nomeEleicao = nomeEleicao,
                percentualAbstencoes = 100 - percetualPresenca,
                percentualPresentes = percetualPresenca,
                secoesImportadas = secoesimportadas,
                totalAbstencoes = supostototal - qtdepresentes,
                totalEleitoresPresentes = qtdepresentes,
                totalSecoes = totalsecoes
            };
        }
    }
    public static CandidatosResult RetornoFinal(int zona, int secao)
    {
        using (OracleConnection connection = new OracleConnection(endereco_banco))
        {
            connection.Open();
            int vostos_valido = 0;
            int quantidadeeleitores = 0;
            int totalVotosValidos = 0;
            int nomeCandidato = 0;
            string url = "";
            float percentualVotosValidos = 0;
            float percentualVotos = 0;
            string? nomeEleicao = "";

            using (OracleCommand cmd = new OracleCommand($@"
             select *
                    from titulo_eleicao t", connection))
            {
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nomeEleicao = reader["nomeeleicao"].ToString();
                    }
                }
            }

            using (OracleCommand cmd = new OracleCommand($@"
             select sum(s.qtde_votos) as qtdevotos
             from secoes s", connection))
            {
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalVotosValidos = Convert.ToInt32(reader["qtdevotos"]);
                    }
                }
            }

            using (OracleCommand cmd = new OracleCommand($@"
            select sum(distinct s.votos_validos) as votosvalidos
            from secoes s", connection))
            {
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vostos_valido = Convert.ToInt32(reader["votosvalidos"]);
                    }
                }
            }
            using (OracleCommand cmd = new OracleCommand($@"
            select sum(z.qtde_eleitores) as quantidadeeleitores
            from zonas z", connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        quantidadeeleitores = Convert.ToInt32(reader["quantidadeeleitores"]);
                    }
                }

            }
            List<CandidatosFinal>? candidatos = new List<CandidatosFinal>();
            using (OracleCommand cmd = new OracleCommand($@"
select s.candidato,
       sum(s.qtde_votos) as qtdevotos,
       c.url as img
  from secoes s
  join candidatos c
    on s.candidato = c.nome
 group by s.candidato,
          c.url", connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        candidatos.Add(new CandidatosFinal { nomeCandidato = reader["candidato"].ToString(),quantidadeVotos = Convert.ToInt32(reader["qtdevotos"]),percentualVotos = (Convert.ToInt32(reader["qtdevotos"]) / (float)quantidadeeleitores) * 100 ,urlImg = reader["img"].ToString() });
                    }
                }
            }
            return new CandidatosResult
            {
               nomeEleicao = nomeEleicao,
               totalVotosValidos = totalVotosValidos,
               percentualVotosValidos = (vostos_valido / (float)totalVotosValidos) * 100,
               candidatos = candidatos,
            };
        }
    }

}
