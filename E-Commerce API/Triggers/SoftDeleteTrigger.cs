using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
namespace E_Commerce_API.Triggers
{
    public class SoftDeleteTrigger : IBeforeSaveTrigger<ISoftDelete>
    {
        private readonly ECommerceDbContext _applicationContext;

        public SoftDeleteTrigger(ECommerceDbContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task BeforeSave(ITriggerContext<ISoftDelete> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Deleted)
            {
                var entry = _applicationContext.Entry(context.Entity);
                entry.State = EntityState.Unchanged;
                context.Entity.DeletedAt = DateTime.Now;
                context.Entity.IsDeleted = true;
            }

            await Task.CompletedTask;
        }
    }

}
