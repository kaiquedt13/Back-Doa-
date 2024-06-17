using Microsoft.AspNetCore.Mvc;

namespace DoaMaisAPI.DTO
{
    public class ONGDTO
    {
        public int ID { get; set; }
        public string? Nome { get; set; }
        public string? Celular { get; set; }
        public string  Email { get; set; }
        public string Senha { get; set; }
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Estado { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Biografia { get; set; }
        public string? Base64 { get; set; }

    }
}
