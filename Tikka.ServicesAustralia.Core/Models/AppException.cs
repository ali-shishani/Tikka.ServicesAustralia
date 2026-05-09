using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Core.Models
{
    public class AppException(string message, string? details)
    {
        public string Message { get; set; } = message;
        public string? Details { get; set; } = details;
    }
}
