using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.ViewModel
{
    public class SearchCategoryRollUpViewModel
    {
        public int ParentCategoryID { get; set; }
        public string ParentName { get; set; }
        public int ChildCategoryID { get; set; }
        public string ChildName { get; set; }
    }
}
