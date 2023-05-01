using crudCuenta.Models;
using crudCuenta.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace crudCuenta.Controllers
{
    public class LibroController : Controller
    {
        private readonly IRepositorioLibro repositorioLibro;

        public LibroController(IRepositorioLibro repositorioLibro)
        {
            this.repositorioLibro = repositorioLibro;
        }
        public async Task<IActionResult> Index()
        {
            var libro = await repositorioLibro.obtenerLibros();
            return View(libro);
        }

        public IActionResult Crear()
        { 
            return View();
        }

        [HttpPost] //Se debe utilizar con un task
        public async Task<IActionResult> Crear(LibroViewModel libro)
        {

            if(!ModelState.IsValid)
            {
                return View(libro);
            }

            await repositorioLibro.crear(libro);
            return RedirectToAction("Index");
        }

        [HttpGet] //Obtiene los datos mediante la url

        public async Task<IActionResult> Editar(int id)
        {
            var libro = await repositorioLibro.obtenerPorId(id);

            if(libro is null)
            {
               return RedirectToAction("NoEncontrado", "Home");
            }

            return View(libro);
        }

        [HttpPost]

        public async Task<IActionResult> Editar(LibroViewModel libro)
        {
            var libroExiste = await repositorioLibro.obtenerPorId(libro.GsIdLibro);

            if (libroExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioLibro.actualizar(libro);
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult>Borrar(int id)
        {
            var libro = await repositorioLibro.obtenerPorId(id);

            if (libro is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(libro);
        }

        [HttpPost]

        public async Task<IActionResult>Eliminar(LibroViewModel libro)
        {
            var libroExiste = await repositorioLibro.obtenerPorId(libro.GsIdLibro);

            if (libroExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioLibro.borrar(libro.GsIdLibro);
            return RedirectToAction("Index");
        }
    }
}
