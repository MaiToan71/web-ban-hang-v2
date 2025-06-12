using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.Orders;

namespace Project.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddOrderDetail(OrderDetailRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.OrderDetail>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.OrdersDetail.Add(item);
            var resId = await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddNew(OrderCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Order>(request);
            item.CreatedUser = CreatedUser;
            item.ModifiedId = CreatedUserId;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.Orders.Add(item);
            var resId = await _context.SaveChangesAsync();
            foreach (var detail in request.OrderDetails)
            {
                detail.OrderId = item.Id;
                await AddOrderDetail(detail, CreatedUserId, CreatedUser);

            }

            return resId > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Orders.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Orders.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<PagedResult<OrderDetailDTOViewModel>> GetAllOrderDetailPaging(GetOrderPagingRequest request)
        {
            //1. query
            var orders = _context.Orders.Where(m => m.Workflow == Workflow.COMPLETED).Where(m => m.IsDeleted == false);

            var query = _context.OrdersDetail.Where(m => m.IsDeleted == false && orders.Select(m => m.Id).Contains(m.OrderId)).OrderByDescending(m => m.Id).AsNoTracking();
            if (request.ProductId != null)
            {
                query = query.Where(m => m.ProductId == request.ProductId);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<OrderDetailDTOViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
            };
            foreach (var d in datapaging.Data)
            {
                var input = new OrderDetailDTOViewModel
                {
                    OrderDetail = d
                };
                var order = orders.FirstOrDefault(m => m.Id == d.OrderId);
                if (order != null)
                {
                    input.Order = order;
                }
                ;
                pagedResult.Items.Add(input);
            }
            await Task.CompletedTask;
            return pagedResult;
        }
        public async Task<PagedResult<OrderDTOViewModel>> GetAllPaging(GetOrderPagingRequest request)
        {
            //1. query
            var query = _context.Orders.Where(m => m.IsDeleted == false).OrderByDescending(m => m.Id).AsNoTracking();
            if (request.UserId != null)
            {
                query = query.Where(m => m.UserId == request.UserId);
            }
            if (request.OrderType != null)
            {
                query = query.Where(m => m.OrderType == request.OrderType);
            }
            if (request.FromDateOrder != null && request.ToDateOrder != null)
            {
                query = query.Where(m => m.DateOrder >= request.FromDateOrder && m.DateOrder <= request.ToDateOrder);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<OrderDTOViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
            };
            var users = _context.AppUsers.Select(m => new UserDetailDTOViewModel
            {
                FullName = m.FullName,
                Id = m.Id,
                UserId = m.Id,
            }).Where(m => datapaging.Data.Select(d => d.UserId).Contains(m.Id)).ToList();

            var orderDetails = await _context.OrdersDetail.Where(m => datapaging.Data.Select(d => d.Id).Contains(m.OrderId)).ToListAsync();
            var products = _context.Products.Where(m => orderDetails.Select(o => o.ProductId).Contains(m.Id)).ToList();
            foreach (var i in datapaging.Data)
            {
                var input = new OrderDTOViewModel()
                {
                    Order = i

                };
                var checkUser = users.FirstOrDefault(m => m.Id == i.UserId);
                if (checkUser != null)
                {
                    input.User = checkUser;
                }

                var orderDetailItems = orderDetails.Where(m => m.OrderId == i.Id);
                List<ProductDetailDTOViewModel> resOrderDetails = new List<ProductDetailDTOViewModel>();
                foreach (var o in orderDetailItems)
                {
                    var resOrderInput = new ProductDetailDTOViewModel()
                    {
                        OrderDetail = o
                    };
                    var product = products.FirstOrDefault(m => m.Id == o.ProductId);
                    if (product != null)
                    {
                        resOrderInput.Product = new ProductDTOViewModel
                        {
                            Id = product.Id,
                            Title = product.Title,
                        };
                    }
                    resOrderDetails.Add(resOrderInput);
                }
                input.OrderDetails = resOrderDetails;
                pagedResult.Items.Add(input);
            }

            await Task.CompletedTask;
            return pagedResult;
        }

        public OrderDTOViewModel GetById(int id)
        {
            var response = new OrderDTOViewModel();
            var order = _context.Orders.Find(id);
            if (order == null) throw new ProductException($"Cannot find a Item with id: {id}");
            response.Order = order;
            var orderDetails = _context.OrdersDetail.Where(m => m.OrderId == id).ToList();
            var products = _context.Products.Where(m => orderDetails.Select(o => o.ProductId).Contains(m.Id)).ToList();
            var resOrderDetails = new List<ProductDetailDTOViewModel>();
            foreach (var item in orderDetails)
            {
                var detail = new ProductDetailDTOViewModel();
                var product = products.FirstOrDefault(m => m.Id == item.ProductId);
                if (product != null)
                {

                    detail.Product = new ProductDTOViewModel
                    {
                        Id = item.ProductId,
                        Title = product.Title,
                    };


                }
                detail.OrderDetail = item;
                resOrderDetails.Add(detail);

            }
            response.OrderDetails = resOrderDetails;
            var checkUser = _context.AppUsers.Select(m => new UserDetailDTOViewModel
            {
                FullName = m.FullName,
                Id = m.Id,
                UserId = m.Id,
            }).FirstOrDefault(m => m.Id == order.UserId);
            if (checkUser != null)
            {
                response.User = checkUser;
            }
            return response;
        }

        public async Task<bool> Update(OrderUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Order>(request);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            //  item.CreatedId = CreatedUserId;
            _context.Orders.Update(item);
            var resId = await _context.SaveChangesAsync();
            // var details = await _context.OrdersDetail.Where(m => m.OrderId == item.Id).ToListAsync();
            //if (details.Count > 0)
            //{
            //    _context.OrdersDetail.RemoveRange(details);
            //    _context.SaveChanges();
            //}
            //foreach (var detail in request.OrderDetails)
            //{
            //    detail.OrderId = request.Id;
            //    await AddOrderDetail(detail, CreatedUserId, CreatedUser);

            //}
            return resId > 0;
        }

        public async Task<bool> SendWorkflow(int OrderId, Workflow workflow)
        {
            var item = _context.Orders.FirstOrDefault(m => m.Id == OrderId);
            if (item == null)
            {
                return false;
            }

            item.Workflow = workflow;
            await UpdateQuantityInProduct(item);
            return true;
        }
        public async Task<int> UpdateQuantityInProduct(Order order)
        {
            var orderDetails = await _context.OrdersDetail.Where(m => m.OrderId == order.Id).ToListAsync();
            var productIds = orderDetails.Select(o => o.ProductId).ToList();
            var products = await _context.Products.Where(m => productIds.Contains(m.Id)).ToListAsync();
            List<Product> productList = new List<Product>();
            foreach (var i in products)
            {
                decimal quantity = orderDetails.Where(m => m.ProductId == i.Id).Sum(o => o.Quantity);
                if (order.Workflow == Workflow.COMPLETED && order.OrderType == OrderType.Input)
                {
                    i.Quantity += quantity;
                }
                if (order.Workflow == Workflow.CANCEL && order.OrderType == OrderType.Input)
                {
                    i.Quantity -= quantity;
                    //   i.SellingPrice += quantity;
                }

                if (order.Workflow == Workflow.COMPLETED && order.OrderType == OrderType.Output)
                {
                    i.Quantity -= quantity;
                    i.QuantitySold += quantity;
                }
                if (order.Workflow == Workflow.CANCEL && order.OrderType == OrderType.Output)
                {
                    i.Quantity += quantity;
                    i.QuantitySold -= quantity;
                }
                productList.Add(i);
            }

            _context.Products.UpdateRange(productList);
            _context.SaveChanges();



            return 1;
        }
    }
}
