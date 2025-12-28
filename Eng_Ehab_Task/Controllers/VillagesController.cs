using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eng_Ehab_Task.Models;

namespace Eng_Ehab_Task.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VillagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVillagesByCity(int cityId)
        {
            var villages = await _context.Villages
                .Where(v => v.CityID == cityId)
                .Select(v => new { value = v.VillageID, text = v.VillageName })
                .ToListAsync();
            return Json(villages);
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Villages.Include(v => v.City);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VillageID,VillageName,CityID")] Village village)
        {
            if (ModelState.IsValid)
            {
                if (_context.Villages.Any(v => v.VillageName == village.VillageName && v.CityID == village.CityID))
                {
                    ModelState.AddModelError("VillageName", "This Village already exists in the selected City.");
                    ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", village.CityID);
                    return View(village);
                }
                _context.Add(village);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", village.CityID);
            return View(village);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var village = await _context.Villages.FindAsync(id);
            if (village == null) return NotFound();
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", village.CityID);
            return View(village);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VillageID,VillageName,CityID")] Village village)
        {
            if (id != village.VillageID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Villages.Any(v => v.VillageName == village.VillageName && v.CityID == village.CityID && v.VillageID != village.VillageID))
                    {
                        ModelState.AddModelError("VillageName", "This Village already exists in the selected City.");
                        ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", village.CityID);
                        return View(village);
                    }
                    _context.Update(village);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VillageExists(village.VillageID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", village.CityID);
            return View(village);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var village = await _context.Villages
                .Include(v => v.City)
                .FirstOrDefaultAsync(m => m.VillageID == id);
            if (village == null) return NotFound();

            return View(village);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var village = await _context.Villages.FindAsync(id);
            if (village != null)
            {
                _context.Villages.Remove(village);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VillageExists(int id)
        {
            return _context.Villages.Any(e => e.VillageID == id);
        }
    }
}
