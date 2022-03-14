using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAD.Domain.Models.Seguranca
{
    public partial class FuncaoModel
    {
        [DisplayName("Id")]
        public int FuncaoId { get; set; }

        [DisplayName("Função")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual PerfilModel Perfil { get; set; }
    }
}
