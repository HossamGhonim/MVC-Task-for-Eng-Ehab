using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eng_Ehab_Task.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eng_Ehab_Task.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var persons = from p in _context.Persons
                          select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                persons = persons.Where(s => s.FirstName.Contains(searchString) 
                                       || s.LastName.Contains(searchString)
                                       || s.SSN.Contains(searchString));
            }

            return View(await persons.ToListAsync());
        }

        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID,FirstName,LastName,SSN,Gender,MilitaryStatus,Salary,MaritalStatus,GovernorateID,CityID,VillageID")] Person person)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", person.GovernorateID);
            return View(person);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null) return NotFound();

            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", person.GovernorateID);
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonID,FirstName,LastName,SSN,Gender,MilitaryStatus,Salary,MaritalStatus,GovernorateID,CityID,VillageID")] Person person)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != person.PersonID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GovernorateID"] = new SelectList(_context.Governorates, "GovernorateID", "GovernorateName", person.GovernorateID);
            return View(person);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null) return NotFound();

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (person == null) return NotFound();

            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonID == id);
        }
    }
}