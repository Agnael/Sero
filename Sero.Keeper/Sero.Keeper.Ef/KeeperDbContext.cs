using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Keeper.Ef
{
    public abstract class KeeperDbContext : DbContext
    {
        public KeeperDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected KeeperDbContext()
        {
        }

        public override int SaveChanges()
        {
            this.OnBeforeSaveChanges(this).Wait();
            int savedId = base.SaveChanges();
            this.OnAfterSaveChanges(this, savedId).Wait();

            return savedId;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await this.OnBeforeSaveChanges(this);
            int savedInt =  await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await this.OnAfterSaveChanges(this, savedInt);

            return savedInt;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await this.OnBeforeSaveChanges(this);
            int savedInt = await base.SaveChangesAsync(cancellationToken);
            await this.OnAfterSaveChanges(this, savedInt);

            return savedInt;
        }

        protected abstract Task OnBeforeSaveChanges(DbContext context);
        protected abstract Task OnAfterSaveChanges(DbContext context, int savedId);
    }
}
