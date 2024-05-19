namespace JsonUtils.DTOs
{
    /* 
       Dto of the Author class for correct data serialization. 
       This will prevent errors when classes call each other cyclically (due to association) 
    */
    public class AuthorDto
    {
        public string AuthorId { get; set; }
        public string Name { get; set; }
        public decimal Earnings { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}