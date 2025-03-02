using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeBook
{
	/// <summary>
	/// Статический класс для работы с пользователем.
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// Обработчик файла.
		/// </summary>
		private static FileHandler? handler = null;
		/// <summary>
		/// Книга с рецептами.
		/// </summary>
		private static RecipeBook? recipeBook = null;
		/// <summary>
		/// Метод, где пользователь выбирает пункты меню.
		/// </summary>
		internal static void Main()
		{
			while (true)
			{
				ConsolePrinter.PrintMenu("Выберите пункт меню:");
				ConsolePrinter.PrintMenu("1. Считать данные из файла");
				ConsolePrinter.PrintMenu("2. Посмотреть все рецепты");
				ConsolePrinter.PrintMenu("3. Вывести информацию о рецепте");
				ConsolePrinter.PrintMenu("4. Найти рецепты");
				ConsolePrinter.PrintMenu("5. Добавить новый рецепт");
				ConsolePrinter.PrintMenu("6. Удалить рецепт");
				ConsolePrinter.PrintMenu("7. Отсортировать рецепты");
				ConsolePrinter.PrintMenu("8. Собрать статистику");
				ConsolePrinter.PrintMenu("9. Отметить избранным");
				ConsolePrinter.PrintMenu("10. Вывести избранные");
				ConsolePrinter.PrintMenu("11. Завершить работу");
				string num = Console.ReadLine();
				switch (num)
				{
					case "1":
						ReadData();
						break;
					case "2":
						PrintData();
						break;
					case "3":
						FindRecipe();
						break;
					case "4":
						FilterRecipes();
						break;
					case "5":
						AddRecipe();
						break;
					case "6":
						DeleteRecipe();
						break;
					case "7":
						SortRecipes();
						break;
					case "8":
						CountRecipes();
						break;
					case "9":
						SetFavourite();
						break;
					case "10":
						PrintFavourite();
						break;
					case "11":
						return;
					default:
						ConsolePrinter.PrintMenu("Некорректный ввод");
						continue;
				}
				ConsolePrinter.PrintLine();
			}
		}
		/// <summary>
		/// Метод, проверяющий наличие книги с рецептами для работы с ней.
		/// </summary>
		/// <returns>Возвращает true, если книга есть и false иначе.</returns>
		internal static bool CheckData()
		{
			if (recipeBook == null)
			{
				ConsolePrinter.PrintMenu("Данные не введены");
				return false;
			}
			return true;
		}
		/// <summary>
		/// Метод, выполняющий команду чтения данных.
		/// </summary>
		internal static void ReadData()
		{
			ConsolePrinter.PrintMenu("Введите путь до файла:");
			string path = Console.ReadLine();
			FileHandler newHandler = new FileHandler(path);
			if (newHandler.Correct)
			{
				if (newHandler.Input(out RecipeBook? newRecipeBook))
				{
					handler = newHandler;
					recipeBook = newRecipeBook;
					ConsolePrinter.PrintMenu("Ввод осуществлен успешно");
				}
				else
				{
					ConsolePrinter.PrintMenu("Структура файла некорректна. Ввод не осуществлен.");
				}
			}
			else
			{
				ConsolePrinter.PrintMenu("Файл не найден");
			}
		}
		/// <summary>
		/// Метод, выполняющий команду печати всех названий рецептов.
		/// </summary>
		internal static void PrintData()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Список рецептов:");
			ConsolePrinter.PrintNames(recipeBook.Recipes);
		}
		/// <summary>
		/// Метод, выполняющий команду поиска рецепта по имени и вывода информации о нем.
		/// </summary>
		internal static void FindRecipe()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Введите название нужного рецепта:");
			string name = Console.ReadLine();
			List<Recipe> filtered = recipeBook.Filter(name);
			if (filtered.Count == 0)
			{
				ConsolePrinter.PrintMenu("С таким названием рецептов нет.");
				return;
			}
			ConsolePrinter.PrintMenu("Информация о рецепте:");
			ConsolePrinter.PrintData(filtered[0].ToString());
		}
		/// <summary>
		/// Метод, выполняющий команду установки рецепта избранным по названию.
		/// </summary>
		internal static void SetFavourite()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Введите название нужного рецепта:");
			string name = Console.ReadLine();
			List<Recipe> filtered = recipeBook.Filter(name);
			if (filtered.Count == 0)
			{
				ConsolePrinter.PrintMenu("С таким названием рецептов нет.");
				return;
			}
			if (filtered[0].Favourite)
			{
				ConsolePrinter.PrintMenu("Он уже избранный");
			}
			else
			{
				filtered[0].Favourite = true; // назначим избранным первый рецепт из найденных, если получилось, что рецептов с таким именем несколько
				ConsolePrinter.PrintMenu("Рецепт установлен избранным");
			}
			WriteData();
		}
		/// <summary>
		/// Метод, выполняющий команду поиска/фильтрации рецептов по критериям.
		/// </summary>
		internal static void FilterRecipes()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Введите название нужного рецепта или пустую строку:");
			string name = Console.ReadLine();
			ConsolePrinter.PrintMenu("Вводите названия нужных ингредиентов в рецепте и закончите ввод пустой строкой:");
			List<string> ingredients = new List<string>();
			while (true)
			{
				string ingredient = Console.ReadLine();
				if (ingredient == "") break;
				ingredients.Add(ingredient);
			}
			ConsolePrinter.PrintMenu("Введите названия нужной категории или пустую строку:");
			string category = Console.ReadLine();
			List<Recipe> filtered = recipeBook.Filter(name, ingredients, category);
			ConsolePrinter.PrintData($"Найдено {filtered.Count} рецептов:");
			ConsolePrinter.PrintNames(filtered);
		}
		/// <summary>
		/// Метод, выполняющий команду добавления нового элемента в книгу.
		/// </summary>
		internal static void AddRecipe()
		{
			if (!CheckData()) return;
			Recipe recipe = InputRecipe();
			recipeBook.Add(recipe);
			WriteData();
		}
		/// <summary>
		/// Метод, выполняющий команду удаления рецепта.
		/// </summary>
		internal static void DeleteRecipe()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Список рецептов:");
			ConsolePrinter.PrintNames(recipeBook.Recipes);
			ConsolePrinter.PrintMenu("Введите название рецепта, который надо удалить:");
			string to_del = Console.ReadLine();
			ConsolePrinter.PrintData($"Удалено { recipeBook.Remove(to_del) } рецептов с таким названием");
			WriteData();
		}
		/// <summary>
		/// Метод, запрашивающий информацию о новом рецепте у пользователя.
		/// </summary>
		private static Recipe InputRecipe()
		{
			ConsolePrinter.PrintMenu("Введите название рецепта:");
			string name = Console.ReadLine();
			ConsolePrinter.PrintMenu("Введите категорию рецепта:");
			string category = Console.ReadLine();
			ConsolePrinter.PrintMenu("Вводите названия ингредиентов в рецепте и закончите ввод пустой строкой:");
			List<Ingredient> ingredients = new List<Ingredient>();
			while (true)
			{
				string data = Console.ReadLine();
				if (data == "") break;
				ingredients.Add(new Ingredient(data));
			}
			ConsolePrinter.PrintMenu("Вводите инструкции по приготовлению и закончите ввод пустой строкой:");
			List<string> instructions = new List<string>();
			while (true)
			{
				string instruction = Console.ReadLine();
				if (instruction == "") break;
				instructions.Add(instruction);
			}
			return new Recipe(name, category, ingredients, instructions);
		}
		/// <summary>
		/// Метод, выполняющий команду сортировки рецептов.
		/// </summary>
		internal static void SortRecipes()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Отсортировать по названию (1) или по категории (2)?");
			string input = Console.ReadLine();
			switch (input)
			{
				case "1":
					recipeBook.SortByName();
					ConsolePrinter.PrintMenu("Рецепты отсортированы по названию");
					break;
				case "2":
					recipeBook.SortByCategory();
					ConsolePrinter.PrintMenu("Рецепты отсортированы по категории");
					break;
				default:
					ConsolePrinter.PrintMenu("Некорректный ввод");
					return;
			}
			WriteData();
		}
		/// <summary>
		/// Метод, выполняющий команду подсчета рецептов.
		/// </summary>
		internal static void CountRecipes()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Посчитать количество рецептов по категориям (1) или суммарное количество (2)?");
			string input = Console.ReadLine();
			switch (input)
			{
				case "1":
					foreach (var data in recipeBook.CountByCategory())
					{
						ConsolePrinter.PrintData($"{data.Key} : {data.Value}");
					}
					break;
				case "2":
					ConsolePrinter.PrintData($"Всего рецептов: {recipeBook.Count}");
					break;
				default:
					ConsolePrinter.PrintMenu("Некорректный ввод");
					return;
			}
		}
		/// <summary>
		/// Метод, выполняющий команду вывода всех избранных рецептов.
		/// </summary>
		internal static void PrintFavourite()
		{
			if (!CheckData()) return;
			ConsolePrinter.PrintMenu("Избранные рецепты:");
			ConsolePrinter.PrintNames(recipeBook.FavouriteRecipes);
		}
		/// <summary>
		/// Метод, записывающий книгу рецептов в файл.
		/// </summary>
		private static void WriteData()
		{
			if (recipeBook == null)
			{
				return;
			}
			if (handler.Output(recipeBook))
			{
				ConsolePrinter.PrintMenu("Запись в файл осуществлена.");
			}
			else
			{
				ConsolePrinter.PrintMenu("Запись в файл не осуществлена. Проверьте доступность файла по указанному пути.");
			}
		}
	}
}
