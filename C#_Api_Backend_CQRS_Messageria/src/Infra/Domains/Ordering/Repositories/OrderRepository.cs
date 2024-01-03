using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domains.Ordering.Models;
using Core.Domains.Ordering.Repositories;
using Core.Models.Core.Ordering;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Infra.QueryCommands.Commands.Topics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        public readonly CoreContext _context;

        public OrderRepository(CoreContext context)
        {
            _context = context;
        }

        public bool CheckIfExistsOrders(int[] ids)
        {
            var query = _context.Orders.Where(x => ids.Contains(x.Id));
            return query.Count() == ids.Length;
        }

        public async Task<IList<Order>> GetOrdersAsync()
        {
            var query = (IList<Order>)await _context.Orders.FirstOrDefaultAsync(x => x.StatusId > 0);
            return query;
        }

        public bool ChangeOrderStatus(int id, int statusId)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return false;

            order.StatusId = statusId;
            order.ModifiedAt = System.DateTime.Now;
            _context.Orders.Update(order);
            _context.SaveChanges();

            return true;
        }
    }
}