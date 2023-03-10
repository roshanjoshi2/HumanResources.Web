using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HumanResources.Web.Models;
using Microsoft.Extensions.Hosting;
using HumanResources.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using HumanResources.Web.Mapper;

namespace HumanResources.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRDbContext _context;

        public EmployeesController(HRDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees =await _context.Employees.Include(e=>e.Department).ToListAsync();
            var employee= await _context.Employees.Include(e => e.Designation).ToListAsync();

            

            var employeeViewModels = employee.ToViewModel();

            return _context.Employees != null ? 
                          View(await _context.Employees.ToListAsync()) :
                          Problem("Entity set 'HRDbContext.Employees'  is null.");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {


            var departmentlist = _context.Departments.Select(department => new SelectListItem { Text = department.Name, Value = department.Id.ToString() }).ToList();
            ViewData["departments"] = departmentlist;


            var designationtlist = _context.Designations.Select(designation => new SelectListItem { Text = designation.Name, Value = designation.Id.ToString() }).ToList();
            ViewData["designations"] = designationtlist;

            

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,Name,Email,Address,Gender,Dob,JoinDate,Department,Designation")]*/ EmployeeViewModel employeeViewModel)
        {
            var relativePath = saveProfileImage(employeeViewModel.ProfileImage);
            //Map view model to model
            var employee = employeeViewModel.ToModel();
            if (ModelState.IsValid)
            {
                employeeViewModel.ProfileImagePath = relativePath;
                _context.Add(employeeViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeViewModel);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,/* [Bind("Id,Name,Email,Address,Gender,Dob,JoinDate,Department,Designation")]*/EmployeeViewModel employeeViewModel)
        {
            var relativePath = saveProfileImage(employeeViewModel.ProfileImage);
            if (id != employeeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employeeViewModel.ProfileImagePath = relativePath;
                    _context.Update(employeeViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employeeViewModel.Id))
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
            return View(employeeViewModel);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Employees == null)
            {
                return Problem("Entity set 'HRDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string saveProfileImage(IFormFile profileImage)
        {


            var fileName = profileImage.FileName;
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var relativePath = $"/images/Profile/{uniqueFileName}";
            var currentAppPath = Directory.GetCurrentDirectory();
            var fullFilePath = Path.Combine(currentAppPath, $"wwwroot/{relativePath}");

            var stream = System.IO.File.Create(fullFilePath);
            profileImage.CopyTo(stream);
            return relativePath;
        }
    }
}
