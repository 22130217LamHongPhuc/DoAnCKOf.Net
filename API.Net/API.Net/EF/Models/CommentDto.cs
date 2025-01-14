namespace API.Net.EF.Models
{
    public class CommentDto
    {
        public int UserId { get; set; }
        public string IdProduct { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
