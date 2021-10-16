using System;
using System.Net;
using System.Runtime.Serialization;

namespace CalorieTracker.Models {
    [Serializable]
    public class CustomException : Exception {
        internal CustomException(string message)
            : this(HttpStatusCode.BadRequest, message) {
        }

        internal CustomException(HttpStatusCode httpStatusCode, string message)
            : base(message) {
            HttpStatusCode = httpStatusCode;
        }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        public HttpStatusCode HttpStatusCode { get; }
    }
}
