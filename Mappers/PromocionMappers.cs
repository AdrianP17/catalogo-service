using catalogo.Dtos.Promocion;
using catalogo.Models;

namespace catalogo.Mappers
{
    public static class PromocionMappers
    {
        public static PromocionDto ToPromocionDto(this Promocion promocionModel)
        {
            return new PromocionDto
            {
                Id = promocionModel.Id,
                Descripcion = promocionModel.Descripcion,
                Monto = promocionModel.Monto,
                Porcentaje = promocionModel.Porcentaje,
                Fecha_inicio = promocionModel.Fecha_inicio.ToUniversalTime(),
                Fecha_limite = promocionModel.Fecha_limite.ToUniversalTime()
            };
        }

        public static Promocion ToPromocionFromCreateDto(this CreatePromocionRequestDto promocionDto)
        {
            return new Promocion
            {
                Descripcion = promocionDto.Descripcion,
                Monto = promocionDto.Monto ?? 0m,
                Porcentaje = promocionDto.Porcentaje ?? 0m,
                Fecha_inicio = promocionDto.Fecha_inicio.ToUniversalTime(),
                Fecha_limite = promocionDto.Fecha_limite.ToUniversalTime()
            };
        }
    }
}
