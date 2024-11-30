namespace CAD.Domain.Models.Seguranca
{
    public partial class RestricaoUsuarioModel
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long? ModuloId { get; set; }
        public long? FormularioId { get; set; }
        public long? EventoId { get; set; }
        public bool IsCheck { get; set; }

        public virtual EventoModel Evento { get; set; }
        public virtual FormularioModel Formulario { get; set; }
        public virtual ModuloModel Modulo { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}
