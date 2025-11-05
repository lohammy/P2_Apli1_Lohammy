using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P2_Apli1_Lohammy.DAL;
using P2_Apli1_Lohammy.Models;

namespace P2_Apli1_Lohammy.Services;

public class RegistroPedidosServices(IDbContextFactory<Contexto> DbFactory)
{

    public async Task<bool> Guardar(RegistroPedidos registro)
    {
        if (!await Existe(registro.PedidoId))
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
        return await contexto.RegistroPedidos.AnyAsync(p => p.PedidoId == id);
    }

    private async Task<bool> Insertar(RegistroPedidos registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.RegistroPedidos.Add(registro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(RegistroPedidos registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entradaAnterior = await contexto.RegistroPedidos
            .Include(e => e.PedidosDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PedidoId == registro.PedidoId);

        if (entradaAnterior == null)
        {
            return false;
        }

        await AfectarRegistroPedidos(detalle: [.. entradaAnterior.PedidosDetalles],
                                      TipoOperacion.Resta);

        await AfectarRegistroPedidos([.. registro.PedidosDetalles], TipoOperacion.Suma);

        contexto.RegistroPedidos.Update(registro);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<RegistroPedidos?> Buscar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RegistroPedidos
            .Include(e => e.PedidosDetalles)
            .FirstOrDefaultAsync(e => e.PedidoId == pedidoId);
    }
    public async Task<bool> Eliminar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entrada = await contexto.RegistroPedidos
            .Include(e => e.PedidosDetalles)
            .FirstOrDefaultAsync(e => e.PedidoId == pedidoId);

        if (entrada == null)
        {
            return false;
        }

        await AfectarRegistroPedidos(detalle: [.. entrada.PedidosDetalles], TipoOperacion.Resta);

        contexto.RegistroPedidosDetalle.RemoveRange(entrada.PedidosDetalles);
        contexto.RegistroPedidos.Remove(entrada);

        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }
    public async Task<List<RegistroPedidos>> Listar(Expression<Func<RegistroPedidos, bool>> criterio)
    {

        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RegistroPedidos
            .Include(e => e.PedidosDetalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
    private async Task AfectarRegistroPedidos(RegistroPedidosDetalle[] detalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            var componente = await contexto.Componentes
                .SingleAsync(t => t.ComponenteId == item.ComponenteId);

            if (tipoOperacion == TipoOperacion.Suma)
            {
                componente.Existencia += item.Cantidad;
            }
            else if (tipoOperacion == TipoOperacion.Resta)
            {
                componente.Existencia -= item.Cantidad;
            }

            await contexto.SaveChangesAsync();
        }
    }
    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
    public async Task<List<Componente>> ListarComponente()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Componentes
            .AsNoTracking()
            .ToListAsync();
    }
}
