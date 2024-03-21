using Dapper;
using Microsoft.Data.SqlClient;
using Presupuesto.Infraestructura;
using Presupuesto.Models;


namespace Presupuesto.Services
{
    public class TiposCuentasServices : ITiposCuentasServices
    {
        private readonly string connectionString;

        public TiposCuentasServices(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task CrearAsync(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
                 Values(@Nombre, 1 , 0)
                 SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;

        }

        public async Task<bool> Existe(string nombre)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection
                .QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas
                     WHERE Nombre = @Nombre;",
                     new { nombre});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                    FROM TiposCuentas WHERE UsuarioId = @UsuarioId",
                   new { usuarioId });
        }

        public async Task<IEnumerable<TipoCuenta>> ObtenerInfCuenta(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas
                Where Id = @Id", new { Id });
        }

        public async Task<IEnumerable<TipoCuenta>> TodosLosRegistros()
        {
            using var connection = new SqlConnection( connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas", new { });
        }

        public async Task<IEnumerable<TipoCuenta>> TodosLosRegistrosPaginados(int start, int length)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas ORDER BY Id
                    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY", new { Start = start * length, Length = length });
        }

        public async Task<int> ObtenerTotalRegistros()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM TiposCuentas");
        }

        public async Task EditarAsync(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>(@"UPDATE TiposCuentas SET
                    Nombre = @Nombre WHERE Id = @Id ", tipoCuenta);

        }



        //Funciones

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                     FROM TiposCuentas WHERE Id=@Id AND UsuarioId=@UsuarioId", new { id,usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection( connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET
                     Nombre=@Nombre WHERE Id = @Id", tipoCuenta);
        }



        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id=@Id", new { id });
        }

    }
}
