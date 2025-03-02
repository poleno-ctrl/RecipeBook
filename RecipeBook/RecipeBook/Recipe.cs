using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeBook
{
	/// <summary>
	/// Класс, описывающий рецепт.
	/// </summary>
	internal class Recipe
	{
		/// <summary>
		/// Поле названия рецепта.
		/// </summary>
		private string _name;
		/// <summary>
		/// Поле категории рецепта.
		/// </summary>
		private string _category;
		/// <summary>
		/// Поле, принимающее true, если рецепт в избранном и false в обратном.
		/// </summary>
		private bool _favourite;
		/// <summary>
		/// Свойство для чтения названия рецепта.
		/// </summary>
		public string Name { get { return _name; } }
		/// <summary>
		/// Свойство для чтения категории рецепта.
		/// </summary>
		public string Category { get { return _category; } }
		/// <summary>
		/// Свойство для добавления и исключения рецепта из избранного.
		/// </summary>
		public bool Favourite { get { return _favourite; } set { _favourite = value; } }
		/// <summary>
		/// Список ингредиентов.
		/// </summary>
		private List<Ingredient> _ingredients;
		/// <summary>
		/// Список инструкций.
		/// </summary>
		private List<string> _instructions;
		/// <summary>
		/// Конструктор, принимающий имя, категорию, список ингредиентов и список инструкций.
		/// </summary>
		/// <param name="name">Название рецепта.</param>
		/// <param name="category">Категория рецепта.</param>
		/// <param name="ingredients">Список ингридиентов.</param>
		/// <param name="instructions">Список инструкций.</param>
		public Recipe(string name, string category, List<Ingredient> ingredients, List<string> instructions)
		{
			_name = name;
			_category = category;
			_ingredients = ingredients;
			_instructions = instructions;
		}
		/// <summary>
		/// Метод, проверяющий наличие ингредиента в рецепте по названию.
		/// </summary>
		/// <param name="name">Название ингредиента.</param>
		/// <returns>Возвращает true, если ингредиент есть в рецепте и false иначе</returns>
		public bool ContainsIngredient(string name)
		{
			foreach (Ingredient ingredient in _ingredients)
			{
				if (ingredient.Name.ToLower() == name.ToLower())
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Перегрузка метода, представляющего рецепт в виде строки.
		/// </summary>
		/// <returns>Рецепт в виде строки.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"Название рецепта: {Name}\n");
			sb.Append($"Категория: {Category}\n");
			sb.Append($"Ингредиенты:\n");
			foreach (Ingredient ingredient in _ingredients)
			{
				sb.Append("\t" + ingredient.ToString() + "\n");
			}
			sb.Append($"Инструкция:\n");
			foreach (string instruction in _instructions)
			{
				sb.Append("\t" + instruction + "\n");
			}
			return sb.ToString();
		}

	}
	/// <summary>
	/// Класс, описывающий ингредиент.
	/// </summary>
	internal class Ingredient
	{
		/// <summary>
		/// Поле названия ингредиента.
		/// </summary>
		private string _name;
		/// <summary>
		/// Поле количества ингредиента
		/// </summary>
		private string _amount;
		/// <summary>
		/// Свойство для чтения названия ингредиента.
		/// </summary>
		public string Name { get { return _name; } }
		/// <summary>
		/// Свойство для чтения количества ингредиента.
		/// </summary>
		public string Amount { get { return _amount; } }
		/// <summary>
		/// Конструктор ингредиента, принимающий строку с информацией об ингредиенте.
		/// </summary>
		/// <param name="data">Строка с информацией об ингредиенте.</param>
		public Ingredient(string data)
		{
			string[] blocks = data.Split('-'); // поделим строку по "-", тоесть поделим на название ингредиента и его количество
			if (blocks.Length > 0) blocks[0] = blocks[0].Trim();
			if (blocks.Length > 1) blocks[1] = blocks[1].Trim();
			_name = blocks.Length > 0 ? blocks[0] : ""; // записываем название ингредиента
			_amount = blocks.Length > 1 ? blocks[1] : ""; // записываем количество ингредиента
		}
		/// <summary>
		/// Перегрузка метода, представляющего ингредиент в виде строки.
		/// </summary>
		/// <returns>Ингредиент в виде строки.</returns>
		public override string ToString()
		{
			if (Amount != "")
			{
				return $"{Name} - {Amount}";
			}
			else
			{
				return $"{Name}";
			}
		}
	}
}
