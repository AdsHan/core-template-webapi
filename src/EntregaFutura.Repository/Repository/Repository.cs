using EntregaFutura.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntregaFutura.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApiDbContext _context;

        public Repository(ApiDbContext contexto)
        {
            _context = contexto;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            // Reforço que ela foi alterada
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }
    }
}
