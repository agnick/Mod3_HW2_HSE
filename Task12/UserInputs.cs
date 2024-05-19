namespace Task12
{
    public static class UserInputs
    {
        /// <summary>
        /// Prompts the user to enter the absolute path to a JSON file for reading.
        /// </summary>
        /// <returns>The absolute file path provided by the user.</returns>
        public static string GetFilePathToRead()
        {
            string? filePath;

            // Loop until a valid file path is provided.
            while (true)
            {
                // Ask the user to enter the absolute path to a file containing JSON data.
                Console.Write("Введите абсолютный путь к файлу с json-данными: ");
                filePath = Console.ReadLine();

                // Check if the file exists at the specified path.
                if (!File.Exists(filePath))
                {
                    // If the file does not exist, display an error message and ask for the input again.
                    ConsoleUtils.PrintBeautyError("Указанный файл не существует, повторите ввод.");
                    continue;
                }

                if (!filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    // If the file does not exist, display an error message and ask for the input again.
                    ConsoleUtils.PrintBeautyError("Указанный файл не является файлом json, повторите ввод.");
                    continue;
                }

                break;
            }

            return filePath;
        }

        /// <summary>
        /// Prompts the user to enter the absolute path to a JSON file for writing data.
        /// </summary>
        /// <returns>The absolute file path provided by the user.</returns>
        public static string GetFilePathToWrite()
        {
            string? filePath;

            // Loop until a valid file path is provided.
            while (true)
            {
                Console.Write($"\nВведите абсолютный путь к файлу для сохранения данных (пример ввода: myFile.json или ...{Path.DirectorySeparatorChar}myFile.json): ");
                filePath = Console.ReadLine();

                // Check if the file path is empty.
                if (string.IsNullOrEmpty(filePath))
                {
                    // If the file path is empty, display an error message and ask for the input again.
                    ConsoleUtils.PrintBeautyError("Передано пустое название файла, повторите ввод.");
                    continue;
                }

                // Check if the file has a JSON extension.
                if (!filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    // If the file does not have a JSON extension, display an error message and ask for the input again.
                    ConsoleUtils.PrintBeautyError("Указанный файл не является файлом json, повторите ввод.");
                    continue;
                }

                break;
            }

            return filePath;
        }

        /// <summary>
        /// Prompts the user to enter a menu option within the specified range.
        /// </summary>
        /// <param name="min">The minimum value of the menu option.</param>
        /// <param name="max">The maximum value of the menu option.</param>
        /// <returns>The user's menu choice.</returns>
        public static int GetUserMenuInput(int min, int max)
        {
            int userChoice;

            // Loop until a valid menu option is provided.
            while (true)
            {
                Console.Write("Укажите номер пункта меню для запуска действия: ");

                // Read the user input and try to parse it as an integer.
                if (int.TryParse(Console.ReadLine(), out userChoice) && userChoice > min && userChoice < max)
                {
                    break;
                }

                // If the input is not valid, display an error message and ask for input again.
                ConsoleUtils.PrintBeautyError("Неизвестная команда, повторите ввод.");
            }

            return userChoice;
        }

        /// <summary>
        /// Prompts the user to enter a string value for updating a specified field.
        /// </summary>
        /// <param name="name">The name of the field being updated.</param>
        /// <returns>The string value entered by the user.</returns>
        public static string GetStringNewField(string name)
        {
            string? userValue;

            // Loop until a non-empty string value is provided.
            while (true)
            {
                Console.Write($"Введите конкретное значение для изменения поля {name}: ");
                userValue = Console.ReadLine();

                // Check if the user input is not null or empty.
                if (!string.IsNullOrEmpty(userValue))
                {
                    break;
                }

                // If the input is empty, display an error message and ask for input again.
                ConsoleUtils.PrintBeautyError($"Передано неккоретное значение поля {name}, повторите ввод.");
            }

            return userValue;
        }

        /// <summary>
        /// Prompts the user to enter an integer value for updating a specified field.
        /// </summary>
        /// <param name="name">The name of the field being updated.</param>
        /// <returns>The integer value entered by the user.</returns>
        public static int GetIntNewField(string name)
        {
            int userValue;

            // Loop until a valid integer value is provided.
            while (true)
            {
                Console.Write($"Введите конкретное значение для изменения поля {name}: ");

                // Read the user input and try to parse it as an integer.
                if (int.TryParse(Console.ReadLine(), out userValue) && userValue > 0)
                {
                    // Check if the input is a positive integer.
                    break;
                }

                // If the input is invalid, display an error message and ask for input again.
                ConsoleUtils.PrintBeautyError($"Передано неккоретное значение поля {name}, повторите ввод.");
            }

            return userValue;
        }

        /// <summary>
        /// Prompts the user to enter a decimal value for updating a specified field.
        /// </summary>
        /// <param name="name">The name of the field being updated.</param>
        /// <returns>The decimal value entered by the user.</returns>
        public static decimal GetDecimalNewField(string name)
        {
            decimal userValue;

            // Loop until a valid decimal value is provided.
            while (true)
            {
                Console.Write($"Введите конкретное значение для изменения поля {name}: ");

                // Read the user input and try to parse it as a decimal.
                if (decimal.TryParse(Console.ReadLine(), out userValue) && userValue > 0)
                {
                    // Check if the input is a positive decimal.
                    break;
                }

                // If the input is invalid, display an error message and ask for input again.
                ConsoleUtils.PrintBeautyError($"Передано неккоретное значение поля {name}, повторите ввод.");
            }

            return userValue;
        }
    }
}