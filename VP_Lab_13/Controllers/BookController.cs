using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VP_Lab_13.Models;
using VP_Lab_13.Models.Database;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDatabaseContext _context;

        public BookController(LibraryDatabaseContext context)
        {
            _context = context;
        }

        // GET: /Book/Index
        public IActionResult Index()
        {
            var list = _context.books.ToList();
            return View(list);
        }

        // GET: /Book/ViewAll  (alternate list view)
        public IActionResult ViewAll()
        {
            var list = _context.books.ToList();
            return View("Index", list); // reuse Index view or create separate ViewAll.cshtml
        }

        // GET: /Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.books.Add(book);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(book);
            }
            catch (Exception ex)
            {
                // temp: pass error to view via ModelState or TempData
                ModelState.AddModelError(string.Empty, "Error saving data: " + ex.Message);
                return View(book);
            }
        }

        // GET: /Book/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _context.books.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book model)
        {
            if (id != model.BookId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.books.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error updating: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: /Book/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _context.books.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _context.books.Find(id);
            if (item == null) return NotFound();

            _context.books.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
