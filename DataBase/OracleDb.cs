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
                percentualAbstencoes = 100 - percetualPresenca,
                percentualPresentes = percetualPresenca,
                secoesImportadas = secoesimportadas,
                totalAbstencoes = supostototal - qtdepresentes,
                totalEleitoresPresentes = qtdepresentes,
                totalSecoes = totalsecoes
            };
        }
    }
}
