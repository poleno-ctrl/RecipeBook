using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook
{
	/// <summary>
	/// Статический класс, для вывода в консоль информации в разных форматах.
	/// </summary>
	internal static class ConsolePrinter
	{
		/// <summary>
		/// Статический метод для вывода всех названий рецептов из списка.
		/// </summary>
		/// <param name="recipes"></param>
		public static void PrintNames(List<Recipe> recipes)
		{
			foreach (var recipe in recipes)
			{
				PrintRecipeName(recipe);
			}
		}
		/// <summary>
		/// Статический метод, для вывода в консоль строки меню.
		/// </summary>
		/// <param name="str">Строка меню.</param>
		public static void PrintMenu(string str)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(str);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
		/// <summary>
		/// Статический метод, для вывода в консоль строки обычной информации.
		/// </summary>
		/// <param name="str">Строка с информацией.</param>
		public static void PrintData(string str)
		{
			Console.WriteLine(str);
		}
		/// <summary>
		/// Статический метод, для вывода в консоль разделительной линии.
		/// </summary>
		public static void PrintLine()
		{
			Console.Write(new string('-', Console.WindowWidth));
			Console.WriteLine();
		}
		/// <summary>
		/// Статический метод для вывода названия рецепта.
		/// </summary>
		/// <param name="recipe">Рецепт, название которого нужно вывести.</param>
		private static void PrintRecipeName(Recipe recipe)
		{
			if (recipe.Favourite)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(recipe.Name);
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			else
			{
				Console.WriteLine(recipe.Name);
			}
		}
	}
}
