﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Retail.Models.ViewModels
{
    public class ProductVM
    {
            public Product Product { get; set; }

        // IEnumberable to hold product in Dropdown
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; } //Name is same as in ProductController


    }
}
