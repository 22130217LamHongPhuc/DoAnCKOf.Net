namespace API.Net.ViewModel
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; } // ID của trạng thái mới
    }
}
