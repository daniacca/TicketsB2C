using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace TicketsB2C.Services.Repository
{
    public class Repository<TContext, T> : IRepository<T> 
        where T : class 
        where TContext: DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }

    public class TicketDBRepository<T> : Repository<TicketsDbContext, T> where T : class
    {
        public TicketDBRepository(TicketsDbContext context) : base(context) { }
    }
}
