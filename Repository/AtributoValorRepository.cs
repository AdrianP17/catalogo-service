using catalogo.Data;
using catalogo.Interfaces.IRepositories;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Repository
{
    public class AtributoValorRepository : IAtributoValorRepository
    {
        private readonly AppDBContext _context;

        public AtributoValorRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AtributoValor>> ObtenerTodosAsync()
        {
            return await _context.AtributoValor
                .Include(av => av.Atributo)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtributoValor>> ObtenerPorAtributoIdAsync(int atributoId)
        {
            return await _context.AtributoValor
                .Include(av => av.Atributo)
                .Where(av => av.AtributoId == atributoId)
                .ToListAsync();
        }

        public async Task<AtributoValor?> ObtenerPorIdAsync(int id)
        {
            return await _context.AtributoValor
                .Include(av => av.Atributo)
                .FirstOrDefaultAsync(av => av.Id == id);
        }

        public async Task<AtributoValor> CrearAsync(AtributoValor atributoValor)
        {
            _context.AtributoValor.Add(atributoValor);
            await _context.SaveChangesAsync();
            return atributoValor;
        }

        public async Task<AtributoValor?> ActualizarAsync(int id, AtributoValor atributoValor)
        {
            var atributoValorExistente = await _context.AtributoValor.FindAsync(id);
            if (atributoValorExistente == null)
                return null;

            atributoValorExistente.AtributoId = atributoValor.AtributoId;
            atributoValorExistente.Valor = atributoValor.Valor;

            await _context.SaveChangesAsync();
            return atributoValorExistente;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var atributoValor = await _context.AtributoValor.FindAsync(id);
            if (atributoValor == null)
                return false;

            _context.AtributoValor.Remove(atributoValor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AtributoValor>> GetAtributosValoresByIdsAsync(ICollection<int> ids)
        {
            return await _context.AtributoValor
                .Where(av => ids.Contains(av.Id))
                .ToListAsync();
        }
    }
}
