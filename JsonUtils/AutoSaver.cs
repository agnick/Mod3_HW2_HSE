namespace JsonUtils
{
    public class AutoSaver
    {
        /* Fields */
        // Private field for storing a collection of authors.
        private List<Author> _authors;
        // Private field for storing the path to the file.
        private string _originalFilePath;
        // Private field for storing the time of the last event.
        private DateTime _lastEventTime = DateTime.MinValue;
        // Private field for storing the time interval between writing to a file.
        private TimeSpan _maxInterval = TimeSpan.FromSeconds(15);
        // Private field to store the state of the first event.
        private bool _isFirstEventHandled = false;

        // Constructor to initialize an empty instance of the AutoSaver class.
        public AutoSaver() : this(string.Empty, new List<Author>()) { }

        // Constructor to initialize an instance of the AutoSaver class with provided values. 
        public AutoSaver(string originalFilePath, List<Author> authors)
        {
            // Constructor with parameters, validating and assigning values.
            if (originalFilePath is null) throw new ArgumentNullException("Переданное имя файла равно null");
            if (authors == null) throw new ArgumentNullException("Переданная коллекция классов равна null");

            _originalFilePath = originalFilePath;
            _authors = authors;
        }

        /// <summary>
        /// Method for subscribing the Author class to the Updated event.
        /// </summary>
        /// <param name="author">Author class that will be subscribed to the event.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided book object is null.</exception>
        public void SubscribeToAuthor(Author author)
        {
            if (author is null) throw new ArgumentNullException("Переданный класс равен null");

            // Subscribing to the event.
            author.Updated += HandleUpdate;
        }

        /// <summary>
        /// Method for subscribing the Book class to the Updated event.
        /// </summary>
        /// <param name="book">Book class that will be subscribed to the event</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided book object is null.</exception>
        public void SubscribeToBook(Book book)
        {
            if (book is null) throw new ArgumentNullException("Переданный класс равен null");

            // Subscribing to the event.
            book.Updated += HandleUpdate;
        }

        /// <summary>
        /// The method that handles the Updated event.
        /// Writes the current state of the Author collection to a file when receiving two events with a difference of no more than 15 seconds.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing the update time.</param>
        /// <exception cref="Exception">Thrown when an error occurs during saving to the JSON file.</exception>
        private void HandleUpdate(object? sender, UpdatedEventArgs e)
        {
            if (sender == null) return;

            /* 
                RU:
                Проблема потери данных из-за условия с записью в tmp файл при получении двух событий с разницей не более 15 секунд.
                Каждая книга является уникальным классом, при изменении поля _earnings у книги пересчитывается доход каждого автора, который содержит данную книгу. 
                После изменения поля _earnings книги будут вызваны следующие события: 
                1) Updated для Book (не будет обработано, если прошло более 15 секунд с предыдущего события Updated (не имеет значения у Author или Book)).
                2) EarningsUpdated для Book.
                3...n) Updated для Author, содержащий данную книгу.
                Назовем это стеком событий, в условии ясно не сказано должен ли блокироваться весь стек, если прошло более 15 секунд с предыдущего события Updated, 
                или только первое событие Updated (3...n) стека (в этом случае произойдет потеря данных при записи в файл, тк доход одного из авторов не пересчитается).
                Для решения этой проблемы, мы вызываем событие 1), которое будет блокироваться (некая заглушка), если прошло более 15 секунд с предыдущего события Updated, 
                благодаря этому доходы авторов корректно пересчитаются и будут записаны в tmp файл без потери данных.
                Для одиночных событий все работает корректно, по условию.

                ENG:
                The problem of data loss due to the condition of writing to the tmp file when receiving two events with a difference of no more than 15 seconds.
                Each book is a unique class, and when the _earnings field of a book changes, the income of each author who contains this book is recalculated.
                After changing the _earnings field of the book, the following events will be triggered:
                1) Updated for Book (will not be processed if more than 15 seconds have passed since the previous Updated event (does not matter for Author or Book)).
                2) EarningsUpdated for Book.
                3...n) Updated for Author, containing this book.
                Let's call this a stack of events. The condition does not clearly state whether the entire stack should be blocked if more than 15 seconds have passed since the previous Updated event, 
                or only the first Updated event (3...n) of the stack (in this case, there will be data loss when writing to the file because the income of one of the authors is not recalculated).
                To solve this problem, we trigger event 1), which will be blocked (some kind of placeholder) if more than 15 seconds have passed since the previous Updated event. 
                Thanks to this, the authors' incomes will be recalculated correctly and will be written to the tmp file without data loss.
                For single events, everything works correctly according to the condition.
            */

            // If it's the first event or the time difference between this and the last event is less than or equal to the maximum interval, proceed.
            if (!_isFirstEventHandled ||
                (e.UpdateTime - _lastEventTime) <= _maxInterval)
            {
                // Checking for exceptions.
                try
                {
                    // Save the current state of the Author collection to a JSON file.
                    JsonWriter.SaveToJsonFile(_originalFilePath, _authors, true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                // Set the flag indicating the first event has been handled.
                _isFirstEventHandled = true;

                Console.WriteLine($"Произошло сохранения текущей коллекции в tmp файл. Время сохранения: {e.UpdateTime}. Объект: {sender.GetType()}");
            }

            // Update the last event time to the current event time.
            _lastEventTime = e.UpdateTime;
        }
    }
}