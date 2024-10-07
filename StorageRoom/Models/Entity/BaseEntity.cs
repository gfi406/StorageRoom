namespace StorageRoom.Models.Entity
{
    public class BaseEntity
    {
        // Уникальный идентификатор
        public Guid Id { get; private set; }

        // Конструктор, который автоматически генерирует новый идентификатор
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
