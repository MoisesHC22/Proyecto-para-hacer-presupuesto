using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presupuesto.Filters;
using Presupuesto.Infraestructura;
using Presupuesto.Models;
using System.Runtime.CompilerServices;

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

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public string draw = "";
        public string start = "";
        public string length = "";
        public int pageSize, skip, recordsTotal;


        [HttpPost]
        public async Task<IActionResult> ObtenerLista()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;


            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tiposCuentas = await _tiposCuentasServices.Obtener(usuarioId);

            recordsTotal = tiposCuentas.Count();
            tiposCuentas = tiposCuentas.Skip(skip).Take(pageSize).ToList();



            return Json(new{
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = tiposCuentas
            });
        

        
        }


        [HttpPost]
        public async Task<IActionResult> Eliminar(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM TiposCuentas WHERE Id = @Id", new { Id });
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Create(string nombre)
        {
            try
            {
                var tipoCuenta = new TipoCuenta { Nombre = nombre };
                await _tiposCuentasServices.CrearAsync(tipoCuenta);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        #region Actualizar 

        [HttpGet]
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tipoCuenta = await _tiposCuentasServices.ObtenerPorId(id, usuarioId);
            if (tipoCuenta is null)
            {
                RedirectToAction("NoEncontrado","Home");
            }

            return View(tipoCuenta);   
        }



        [HttpPost]
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            //Obtenemos el identificacdor de usuario
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tipoCuentaExiste = await _tiposCuentasServices.ObtenerPorId(tipoCuenta.Id, usuarioId);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _tiposCuentasServices.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        #endregion


        #region Eliminar 
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<ActionResult> EliminarR(int id)
        {
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tipoCuenta = await _tiposCuentasServices.ObtenerPorId(id, usuarioId);
            if (tipoCuenta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<ActionResult> EliminarTipoCuenta(int Id)
        {
            //Obtenemos el identificacdor de usuario
            var usuarioId = _usuariosServices.ObtenerUsuariosId();
            var tipoCuenta = await _tiposCuentasServices.ObtenerPorId(Id, usuarioId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _tiposCuentasServices.Borrar(Id);
            return RedirectToAction("Index");
        }

        #endregion


    }
}
