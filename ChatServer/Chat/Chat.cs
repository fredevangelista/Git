
#region "Diretiva de Uso "
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
#endregion

namespace Chat.Server {
    /// <summary>
    /// Implementacao do lado server do Chat
    /// </summary>
    public class Chat {

        #region " Atributos "
        private static object _historicoLock = new object();
        // limite de mensagens empilhadas na memoria
        public static readonly int MaximoMensagens = 500;
        // pilha de mensagens
        private static Queue<Mensagem> _historico = new Queue<Mensagem>(MaximoMensagens);
        // Timeout do long pool - obrigatorio como const para uso no decorator AsyncTimeout
        public const int TimeOutLongPool = 40;
        // implementando thread safe para gravar uma mensagem de cada vez
        private static object _mensagemLock = new object();
        // implementando observeble
        private static Subject<Mensagem> _mensagens = new Subject<Mensagem>();
        // total usuarios conectados no momento
        private static long totalConectado = 0;
        // session id do usuario logado
        private static long usuarioLogado = 0;
        #endregion

        /// <summary>
        /// Construtor deve ser statis para trabalhos assincronos
        /// </summary>
        static Chat() {
            // executando acao onNext ( Chamado pelo subject para passar novas mensagens - desefileirado limite defido MaximoMensagens)
            _mensagens.Subscribe(ms => {
                                    lock (_historicoLock) {
                                        while (_historico.Count > MaximoMensagens) {
                                            _historico.Dequeue();
                                        }
                                        _historico.Enqueue(ms);
                                    }
                                });
        }

        /// <summary>
        /// Valida se chegaram novas mensagens
        /// </summary>
        /// <param name="mensagens"></param>
        public static void ValidarMensagemAsincronamente(Action<List<Mensagem>> mensagens) {
            var queued = ThreadPool.QueueUserWorkItem(
                new WaitCallback(parm => {
                                var msgs = new List<Mensagem>();
                                var wait = new AutoResetEvent(false);
                                using (var subscriber = _mensagens.Subscribe(msg => {
                                                                                    msgs.Add(msg);
                                                                                    wait.Set();
                                                                                })) {
                                    // espera maxima para uma nova mensagem
                                    wait.WaitOne(TimeSpan.FromSeconds(TimeOutLongPool));
                                }

                                ((Action<List<Mensagem>>)parm)(msgs);
                            }), mensagens
            );

            // caso primeira mensagem executa instancia de mensagens
            if (!queued) {
                mensagens(new List<Mensagem>());
            }
        }

        /// <summary>
        /// Adiciona mensagem com usuario logado na lista
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="mensagem"></param>
        public static void InserirMensagem(string nome, string mensagem) {

            Mensagem msg = new Mensagem {
                TotalConectado = totalConectado++,
                Texto = mensagem,
                DataHora = DateTime.Now,
                Usuario = new Usuario {
                    SessaoBrowser = usuarioLogado++,
                    Nome = nome
                }
            };
            _mensagens.OnNext(msg);
            
            Persistencia m = new Persistencia(msg);
            m.Gravar();
        }


        /// <summary>
        /// Lista mensagens persistidas no primeiro acesso ao da fila de mensagens (memoria)
        /// </summary>
        /// <returns></returns>
        public static List<Mensagem> ListarHistorico() {
            List<Mensagem> mensagens = new List<Mensagem>();
            lock (_historicoLock) {
                //if (_historico.Count <= 0) {
                //    mensagens = Persistencia.Retornar().ToList();
                //}
                //else {
                   mensagens = _historico.ToList();
                //}
            }
            return mensagens;
        }

    }// fim class
}// fim namespace
