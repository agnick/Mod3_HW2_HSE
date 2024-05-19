using System.Globalization;
using System.Text.Json.Serialization;

namespace JsonUtils
{
    public class Book
    {
        /* Events */
        // Event to notify when book information is updated.
        public event EventHandler<UpdatedEventArgs>? Updated;
        // Event to notify when the earnings of the book are updated.
        public event EventHandler? EarningsUpdated;

        /* Fields */
        // Using JsonPropertyName and JsonIgnore for correct deserializatian.
        [JsonIgnore]
        private readonly List<Author> _authors = new List<Author>();
        [JsonPropertyName("bookId")]
        private string _bookId;
        [JsonPropertyName("title")]
        private string _title;
        [JsonPropertyName("publicationYear")]
        private int _publicationYear;
        [JsonPropertyName("genre")]
        private string _genre;
        [JsonPropertyName("earnings")]
        private decimal _earnings;

        /* Properties */
        // Property to access the book's Authors.
        public List<Author> Authors { get { return new List<Author>(_authors); } }
        // Property to access the book's ID.
        public string BookId { get { return _bookId; } }
        // Property to access or set the book's title.
        public string Title
        {
            get { return _title; }
            set
            {
                // Setter for the book's title, triggers an update event.
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Переданное название книги пустое или равно null");

                _title = value;

                // Triggers an update event.
                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
            }
        }
        // Property to access or set the book's publication year.
        public int PublicationYear
        {
            get { return _publicationYear; }
            set
            {
                // Setter for the books's publication year, triggers an update event.
                if (value < 0) throw new ArgumentOutOfRangeException("Переданное значение года публикации меньше нуля");

                _publicationYear = value;

                // Triggers an update event.
                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
            }
        }
        // Property to access or set the book's genre.
        public string Genre
        {
            get { return _genre; }
            set
            {
                // Setter for the book's genre, triggers an update event.
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Переданное название жанра пустое или равно null");

                _genre = value;

                // Triggers an update event.
                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
            }
        }
        // Property to access or set the book's earnings.
        public decimal Earnings
        {
            get { return _earnings; }
            set
            {
                // Setter for the book's earinigs, triggers an update event and a special one event.
                if (value < 0) throw new ArgumentOutOfRangeException("Переданный доход с книги меньше нуля");

                _earnings = value;

                Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
                // Triggering special event to update the earnings of all authors of this book.
                EarningsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        // Constructor to initialize an empty instance of the Book class.
        public Book()
        {
            _bookId = string.Empty;
            _title = string.Empty;
            _publicationYear = 0;
            _genre = string.Empty;
            _earnings = 0;
        }

        // Constructor to initialize an instance of the Book class with provided values.
        // Using JsonConstructor for correct deserializatian.
        [JsonConstructor]
        public Book(string bookId, string title, int publicationYear, string genre, decimal earnings)
        {
            // Constructor with parameters, validating and assigning values.
            if (bookId is null) throw new ArgumentNullException("Переданное значение bookId равно null");
            if (title is null) throw new ArgumentNullException("Переданное значение title равно null");
            if (genre is null) throw new ArgumentNullException("Переданное значение genre равно null");
            if (publicationYear < 0) throw new ArgumentOutOfRangeException("Переданное значение publicationYear меньше нуля");
            if (earnings < 0) throw new ArgumentOutOfRangeException("Переданное значение earnings меньше нуля");

            _bookId = bookId;
            _title = title;
            _publicationYear = publicationYear;
            _genre = genre;
            _earnings = earnings;
        }

        /// <summary>
        /// A method that adds an author for a given book after checks.
        /// </summary>
        /// <param name="author">The Author class to be added to the book’s collection of authors.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided book object is null.</exception>
        public void AddAuthor(Author author)
        {
            if (author is null) throw new ArgumentNullException("Переданный класс равен null");

            // Checking that the submitted author has not yet been added to the collection of authors of this book.
            if (!_authors.Contains(author))
            {
                // Adding an author to the author collection.
                _authors.Add(author);
            }
        }

        /// <summary>
        /// A method that removes an author for a given book after checks.
        /// </summary>
        /// <param name="author">The Author class to be removed from the book’s collection of authors.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveAuthor(Author author)
        {
            if (author is null) throw new ArgumentNullException("Переданный класс равен null");

            // Checking that the submitted author has been added to the collection of authors of this book.
            if (_authors.Contains(author))
            {
                // Removing an author from the author collection.
                _authors.Remove(author);
            }
        }

        /// <summary>
        /// Converts the Book object to a JSON-formatted string.
        /// </summary>
        /// <returns>A JSON-formatted string representing the Book object.</returns>
        public string ToJSON()
        {
            return $"    {{\n" +
                   $"      \"bookId\": \"{_bookId}\",\n" +
                   $"      \"title\": \"{_title}\",\n" +
                   $"      \"publicationYear\": {_publicationYear},\n" +
                   $"      \"genre\": \"{_genre}\",\n" +
                   $"      \"earnings\": {_earnings.ToString(CultureInfo.InvariantCulture)}\n" +
                   $"    }}";
        }
    }
}