using ControleDeContatos.Models;

namespace ControleDeContatos.Helper
{
    public interface ISessao
    {
        UsuarioModel? BuscarSessaoUsuario();
        void CriarSessaoUsuario(UsuarioModel usuario);
        void RemoverSessaoUsuario();
    }
}
