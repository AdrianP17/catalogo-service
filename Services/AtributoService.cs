using catalogo.Dtos.Atributo;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;

namespace catalogo.Services
{
    public class AtributoService : IAtributoService
    {
        private readonly IAtributoRepository _atributoRepository;
        public AtributoService(IAtributoRepository atributoRepository)
        {
            _atributoRepository = atributoRepository;
        }

        public async Task<Atributo> CreateAsync(CrearAtributoDto atributoDto)
        {
            var atributo = new Atributo
            {
                Nombre = atributoDto.Nombre,
                AtributoValores = new List<AtributoValor>()
            };
            await _atributoRepository.CreateAsync(atributo);
            return atributo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _atributoRepository.DeleteAsync(id);
            return deleted;
        }

        public async Task<List<Atributo>> GetAllAsync()
        {
            return await _atributoRepository.GetAllAsync();
        }

        public Task<Atributo?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Atributo?> UpdateAsync(int id, ActualizarAtributoDto atributoDto)
        {
            var atributoOriginal = await _atributoRepository.GetByIdAsync(id);
            if (atributoOriginal == null) return null;
            atributoOriginal.Nombre = atributoDto.Nombre;
            await _atributoRepository.SaveChangesAsync();

            return atributoOriginal;
        }
    }
}
