using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Utilities.Exceptions;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class BaseService<TEntity, TEntityViewModel, GetTEntityPagingRequest, TEntityCreateRequest, TEntityUpdateRequest> : IBaseService<TEntity, TEntityViewModel, GetTEntityPagingRequest, TEntityCreateRequest, TEntityUpdateRequest> 
        where TEntity : BaseViewModel
        where GetTEntityPagingRequest : PagingRequestBase
    {
        protected readonly IMapper _mapper;
        protected readonly string TableName = typeof(TEntity).Name;
        protected readonly ProductDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;  
        public BaseService(ProductDbContext context,  IMapper mapper) 
        {
            _mapper = mapper;   
            _context = context; 
            _dbSet = context.Set<TEntity>();
        }    
        public virtual async Task<int> Add(TEntityCreateRequest request, Guid userId, string fullname)
        {
            await ValidateCreateLogic(request);   
            var entity = MapEntityCreateRequestToEntiy(request, userId, fullname);
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if(entity == null) 
            {
                throw new ProductException($"Cannot find {TableName}");
            }
            _context.Entry(entity).State = EntityState.Detached;
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();   
        }

        /// <summary>
        /// Hàm tìm kiểm Entity theo Id
        /// </summary>
        /// <param name="Id">int</param>
        /// <returns></returns>
        public virtual async Task<TEntityViewModel> FindById(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            var result = MapEntityToEntityViewModel(entity);    
            return result;
        }

        public virtual async Task<int> Update(TEntityUpdateRequest request, Guid userId, string fullname)
        {
            await ValidateUpdateLogic(request);   
            var entity = MapEntityUpdateRequestToEntiy(request, userId, fullname);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if(existEntity == null)
            {
                throw new ProductException($"Cannot find {TableName}");
            }
            _context.Entry(existEntity).State = EntityState.Detached;
            entity.CreatedUser = existEntity.CreatedUser;
            entity.CreatedTime = existEntity.CreatedTime;   
            existEntity = _mapper.Map(entity, existEntity);
            existEntity.ModifiedTime = DateTime.UtcNow;
            existEntity.ModifiedUser = fullname;
            _context.Entry(existEntity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        

        public virtual async Task<PagedResult<TEntityViewModel>> GetAllPaging(GetTEntityPagingRequest request)
        {
           // 1. Select join
           var query = _dbSet.OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).AsQueryable();

            // 2. Paging
            int totalRow = await query.CountAsync();    
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(product => _mapper.Map<TEntityViewModel>(product)).ToListAsync();
            var pagedResult = new PagedResult<TEntityViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }

        #region Validate
        public virtual async Task ValidateCreateLogic(TEntityCreateRequest entityCreateEntity)
        {
            await Task.CompletedTask;
        }

        public virtual async Task ValidateUpdateLogic(TEntityUpdateRequest entityUpdateRequest)
        {
            await Task.CompletedTask;
        }
        #endregion

        #region Mapping
        public virtual TEntity MapEntityCreateRequestToEntiy(TEntityCreateRequest createRequest, Guid userId, string fullName)
        {
            var entity = _mapper.Map<TEntity>(createRequest);
            entity.CreatedUser = fullName;
            entity.CreatedTime = DateTime.UtcNow;
            entity.IsDeleted = false;
            return entity;
        }

        public virtual TEntity MapEntityUpdateRequestToEntiy(TEntityUpdateRequest updateRequest, Guid userId, string fullName)
        {
            var entity = _mapper.Map<TEntity>(updateRequest);
            entity.ModifiedUser = fullName;
            entity.ModifiedTime = DateTime.UtcNow;
            return entity;
        }

        public virtual TEntityViewModel MapEntityToEntityViewModel(TEntity entity)
        {
            var entityViewModel = _mapper.Map<TEntityViewModel>(entity);
            return entityViewModel;
        }
        #endregion
    }
}
