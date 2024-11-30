using CAD.Domain.Models.Seguranca;
using System.Collections.Generic;

namespace CAD.Domain.Models.Response
{
    public class RegistroModel
    {
        public SegurModel Seguranca { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Authenticated { get; set; }
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
