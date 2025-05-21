using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;

namespace TicketsB2C.Services.Repository
{
    public interface IUnitOfWork
    {
        ITicketRepository Tickets { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork(TicketsDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly TicketsDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        private ITicketRepository? _tickets;

        private IRepository<City>? _cities;

        private IRepository<Company>? _companies;

        public ITicketRepository Tickets => _tickets ??= new TicketRepository(_context);

        public IRepository<City> Cities => _cities ??= new TicketDBRepository<City>(_context);

        public IRepository<Company> Companies => _companies ??= new TicketDBRepository<Company>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
