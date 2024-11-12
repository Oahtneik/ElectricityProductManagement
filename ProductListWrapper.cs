using SalesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager
{
    public class ProductListWrapper
    {
        private readonly QlbanHangContext _context;

        public ProductListWrapper(QlbanHangContext context)
        {
            _context = context;
        }

        // Phương thức để lấy danh sách sản phẩm từ database
        public object GetProductListForNavigation()
        {
            // Truy xuất danh sách sản phẩm từ database
            var productList = _context.SanPhams.ToList();

            // Thực hiện chuyển đổi ProductList thành đối tượng mà NavigationServices yêu cầu, nếu cần
            return productList; // Hoặc thay đổi thành kiểu dữ liệu phù hợp cho NavigationServices
        }
    }


}
