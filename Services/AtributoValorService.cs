using catalogo.Dtos.Atributo;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;

namespace catalogo.Services
{
    public class AtributoValorService : IAtributoValorService
    {
        private readonly IAtributoValorRepository _atributoValorRepository;
        private readonly IAtributoRepository _atributoRepository;

        public AtributoValorService(IAtributoValorRepository atributoValorRepository, IAtributoRepository atributoRepository)
        {
            _atributoValorRepository = atributoValorRepository;
            _atributoRepository = atributoRepository;
        }

        public async Task<IEnumerable<AtributoValorDetalleDto>> ObtenerTodosAsync()
        {
            var atributoValores = await _atributoValorRepository.ObtenerTodosAsync();
            return atributoValores.Select(MapToDetalleDto);
        }

        public async Task<IEnumerable<AtributoValorDetalleDto>?> ObtenerPorAtributoIdAsync(int atributoId)
        {
            var atributo = await _atributoRepository.GetByIdAsync(atributoId);
            if (atributo == null) return null;

            var atributoValores = await _atributoValorRepository.ObtenerPorAtributoIdAsync(atributoId);
            return atributoValores.Select(MapToDetalleDto);
        }

        public async Task<AtributoValorDetalleDto?> ObtenerPorIdAsync(int id)
        {
            var atributoValor = await _atributoValorRepository.ObtenerPorIdAsync(id);
            return atributoValor != null ? MapToDetalleDto(atributoValor) : null;
        }

        public async Task<AtributoValorDetalleDto?> CrearAsync(int atributoId, CrearAtributoValorDto crearAtributoValorDto)
        {
            var atributo = await _atributoRepository.GetByIdAsync(atributoId);
            if (atributo == null) return null;

            var atributoValor = new AtributoValor
            {
                AtributoId = atributoId,
                Valor = crearAtributoValorDto.Valor
            };

            var resultado = await _atributoValorRepository.CrearAsync(atributoValor);
            return MapToDetalleDto(resultado);
        }

        public async Task<AtributoValorDetalleDto?> ActualizarAsync(int id, ActualizarAtributoValorDto actualizarAtributoValorDto)
        {
            var atributoValor = new AtributoValor
            {
                AtributoId = actualizarAtributoValorDto.AtributoId,
                Valor = actualizarAtributoValorDto.Valor
            };

            var resultado = await _atributoValorRepository.ActualizarAsync(id, atributoValor);
            return resultado != null ? MapToDetalleDto(resultado) : null;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _atributoValorRepository.EliminarAsync(id);
        }

        private AtributoValorDetalleDto MapToDetalleDto(AtributoValor atributoValor)
        {
            return new AtributoValorDetalleDto
            {
                Id = atributoValor.Id,
                AtributoId = atributoValor.AtributoId,
                Valor = atributoValor.Valor,
                NombreAtributo = atributoValor.Atributo?.Nombre
            };
        }
    }
}
