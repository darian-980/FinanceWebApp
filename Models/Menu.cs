using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupProject.Models
{
    [Table("Menutbl")]
    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public int? ParentId { get; set; } // Allow null for top-level menus
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}
