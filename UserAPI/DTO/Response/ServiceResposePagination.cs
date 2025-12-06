namespace UserAPI.DTO.Response
{
    public class ServiceResposePagination <T>
    {
        public List<T> data { get; set; }
        public int total { get; set; }
        public int count { get; set; }
    }
}
