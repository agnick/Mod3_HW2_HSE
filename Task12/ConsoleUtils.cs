using JsonUtils;

namespace Task12
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// A method that outputs a red - marked error.
        /// </summary>
        /// <param name="errorMessage">Transmitted error message.</param>
        public static void PrintBeautyError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }

        /// <summary>
        /// A method that outputs a success message highlighted in green.
        /// </summary>
        /// <param name="successMessage">The success message to be displayed.</param>
        public static void PrintBeautySuccessMessage(string successMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(successMessage);
            Console.ResetColor();
        }

        /// <summary>
        /// A method that outputs a warning message highlighted in yellow.
        /// </summary>
        /// <param name="warningMessage">The warning message to be displayed.</param>
        public static void PrintBeautyWarningMessage(string warningMessage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(warningMessage);
            Console.ResetColor();
        }

        /// <summary>
        /// Displays the main menu and allows the user to perform various operations on the list of authors.
        /// </summary>
        /// <param name="authors">The list of authors to be manipulated.</param>
        /// <exception cref="Exception">Thrown when an error occurs during menu operations.</exception>
        public static void MainMenu(List<Author> authors)
        {
            // Create a mutable copy of the authors list.
            List<Author> authorsMutable = authors;

            while (true)
            {
                // Display menu options.
                PrintBeautyWarningMessage("-------------------------------------------------");
                Console.WriteLine("1. Отсортировать коллекцию объектов по одному из полей\n2. Выбрать объект и отредактировать в нем поле\n3. Сохранить текущую коллекцию объектов в файл (дополнительная опция)\n4. Выйти из меню");
                PrintBeautyWarningMessage("-------------------------------------------------");

                // Get user choice from the menu.
                int сhoice = UserInputs.GetUserMenuInput(0, 5);

                // Perform actions based on user choice.
                switch (сhoice)
                {
                    case 1:
                        // Checking for exceptions.
                        try
                        {
                            // Sort the authors list based on user choice.
                            authorsMutable = SortMenu(authorsMutable);
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
                            // Navigate to the submenu for changing fields of selected author.
                            ChangeClassFieldMenu(authorsMutable);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        break;
                    case 3:
                        // Save the modified authors list to file.
                        SaveToFile(authorsMutable);
                        break;
                    case 4:
                        return;
                }
            }
        }

        /// <summary>
        /// Displays the sort menu and sorts the list of authors based on user choice.
        /// </summary>
        /// <param name="authors">The list of authors to be sorted.</param>
        /// <returns>The sorted list of authors.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during sorting.</exception>
        private static List<Author> SortMenu(List<Author> authors)
        {
            // Display sort menu options.
            Console.WriteLine("\nВыберете поле по которому требуется отсортировать коллекцию объектов:");
            PrintBeautyWarningMessage("-------------------------------------------------");
            Console.WriteLine("1. Прямая сортирока по полю _authorId\n2. Прямая сортирока по полю _name\n3. Прямая сортирока по полю _earnings\n4. Обратная сортирока по полю _authorId\n5. Обратная сортирока по полю _name\n6. Обратная сортирока по полю _earnings");
            PrintBeautyWarningMessage("-------------------------------------------------");

            // Get user choice from the menu.
            int choice = UserInputs.GetUserMenuInput(0, 7);

            // Declaration of a nullable Comparison delegate, which will be used for sorting the list of authors.
            // Initialized as null initially until a specific comparison function is assigned based on user choice.
            Comparison<Author>? comparison = null;

            // Perform actions based on user choice.
            switch (choice)
            {
                case 1:
                    comparison = (a, b) => a.AuthorId.CompareTo(b.AuthorId);
                    break;
                case 2:
                    comparison = (a, b) => a.Name.CompareTo(b.Name);
                    break;
                case 3:
                    comparison = (a, b) => a.Earnings.CompareTo(b.Earnings);
                    break;
                case 4:
                    comparison = (a, b) => b.AuthorId.CompareTo(a.AuthorId);
                    break;
                case 5:
                    comparison = (a, b) => b.Name.CompareTo(a.Name);
                    break;
                case 6:
                    comparison = (a, b) => b.Earnings.CompareTo(a.Earnings);
                    break;
            }

            // Ckecking that comparison result is not null.
            if (comparison != null)
            {
                // Checking for exceptions.
                try
                {
                    // Sort the authors list using the selected comparison.
                    authors.Sort(comparison);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при сортировке коллекции объектов.");
                }

                PrintBeautySuccessMessage("Коллекция объектов успешно отсортирована!");
            }

            return authors;
        }

        /// <summary>
        /// Displays a menu for editing fields of an author or one of their books.
        /// </summary>
        /// <param name="authors">The list of authors.</param>
        /// <exception cref="Exception">Thrown if an error occurs during field editing.</exception>
        private static void ChangeClassFieldMenu(List<Author> authors)
        {
            // Choose an author from the provided list.
            Author chosenAuthor = ChooseAuthor(authors);

            // Display information about the chosen author.
            DisplayChosenAuthor(chosenAuthor);

            // Prompt the user to choose between editing the author's name or selecting a book for editing.
            Console.WriteLine("1. Отредактировать поле _name\n2. Выбрать поле одной из книг автора");
            // Get user choice from the menu.
            int choice = UserInputs.GetUserMenuInput(0, 3);

            // Checking for exceptions.
            try
            {
                if (choice == 1)
                {
                    // Edit the author's name.
                    ClassChanger.EditAuthorName(chosenAuthor);
                }
                else
                {
                    // Choose a book from the author's collection and edit its fields.
                    Book chosenBook = ChooseBook(chosenAuthor);
                    ClassChanger.EditBookField(chosenBook);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Display the modified author's information.
            DisplayModifiedAuthor(chosenAuthor);
        }

        /// <summary>
        /// Allows the user to choose an author from the provided list.
        /// </summary>
        /// <param name="authors">The list of authors.</param>
        /// <returns>The chosen author.</returns>
        private static Author ChooseAuthor(List<Author> authors)
        {
            // Prompt the user to choose an author from the provided list.
            Console.WriteLine("\nВыберете объект из коллекции объектов для последующего редактирования полей:");

            // Display a numbered list of authors for user selection.
            PrintBeautyWarningMessage("-------------------------------------------------");
            for (int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Объект {i + 1}: {authors[i].Name}");
            }
            PrintBeautyWarningMessage("-------------------------------------------------");

            // Get user choice from the menu.
            int choice = UserInputs.GetUserMenuInput(0, authors.Count + 1);

            // Return the chosen author.
            return authors[choice - 1];
        }

        /// <summary>
        /// Allows the user to choose a book from the provided author's collection.
        /// </summary>
        /// <param name="author">The author whose books are being chosen from.</param>
        /// <returns>The chosen book.</returns>
        private static Book ChooseBook(Author author)
        {
            // Prompt the user to choose a book from the provided author's collection.
            Console.WriteLine("\nВыберете одну из книг автора для последующего редактирования ее поля:");

            // Display a numbered list of the author's books for user selection.
            PrintBeautyWarningMessage("-------------------------------------------------");
            for (int i = 0; i < author.Books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Книга {i + 1}: {author.Books[i].Title}");
            }
            PrintBeautyWarningMessage("-------------------------------------------------");

            // Get user choice from the menu.
            int choice = UserInputs.GetUserMenuInput(0, author.Books.Count + 1);

            // Return the chosen book.
            return author.Books[choice - 1];
        }

        /// <summary>
        /// Saves the data of the provided authors to a JSON file chosen by the user.
        /// </summary>
        /// <param name="authors">The list of authors whose data will be saved.</param>
        private static void SaveToFile(List<Author> authors)
        {
            // Loop until a valid file path is provided by the user.
            while (true)
            {
                // Get the file path where the data will be saved.
                string saveFilePath = UserInputs.GetFilePathToWrite();

                // Checking for exceptions.
                try
                {
                    // Save the data to the JSON file.
                    JsonWriter.SaveToJsonFile(saveFilePath, authors, false);

                    // Display success message and exit the loop.
                    PrintBeautySuccessMessage("Данные успешно записаны в файл!");
                    break;
                }
                catch (Exception ex)
                {
                    // Display error message if an exception occurs and continue the loop to prompt for a valid file path.
                    PrintBeautyError(ex.Message);
                    continue;
                }
            }
        }

        /// <summary>
        /// Displays the information of the chosen author to the console.
        /// </summary>
        /// <param name="author">The author whose information will be displayed.</param>
        private static void DisplayChosenAuthor(Author author)
        {
            // Display a header indicating the chosen author.
            PrintBeautySuccessMessage("Выбранный объект:");
            PrintBeautyWarningMessage("-------------------------------------------------");
            // Display the JSON representation of the author.
            Console.WriteLine(author.ToJSON());
            // Display a footer.
            PrintBeautyWarningMessage("-------------------------------------------------");
        }

        /// <summary>
        /// Displays the modified information of the author to the console.
        /// </summary>
        /// <param name="author">The author whose modified information will be displayed.</param>
        private static void DisplayModifiedAuthor(Author author)
        {
            // Display a header indicating the object after modifications.
            PrintBeautyWarningMessage("Объект после изменений:");
            PrintBeautyWarningMessage("-------------------------------------------------");
            // Display the JSON representation of the author after modifications.
            Console.WriteLine(author.ToJSON());
            // Display a footer.
            PrintBeautyWarningMessage("-------------------------------------------------");
        }
    }
}