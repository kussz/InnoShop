using InnoShop.Contracts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class UserTypeController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.UserTypeService.GetAllUserTypes(), "Id", "Name"));
        }
        // GET: UserTypeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
