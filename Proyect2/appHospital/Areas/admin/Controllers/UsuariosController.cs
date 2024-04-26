using appHospital.Data.Repository.IRepository;
using appHospital.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace appHospital.Areas.admin.Controllers
{

    [Area("admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
           
            return View(_contenedorTrabajo.Usuario.GetAll());
            //}
            //[HttpGet]
            //public IActionResult Create()
            //{

            //    return View();
            //}
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public IActionResult Create(Usuario usuario)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        //logica para guardar en bd
            //        _contenedorTrabajo.Usuario.Add(usuario);
            //        _contenedorTrabajo.Save();
            //        return RedirectToAction(nameof(Index));

            //    }
            //    return View(usuario);
            //}
            //[HttpGet]
            //public IActionResult Edit(int id)
            //{
            //    Usuario usuario = new Usuario();
            //    usuario = _contenedorTrabajo.Usuario.Get(id);
            //    if (usuario == null)
            //    {
            //        return NotFound();

            //    }
            //    return View(usuario);
            //}
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public IActionResult Edit(Usuario usuario)
            //{

            //    if (ModelState.IsValid)
            //    {
            //        _contenedorTrabajo.Usuario.Update(usuario);
            //        _contenedorTrabajo.Save();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    return View(usuario);
            //}
            //#region llamadas a la api
            //[HttpGet]
            //public IActionResult GetAll()
            //{
            //    return Json(new { data = _contenedorTrabajo.Usuario.GetAll() });
        }
        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return View("Index",_contenedorTrabajo.Usuario.GetAll());
        }
        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return View("Index", _contenedorTrabajo.Usuario.GetAll());
        }
        //#endregion
    }
}
