﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTOs
{
    public class UpdateUserRequest
    {
        public required string FullName { get; set; }
    }
}
