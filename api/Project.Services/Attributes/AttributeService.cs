using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;

using Project.ViewModels.Attributes;

namespace Project.Services.Attributes
{
    public class AttributeService : IAttributeService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public AttributeService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(AttributeCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Attribute>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.Attributes.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Attributes.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Attributes.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Project.Data.Entities.Attribute>> GetAllPaging(GetAttributePagingRequest request)
        {
            //1. query

            var query = _context.Attributes
                .Where(m => m.IsDeleted == false).AsNoTracking();

            if (request.AttributeEnum != null)
            {
                query = query.Where(m => m.Type == request.AttributeEnum);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Project.Data.Entities.Attribute>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data
            };

            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<Data.Entities.Attribute> GetById(int id)
        {
            var item = await _context.Attributes.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");
            return item;
        }





        public async Task<bool> Update(AttributeUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Attributes.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Attributes.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
