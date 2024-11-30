using CAD.Domain.Models.Aplicacao;
using System.ComponentModel.DataAnnotations;

namespace CAD.Domain.Models.Seguranca
{
    public class SegurModel : _Model
    {
        [Required]
        public UsuarioModel Usuario { get; set; }

        [Required]
        public ModuloModel Modulo { get; set; }

        [Required]
        public PerfilModel Perfil { get; set; }
    }
}
