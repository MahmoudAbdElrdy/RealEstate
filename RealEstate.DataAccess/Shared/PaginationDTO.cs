namespace RealEstate.DataAccess
{
    public class PaginationDto
    {
        public PaginationDto()
        {
            PageNumber = 1;
            PageSize = 20;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //  public int skip { get { return pageNumber * pageSize; } }
    }
}
