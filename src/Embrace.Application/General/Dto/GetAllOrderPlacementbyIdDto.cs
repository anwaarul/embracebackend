﻿using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllOrderPlacementbyIdDto 
    {
        public  long OrderId { get; set; }
        public string Message { get; set; }
       
    }
}
