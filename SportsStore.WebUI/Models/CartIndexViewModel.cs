using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart _Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}