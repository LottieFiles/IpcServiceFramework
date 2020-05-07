﻿using System;
using System.Runtime.Serialization;

namespace JKang.IpcServiceFramework
{
    [DataContract]
    public class IpcResponse
    {
        public static IpcResponse Success(object data)
            => new IpcResponse(IpcStatus.Ok, data, null, null);

        public static IpcResponse BadRequest()
            => new IpcResponse(IpcStatus.BadRequest, null, null, null);

        public static IpcResponse BadRequest(string errorDetails)
            => new IpcResponse(IpcStatus.BadRequest, null, errorDetails, null);

        public static IpcResponse BadRequest(string errorDetails, Exception innerException)
            => new IpcResponse(IpcStatus.BadRequest, null, errorDetails, innerException);

        public static IpcResponse InternalServerError()
            => new IpcResponse(IpcStatus.InternalServerError, null, null, null);

        public static IpcResponse InternalServerError(string errorDetails)
            => new IpcResponse(IpcStatus.BadRequest, null, errorDetails, null);

        public static IpcResponse InternalServerError(string errorDetails, Exception innerException)
            => new IpcResponse(IpcStatus.BadRequest, null, errorDetails, innerException);

        public IpcResponse(
            IpcStatus status,
            object data,
            string errorMessage,
            Exception innerException)
        {
            Status = status;
            Data = data;
            ErrorMessage = errorMessage;
            InnerException = innerException;
        }

        [DataMember(Order = 0)]
        public IpcStatus Status { get; set; }

        [DataMember(Order = 10)]
        public dynamic Data { get; set; }

        [DataMember(Order = 20)]
        public string ErrorMessage { get; set; }

        [DataMember(Order = 30)]
        public Exception InnerException { get; set; }

        public bool Succeed() => Status == IpcStatus.Ok;

        /// <summary>
        /// Create an exception that contains error information
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the status is doesn't represent any error</exception>
        public IpcFaultException CreateFaultException()
        {
            if (Status <= IpcStatus.Ok)
            {
                throw new InvalidOperationException("The response doesn't contain any error");
            }

            return new IpcFaultException(Status, ErrorMessage, InnerException);
        }
    }
}