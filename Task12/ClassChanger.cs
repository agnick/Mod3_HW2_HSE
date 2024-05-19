using JsonUtils;

namespace Task12
{
    public static class ClassChanger
    {
        /// <summary>
        /// Edits the name of the specified author.
        /// </summary>
        /// <param name="author">The Author object whose name is to be edited.</param>
        /// <exception cref="Exception">Thrown when an error occurs while editing the name.</exception>
        public static void EditAuthorName(Author author)
        {
            // Get the new name from user input.
            string newName = UserInputs.GetStringNewField("_name");

            try
            {
                // Attempt to set the new name for the author.
                author.Name = newName;
            }
            catch (Exception ex)
            {
                // Throw an exception with the original error message.
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Edits a field of the specified book.
        /// </summary>
        /// <param name="book">The Book object whose field is to be edited.</param>
        /// <exception cref="Exception">Thrown when an error occurs while editing the field.</exception>
        public static void EditBookField(Book book)
        {
            // Display menu options for editing book fields.
            ConsoleUtils.PrintBeautyWarningMessage("-------------------------------------------------");
            Console.WriteLine("1. Отредактировать поле _title\n2. Отредактировать поле _publicationYear\n3. Отредактировать поле _genre\n4. Отредактировать поле _earnings (активирует дополнительное событие)");
            ConsoleUtils.PrintBeautyWarningMessage("-------------------------------------------------");

            // Get user choice from the menu.
            int choice = UserInputs.GetUserMenuInput(0, 5);

            // Perform actions based on user choice.
            switch (choice)
            {
                case 1:
                    // Checking for exceptions.
                    try
                    {
                        // Edit the _title field.
                        book.Title = UserInputs.GetStringNewField("_title");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    break;
                case 2:
                    // Checking for exceptions.
                    try
                    {
                        // Edit the _publicationYear field.
                        book.PublicationYear = UserInputs.GetIntNewField("_publicationYear");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }                   
                    break;
                case 3:
                    // Checking for exceptions.
                    try
                    {
                        // Edit the _genre field.
                        book.Genre = UserInputs.GetStringNewField("_genre");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }                   
                    break;
                case 4:
                    // Checking for exceptions.
                    try
                    {
                        // Edit the _earnings field.
                        book.Earnings = UserInputs.GetDecimalNewField("_earnings");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }                   
                    break;
            }
        }
    }
}