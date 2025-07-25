using ActionTest.Data;
using ActionTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionTest.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly _22bitv01EmployeeContext _context;

        public EmployeesController(_22bitv01EmployeeContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchstring)
        {
            ViewData["CurrentFilter"] = searchstring;
            var termContext = _context.Employees.Include(b => b.Department).AsQueryable();
            if (!string.IsNullOrEmpty(searchstring))
            {
                termContext = termContext.Where(b =>
                b.EmployeeId.ToString().Contains(searchstring) ||
                b.EmployeeName.Contains(searchstring) ||
                b.DepartmentId.ToString().Contains(searchstring) ||
                b.PhotoImagePath.Contains(searchstring) ||
                (searchstring == "0" && b.Gender) ||
                (searchstring == "1" && !b.Gender)
                   );
            }
            return View(await termContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var employee = new Employee();
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,Gender,Email,Phone,DepartmentId")] Employee employee, IFormFile PhotoImagePath)
        {
            if (ModelState.IsValid)
            {
                if (PhotoImagePath != null && PhotoImagePath.Length > 0)
                {
                    var fileName = MyTool.UploadImageToFolder(PhotoImagePath, "Photos");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        employee.PhotoImagePath = Path.Combine("Images", "Photos", fileName);
                    }
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,Gender,Email,Phone,DepartmentId")] Employee employee, IFormFile PhotoImagePath)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (PhotoImagePath != null && PhotoImagePath.Length > 0)
                {
                    var fileName = MyTool.UploadImageToFolder(PhotoImagePath, "Photos");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        employee.PhotoImagePath = Path.Combine("Images", "Photos", fileName);
                    }
                }
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Stats()
        {
            var data = _context.Employees
                .GroupBy(b => new
                {
                    b.DepartmentId,
                    b.Department.DepartmentName
                })
                .Select(g => new Stat
                {
                    Id = g.Key.DepartmentId,                      
                    DepartmentId = g.Key.DepartmentId,
                    DepartmentName = g.Key.DepartmentName,
                    TotalEmployee = g.Count(),
                    TotalMale = g.Count(b => b.Gender),          
                    TotalFemale = g.Count(b => !b.Gender)         
                })
                .ToList();

            return View(data);
        }



        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
