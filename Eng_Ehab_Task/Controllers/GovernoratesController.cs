using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eng_Ehab_Task.Models;

namespace Eng_Ehab_Task.Controllers
{
    [Authorize]
    public class GovernoratesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GovernoratesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Governorates.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GovernorateID,GovernorateName")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(governorate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(governorate);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate == null) return NotFound();
            return View(governorate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GovernorateID,GovernorateName")] Governorate governorate)
        {
            if (id != governorate.GovernorateID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(governorate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GovernorateExists(governorate.GovernorateID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(governorate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var governorate = await _context.Governorates
                .FirstOrDefaultAsync(m => m.GovernorateID == id);
            if (governorate == null) return NotFound();

            return View(governorate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate != null)
            {
                _context.Governorates.Remove(governorate);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GovernorateExists(int id)
        {
            return _context.Governorates.Any(e => e.GovernorateID == id);
        }
    }
}
