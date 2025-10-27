using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Models
{
    /// <summary>
    /// Request model for creating a note.
    /// </summary>
    public class CreateNoteRequest
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string? Content { get; set; }
    }

    /// <summary>
    /// Request model for updating a note.
    /// </summary>
    public class UpdateNoteRequest
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string? Content { get; set; }
    }
}
