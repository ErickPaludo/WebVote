using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.DataBase;

public static class OracleDb
{
    private static string endereco_banco = "Data Source=25.49.76.159:1521/freepdb1;User Id =vote;Password=vote;";

    //public void Env(string id_r, string id_d, string mensagem)
    //{
    //    using (OracleConnection connection = new OracleConnection(endereco_banco))
    //    {
    //        try
    //        {
    //            connection.Open();
    //            using (OracleCommand cmd = new OracleCommand("prc_env_msg", connection))
    //            {
    //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //                cmd.Parameters.Add("v_id_remetente", OracleDbType.Int16).Value = Convert.ToInt32(id_r);
    //                cmd.Parameters.Add("v_id_destinatario", OracleDbType.Int16).Value = Convert.ToInt32(id_d);
    //                cmd.Parameters.Add("v_msg", OracleDbType.Varchar2).Value = mensagem;
    //                cmd.ExecuteNonQuery();
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //}

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
                        comandos.Add($"INSERT INTO candidatos (nome) VALUES ('{candidatos}')");
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
    public static ResponseSecoes RetornoSecoes(int zona,int secao)
    {
        using (OracleConnection connection = new OracleConnection(endereco_banco))
        {
            connection.Open();
            using (OracleCommand cmd = new OracleCommand($@"
            select s.votos_validos,
                   sum(s.qtde_presentes) as qtdepresentes,
                   sum(s.votos_validos) as totaleleitorespresentes,
                   count(distinct s.id) as secoesimportadas,
                   (select count(z.secao)
                      from zonas z
                     where {zona} = 0 or z.id = {zona}) as totalsecoes
              from secoes s
             where ({secao} = 0 or s.id = {secao})
               and ({zona} = 0 or s.id_zona = {zona})
             group by s.votos_validos", connection))
            {
 
                // Use 0 como padrão para representar "todos".
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int eleitorespresentes = Convert.ToInt32(reader["totalEleitoresPresentes"]);
                        int qtdepresentes = Convert.ToInt32(reader["qtdepresentes"]);
                        float presentes = (qtdepresentes / (float)eleitorespresentes) * 100;
                        float abstencao = 100 - presentes;
                        return new ResponseSecoes { percentualAbstencoes = abstencao, 
                            percentualPresentes = presentes, 
                            secoesImportadas = Convert.ToInt32(reader["secoesImportadas"]), 
                            totalAbstencoes = Convert.ToInt32(reader["totalEleitoresPresentes"]) - Convert.ToInt32(reader["qtdepresentes"]), 
                            totalEleitoresPresentes = Convert.ToInt32(reader["totalEleitoresPresentes"]), 
                            totalSecoes = Convert.ToInt32(reader["totalSecoes"]) 
                        };
                    }
                }
                return new ResponseSecoes { percentualAbstencoes = 0, percentualPresentes = 0, secoesImportadas = 0, totalAbstencoes = 0, totalEleitoresPresentes = 0, totalSecoes = 0 };
            }

        }
    }
}
