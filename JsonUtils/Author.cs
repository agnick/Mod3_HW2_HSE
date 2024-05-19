using System.Globalization;
using System.Text.Json.Serialization;

namespace JsonUtils
{
    public class Author
    {
        /* Events */
        // Event to notify when author information is updated.
        public event EventHandler<UpdatedEventArgs>? Updated;

        /* Fields */
        // Using JsonPropertyName for correct deserializatian.
        [JsonPropertyName("authorId")]
        private string _authorId;
        [JsonPropertyName("name")]
        private string _name;
        [JsonPropertyName("earnings")]
        private decimal _earnings;
        [JsonPropertyName("books")]
        private readonly List<Book> _books;

        /* Properties */
        // Property to access the author's ID.
        public string AuthorId { get { return _authorId; } }
        // Property to access or set the author's name.
        public string Name
        {
            get { return _name; }
            set
            {
                // Setter for the author's name, triggers an update event.
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Переданное имя пустое или равно null");

                _name = value;

                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
            }
        }
        // Property to access or set the author's earnings.
        public decimal Earnings
        {
            get { return _earnings; }
            set
            {
                // Setter for the author's earnings, triggers an update event.
                if (value < 0) throw new ArgumentOutOfRangeException("Переданное значение доходов автора меньше нуля");

                _earnings = value;

                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
            }
        }
        // Property to access the list of books authored by the author.
        public List<Book> Books { get { return new List<Book>(_books); } }

        // Constructor to initialize an empty instance of the Author class.
        public Author()
        {
            _authorId = string.Empty;
            _name = string.Empty;
            _earnings = 0;
            _books = new List<Book>();
        }

        // Constructor to initialize an instance of the Author class with provided values.
        // Using JsonConstructor for correct deserializatian.
        [JsonConstructor]
        public Author(string authorId, string name, decimal earnings, List<Book> books)
        {
            // Constructor with parameters, validating and assigning values.
            if (authorId is null) throw new ArgumentNullException("Переданное значение authorId равно null");
            if (name is null) throw new ArgumentNullException("Переданное значение name равно null");
            if (books is null) throw new ArgumentNullException("Переданное значение коллекции books равно null");
            if (earnings < 0) throw new ArgumentOutOfRangeException("Переданное значение earnings меньше нуля");

            _authorId = authorId;
            _name = name;
            _earnings = earnings;
            _books = books ?? new List<Book>();
        }

        /// <summary>
        /// Method to add a book authored by the author.
        /// </summary>
        /// <param name="book">The Book class to be added to the author’s collection of books.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided book object is null.</exception>
        public void AddBook(Book book)
        {
            if (book is null) throw new ArgumentNullException("Переданный класс book равен null");

            // Сhecking that there is no such book.
            if (!_books.Contains(book))
            {
                // Adding book.
                _books.Add(book);
                // Adding this author to the book.
                book.AddAuthor(this);

                // Subscribes to its earnings update event.
                book.EarningsUpdated += BookEarningsUpdated;
            }
        }

        /// <summary>
        /// Removes a book from the list of books authored by the author.
        /// </summary>
        /// <param name="book">The Book class to be removed from the author’s collection of books.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided book object is null.</exception>
        public void RemoveBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));

            // Сhecking that there is such book.
            if (_books.Contains(book))
            {
                // Removing book
                _books.Remove(book);
                // Removing this author from the book.
                book.RemoveAuthor(this);

                // Unsubscribes to its earnings update event.
                book.EarningsUpdated -= BookEarningsUpdated;
            }
        }

        /// <summary>
        /// Converts the Author object to a JSON-formatted string.
        /// </summary>
        /// <returns>A JSON-formatted string representing the Author object.</returns>
        public string ToJSON()
        {
            // Converts the Author object and its associated books to a JSON-formatted string.
            var booksJson = string.Join(",\n", Books.Select(b => b.ToJSON()));
            return $"{{\n" +
                   $"  \"authorId\": \"{_authorId}\",\n" +
                   $"  \"name\": \"{_name}\",\n" +
                   $"  \"earnings\": {_earnings.ToString(CultureInfo.InvariantCulture)},\n" +
                   $"  \"books\": [\n{booksJson}\n  ]\n" +
                   $"}}";
        }

        /// <summary>
        /// Recalculates author earnings when any of the books' earnings are updated.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void BookEarningsUpdated(object? sender, EventArgs e)
        {
            // Checking that books collection is not empty.
            if (_books.Count == 0)
            {
                _earnings = 0;
                return;
            }

            // Recalculating author earnings.
            Earnings = _books.Sum(book => book.Earnings);
        }
    }
}