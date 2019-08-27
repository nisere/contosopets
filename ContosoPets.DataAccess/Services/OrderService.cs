using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoPets.DataAccess.Data;
using ContosoPets.Domain.DataTransferObjects;
using ContosoPets.Domain.Models;

namespace ContosoPets.DataAccess.Services
{
    public class OrderService
    {
        private readonly ContosoPetsContext _context;

        public OrderService(ContosoPetsContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerOrder>> GetAll()
        {
            List<CustomerOrder> orders = await (
                from o in _context.Orders.AsNoTracking()
                orderby o.OrderPlaced descending
                select new CustomerOrder
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? 
                        o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (from po in o.ProductOrders
                                    select new OrderLineItem
                                    {
                                        ProductQuantity = po.Quantity,
                                        ProductName = po.Product.Name
                                    })
                })
                .ToListAsync();

            return orders;
        }
        /*public async Task<List<CustomerOrder>> GetAll()
        {
            List<CustomerOrder> orders = await (_context.Orders.AsNoTracking()
                .OrderByDescending(o => o.OrderPlaced)
                .Select(o => new CustomerOrder
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? 
                        o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (o.ProductOrders.Select(po => new OrderLineItem
                    {
                        ProductQuantity = po.Quantity,
                        ProductName = po.Product.Name
                    }))
                })).ToListAsync();

            return orders;
        }*/

        private IQueryable<Order> GetOrderById(int id) =>
            from o in _context.Orders.AsNoTracking()
                                    .TagWith(nameof(GetOrderById))
            where o.Id == id
            select o;
        /*private IQueryable<Order> GetOrderById(int id) =>
            _context.Orders.AsNoTracking()
                .Where(o => o.Id == id);*/

        public async Task<CustomerOrder> GetById(int id)
        {
            CustomerOrder order = await (
                from o in GetOrderById(id)
                select new CustomerOrder
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? 
                        o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (from po in o.ProductOrders
                                    select new OrderLineItem
                                    {
                                        ProductQuantity = po.Quantity,
                                        ProductName = po.Product.Name
                                    })
                })
                .TagWith(nameof(GetById))
                .FirstOrDefaultAsync();

            return order;
        }
        /*public async Task<CustomerOrder> GetById(int id)
        {
            CustomerOrder order = await GetOrderById(id)
                .Select(o => new CustomerOrder
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? 
                        o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (o.ProductOrders.Select(po => new OrderLineItem
                    {
                        ProductQuantity = po.Quantity,
                        ProductName = po.Product.Name
                    }))
                })
                .FirstOrDefaultAsync();

            return order;
        }*/

        public async Task<Order> Create(Order newOrder)
        {
            newOrder.OrderPlaced = DateTime.UtcNow;

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }

        public async Task<bool> SetFulfilled(int id)
        {
            bool isFulfilled = false;
            Order order = await GetOrderById(id).FirstOrDefaultAsync();

            if (order != null)
            {
                order.OrderFulfilled = DateTime.UtcNow;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                isFulfilled = true;
            }

            return isFulfilled;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted = false;
            Order order = await GetOrderById(id).FirstOrDefaultAsync();

            if (order != null)
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
