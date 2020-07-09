using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Service.Utilities
{
    public class RequestActionResult<T> where T : class
    {
        public T Entity { get; private set; }
        public ActionStatus Status { get; private set; }

        public Exception Exception { get; private set; }
        public string Message { get; private set; }


        public RequestActionResult(T entity, ActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        public RequestActionResult(T entity, ActionStatus status, Exception exception) : this(entity, status)
        {
            Exception = exception;
        }
        public RequestActionResult(T entity, ActionStatus status, string message) : this(entity, status)
        {
            Message = message;
        }
    }
}
