using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SalesManager
{
    //class CustomNavigationService : NavigationService
    //{
    //    protected override void OnContentReady(ContentType contentType, object bp, Uri bpu, object navState)
    //    {
    //        // Thực hiện các thay đổi hoặc kiểm tra bổ sung ở đây
    //        if (!IsValidRootElement(bp))
    //        {
    //            throw new InvalidOperationException($"Root element {bp?.GetType().Name} is not valid for navigation.");
    //        }

    //        // Gọi phương thức gốc để duy trì các chức năng mặc định
    //        base.OnContentReady(contentType, bp, bpu, navState);
    //    }

    //    private bool IsValidRootElement(object bp)
    //    {
    //        bool result = true;
    //        if (!AllowWindowNavigation && bp != null && bp is Window)
    //        {
    //            result = false;
    //        }
    //        return result;
    //    }
    //}
}
