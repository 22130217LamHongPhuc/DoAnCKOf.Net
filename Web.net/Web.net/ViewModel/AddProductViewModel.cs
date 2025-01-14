using System.ComponentModel.DataAnnotations;

namespace API.Net.ViewModel;
[Serializable]

public class AddProductViewModel
{
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public int Price { get; set; }

        [Range(0, 100, ErrorMessage = "Giảm giá mặc định phải từ 0 đến 100")]
        public int DiscountDefault { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Mã danh mục con là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã danh mục con không được vượt quá 50 ký tự")]
        public string SubCategoryID { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải là số không âm")]
        public int QuantityStock { get; set; }

        [Required(ErrorMessage = "Số lượng đã bán là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng đã bán phải là số không âm")]
        public int QuantitySell { get; set; }

        [Url(ErrorMessage = "Hình ảnh sản phẩm phải là một URL hợp lệ")]
        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 255 ký tự")]
        public string Thumbnail { get; set; }

        [StringLength(255, ErrorMessage = "Kích thước không được vượt quá 255 ký tự")]
        public string Dimensions { get; set; }

        [StringLength(255, ErrorMessage = "Chất liệu không được vượt quá 255 ký tự")]
        public string Material { get; set; }

        [StringLength(100, ErrorMessage = "Nguồn gốc không được vượt quá 100 ký tự")]
        public string Original { get; set; }

        [StringLength(255, ErrorMessage = "Tiêu chuẩn không được vượt quá 255 ký tự")]
        public string Standard { get; set; }
        public List<string> SubImg { get; set; }
}


