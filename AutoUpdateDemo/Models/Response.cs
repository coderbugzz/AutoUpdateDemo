﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Models
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string message { get; set; }
        public T Data { get; set; }
    }
}
