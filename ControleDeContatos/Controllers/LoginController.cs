using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
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
                            _sessao.CriarSessaoUsuario(usuario);
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

        [HttpPost]
        public IActionResult EnviarLinkRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Email, redefinirSenhaModel.Login);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();

                        TempData["MenssagemSucesso"] = $"Enviamos para seu e-mail cadastrado uma nova senha.";
                        return RedirectToAction("Index", "Login");
                    }

                    TempData["MenssagemErro"] = $"Não conseguimos redefinir sua senha. Por favor, verifique os dados informados.";

                }
                return View("Index");
            }
            catch (Exception e)
            {
                TempData["MenssagemErro"] = $"Ops, não conseguimos redefinir sua senha, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
