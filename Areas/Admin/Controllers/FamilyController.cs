using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetWeeding.Areas.Admin.ViewModels;
using SweetWeeding.DAL;
using SweetWeeding.Models;
using SweetWeeding.Utilities.Extension;

namespace SweetWeeding.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles ="Admin")]
	public class FamilyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FamilyController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
           _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Family> families = await _context.Families.ToListAsync();
            return View(families);
        }

        public async Task<IActionResult> Create(CreateFamilyVM familyVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if (!familyVM.Photo.FileType("image/"))
            {
                ModelState.AddModelError("Photo", "sekil tipi uygun deyil");
                return View();

            }

            if (!familyVM.Photo.FileSize(5*1024))
            {
                ModelState.AddModelError("Photo", "sekil olcusu uygun deyil");
                return View();
            }


            string fileName = await familyVM.Photo.CreateFileAsync(_env.WebRootPath, "images");

            Family family = new Family
            {
                Name = familyVM.Name,
                Description= familyVM.Description,
                Image= fileName
            };


            await _context.Families.AddAsync(family);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Update(int id)
        {
            if(id<=0) return BadRequest();

            Family family = await _context.Families.FirstOrDefaultAsync(f=>f.Id==id);

            if(family==null) return NotFound();

            UpdateFamilyVM familyVM = new UpdateFamilyVM
            {
                Name = family.Name,
                Description = family.Description,
                Image = family.Image
            };

            return View(familyVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateFamilyVM familyVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Family family = await _context.Families.FirstOrDefaultAsync(f => f.Id==id);

            if(family==null) return NotFound();

            if(familyVM.Photo is not null)
            {
                if (!familyVM.Photo.FileType("image/"))
                {
                    ModelState.AddModelError("Photo", "sekil tipi uygun deyil");
                    return View(familyVM);

                }

                if (!familyVM.Photo.FileSize(5 * 1024))
                {
                    ModelState.AddModelError("Photo", "sekil olcusu uygun deyil");
                    return View(familyVM);
                }

                string newImage = await familyVM.Photo.CreateFileAsync(_env.WebRootPath, "images");

                family.Image.DeleteFile(_env.WebRootPath, "images");
                family.Image= newImage;

            }
            family.Name= familyVM.Name;
            family.Description = familyVM.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id<=0) return BadRequest();
            Family family = await _context.Families.FirstOrDefaultAsync(f=>f.Id==id);
            if(family==null) return NotFound();

            family.Image.DeleteFile(_env.WebRootPath, "images");

            _context.Families.Remove(family);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
