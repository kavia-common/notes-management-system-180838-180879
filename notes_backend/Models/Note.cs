using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Models
{
    /// <summary>
    /// Represents a Note resource.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Unique identifier for the note.
        /// </summary>
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Title of the note. Required. Max length 256.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content/body of the note. Optional. Max length 5000.
        /// </summary>
        [MaxLength(5000)]
        public string? Content { get; set; }

        /// <summary>
        /// UTC timestamp when the note was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the note was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
