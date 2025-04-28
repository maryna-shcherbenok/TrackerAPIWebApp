using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackerAPIWebApp.Models;

namespace TrackerAPIWebApp.Controllers
{
    public class MoodOptionsController : Controller
    {
        private readonly TrackerAPIContext _context;

        public MoodOptionsController(TrackerAPIContext context)
        {
            _context = context;
        }

        // GET: MoodOptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.MoodOptions.ToListAsync());
        }

        // GET: MoodOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moodOption = await _context.MoodOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moodOption == null)
            {
                return NotFound();
            }

            return View(moodOption);
        }

        // GET: MoodOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MoodOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MoodOption moodOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moodOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moodOption);
        }

        // GET: MoodOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moodOption = await _context.MoodOptions.FindAsync(id);
            if (moodOption == null)
            {
                return NotFound();
            }
            return View(moodOption);
        }

        // POST: MoodOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MoodOption moodOption)
        {
            if (id != moodOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moodOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoodOptionExists(moodOption.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(moodOption);
        }

        // GET: MoodOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moodOption = await _context.MoodOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moodOption == null)
            {
                return NotFound();
            }

            return View(moodOption);
        }

        // POST: MoodOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moodOption = await _context.MoodOptions.FindAsync(id);
            if (moodOption != null)
            {
                _context.MoodOptions.Remove(moodOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoodOptionExists(int id)
        {
            return _context.MoodOptions.Any(e => e.Id == id);
        }
    }
}
