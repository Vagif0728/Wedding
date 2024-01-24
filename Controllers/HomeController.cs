using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetWeeding.DAL;
using SweetWeeding.Models;
using SweetWeeding.ViewModels;

namespace SweetWeeding.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Family> families = await _context.Families.ToListAsync();
            HomeVM home = new HomeVM
            {
                Families= families
            };


            return View(home);
        }
    }
}
