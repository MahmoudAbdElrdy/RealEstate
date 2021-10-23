namespace RealEstate.DataAccess
{
    public class ResponseData
    {
        public EResponse Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
        public dynamic Data { get; set; }
        public int? TotalRecordsCount { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public int? PageCount { get; set; }
     
    }

    public enum EResponse
    {
        OK,
        Unauthorized,
        NoPermission,
        NoData,
        ValidationError,
        UnSuccess,
        UnexpectedError
    }
}

