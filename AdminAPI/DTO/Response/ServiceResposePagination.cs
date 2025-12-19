namespace AdminAPI.DTO.Response
{
    public class ServiceResponsePagination<T>
    {
        public List<T> data { get; set; }
        public int total { get; set; }
        public int count { get; set; }
    }
}
