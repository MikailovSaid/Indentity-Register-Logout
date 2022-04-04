﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.ViewModels
{
    public class BasketDetailVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        

    }
}
