using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repository;

namespace PizzaBox.Client.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly UnitOfWork _context;

        public OrderController(UnitOfWork context)
        {
            _context = context;
        }

        [HttpGet("/stores")]
        public IActionResult Stores(CustomerViewModel model)
        {
            model.Order = new OrderViewModel()
            {
                Stores = _context.GetAll<Store>().ToList()
            };
            return View("Stores", model);
        }

        [HttpGet("/pizza")]
        public IActionResult Pizza(CustomerViewModel model)
        {
            model.Pizza.Crusts = _context.GetAll<Crust>().ToList();
            model.Pizza.Sizes = _context.GetAll<Size>().ToList();

            return View("Order", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(OrderViewModel model)
        {
            if(ModelState.IsValid)
            {
                var order = new Order(){};
                return View("OrderPlaced");
            }
            return View("home", model);
        }
    }
}
