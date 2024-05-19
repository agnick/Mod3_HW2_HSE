using JsonUtils;
using System.Text.Json;

namespace Task12
{
    public static class JsonSerealization
    {
        /// <summary>
        /// Deserializes JSON data from the specified file path and returns a list of Author objects.
        /// </summary>
        /// <param name="filePath">The path to the JSON file to deserialize.</param>
        /// <returns>A list of Author objects deserialized from the JSON file.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid file name is provided.</exception>
        /// <exception cref="IOException">Thrown when an error occurs while opening or writing to the file.</exception>
        /// <exception cref="JsonException">Thrown when an error occurs during JSON deserialization.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static List<Author>? DeserializeJson(string filePath)
        {
            List<Author>? authors;

            // Checking for exceptions.
            try
            {
                // Read the JSON data from the file.
                string jsonString = File.ReadAllText(filePath);

                // Configure JSON deserialization options.
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                // Deserialize the JSON data to a list of Author objects.
                authors = JsonSerializer.Deserialize<List<Author>>(jsonString, options);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Ошибка десериализации JSON. Проверьте структуру файла.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }

            // Check if the deserialized list is null or empty.
            if (authors is null || authors.Count == 0)
            {
                return null;
            }

            // Checking for exceptions.
            try
            {
                // Configure Author list.
                authors = ConfigurateAuthorsBooks(authors, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return authors;
        }

        /// <summary>
        /// Configures the books of each author in the list by ensuring uniqueness based on BookId.
        /// Also, initializes an AutoSaver instance to subscribe to author and book update events for automatic saving.
        /// </summary>
        /// <param name="authors">The list of Author objects to configure.</param>
        /// <param name="filePath">The file path to which the data will be saved.</param>
        /// <returns>The list of Author objects with configured books.</returns>
        private static List<Author> ConfigurateAuthorsBooks(List<Author> authors, string filePath)
        {
            // Dictionary to store unique books based on BookId.
            Dictionary<string, Book> booksDictionary = new Dictionary<string, Book>();

            // Iterate through each author in the list.
            foreach (Author author in authors)
            {
                // Temporary list to store unique books.
                List<Book> newBooksList = new List<Book>();

                // Iterate through each book of the current author.
                foreach (Book book in author.Books)
                {
                    Book uniqueBook;

                    // Check if the book already exists in the dictionary.
                    if (booksDictionary.ContainsKey(book.BookId))
                    {
                        // If the book exists, use the unique instance from the dictionary.
                        uniqueBook = booksDictionary[book.BookId];
                    }
                    else
                    {
                        // If the book is not in the dictionary, it's unique. Add it to the dictionary.
                        uniqueBook = book;
                        booksDictionary.Add(book.BookId, book);
                    }

                    // Add the unique book to the temporary list.
                    newBooksList.Add(uniqueBook);
                }

                // Clear the author's book list and add the unique instances back.
                while (author.Books.Any())
                {
                    author.RemoveBook(author.Books.First());
                }

                foreach (Book uniqueBook in newBooksList)
                {
                    author.AddBook(uniqueBook);
                }
            }

            // Initialize an AutoSaver instance to automatically save changes to the file.
            AutoSaver autoSaver = new AutoSaver(filePath, authors);

            // Subscribe AutoSaver to author update events.
            foreach (Author author in authors)
            {
                autoSaver.SubscribeToAuthor(author);
            }

            // Subscribe AutoSaver to book update events.
            foreach (Book book in booksDictionary.Values)
            {
                autoSaver.SubscribeToBook(book);
            }

            return authors;
        }
    }
}