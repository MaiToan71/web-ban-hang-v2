using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.Configs;

namespace Project.Services.Configs
{

    public class ConfigService : IConfigService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public ConfigService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(ConfigCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            if (request.Title == null)
            {
                request.Title = "";
            }
            var item = _mapper.Map<Data.Entities.Config>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.Configs.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Configs.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Configs.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Project.Data.Entities.Config>> GetAllPaging(GetConfigPagingRequest request)
        {
            //1. query

            var query = _context.Configs
                .Where(m => m.IsDeleted == false).AsNoTracking();

            if (request.ConfigEnum != null)
            {
                query = query.Where(m => m.ConfigEnum == request.ConfigEnum);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Project.Data.Entities.Config>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data
            };

            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<Data.Entities.Config> GetById(int id)
        {
            var item = await _context.Configs.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");
            return item;
        }


        public async Task<bool> Update(ConfigUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Configs.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Configs.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
