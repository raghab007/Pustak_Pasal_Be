
using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CreateReviewDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid BookId { get; set; }
    }
