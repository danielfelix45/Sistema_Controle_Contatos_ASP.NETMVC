
using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        public ContatoController(IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }

        public IActionResult Index()
        {
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodosContatos();
            return View(contatos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.BuscarPorId(id);
            return View(contato);
        }

        public IActionResult ConfirmApagar(int id)
        {
            ContatoModel contato = _contatoRepositorio.BuscarPorId(id);
            return View(contato);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _contatoRepositorio.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar o contato!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar o contato, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(contato);
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar o contato, tente novamente, detalhe do erro:{e.Message}";
                return RedirectToAction("Index");
            }     
            
        }

        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", contato);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar o contato, tente novamente, detalhe do erro:{e.Message}";
                return RedirectToAction("Index");
            }
        }
            

    }
}
