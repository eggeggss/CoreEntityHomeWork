using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace webapi01.Models
{
    public partial class ContosoUniversityContext
    {
        private void TraceModify()
        {
            var entities = this.ChangeTracker.Entries();

            foreach (var entity in entities)
            {
                if (entity.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    entity.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }

                if (entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                {
                    entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    entity.CurrentValues.SetValues(new { IsDeleted = 1 });
                    entity.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }

            }
        }

        public override int SaveChanges()
        {
            TraceModify();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TraceModify();
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
