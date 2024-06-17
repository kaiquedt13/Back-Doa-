namespace DoaMaisAPI.DTO
{
    public class PedidoDoacaoDTO
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public int? ID_Tipo { get; set; }
        public string Descricao { get; set; }
        public int? ID_ONG { get; set; }
        public bool? Status { get; set; }
        public ONGDTO? ONG { get; set; }
        public TipoDoacaoDTO? TipoDoacao { get; set; }
        public List<ImagemPedidoDoacaoDTO>? ImagensPedido { get; set; }
    }
}

