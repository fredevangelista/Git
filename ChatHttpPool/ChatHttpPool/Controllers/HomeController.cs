using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatHttpPool.Models;
using CS = Chat.Server;

namespace ChatHttpPool.Controllers {
    public class HomeController : Controller {
        
        public ActionResult Index() {
            var viewHome = new HomeModel() {
                Mensagens = CS.Chat.ListarHistorico()
            };

            return View(viewHome);
        }

    }
}
