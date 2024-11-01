using DiamondShop.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShop.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn231DiamondShopContext _context;
        public UnitOfWork(Prn231DiamondShopContext context)
        {
            _context = context;
        }

        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

