﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class PhongViewModelSearch : PageViewModel
    {
        public string? name { get; set; }
        public int? categoryId { get; set; }
        public int differentiate { get; set; }
        public int status { get; set; }
        public int loaiphong { get; set; }
    }
}
