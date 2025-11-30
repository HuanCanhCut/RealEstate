namespace UserAPI.DTO.Response
{
    public class MetaPagination
    {
        public Pagination pagination { get; set; }

        public MetaPagination(int total, int count, int current_page, int per_page)
        {
            this.pagination = new Pagination(total, count, current_page, per_page);
        }
    }
}