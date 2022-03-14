using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAD.Domain.Models.Seguranca
{
    public partial class UsuarioModel
    {
        public UsuarioModel()
        {
            PerfisUsuario = new HashSet<PerfilUsuarioModel>();
            RestricoesUsuario = new HashSet<RestricaoUsuarioModel>();
        }

        [DisplayName("Id")]
        public int UsuarioId { get; set; }

        [DisplayName("Usuário")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        [EmailAddress]
        public string Email { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<PerfilUsuarioModel> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoUsuarioModel> RestricoesUsuario { get; set; }
    }
}
