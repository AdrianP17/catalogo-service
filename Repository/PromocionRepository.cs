using catalogo.Data;
using catalogo.Dtos.Promocion;
using catalogo.Interfaces.IRepositories;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Repository
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly AppDBContext _context;
        public PromocionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Promocion> CreateAsync(Promocion promocion)
        {
            await _context.Promociones.AddAsync(promocion);
            await _context.SaveChangesAsync();
            return promocion;
        }

        public async Task<Promocion?> DeleteAsync(int id)
        {
            var promocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
            {
                return null;
            }

            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();
            return promocion;
        }


        public async Task<List<Promocion>> GetAllAsync()
        {
            return await _context.Promociones.ToListAsync();
        }

        public async Task<Promocion?> GetByIdAsync(int id)
        {
            return await _context.Promociones.FindAsync(id);
        }

        public async Task<Promocion?> UpdateAsync(int id, UpdatePromocionRequestDto promocion)
        {
            var existingPromocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == id);

            if (existingPromocion == null)
            {
                return null;
            }

            existingPromocion.Descripcion = promocion.Descripcion;
            existingPromocion.Monto = promocion.Monto;
            existingPromocion.Porcentaje = promocion.Porcentaje;
            existingPromocion.Fecha_inicio = promocion.Fecha_inicio.ToUniversalTime();
            existingPromocion.Fecha_limite = promocion.Fecha_limite.ToUniversalTime();

            await _context.SaveChangesAsync();

            return existingPromocion;
        }
    }
}
