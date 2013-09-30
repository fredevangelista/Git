using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS = Chat.Server;

namespace ChatHttpPool.Controllers
{
    public class ChatController : AsyncController
    {
        [AsyncTimeout(CS.Chat.TimeOutLongPool * 1000)]
        public void IndexAsync() {
            AsyncManager.OutstandingOperations.Increment();
            CS.Chat.ValidarMensagemAsincronamente(msgs => {
                AsyncManager.Parameters["response"] = new CS.Respostas {
                    mensagens = msgs
                };
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult IndexCompleted(CS.Respostas response) {
            return Json(response);
        }

        [HttpPost]
        public ActionResult Adicionar(string nome, string msg) {
            CS.Chat.InserirMensagem(nome, msg);
            return Json(new {
                d = 1 // OK
            });
        }

    }// fim class
}// namespace
