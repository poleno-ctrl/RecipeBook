using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeBook
{
	/// <summary>
	/// Класс, описывающий книгу с рецептами.
	/// </summary>
	internal class RecipeBook
	{
		/// <summary>
		/// Список рецептов.
		/// </summary>
		private List<Recipe> _recipes;
		/// <summary>
		/// Свойство для получения количества рецептов в книге.
		/// </summary>
		public int Count { get {  return _recipes.Count; } }
		/// <summary>
		/// Свойство для получения списка всех рецептов в книге.
		/// </summary>
		public List<Recipe> Recipes { get { return _recipes; } }
		/// <summary>
		/// Свойство для получения списка всех избранных рецептов в книге.
		/// </summary>
		public List<Recipe> FavouriteRecipes { get { return _recipes.FindAll(x => x.Favourite == true); } }
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public RecipeBook() 
		{ 
			_recipes = new List<Recipe>();
		}
		/// <summary>
		/// Конструктор, принимающий список рецептов.
		/// </summary>
		/// <param name="recipes">Список рецептов.</param>
		public RecipeBook(List<Recipe> recipes)
		{
			_recipes = recipes;
		}
		/// <summary>
		/// Метод, добавляющий новый рецепт в книгу.
		/// </summary>
		/// <param name="recipe">Новый рецепт.</param>
		public void Add(Recipe recipe)
		{
			_recipes.Add(recipe);
		}
		/// <summary>
		/// Метод, удаляющий рецепт из книги по названию.
		/// </summary>
		/// <param name="s">Название рецепта.</param>
		/// <returns>Количество удаленных рецептов.</returns>
		public int Remove(string name)
		{
			return _recipes.RemoveAll(x => x.Name.ToLower() == name.ToLower());
		}
		/// <summary>
		/// Метод, сортирующий рецепты по названию.
		/// </summary>
		public void SortByName()
		{
			_recipes.Sort((a, b) => a.Name.CompareTo(b.Name));
		}
		/// <summary>
		/// Метод,сортирующий рецепты по категории.
		/// </summary>
		public void SortByCategory()
		{
			_recipes.Sort((a, b) => a.Category.CompareTo(b.Category));
		}
		/// <summary>
		/// Метод, ищущий рецепт по названию.
		/// </summary>
		/// <param name="name">Название рецпта.</param>
		/// <returns>Найденный рецепт.</returns>
		public Recipe? FindByName(string name)
		{
			name = name.ToLower();
			foreach (Recipe recipe in _recipes)
			{
				if (recipe.Name.ToLower() == name)
				{
					return recipe;
				}
			}
			return null;
		}
		/// <summary>
		/// Метод, фильтрующий список рецептов по названию.
		/// </summary>
		/// <param name="recipes">Список рецептов.</param>
		/// <param name="name">Название рецепта.</param>
		/// <returns>Список отфильрованных рецептов.</returns>
		private List<Recipe> FilterByName(List<Recipe> recipes, string? name)
		{
			List<Recipe> match = new List<Recipe>();
			if (name == null || name == "") return recipes; // если имя не задано, то подходят все рецепты
			foreach (Recipe recipe in recipes)
			{
				if (recipe.Name.ToLower() == name.ToLower())
				{
					match.Add(recipe);
				}
			}
			return match;
		}
		/// <summary>
		/// Метод, фильтрующий список рецептов по наличию ингредиентов.
		/// </summary>
		/// <param name="recipes">Список рецептов.</param>
		/// <param name="ingredients">Список ингредиентов.</param>
		/// <returns>Список отфильрованных рецептов.</returns>
		private List<Recipe> FilterByIngredients(List<Recipe> recipes, List<string>? ingredients)
		{
			List<Recipe> match = new List<Recipe>();
			if (ingredients == null || ingredients.Count == 0) return recipes; // если ингредиенты не задано, то подходят все рецепты
			foreach (Recipe recipe in recipes)
			{
				foreach (string categoryName in ingredients)
				{
					if (recipe.ContainsIngredient(categoryName))
					{
						match.Add(recipe);
						break;
					}
				}
			}
			return match;
		}
		/// <summary>
		/// Метод, фильтрующий список рецептов по категории.
		/// </summary>
		/// <param name="recipes">Список рецептов.</param>
		/// <param name="category">Категория.</param>
		/// <returns>Список отфильрованных рецептов.</returns>
		private List<Recipe> FilterByCategory(List<Recipe> recipes, string? category)
		{
			List<Recipe> match = new List<Recipe>();
			if (category == null || category == "") return recipes;  // если категория не задана, то подходят все рецепты
			foreach (Recipe recipe in recipes)
			{
				if (recipe.Category.ToLower() == category.ToLower())
				{
					match.Add(recipe);
				}
			}
			return match;
		}
		/// <summary>
		/// Метод, фильтрующий рецепты в книге по названию, наличию ингредиентов, категории.
		/// </summary>
		/// <param name="name">Название рецепта.</param>
		/// <param name="ingredients">Список ингредиентов.</param>
		/// <param name="categories">Категория.</param>
		/// <returns>Список отфильрованных рецептов.</returns>
		public List<Recipe> Filter(string name = "", List<string>? ingredients = null, string? categories = null)
		{
			List<Recipe> match = FilterByName(_recipes, name);
			match = FilterByIngredients(match, ingredients);
			match = FilterByCategory(match, categories);
			return match;
		}
		/// <summary>
		/// Считает количество рецептов по категориям.
		/// </summary>
		/// <returns>Словарь с ключами из категорий и значениями - количеством рецептов в категории.</returns>
		public Dictionary<string, int> CountByCategory()
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			foreach (Recipe recipe in _recipes)
			{
				if (!result.ContainsKey(recipe.Category))
				{
					result[recipe.Category] = 0;
				}
				result[recipe.Category]++;
			}
			return result;
		}
		/// <summary>
		/// Перегрузка метода, представляющего книгу рецептов в виде строки.
		/// </summary>
		/// <returns>Книга рецептов в виде строки.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Recipe recipe in _recipes)
			{
				sb.Append(recipe.ToString());
				sb.Append('\n');
			}
			return sb.ToString();
		}
		/// <summary>
		/// Метод, преобразующий строку в список рецептов.
		/// </summary>
		/// <param name="data">Строка с информацией.</param>
		/// <param name="recipeBook">Ссылка, куда следует записать полученную книгу рецептов.</param>
		/// <returns>Возвращает true, если преобразование успешно и false иначе.</returns>
		public static bool TryParse(string data, out RecipeBook? recipeBook)
		{
			List<string> clear = RecipeBookParser.GetClearStrings(data);
			recipeBook = new RecipeBook();
			int j = 0;
			while (true)
			{
				if (j >= clear.Count) break;
				if (RecipeBookParser.ParseRecipe(clear, ref j, out Recipe? recipe))
				{
					recipeBook.Add(recipe);
				}
				else
				{
					return false;
				}
			}
			return true;
		}
	}
	/// <summary>
	/// Статический класс для преобразования строки в книгу рецептов.
	/// </summary>
	internal static class RecipeBookParser
	{
		/// <summary>
		/// Метод "чистит" строку, и возвращает список "чистых" строк, удаляя пустые строки и незначащие символы по краям строки.
		/// </summary>
		/// <param name="data">Строка для "чистки".</param>
		/// <returns>Список "чистых" строк.</returns>
		internal static List<string> GetClearStrings(string data)
		{
			string[] strings = data.Split('\n');
			List<string> clear = new List<string>();
			for (int i = 0; i < strings.Length; i++)
			{
				strings[i] = strings[i].Trim();
				if (strings[i] != "") clear.Add(strings[i]);
			}
			return clear;
		}
		/// <summary>
		/// Делит строку на ключ и значение по ":" и удаляет лишние символы в получившихся строках.
		/// </summary>
		/// <param name="str">Строка для деления.</param>
		/// <returns>Список полученных строк.</returns>
		internal static string[] SplitField(string str)
		{
			string[] ans = str.Split(':');
			for (int i = 0; i < ans.Length; i++)
			{
				ans[i] = ans[i].Trim();
			}
			return ans;
		}
		/// <summary>
		/// Преобразует строку в рецепт.
		/// </summary>
		/// <param name="strings">Список строк из которых надо считать рецепт.</param>
		/// <param name="j">Индекс строки, с которой надо начать чтение.</param>
		/// <param name="recipe">Ссылка на рецепт, куда надо записать полученный рецепт.</param>
		/// <returns>Возвращает true, если чтение успешно и false иначе.</returns>
		internal static bool ParseRecipe(List<string> strings, ref int j, out Recipe? recipe)
		{
			string name;
			string category;
			List<Ingredient> ingredients = new List<Ingredient>();
			List<string> instructions = new List<string>();
			recipe = null;
			try
			{
				string[] nameField = SplitField(strings[j]);
				if (nameField[0].ToLower() != "название рецепта") return false;
				name = nameField[1];
				j++;
				string[] categoryField = SplitField(strings[j]);
				if (categoryField[0].ToLower() != "категория") return false;
				category = categoryField[1];
				j++;
				string[] ingredientsField = SplitField(strings[j]);
				if (ingredientsField[0].ToLower() != "ингредиенты") return false;
				j++;
				while (!strings[j].Contains(':')) // если в строке есть ":", значит в ней уже название нового поля рецепта
				{
					ingredients.Add(new Ingredient(strings[j]));
					j++;
				}
				string[] instructionsField = SplitField(strings[j]);
				if (instructionsField[0].ToLower() != "инструкция") return false;
				j++;
				while (j < strings.Count && !strings[j].Contains(':')) // если в строке есть ":", значит в ней уже название нового поля рецепта
				{
					instructions.Add(strings[j]);
					j++;
				}
			}
			catch (IndexOutOfRangeException) // если мы вышли за пределы массива, значит у рецепта в строке присутствуют не все поля
			{
				return false;
			}
			recipe = new Recipe(name, category, ingredients, instructions);
			return true;
		}
	}
}
