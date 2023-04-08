using DietPlanner.DAL.Context;
using DietPlanner.DAL.Entities;

using Math.DAL.Abstract.Repository;

using Math.DAL.Repository.Base;

namespace Math.DAL.Repository;

public class AppUserRepository : GenericRepository<int, AppUser>, IAppUserRepository
{
    public AppUserRepository(DietPlannerContext dbContext) : base(dbContext)
    {
    }
}
