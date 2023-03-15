using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public LoginController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if(usuario != null)
                    {
                        if(usuario.SenhaValida(loginModel.Senha))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MenssagemErro"] = $"A senha do usuário é inválida, tente novamente";
                    }

                    TempData["MenssagemErro"] = $"Usuário e/ou senha inválido(s), tente novamente";
                    
                }
                return View("Index");
            }
            catch(Exception e)
            {
                TempData["MenssagemErro"] = $"Ops, não conseguimos realizar o login, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }
            
        }
    }
}
