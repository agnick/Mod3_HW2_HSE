namespace JsonUtils.DTOs
{
    /* 
       Dto of the Book class for correct data serialization. 
       This will prevent errors when classes call each other cyclically (due to association) 
    */
    public class BookDto
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
        public decimal Earnings { get; set; }
    }
}