
namespace WebSharp
{
    public enum HttpStatusCode
    {
        /// <summary> HTTP Status-Code: 0 (Communication with Server failed) </summary>
        CommunicationFailed = 0,

        /// <summary> HTTP Status-Code: 200 (OK) </summary>
        OK = 200,

        /// <summary> HTTP Status-Code: 201 (Created) </summary>
        Created = 201,

        /// <summary> HTTP Status-Code: 202 (Accepted) </summary>
        Accepted = 202,

        /// <summary> HTTP Status-Code: 203 (Non-Authoritative Information) </summary>
        NotAuthoritative = 203,

        /// <summary> HTTP Status-Code: 204 (No Content) </summary>
        NoContent = 204,

        /// <summary> HTTP Status-Code: 205 (Reset Content) </summary>
        Reset = 205,

        /// <summary> HTTP Status-Code: 206 (Partial Content) </summary>
        Partial = 206,

        /// <summary> HTTP Status-Code: 300 (Multiple Choices) </summary>
        MultChoice = 300,

        /// <summary> HTTP Status-Code: 301 (Moved Permanently) </summary>
        MovedPerm = 301,

        /// <summary> HTTP Status-Code: 302 (Temporary Redirect) </summary>
        MovedTemp = 302,

        /// <summary> HTTP Status-Code: 303 (See Other) </summary>
        SeeOther = 303,

        /// <summary> HTTP Status-Code: 304 (Not Modified) </summary>
        NotModified = 304,

        /// <summary> HTTP Status-Code: 305 (Use Proxy) </summary>
        UseProxy = 305,

        /// <summary> HTTP Status-Code: 400 (Bad Request) </summary>
        BadRequest = 400,

        /// <summary> HTTP Status-Code: 401 (Unauthorized) </summary>
        ServerError = 401,

        /// <summary> HTTP Status-Code: 402 (Payment Required) </summary>
        PaymentRequired = 402,

        /// <summary> HTTP Status-Code: 403 (Forbidden) </summary>
        Forbidden = 403,

        /// <summary> HTTP Status-Code: 404 (Not Found) </summary>
        NotFound = 404,

        /// <summary> HTTP Status-Code: 405 (Method Not Allowed) </summary>
        BadMethod = 405,

        /// <summary> HTTP Status-Code: 406 (Not Acceptable) </summary>
        NotAcceptable = 406,

        /// <summary> HTTP Status-Code: 407 (Proxy Authentication Required) </summary>
        ProxyAuth = 407,

        /// <summary> HTTP Status-Code: 408 (Request Time-Out) </summary>
        ClientTimeout = 408,

        /// <summary> HTTP Status-Code: 409 (Conflict) </summary>
        Conflict = 409,

        /// <summary> HTTP Status-Code: 410 (Gone) </summary>
        Gone = 410,

        /// <summary> HTTP Status-Code: 411 (Length Required) </summary>
        LengthRequired = 411,

        /// <summary> HTTP Status-Code: 412 (Precondition Failed) </summary>
        PreconFailed = 412,

        /// <summary> HTTP Status-Code: 413 (Request Entity Too Large) </summary>
        EntityTooLarge = 413,

        /// <summary> HTTP Status-Code: 414 (Request-URI Too Large) </summary>
        ReqTooLong = 414,

        /// <summary> HTTP Status-Code: 415 (Unsupported Media Type) </summary>
        UnsupportedType = 415,

        /// <summary> HTTP Status-Code: 500 (Internal Server Error) </summary>
        InternalError = 500,

        /// <summary> HTTP Status-Code: 501 (Not Implemented) </summary>
        NotImplemented = 501,

        /// <summary> HTTP Status-Code: 502 (Bad Gateway) </summary>
        BadGateway = 502,

        /// <summary> HTTP Status-Code: 503 (Service Unavailable) </summary>
        Unavailable = 503,

        /// <summary> HTTP Status-Code: 504 (Gateway Timeout) </summary>
        GatewayTimeout = 504,

        /// <summary> HTTP Status-Code: 505 (HTTP Version Not Supported) </summary>
        Version = 505,

    }
}
