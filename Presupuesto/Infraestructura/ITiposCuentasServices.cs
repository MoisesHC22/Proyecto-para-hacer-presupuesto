using Presupuesto.Models;

namespace Presupuesto.Infraestructura
{
    public interface ITiposCuentasServices
    {
        // Acciones
        Task CrearAsync(TipoCuenta tipoCuenta);
        Task EditarAsync(TipoCuenta tipoCuenta);
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);

        // Consultas
        Task<bool> Existe(string nombre);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<IEnumerable<TipoCuenta>> TodosLosRegistros();
        Task<IEnumerable<TipoCuenta>> ObtenerInfCuenta(int Id);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);



        //Paginación
        Task<IEnumerable<TipoCuenta>> TodosLosRegistrosPaginados(int Skip, int take);
        Task<int> ObtenerTotalRegistros();


        // Filtro
        Task<IEnumerable<TipoCuenta>> ObtenerListaFiltrada(int skip, int take, string searchValue);
        Task<int> ObtenerTotalRegistrosFiltros(string searchValue);
    }
}
