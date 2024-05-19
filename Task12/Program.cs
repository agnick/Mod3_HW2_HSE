/*
   Студент:     Агафонов Никита Максимович    
   Группа:      БПИ234
   Вариант:     12
*/
using JsonUtils;

namespace Task12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                // User input of absolute file path.
                string filePath = UserInputs.GetFilePathToRead();
                // Initializing a reference to a collection of Author objects.
                List<Author>? authors = null;

                // Сhecking for exceptions.
                try
                {
                    // Calling a method that deserializes data from a json file.
                    authors = JsonSerealization.DeserializeJson(filePath);                  
                } 
                catch(Exception ex)
                {
                    ConsoleUtils.PrintBeautyError(ex.Message);
                }

                // Сhecking that the returned collection is not null and not empty.
                if (authors is not null && authors.Count != 0)
                {
                    ConsoleUtils.PrintBeautySuccessMessage("Данные из json-файла успешно десериализованы!");

                    // Сhecking for exceptions.
                    try
                    {
                        // Calling a method that provides the user with the functionality of the main menu.
                        ConsoleUtils.MainMenu(authors);
                    }
                    catch (Exception ex)
                    {
                        ConsoleUtils.PrintBeautyError(ex.Message);
                    }
                }

                // Repeating the solution at the user's request.
                Console.Write("Для выхода из программы нажмите клавишу ESC, для перезапуска программы нажмите любую другую клавишу: ");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape) { break; }
            }
        }
    }
}