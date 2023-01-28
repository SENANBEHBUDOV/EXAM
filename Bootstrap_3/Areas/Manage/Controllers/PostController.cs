using BootStrap.Models;
using Bootstrap_3.DAL;
using Bootstrap_3.Extension;
using Bootstrap_3.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bootstrap_3.Areas.Manage.Controllers
{
    namespace Anyar.Areas.Manage.Controllers
    {
        //[Authorize(Roles = "Admin")]
        [Area("Manage")]
        public class EmployeeController : Controller
        {
            public AppDbContext _context { get; }
            public IWebHostEnvironment _env { get; }
            public EmployeeController(AppDbContext context, IWebHostEnvironment env)
            {
                _context = context;
                _env = env;
            }
            public async Task<IActionResult> Index()
            {
                return View(_context.Projects.Include(p => p.Name));
            }
            public async Task<IActionResult> Create()
            {
                ViewBag.Projects = new SelectList(_context.Projects, nameof(Project.Id), nameof(Project.Name));
                return View();
            }
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null) { return BadRequest(); }
               Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project == null) { return NotFound(); }
                _context.Projects.Remove(project);
               project.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img");
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            [HttpPost]
            public async Task<IActionResult> Create(CreateProjectVM ProjectVM)
            {
                string result = ProjectVM.ImageUrl.CheckValidate("image/", 800);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }
                
                if (!ModelState.IsValid)
                {
                    ViewBag.Projects = new SelectList(_context.Projects, nameof(Project.Id), nameof(Project.Name));
                    return View();
                }
                Project project = new Project
                {
                    Name= ProjectVM.Name,
                    //ImageUrl = ProjectVM.ImageUrl,
                    Description= ProjectVM.Description,
                    Author= ProjectVM.Author,

                    ImageUrl = ProjectVM.ImageUrl.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img"))
                };
                _context.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            public async Task<IActionResult> Update(int? id)
            {
                if (id == null) { return BadRequest(); }
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project == null) { return NotFound(); }
                UpdateProjectVM employeeVM = new UpdateProjectVM
                {
                   Name= project.Name,
                   //ImageUrl=project.ImageUrl,
                   Description= project.Description,
                   Author= project.Author,
                };
                ViewBag.Positions = new SelectList(_context.Projects, nameof(project.Id), nameof(project.Name));
                return View(employeeVM);
            }
            [HttpPost]
            public async Task<IActionResult> Update(int? id, UpdateProjectVM employeeVM)
            {
                if (id == null) { return BadRequest(); }
                var existedemployee = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (existedemployee == null) { return NotFound(); }
            
                //if (employeeVM.ImageUrl != null)
                //{
                //    //string result = employeeVM.Image.CheckValidate("image/", 800);
                //    //if (result.Length > 0)
                //    //{
                //    //    ModelState.AddModelError("Image", result);
                //    //}
                //    else
                //    {
                //        existedemployee.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img");
                //    }
                //}
                if (!ModelState.IsValid)
                {
                    ViewBag.Positions = new SelectList(_context.Projects, nameof(Project.Id), nameof(Project.Name));
                    return View();
                }
                existedemployee.Name = employeeVM.Name;
                existedemployee.Description = employeeVM.Description;
                //existedemployee.ImageUrl = employeeVM.ImageUrl;
                existedemployee.Author = employeeVM.Author;
                
                if (employeeVM.ImageUrl != null)
                {
                    existedemployee.ImageUrl = employeeVM.ImageUrl.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img"));
                }
                ViewBag.Projects = new SelectList(_context.Projects, nameof(Project.Id), nameof(Project.Name));
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
