using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Interfaces;

namespace PizzaBox.Storing.Repository
{
    internal class StoreRepository : IStoreRepository
    {
        
        private readonly PizzaBoxContext _db;
        
        public StoreRepository(PizzaBoxContext context)
        {
            _db = context;
        }

        public Store GetStoreByName(string name)
        {
            return _db.Set<Store>().FirstOrDefault(s => s.Name == name);
        }

        public Store GetStoreByID(long id)
        {
            return _db.Set<Store>().FirstOrDefault(s => s.EntityID == id);
        }
        
        public IEnumerable<Order> GetUserOrders(User user)
        {
            var query = _db.Set<User>()
            .Include(u => u.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Crust)
            .Include(u => u.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Size)
            .Include(u => u.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Toppings)
            .FirstOrDefault<User>(u => u.EntityID == user.EntityID);
            return query.Orders;
        }
        
        public IEnumerable<Order> GetOrdersByStore(Store store)
        {
            var query = _db.Set<Store>()
            .Include(store => store.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Crust)
            .Include(store => store.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Size)
            .Include(store => store.Orders)
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Toppings)
            .FirstOrDefault<Store>(s => s.EntityID == store.EntityID);
            return query.Orders;
        }
        
        public IEnumerable<Order> ReadStoreOrdersByUser(Store store, User user)
        {
            var query = _db.Set<Store>()
            .Include(store => store.Orders
                .Where(o => o.UserEntityID == user.EntityID))
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Crust)
            .Include(store => store.Orders
                .Where(o => o.UserEntityID == user.EntityID))
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Size)
            .Include(store => store.Orders
                .Where(o => o.UserEntityID == user.EntityID))
                .ThenInclude(order => order.Pizzas)
                    .ThenInclude(pizza => pizza.Toppings)
            .FirstOrDefault<Store>(s => s.EntityID == store.EntityID);
            return query.Orders;
        }
        
        public IEnumerable<Order> GetOrderByDateRange(Store store, DateTime startDate, int days)
        {
            var endDate = startDate.Date.AddDays(days);
            var query = _db.Set<Store>()
            .Include(s => s.Orders
                .Where(o => o.Date >= startDate && o.Date <= endDate))
                .ThenInclude(o => o.Pizzas)
                    .ThenInclude(o => o.Size)
            .Include(s => s.Orders
                .Where(o => o.Date >= startDate && o.Date <= endDate))
                .ThenInclude(o => o.Pizzas)
                    .ThenInclude(o => o.Crust)
            .Include(s => s.Orders
                .Where(o => o.Date >= startDate && o.Date <= endDate))
                .ThenInclude(o => o.Pizzas)
                    .ThenInclude(o => o.Toppings)
            .FirstOrDefault<Store>(s => s.EntityID == store.EntityID).Orders;
            return query;
        }

        public IEnumerable<Order> GetOrdersByDateRange(DateTime startDate, int days)
        {
            var endDate = startDate.Date.AddDays(-1 * days);
            
            var query = _db.Set<Order>()
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.Size)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.Crust)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.Toppings)
            .Where(o => o.Date <= startDate && o.Date >= endDate);

            return query;
        }
    }
}
