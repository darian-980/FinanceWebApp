using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GroupProject.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<MenuViewModel> SubMenus { get; set; }

        // Constructor to initialize the SubMenus list
        public MenuViewModel()
        {
            SubMenus = new List<MenuViewModel>();
        }
    }
}
