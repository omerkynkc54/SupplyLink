using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyLink.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using SupplyLink.Models;
using SupplyLink.Data;

namespace SupplyLink.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Order/Index
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            List<Order> orders;
            if (currentUser != null)
            {
                var userRole = await _userManager.GetRolesAsync(currentUser);
                var isAdminOrPurchase = userRole.Contains("SatýnAlma") || userRole.Contains("Admin");

                if (isAdminOrPurchase)
                {
                    orders = await _db.Orders.OrderBy(o => o.OrderDate).ToListAsync();
                }
                else
                {
                    orders = await _db.Orders.Where(o => o.RequesterId == currentUser.Id).OrderBy(o => o.OrderDate).ToListAsync();
                }
            }
            else
            {
                orders = new List<Order>();
            }

            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Quantity = o.Quantity,
                ImagePath = o.ImagePath,
                Notes = o.Notes,
                OrderDate = o.OrderDate,
                Type = _db.OrderTypes.FirstOrDefault(t => t.Id == o.TypeId)?.Name,
                UnitOfMeasure = _db.UnitOfMeasures.FirstOrDefault(u => u.Id == o.UnitOfMeasureId)?.Name,
                Status = _db.OrderStatuses.FirstOrDefault(s => s.Id == o.StatusId)?.Name,
                RequesterName = _db.Users.FirstOrDefault(u => u.Id == o.RequesterId)?.UserName
            }).ToList();

            ViewBag.IsAdminOrPurchase = currentUser != null && (await _userManager.GetRolesAsync(currentUser)).Any(r => r == "SatýnAlma" || r == "Admin");

            return View(orderViewModels);
        }

        // GET: /Order/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_db.OrderTypes, "Id", "Name");
            ViewData["UnitOfMeasureId"] = new SelectList(_db.UnitOfMeasures, "Id", "Name");
            return View();
        }

        // POST: /Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, IFormFile imagePath)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            order.SetRequesterId(_userManager, User);

            order.OrderDate = DateTime.Now;
            order.StatusId = _db.OrderStatuses.FirstOrDefault(s => s.Name == "Onaylandý").Id;

            try
            {
                if (imagePath != null && imagePath.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var fileName = Path.GetFileName(imagePath.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagePath.CopyToAsync(stream);
                    }

                    order.ImagePath = Path.Combine("images", fileName).Replace("\\", "/");
                    Console.WriteLine($"Image path set to {order.ImagePath}");
                }
                else
                {
                    Console.WriteLine("No image file received.");
                }

                _db.Add(order);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                // Re-populate the dropdown lists in case of an error
                ViewData["TypeId"] = new SelectList(_db.OrderTypes, "Id", "Name", order.TypeId);
                ViewData["UnitOfMeasureId"] = new SelectList(_db.UnitOfMeasures, "Id", "Name", order.UnitOfMeasureId);
                return View(order);
            }
        }

        // GET: /Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["TypeId"] = new SelectList(_db.OrderTypes, "Id", "Name", order.TypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_db.UnitOfMeasures, "Id", "Name", order.UnitOfMeasureId);
            return View(order);
        }

        [Authorize(Roles = "Admin,SatýnAlma")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
