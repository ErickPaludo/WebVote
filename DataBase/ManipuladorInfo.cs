using Microsoft.AspNetCore.Mvc;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.DataBase
{
    public static class ManipuladorInfo
    {
        public static ResponseModelSend InformacoesIniciais(EleicoesModelSend model)
        {
            if (!string.IsNullOrEmpty(model.nomeEleicao))
            {
                foreach (var candidatos in model.candidatos)
                {
                    if (string.IsNullOrEmpty(candidatos.nomeCandidato) || string.IsNullOrEmpty(candidatos.urlImg))
                    {
                        return new ResponseModelSend { code = 400, message = "Existem candidatos não nomeados!" };
                    }  
                }
                foreach(var zonas in model.zonasEleitorais)
                {
                    if (!string.IsNullOrEmpty(zonas.idZona))
                    {
                        foreach(var secoes in zonas.secoes)
                        {
                            if(string.IsNullOrEmpty(secoes.idSecao) || secoes.quantidadeEleitoresZona == 0)
                            {
                                return new ResponseModelSend { code = 400, message = "Seção inválida!" };
                            }
                        }
                    }
                    else
                    {
                        return new ResponseModelSend { code = 400, message = "Zona sem id!" };
                    }
                }
            }
            else
            {
                return new ResponseModelSend { code = 400, message = "Eleição não nomeada!" };
            }
             return OracleDb.ImportaConfig(model);         
        }

        public static ResponseModelSend ImportaSecao(CandidatoModelSendList model)
        {
            return OracleDb.ImportaSecoes(model);
        }
    }
}
