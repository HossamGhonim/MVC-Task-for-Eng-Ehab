using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eng_Ehab_Task.Models;

namespace Eng_Ehab_Task.Controllers
{
    [Authorize(Roles = "Admin")]
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
                if (_context.Governorates.Any(g => g.GovernorateName == governorate.GovernorateName))
                {
                    ModelState.AddModelError("GovernorateName", "This Governorate already exists.");
                    return View(governorate);
                }
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
                    if (_context.Governorates.Any(g => g.GovernorateName == governorate.GovernorateName && g.GovernorateID != governorate.GovernorateID))
                    {
                        ModelState.AddModelError("GovernorateName", "This Governorate already exists.");
                        return View(governorate);
                    }
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
                var cities = _context.Cities.Where(c => c.GovernorateID == id).ToList();
                foreach (var city in cities)
                {
                    var villages = _context.Villages.Where(v => v.CityID == city.CityID).ToList();
                    _context.Villages.RemoveRange(villages);
                }
                _context.Cities.RemoveRange(cities);
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
