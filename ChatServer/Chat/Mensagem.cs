using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Server {
    /// <summary>
    /// 
    /// </summary>
    public class Mensagem {

        private string _texto;
        public string Texto {
            get { return _texto; }
            set { _texto = value; }
        }

        private DateTime _dataHora;
        public DateTime DataHora {
            get { return _dataHora; }
            set { _dataHora = value; }
        }

        private Usuario _usuario;
        public Usuario Usuario {
            get { return _usuario; }
            set { _usuario = value; }
        }

        private long _totalConectado;
        public long TotalConectado {
            get { return _totalConectado; }
            set { _totalConectado = value; }
        }


        public Mensagem() { }

        public Mensagem(long totalConectado, string texto, DateTime dataHora, Usuario usuario) {
            this._texto = texto;
            this._dataHora = dataHora;
            this._usuario = usuario;
            this._totalConectado = totalConectado;
        }

        public void Gravar() { 
            //TODO: serializar em json arquivo fisico
        }

    }// fim classe
}// fim namespace