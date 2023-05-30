using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PublicModule.Controllers
{
    public class AdminController : Controller
    {
        private RwaMoviesContext _db;
        private IVideoRepository _videoRepo;

        public AdminController(RwaMoviesContext db, IVideoRepository videoRepo)
        {
            _db = db;
            _videoRepo = videoRepo;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View(_db.Videos);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View(_videoRepo.GetById(id));
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_videoRepo.GetById(id));
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Video request)
        {
            try
            {
                _videoRepo.Edit(id, request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_videoRepo.GetById(id));
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _videoRepo.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
