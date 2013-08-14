using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebChat.Repository
{
    public class EFRepository<T> : IDisposable, IRepositoty<T> where T : class
    {
        public EFRepository(DbContext context)
        {
            this.DbContext = context;
            this.Table = this.DbContext.Set<T>();
        }

        public DbContext DbContext { get; set; }

        public IDbSet<T> Table { get; set; }

        public T Add(T item)
        {
            DbEntityEntry entry = this.DbContext.Entry<T>(item);

            if (entry.State == System.Data.EntityState.Detached)
            {
                this.Table.Add(item);
            }
            else
            {
                entry.State = System.Data.EntityState.Added;
            }

            DbContext.SaveChanges();
            return item;
        }

        public T Update(T item)
        {
            DbEntityEntry entry = this.DbContext.Entry<T>(item);

            if (entry.State == System.Data.EntityState.Detached)
            {
                this.Table.Attach(item);
            }

            entry.State = System.Data.EntityState.Modified;
            this.DbContext.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var item = this.Table.Find(id);

            if (item != null)
            {
                this.Delete(item);
            }
        }

        public void Delete(T item)
        {
            DbEntityEntry entry = this.DbContext.Entry(item);

            if (entry.State != System.Data.EntityState.Deleted)
            {
                entry.State = System.Data.EntityState.Deleted;
            }
            else
            {
                this.Table.Attach(item);
                this.Table.Remove(item);
            }

            this.DbContext.SaveChanges();
        }

        public T Get(int id)
        {
            var item = this.Table.Find(id);
            return item;
        }

        public IQueryable<T> All()
        {
            return this.Table;
        }

        // x=>x.Name == "Pesho"
        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.Table.Where(predicate);
        }

        public void Attach<N>(N item) where N : class
        {
            var newTable = this.DbContext.Set<N>();
            newTable.Attach(item);
        }

        public void Detach<N>(N item) where N : class
        {
            DbEntityEntry entry = this.DbContext.Entry<N>(item);
            entry.State = System.Data.EntityState.Detached;
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }
}
