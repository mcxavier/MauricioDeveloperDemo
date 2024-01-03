using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.SeedWork;
using Core.SharedKernel;
using Infra.Extensions;
using Infra.ExternalServices.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infra.EntitityConfigurations.Contexts
{
    public class DataContext : DbContext
    {
        private readonly ILogger _logger;
        private readonly SmartSalesIdentity identity;
        private IDbContextTransaction _currentTransaction;

        protected DataContext(DbContextOptions options, SmartSalesIdentity identity, ILogger logger) : base(options)
        {
            this._logger = logger;
            this.identity = identity;
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public override int SaveChanges()
        {
            if (this.BeforeSaveHandle().Any())
            {
                return 0;
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException errors)
            {
                _logger.LogError("Error on CoreContext {@exception}", errors);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on CoreContext {@ex}", ex);

                throw;
            }
        }

        public List<ValidationResult> ValidateAndSave()
        {
            var errors = this.BeforeSaveHandle();
            if (errors.Any())
                return errors;

            this.SaveChanges();

            return new List<ValidationResult>();
        }

        public async Task<int> SaveChangesAsync()
        {
            this.BeforeSaveHandle();

            return await base.SaveChangesAsync();
        }

        private List<ValidationResult> BeforeSaveHandle()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var errors = new List<ValidationResult>();

            foreach (var entry in entries)
            {
                var validationContext = new ValidationContext(entry);
                Validator.TryValidateObject(entry, validationContext, errors, true);

                if (entry.Entity is IEntityWithMetadata metadata)
                {
                    if (entry.State == EntityState.Added)
                    {
                        metadata.CreatedAt = DateTime.UtcNow;
                        metadata.CreatedBy = string.IsNullOrEmpty(metadata.CreatedBy) ? identity.Name : metadata.CreatedBy;
                    }

                    metadata.ModifiedAt = DateTime.UtcNow;
                    metadata.ModifiedBy = string.IsNullOrEmpty(metadata.ModifiedBy) ? identity.Name : metadata.ModifiedBy;
                }

                if (entry.Entity is IStoreReferenced storeReferenced)
                {
                    if (entry.State == EntityState.Added)
                    {
                        storeReferenced.StoreCode = identity.CurrentStoreCode;
                    }
                }
            }

            this.AutoTruncateStringToMaxLength();

            return errors;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await this.SaveChangesAsync();

                transaction.Commit();
            }
            catch
            {
                this.RollbackTransaction();

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}