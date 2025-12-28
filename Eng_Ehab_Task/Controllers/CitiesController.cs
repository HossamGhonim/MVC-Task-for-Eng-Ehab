using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eng_Ehab_Task.Models;

namespace Eng_Ehab_Task.Controllers
{
    [Authorize]
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesByGovernorate(int governorateId)
        {
            var cities = await _context.Cities
                .Where(c => c.GovernorateID == governorateId)
                .Select(c => new { value = c.CityID, text = c.CityName })
                .ToListAsync();
            return Json(cities);
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cities.Include(c => c.Governorate);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityID,CityName,GovernorateID")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", city.GovernorateID);
            return View(city);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", city.GovernorateID);
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityID,CityName,GovernorateID")] City city)
        {
            if (id != city.CityID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", city.GovernorateID);
            return View(city);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var city = await _context.Cities
                .Include(c => c.Governorate)
                .FirstOrDefaultAsync(m => m.CityID == id);
            if (city == null) return NotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.CityID == id);
        }
    }
}
