﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.ViewModel
{
    public class SearchCategoryRollUpViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentCategoryID { get; set; }
        public int ChildCategoryID { get; set; }
    }
}
