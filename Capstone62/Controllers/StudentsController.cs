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
            var student = await GetCurrentStudent();
            if (student.IsAdmin)
            {
                return View(await _context.Students.ToListAsync());
            }
            return RedirectToAction("Details");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var student = await GetCurrentStudent();

            if (student.IsAdmin)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var viewStudent = await _context.Students.FindAsync(id);
                if (viewStudent == null)
                {
                    return NotFound();
                }
                var vm = new DataVM();
                vm.CurrentStudent = viewStudent;
                vm.Enrollments = _context.Enrollments.Where(x => x.Student.StudentId == student.StudentId).ToList();
                vm.Courses = new List<Course>();
                foreach (var enrollment in vm.Enrollments)
                {
                    vm.Courses.Add(_context.Courses.Where(x => x.CourseId == enrollment.CourseId).FirstOrDefault());
                }

                return View(vm);
            }

            if (student == null)
            {
                return RedirectToAction("Create");
            }
            if (true)
            {

                var vm = new DataVM();
                vm.CurrentStudent = student;
                vm.Enrollments = _context.Enrollments.Where(x => x.Student.StudentId == student.StudentId).ToList();
                vm.Courses = new List<Course>();
                foreach (var enrollment in vm.Enrollments)
                {
                    vm.Courses.Add(_context.Courses.Where(x => x.CourseId == enrollment.CourseId).FirstOrDefault());
                }

                return View(vm);
            }
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
                return RedirectToAction(nameof(Details));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var student = await GetCurrentStudent();

            if (student.IsAdmin)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var viewStudent = await _context.Students.FindAsync(id);
                if (viewStudent == null)
                {
                    return NotFound();
                }
                return View(viewStudent);
            }

            if (student == null)
            {
                return RedirectToAction("Create");
            }

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,UserName,Role,IsAdmin")] Student student)
        {
            if (id != student.StudentId)
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
                    if (!StudentExists(student.StudentId))
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var student = await GetCurrentStudent();

            if (student.IsAdmin)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var viewStudent = await _context.Students.FindAsync(id);
                if (viewStudent == null)
                {
                    return NotFound();
                }
                return View(viewStudent);
            }

            if (student == null)
            {
                return RedirectToAction("Create");
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
            return _context.Students.Any(e => e.StudentId == id);
        }

        private async Task<Student> GetCurrentStudent()
        {
            return await _context.Students.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
        }
    }
}
