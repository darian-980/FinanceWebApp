using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GroupProject.Models;

namespace GroupProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly UserContext _context;

        public MenuController(UserContext context)
        {
            _context = context;
        }

        // Action to load the menu and pass it to the view
        public IActionResult Index()
        {
            // Fetch menu items from the database
            var menuItems = _context.Menu.ToList();

            // Build the hierarchical menu structure
            var menu = BuildMenu(menuItems, null);

            // Pass the menu data to the view
            return View(menu);
        }

        //  hierarchical menu structure
        private List<MenuViewModel> BuildMenu(List<Menu> menuItems, int? parentId)
        {
            return menuItems
                .Where(m => m.ParentId == parentId)
                .Select(m => new MenuViewModel
                {
                    Id = m.Id,
                    MenuName = m.MenuName,
                    Action = m.Action,
                    Controller = m.Controller,
                    SubMenus = BuildMenu(menuItems, m.Id) //  call for submenus
                })
                .ToList();
        }
    }
}
