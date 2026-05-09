using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;

namespace Tikka.ServicesAustralia.Core.Models
{
    public class ApiResponse<T>()
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public List<AppException>? Errors { get; set; }

        // Static helper for success responses
        public static ApiResponse<T> SuccessResponse(T data)
            => new() { Success = true, StatusCode = 200, Data = data };

        // Static helper for failure responses
        public static ApiResponse<T> FailureResponse(int statusCode, List<AppException>? errors = null)
            => new() { Success = false, Errors = errors };
    }
}
