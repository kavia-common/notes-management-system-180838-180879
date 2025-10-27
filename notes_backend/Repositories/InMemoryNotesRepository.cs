using System.Collections.Concurrent;
using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    /// <summary>
    /// Thread-safe in-memory repository for storing notes during runtime.
    /// </summary>
    public class InMemoryNotesRepository : INotesRepository
    {
        private readonly ConcurrentDictionary<Guid, Note> _notes = new();

        public InMemoryNotesRepository()
        {
            // Seed a sample note for convenience
            var sample = new Note
            {
                Title = "Welcome Note",
                Content = "This is a sample note created at startup.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _notes[sample.Id] = sample;
        }

        public IEnumerable<Note> GetAll() => _notes.Values.OrderByDescending(n => n.UpdatedAt);

        public Note? GetById(Guid id) => _notes.TryGetValue(id, out var note) ? note : null;

        public Note Create(Note note)
        {
            note.Id = note.Id == Guid.Empty ? Guid.NewGuid() : note.Id;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = note.CreatedAt;
            _notes[note.Id] = note;
            return note;
        }

        public bool Update(Note note)
        {
            if (!_notes.ContainsKey(note.Id)) return false;

            note.UpdatedAt = DateTime.UtcNow;
            _notes[note.Id] = note;
            return true;
        }

        public bool Delete(Guid id) => _notes.TryRemove(id, out _);
    }
}
