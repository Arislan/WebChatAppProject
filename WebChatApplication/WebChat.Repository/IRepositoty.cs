using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Repository
{
    public interface IRepositoty<T> : IDisposable
    {
        T Add(T item);
        T Update(T item);
        void Delete(int id);
        void Delete(T item);
        T Get(int id);
        IQueryable<T> All();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Attach<N>(N item) where N : class;
        void Detach<N>(N item) where N : class;
    }
}
