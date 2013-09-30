using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS = Chat.Server;

namespace ChatHttpPool.Models {
    public class HomeModel {
        public List<CS.Mensagem> Mensagens { get; set; }
    }
}