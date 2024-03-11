using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presupuesto.Filters;
using Presupuesto.Infraestructura;
using Presupuesto.Models;

namespace Presupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly string connectionString;
        private readonly ITiposCuentasServices _tiposCuentasServices;
        private readonly IUsuariosServices _usuariosServices;

        public TiposCuentasController(ITiposCuentasServices tiposCuentasService, IConfiguration configuration, IUsuariosServices usuariosServices)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _tiposCuentasServices = tiposCuentasService;
            _usuariosServices = usuariosServices;
        }

        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TipoCuenta tipoCuenta)
        {
            if (ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            var ExistAccount = await
                _tiposCuentasServices.Existe(tipoCuenta.Nombre, tipoCuenta.UduarioId);
            if (ExistAccount)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El bombre {tipoCuenta} ya existe.");
                return View(tipoCuenta);
            }

            ///hardcodeamos el usuario con su id
            tipoCuenta.UduarioId = _usuariosServices.ObtenerUsuariosId();

            ///_tiposCuentasServices.Crear(tipoCuenta);
            ///insertamos el objeto del tipo tipoCuenta
            await _tiposCuentasServices.CrearAsync(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var Exist = await _tiposCuentasServices.Existe(nombre, usuarioId);
            if (Exist)
            {
                return Json($"El nombre {nombre} ya existe");
            }
            return Json(true);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tiposCuentas = await _tiposCuentasServices.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TipoCuentas SET
                     Nombre=@Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id,Nombre,
                              Orden FROM TiposCuentas WHERE Id=@Id AND UsuarioId=@UsuarioId",
                   new { id, usuarioId });
        }


 
    }
}
