namespace AlishPustakGhar.Dtos;

public class BookResponseDto
{
    public List<BookViewDto> Books { get; set; }
    
    public int TotalCount { get; set; }
}