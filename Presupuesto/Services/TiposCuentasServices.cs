using Dapper;
using Microsoft.Data.SqlClient;
using Presupuesto.Infraestructura;
using Presupuesto.Models;


namespace Presupuesto.Services
{
    public class TiposCuentasServices: ITiposCuentasServices
    {
        private readonly string connectionString;

        public TiposCuentasServices(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var id = connection
                 .QuerySingle<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
                      values(@Nombre, @UsuarioId, 0)
                      SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task CrearAsync(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>($@"INSERT INTO TipoCuentas (Nombre, UsuarioId, Orden)
                 Values(@Nombre, @UsuarioId, 0)
                 SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;

        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection
                .QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas
                     WHERE Nombre = @Nombre AND UsaurioId = @UsuarioId;",
                     new { nombre, usuarioId });
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id,Nombre, Orden
                   FROM TiposCuentas WHERE UsuarioId = @UsaurioId", new { usuarioId });
        }



    }
}
