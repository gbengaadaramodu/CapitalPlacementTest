﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application
{
    public class APIResponseContent
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }
}
