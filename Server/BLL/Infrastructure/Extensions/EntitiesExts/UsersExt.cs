using System.Linq;
using DAL.Models;

namespace BLL.Infrastructure.Extensions.EntitiesExts
{
    public static class UsersExt
    {
        public static IQueryable<User> Searching(this IQueryable<User> query, string search)
        {
            return search.IsNullOrEmpty() ? query : query.Where(i => i.Name.Contains(search));
        }
        
    }
}
