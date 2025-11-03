using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P2_Apli1_Lohammy.DAL;
using P2_Apli1_Lohammy.Models;

namespace P2_Apli1_Lohammy.Services;

public class RegistroServices (IDbContextFactory<Contexto> DbFactory)
{

    public async Task<bool> Guardar(RegistroPedidos registro)
    {
        if (!await Existe(registro.IdEntrada))
        {
            return await Insertar(registro);
        }
        else
        {
            return await Modificar(registro);
        }
    }
    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registro.AnyAsync(p => p.IdEntrada == id);
    }

    private async Task<bool> Insertar(RegistroPedidos registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Registro.Add(registro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(RegistroPedidos registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entradaAnterior = await contexto.Registro
            .Include(e => e.entradaHuacalesDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEntrada == registro.IdEntrada);

        if (entradaAnterior == null)
        {
            return false;
        }

        await AfectarEntradasHuacales(detalle: [.. entradaAnterior.entradaHuacalesDetalle],
                                      TipoOperacion.Resta);

        await AfectarEntradasHuacales([.. registro.entradaHuacalesDetalle], TipoOperacion.Suma);

        contexto.Registro.Update(registro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<RegistroPedidos>> Listar(Expression<Func<RegistroPedidos, bool>> criterio)
    {

        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registro
            .Include(e => e.entradaHuacalesDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
