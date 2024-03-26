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


        #region AccionesDB
        public async Task CrearAsync(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
                 Values(@Nombre, 1 , 0)
                 SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;

        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET
                     Nombre=@Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id=@Id", new { id });
        }

        public async Task EditarAsync(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>(@"UPDATE TiposCuentas SET
                    Nombre = @Nombre WHERE Id = @Id ", tipoCuenta);

        }


        #endregion


        #region Consultas 

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                     FROM TiposCuentas WHERE Id=@Id AND UsuarioId=@UsuarioId", new { id, usuarioId });
        }

        public async Task<bool> Existe(string nombre)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection
                .QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas
                     WHERE Nombre = @Nombre;",
                     new { nombre });
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

        #endregion


        #region PaginaciónAnterior     
        public async Task<IEnumerable<TipoCuenta>> TodosLosRegistrosPaginados(int skip, int take)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas ORDER BY Id
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY", new { Skip = skip, Take = take });
        }

        public async Task<int> ObtenerTotalRegistros()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM TiposCuentas");
        }
        #endregion


        #region Filtro 
        public async Task<IEnumerable<TipoCuenta>> ObtenerListaFiltrada(int skip, int take, string searchValue)
        {
            using var connection = new SqlConnection( connectionString);
            var tiposCuentas = await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas WHERE Nombre LIKE @SearchValue ORDER BY Id
                        OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY", new { SearchValue = "%" + searchValue + "%", Skip = skip, Take = take });

            return tiposCuentas;
        }

        public async Task<int> ObtenerTotalRegistrosFiltros(string searchValue)
        {
            using var connection = new SqlConnection(connectionString);
            var totalRegistros = await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM TiposCuentas WHERE Nombre LIKE @SearchValue", new { SearchValue  = "%" + searchValue + "%"});
            return totalRegistros;
        }
        #endregion
    }
}
