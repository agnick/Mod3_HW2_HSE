using System.Text;
using System.Text.Json;
using JsonUtils.DTOs;

namespace JsonUtils
{
    public static class JsonWriter
    {
        /// <summary>
        /// Saves the list of authors to a JSON file.
        /// </summary>
        /// <param name="originalFilePath">The path to the original JSON file.</param>
        /// <param name="authors">The list of authors to be saved.</param>
        /// <param name="modifyFileName">A boolean value indicating whether to modify the file name to create a temporary file.</param>
        /// <exception cref="ArgumentException">Thrown when the provided file name is invalid.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs during file operations.</exception>
        /// <exception cref="NotSupportedException">Thrown when the operation is not supported by the file system.</exception>
        /// <exception cref="JsonException">Thrown when an error occurs during JSON serialization.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static void SaveToJsonFile(string originalFilePath, List<Author> authors, bool modifyFileName)
        {
            // Ckecking for exceptions.
            try
            {
                string filePath = modifyFileName ? CreateTempFilePath(originalFilePath) : originalFilePath;

                /* Serialization way */
                /* ------------------------------------------------------------------------ */
                // Map authors to AuthorDto objects.
                List<AuthorDto> authorDtos = MapToDto(authors);

                // Configure JSON serialization options.
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                // Serialize the AuthorDto list to JSON string.
                string jsonString = JsonSerializer.Serialize(authorDtos, options);

                // Write the JSON string to the file.
                File.WriteAllText(filePath, jsonString, Encoding.UTF8);
                /* ------------------------------------------------------------------------ */

                /* Using Author.ToJSON method way. Uncomment code below and comment code above to use this way. */
                /* ------------------------------------------------------------------------ */
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("[");

                //for (int i = 0; i < authors.Count; i++)
                //{
                //    sb.Append(authors[i].ToJSON());

                //    if (i < authors.Count - 1)
                //    {
                //        sb.AppendLine(",");
                //    }
                //}

                //sb.AppendLine("\n]");

                //File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                /* ------------------------------------------------------------------------ */
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (NotSupportedException ex)
            {
                throw new NotSupportedException("Операция не поддерживается. Проверьте путь к файлу и формат данных.");
            }
            catch (JsonException ex)
            {
                throw new JsonException("Ошибка при сериализации объекта в JSON.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }
        }

        /// <summary>
        /// Creates a temporary file path by appending "_tmp" to the original file name.
        /// </summary>
        /// <param name="filePath">The original file path.</param>
        /// <returns>A temporary file path.</returns>
        private static string CreateTempFilePath(string filePath)
        {
            // Get the absolute path of the original file.
            string absolutePath = Path.GetFullPath(filePath);

            // Extract the file name without extension.
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(absolutePath);

            // Append "_tmp" to the file name to create the temporary file name.
            string newFileName = $"{fileNameWithoutExtension}_tmp.json";

            // Return the new temporary file path.
            return newFileName;
        }

        /// <summary>
        /// Maps a list of Author objects to a list of AuthorDto objects.
        /// </summary>
        /// <param name="authors">The list of Author objects to be mapped.</param>
        /// <returns>A list of AuthorDto objects.</returns>
        private static List<AuthorDto> MapToDto(List<Author> authors)
        {
            // Map each Author object to an AuthorDto object.
            List<AuthorDto> authorDtos = authors.Select(author => new AuthorDto
            {
                // Map Author properties to AuthorDto properties.
                AuthorId = author.AuthorId,
                Name = author.Name,
                Earnings = author.Earnings,
                // Map each Book object in the Author's Books list to a BookDto object.
                Books = author.Books.Select(book => new BookDto
                {
                    // Map Book properties to BookDto properties.
                    BookId = book.BookId,
                    Title = book.Title,
                    PublicationYear = book.PublicationYear,
                    Genre = book.Genre,
                    Earnings = book.Earnings
                }).ToList()
            }).ToList();

            return authorDtos;
        }
    }
}