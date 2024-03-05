using Presupuesto.Models;

namespace Presupuesto.Infraestructura
{
    public interface ITiposCuentasServices
    {
        void Crear(TipoCuenta tipoCuenta);
        Task CrearAsync(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
    }
}
