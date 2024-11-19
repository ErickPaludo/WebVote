using Microsoft.AspNetCore.Mvc;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.DataBase
{
    public static class ManipuladorInfo
    {
        public static ResponseModelSend InformacoesIniciais(EleicoesModelSend model)
        {
            return OracleDb.ImportaConfig(model);
        }

        public static ResponseModelSend ImportaSecao(CandidatoModelSendList model)
        {
            return OracleDb.ImportaSecoes(model);
        }
    }
}
