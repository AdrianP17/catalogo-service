using catalogo.Data;
using catalogo.Interfaces.IRepositories;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Repository
{
    public class AtributoRepository : IAtributoRepository
    {
        private readonly AppDBContext _context;
        public AtributoRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Atributo> CreateAsync(Atributo atributo)
        {
            _context.Atributo.Add(atributo);
            await _context.SaveChangesAsync();
            return atributo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var atributo = await _context.Atributo.FirstOrDefaultAsync(a => a.Id == id);
            if (atributo == null) return false;

            _context.Atributo.Remove(atributo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Atributo>> GetAllAsync()
        {
            return await _context.Atributo.Include(a => a.AtributoValores).ToListAsync();
        }

        public async Task<Atributo?> GetByIdAsync(int id)
        {
            var atributo = await _context.Atributo.Include(a => a.AtributoValores).FirstOrDefaultAsync(a => a.Id == id);
            if (atributo == null) return null;
            return atributo;
        }

        public async Task<Dictionary<string, Dictionary<string, int>>> LoadAllAtributosAsync()
        {
            var data = await _context.Atributo
                .AsNoTracking()
                .Select(a => new
                {
                    Nombre = a.Nombre,
                    Valores = a.AtributoValores.Select(av => new { Valor = av.Valor, Id = av.Id }).ToList()
                })
                .ToListAsync();

            return data.ToDictionary(
                    a => a.Nombre,
                    a => a.Valores.ToDictionary(
                        av => av.Valor.ToLower(),
                        av => av.Id
                    )
                );
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
