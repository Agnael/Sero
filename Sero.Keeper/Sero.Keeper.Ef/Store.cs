using Microsoft.EntityFrameworkCore;
using Sero.Keeper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Mapper;

namespace Sero.Keeper.Ef
{
    public abstract class Store<TModel, TEntity> : IStore<TModel> 
        where TEntity : Entity
    {
        private DbContext _dbContext;

        protected Sero.Mapper.Mapper Mapper;
        protected virtual IQueryable<TEntity> DbSet => _dbContext.Set<TEntity>();

        public Store(EfKeeper keeper, Mapper.Mapper mapper)
        {
            _dbContext = keeper.DbContext;
            Mapper = mapper;
        }

        protected void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        protected async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            TEntity target = DbSet.FirstOrDefault(x => x.Id == id);

            if (target == null)
                throw new DeleteUnexistingEntityException(id);

            _dbContext.Remove(target);
            SaveChanges();

            //if(target == null)
            //    throw new DeleteUnexistingEntityException(id);

            //if (target.IsDeleted)
            //    throw new AlreadyDeletedEntityException(id);

            //target.IsDeleted = true;
            //_dbContext.Update<TEntity>(target);
            //_dbContext.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            TEntity target = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (target == null)
                throw new DeleteUnexistingEntityException(id);

            _dbContext.Remove(target);
            await SaveChangesAsync();

            //if (target == null)
            //    throw new DeleteUnexistingEntityException(id);

            //if (target.IsDeleted)
            //    throw new AlreadyDeletedEntityException(id);

            //target.IsDeleted = true;
            //_dbContext.Update<TEntity>(target);
            //await _dbContext.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            bool result = DbSet.NonDeletedOnly().Any(x => x.Id == id);
            return result;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            bool result = await DbSet.NonDeletedOnly().AnyAsync(x => x.Id == id);
            return result;
        }

        public TModel Fetch(int id)
        {
            TEntity result = DbSet.NonDeletedOnly()
                                    .FirstOrDefault(x => x.Id == id);

            TModel dto = Mapper.Map<TModel>(result);
            return dto;
        }

        public ICollection<TModel> FetchAll()
        {
            var list = DbSet.NonDeletedOnly().ToList();
            var dtoList = Mapper.MapList<TModel>(list);

            return dtoList;
        }

        public async Task<ICollection<TModel>> FetchAllAsync()
        {
            var list = await DbSet.NonDeletedOnly().ToListAsync();
            var dtoList = Mapper.MapList<TModel>(list);

            return dtoList;
        }

        public async Task<TModel> FetchAsync(int id)
        {
            TEntity result = await DbSet.NonDeletedOnly()
                                    .FirstOrDefaultAsync(x => x.Id == id);

            TModel dto = Mapper.Map<TModel>(result);

            return dto;
        }

        public int Save(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);           
            
            _dbContext.Add<TEntity>(entity);
            SaveChanges();

            return entity.Id;
        }

        public async Task<int> SaveAsync(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            _dbContext.Add<TEntity>(entity);
            await SaveChangesAsync();

            return entity.Id;
        }

        public void Update(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            
            if(!_dbContext.Set<TEntity>().Any(x => x == entity))
                _dbContext.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;

            SaveChanges();
        }

        public async Task UpdateAsync(TModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
