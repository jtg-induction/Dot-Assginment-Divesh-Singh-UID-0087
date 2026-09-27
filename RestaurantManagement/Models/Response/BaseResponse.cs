using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
  /// <summary>
  /// base/generic response used to send response
  /// </summary>
  /// <typeparam name="T"></typeparam>
    public class BaseResponse<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}