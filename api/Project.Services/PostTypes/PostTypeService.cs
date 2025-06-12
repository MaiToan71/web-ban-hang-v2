using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Utilities.Exceptions;
using Project.ViewModel.PostTypes;
using Project.ViewModels;

namespace Project.Services.PostTypes
{
    public class PostTypeService : IPostTypeService
    {
        private readonly ProductDbContext _context;
        public PostTypeService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<PostTypeViewModel> FindById(int Id)
        {
            var category = await _context.PostTypes.FirstOrDefaultAsync(m => m.Id == Id);
            if (category == null)
            {
                throw new ProductException("Cannot find Category");
            }
            var item = new PostTypeViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SortOrder = category.SortOrder,
                IsShowOnHome = category.IsShowOnHome,
                ParentId = category.ParentId,
                Status = category.Status,
                IsDelete = category.IsDeleted,
            };
            return item;
        }

        public async Task<int> Add(PostTypeCreateRequest request)
        {
            var category = new PostType()
            {
                Name = request.Name,
                SortOrder = request.SortOrder,
                IsShowOnHome = request.IsShowOnHome,
                ParentId = request.ParentId,
                PostTypeEnum = request.PostTypeEnum,
                Status = request.Status,
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
            };
            if (request.Description != null)
            {
                category.Description = request.Description;

            }
            if (request.Phonenumber != null)
            {
                category.Phonenumber = request.Phonenumber;

            }
            _context.PostTypes.Add(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int Id)
        {
            var category = await _context.PostTypes.FindAsync(Id);
            if (category == null)
            {
                throw new ProductException("Cannot find Category");
            }
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<PagedResult<PostTypeViewModel>> GetAllPaging(GetPostTypePagingRequest request)
        {
            var query = _context.PostTypes
                .Where(m => m.IsDeleted == false)
                .OrderBy(m => m.SortOrder)
               .AsQueryable();
            // 2.Filter
            if (request.PostTypeEnum != null)
            {
                query = query.Where(m => m.PostTypeEnum == request.PostTypeEnum);
            }
            if (request.SortOrder != null)
            {
                if (request.SortOrder == true)
                {
                    query = query.OrderBy(m => m.SortOrder);
                }
            }

            if (request.Status != null)
            {
                query = query.Where(m => m.Status == request.Status);

            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(m => m.Name.Contains(request.Name));

            }


            //3. Paging
            int totalRow = await query.CountAsync();
            dynamic data;
            dynamic pagedResult;
            if (request.IsAll != null)
            {
                if (request.IsAll == true)
                {
                    data = await query.ToListAsync();
                    //4 .Select and projecttion
                    pagedResult = new PagedResult<PostTypeViewModel>()
                    {
                        TotalRecord = totalRow,
                        Items = data,
                    };
                    return pagedResult;

                }

            }

            data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                   .Take(request.PageSize).Select(i => new PostTypeViewModel
                   {
                       TotalOfPosts = request.Workflow != null ? i.Posts.Where(p => p.WorkflowId == request.Workflow).Count() : i.Posts.Count(),
                       Id = i.Id,
                       Name = i.Name,
                       SortOrder = i.SortOrder,
                       Description = i.Description,
                       IsShowOnHome = i.IsShowOnHome,
                       ParentId = i.ParentId,
                       Status = i.Status,
                       Phonenumber = i.Phonenumber,
                       PostTypeEnum = i.PostTypeEnum
                   })
                   .ToListAsync();

            //4 .Select and projecttion
            pagedResult = new PagedResult<PostTypeViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,

            };
            return pagedResult;
        }


        public async Task<int> Update(PostTypeUpdateRequest request)
        {
            var category = await _context.PostTypes.FindAsync(request.Id);
            if (category == null)
            {
                throw new ProductException("Cannot find Category");
            }
            category.Name = request.Name;
            category.ModifiedTime = DateTime.Now;
            category.PostTypeEnum = request.PostTypeEnum;
            if (request.ParentId != null)
            {
                category.ParentId = (int)request.ParentId;
            }
            if (request.Status != null)
            {
                category.Status = (Status)request.Status;
            }
            if (request.IsShowOnHome != null)
            {
                category.IsShowOnHome = (bool)request.IsShowOnHome;
            }
            if (request.SortOrder != null)
            {
                category.SortOrder = (int)request.SortOrder;
            }
            if (request.Description != null)
            {
                category.Description = request.Description;
            }
            if (request.Phonenumber != null)
            {
                category.Phonenumber = request.Phonenumber;
            }

            await _context.SaveChangesAsync();
            return 1;
        }

    }
}
