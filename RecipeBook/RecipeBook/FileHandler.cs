using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook
{
	/// <summary>
	/// Класс для работы с файлом: записью и чтением данных в определенном формате.
	/// </summary>
	internal class FileHandler
	{
		/// <summary>
		/// Поле пути к файлу.
		/// </summary>
		private string? _path;
		/// <summary>
		/// Свойство для чтения и записи пути к файлу.
		/// </summary>
		public string? Path { get { return _path; } set { _path = value; } }
		/// <summary>
		/// Свойство для получения пути к файлу с избранными рецептами.
		/// </summary>
		public string? FavouritePath
		{
			get 
			{ 
				if (Correct == false) return null;
				string[] fields = Path.Split('.');
				return fields[0] + "(favourite)." + fields[1];
			}
		}
		/// <summary>
		/// Свойство, для проверки пути к файлу на корректность.
		/// </summary>
		public bool Correct { get { return File.Exists(_path); } }
		/// <summary>
		/// Констуктор, принимающий путь к файлу.
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		public FileHandler (string? path)
		{
			_path = path;
		}
		/// <summary>
		/// Метод для ввода данных из файла в RecipeBook.
		/// </summary>
		/// <param name="recipe">RecipeBook для записи данных.</param>
		/// <returns>В случае успешного ввода возвращает true и false в обратном случае.</returns>
		public bool Input(out RecipeBook? recipe)
		{
			recipe = null;
			if (!Correct)
			{
				return false;
			}
			string data;
			try
			{
				data = File.ReadAllText(Path);
			}
			catch
			{
				return false;
			}
			if (RecipeBook.TryParse(data, out recipe))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Метод для вывода данных о RecipeBook в файл.
		/// </summary>
		/// <param name="recipeBook">RecipeBook для вывода данных.</param>
		/// <returns>В случае успешного вывода возвращает true и false в обратном случае.</returns>
		public bool Output(RecipeBook recipeBook)
		{
			if (!Correct)
			{
				return false;
			}
			RecipeBook favouriteRecipes = new RecipeBook(recipeBook.FavouriteRecipes);
			try
			{
				File.WriteAllText(Path, recipeBook.ToString());
				File.WriteAllText(FavouritePath, favouriteRecipes.ToString());
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
