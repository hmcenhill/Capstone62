using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone62.Models;

namespace Capstone62.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DataContext _context;

        public StudentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            if (await IsUserAdminAsync())
            {
                return View(await _context.Students.ToListAsync());
            }
            return RedirectToAction("Details");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.UserExtendId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UserName = User.Identity.Name;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,UserExtendId,UserName,Role,IsAdmin")] Student student)
        {
            if (id != student.UserExtendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.UserExtendId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.UserExtendId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.UserExtendId == id);
        }
        private async Task<bool> IsUserAdminAsync()
        {
            var tryLogin = await _context.UserExtends.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            if (tryLogin == null) return false;
            return tryLogin.IsAdmin;
        }
        private async Task<bool> IsUserStudent()
        {
            var tryStudent = await _context.Students.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            if (tryStudent == null) return false;
            return true;
        }
        private async Task<int> GetStudentId()
        {
            var tryId = await _context.Students.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            return tryId.StudentId;
        }
    }
}
