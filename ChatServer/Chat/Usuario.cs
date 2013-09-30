using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Server {
    /// <summary>
    /// 
    /// </summary>
    public class Usuario {

        private string _nome;
        public string Nome {
            get { return _nome; }
            set { _nome = value; }
        }

        private long _sessaoBrowser;
        public long SessaoBrowser {
            get { return _sessaoBrowser; }
            set { _sessaoBrowser = value; }
        }


    }// fim class
}// fim namespace
