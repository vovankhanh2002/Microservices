﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.ShareLibary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
    }
}
