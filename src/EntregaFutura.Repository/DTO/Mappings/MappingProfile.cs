using AutoMapper;
using EntregaFutura.Domain.Models;

namespace EntregaFutura.Repository.DTO.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProdutoModel, ProdutoDTO>().ReverseMap();
            CreateMap<ProdutoGrupoModel, ProdutoGrupoDTO>().ReverseMap();
            CreateMap<EntregaModel, EntregaDTO>().ReverseMap();
            CreateMap<UsuarioModel, UsuarioDTO>().ReverseMap();
            CreateMap<ListaPrecoModel, ListaPrecoDTO>().ReverseMap();
            CreateMap<ListaPrecoProdutoModel, ListaPrecoProdutoDTO>().ReverseMap();
            CreateMap<ProdutoImagemModel, ProdutoImagemDTO>().ReverseMap();
        }
    }
}
