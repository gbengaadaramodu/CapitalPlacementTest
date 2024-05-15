using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Core.Application
{
    public class ApiResponse
    {
        public string Message;
        public HttpStatusCode StatusCode;
        public object Content;
        public bool Success;
        public int TotalSize;

        public ApiResponse(string message, HttpStatusCode statusCode = HttpStatusCode.OK, bool error = false)
        {
            Message = message;
            StatusCode = statusCode;
            Success = error;
        }
        public ApiResponse(string message, HttpStatusCode statusCode = HttpStatusCode.OK, object content = null, bool error = false)
        {
            Message = message;
            StatusCode = statusCode;
            Content = content;
            Success = error;
        }
        public ApiResponse(string message, HttpStatusCode statusCode = HttpStatusCode.OK, object content = null, bool error = false, int totalSize = 0)
        {
            Message = message;
            StatusCode = statusCode;
            Content = content;
            Success = error;
            TotalSize = totalSize;
        }

        public ApiResponse()
        {

        }
    }
}
