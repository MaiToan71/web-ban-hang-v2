using Project.Enums;
using Project.ViewModels;
using Project.ViewModels.Orders;

namespace Project.Services.Orders
{
    public interface IOrderService
    {
        Task<PagedResult<OrderDTOViewModel>> GetAllPaging(GetOrderPagingRequest request);
        OrderDTOViewModel GetById(int id);
        Task<bool> AddNew(OrderCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(OrderUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
        Task<PagedResult<OrderDetailDTOViewModel>> GetAllOrderDetailPaging(GetOrderPagingRequest request);
        Task<bool> SendWorkflow(int OrderId, Workflow workflow);
    }
}
