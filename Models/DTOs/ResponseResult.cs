using System.Collections.Generic;

namespace TodoApp.Models.DTOs
{
   public class ResponseResult<T>
   {
      public bool Success { get; set; }
      public List<string> Errors { get; set; }
        public T Payload { get; set; }
   }
}
