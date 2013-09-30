using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chat.Server {
    class Persistencia {

        private Mensagem _mensagem;
        private static string _diretorioPersistencia = string.Format("{0}\\ChatFiles", AppDomain.CurrentDomain.BaseDirectory);
        private static string _nomeArquivoPersistido = "chat.json";
        private static string _nomeArquivoCompleto = string.Format("{0}\\{1}", _diretorioPersistencia, _nomeArquivoPersistido);
        public Persistencia(Mensagem mensagem) {
            this._mensagem = mensagem;
        }

        public void Gravar() {
            using (StreamWriter file = File.CreateText(Path.Combine(_diretorioPersistencia, _nomeArquivoPersistido)))
            {
              JsonSerializer serializer = new JsonSerializer();
              serializer.Serialize(file, _mensagem);
            }
        }

        private static void CriarDiretorioPersistencia() {
            Directory.CreateDirectory(_diretorioPersistencia);
        }

        private static void ValidarDiretorioPersistencia() { 
            if(!Directory.Exists(_diretorioPersistencia)){
                CriarDiretorioPersistencia();
            }
        }

        private static void CriarArquivoPersistencia() { 
            ValidarDiretorioPersistencia();
            if (!File.Exists(_nomeArquivoCompleto)) {
                File.Create(_nomeArquivoCompleto);
            }
        }

        /// <summary>
        /// retorna todos
        /// </summary>
        /// <returns></returns>
        public static List<Mensagem> Retornar() {
            CriarArquivoPersistencia();
            JsonSerializer serializer = new JsonSerializer();
            List<Mensagem> mensagem = JsonConvert.DeserializeObject<List<Mensagem>>(File.ReadAllText(_nomeArquivoCompleto));
            if (mensagem == null) {
                return new List<Mensagem>();
            }
            if (mensagem.Count <= 0) { 
                return new List<Mensagem>();
            }
            return mensagem;
        }

        /// <summary>
        /// Retorna linhas de acordo com parametro
        /// </summary>
        /// <param name="limiteRetorno"></param>
        /// <returns></returns>
        public static List<Mensagem> Retornar(int limiteRetorno) {
            using (StreamReader file = File.OpenText(Path.Combine(_diretorioPersistencia,_nomeArquivoPersistido))){
                JsonSerializer serializer = new JsonSerializer();
                List<Mensagem> mensagem = JsonConvert.DeserializeObject<List<Mensagem>>(file.ReadToEnd());
                var msg = from m in mensagem
                            orderby m.DataHora descending  
                            select m;
                if (mensagem.Count <= 0) {
                    return new List<Mensagem>();
                }

                return (msg as List<Mensagem>).Take(limiteRetorno).OrderBy(m => m.DataHora).ToList();
            }
        }



    }// fim class
}// fim namespace
