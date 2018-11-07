using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs;
using BLL.Infrastructure;
using BLL.Infrastructure.Extensions;
using BLL.Infrastructure.Extensions.EntitiesExts;
using BLL.Infrastructure.Filters;
using BLL.Interfaces;
using DAL;
using DAL.Models;

namespace BLL.Services
{
    public class UsersService : AppService, IUsersService
    {
        private readonly IMapper _mapper;
        public UsersService(ApplicationContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        
        public async Task<Result<UserDto>> GetAsync(FilterBase filter)
        {
            return await Repo.UsersRepository.GetQueryable()
                .MaybeWhere(filter.Where)
                .Searching(filter.Search) //mb delete, using dynamic linq where logic
                .MaybeOrderBy(filter.OrderBy)
                .SkipAndTake(filter)
                .MaybeSelect(filter.Select)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToResultAsync(filter);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            return _mapper.Map<UserDto>(await Repo.UsersRepository.FindAsync(id));
        }

        public async Task UpdateAsync(int id, UserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            await Repo.UsersRepository.UpdateAsync(id, entity);
        }

        public async Task UpdateSpecificAsync(int id, Dictionary<string, object> dictionary)
        {
            await Repo.UsersRepository.UpdateSpecificAsync(id, dictionary);
        }

        public async Task InsertAsync(UserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            await Repo.UsersRepository.InsertAsync(entity);
            await Repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await Repo.UsersRepository.DeleteAsync(id);
        }
    }
}
