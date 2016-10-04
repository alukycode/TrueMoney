namespace TrueMoney.Infrastructure.Repositories
{
    using TrueMoney.Infrastructure.Entities;

    public interface IUserRepository : IRepository<User>
    {
        string GetShit();
    }
}
