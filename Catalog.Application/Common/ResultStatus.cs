namespace e_bookshop.Catalog.Application.Common
{
    public enum ResultStatus
    {
        Success = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        PartialContent = 206,
        BadRequest = 400,
        Conflict = 409,
        NotFound = 404,
        Forbidden = 403,
        TooManyRequests = 429,
        InternalServerError = 500
    }
}
