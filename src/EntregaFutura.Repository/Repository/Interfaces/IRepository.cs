using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntregaFutura.Repository.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        // Eu uso um delegate Func como parametro de entrar (lambda) e um predicate para validar o criterio (bool)
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
