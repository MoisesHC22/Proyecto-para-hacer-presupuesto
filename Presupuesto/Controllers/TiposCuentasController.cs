using Microsoft.AspNetCore.Mvc;
using Presupuesto.Filters;
using Presupuesto.Infraestructura;
using Presupuesto.Models;

namespace Presupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly string connectionString;
        private readonly ITiposCuentasServices _tiposCuentasServices;

        public TiposCuentasController(ITiposCuentasServices tiposCuentasService, IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _tiposCuentasServices = tiposCuentasService;
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
            tipoCuenta.UduarioId = 1;

            ///_tiposCuentasServices.Crear(tipoCuenta);
            ///insertamos el objeto del tipo tipoCuenta
            await _tiposCuentasServices.CrearAsync(tipoCuenta);

            return View();
        }



    }
}
