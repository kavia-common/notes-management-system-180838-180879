using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    // PUBLIC_INTERFACE
    public interface INotesRepository
    {
        /// <summary>
        /// Retrieve all notes.
        /// </summary>
        IEnumerable<Note> GetAll();

        /// <summary>
        /// Retrieve a note by Id.
        /// </summary>
        Note? GetById(Guid id);

        /// <summary>
        /// Create a new note.
        /// </summary>
        Note Create(Note note);

        /// <summary>
        /// Update an existing note.
        /// </summary>
        bool Update(Note note);

        /// <summary>
        /// Delete a note by Id.
        /// </summary>
        bool Delete(Guid id);
    }
}
