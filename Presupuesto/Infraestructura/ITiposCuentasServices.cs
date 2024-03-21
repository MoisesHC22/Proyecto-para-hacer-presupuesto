using Presupuesto.Models;

namespace Presupuesto.Infraestructura
{
    public interface ITiposCuentasServices
    {
        Task CrearAsync(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<IEnumerable<TipoCuenta>> TodosLosRegistros();
        Task<IEnumerable<TipoCuenta>> TodosLosRegistrosPaginados(int Start,int length);
        Task<int> ObtenerTotalRegistros();

        Task<IEnumerable<TipoCuenta>> ObtenerInfCuenta(int Id);
        Task EditarAsync(TipoCuenta tipoCuenta);
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
}
