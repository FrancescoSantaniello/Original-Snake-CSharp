using System.Windows.Media;

namespace Snake.Models;
class Food
{
	public Cell Cella { get; init; }
	
	public static Food GenFood(Snake snake)
	{
		Random rand = new();
		int x = rand.Next(0, 600 / Cell.CELL_SIZE), y = rand.Next(0, 600 / Cell.CELL_SIZE);

		while (snake.Celle.Where(c => c == new Cell(x, y)).Any())
		{
			x = rand.Next(0, 600 / Cell.CELL_SIZE);
			y = rand.Next(0, 600 / Cell.CELL_SIZE);
		}

		return new(new(x * Cell.CELL_SIZE, y * Cell.CELL_SIZE));
	}

	public Food(Cell cella)
	{
		Cella = cella;
	}
}
