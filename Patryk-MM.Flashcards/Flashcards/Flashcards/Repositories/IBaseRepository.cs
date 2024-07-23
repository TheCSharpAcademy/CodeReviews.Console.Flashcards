using Flashcards.Models;

namespace Flashcards.Repositories {
    public interface IBaseRepository<T> where T : BaseEntity {
        public Task<T> GetByIdAsync(int id);

    }
}
