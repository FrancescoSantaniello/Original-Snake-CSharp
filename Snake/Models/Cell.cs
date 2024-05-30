namespace Snake.Models;
class Cell
{
	public const int CELL_SIZE = 25;

	private int _x;
	private int _y;
	public static bool operator ==(Cell a, Cell b)
	{
		return (a.X == b.X) && (a.Y == b.Y);
	}
	public static bool operator !=(Cell a, Cell b)
	{
		return !(a == b);
	}
	public static Cell operator +(Cell a, (int x, int y) k)
	{
		return new(a.X + (k.x * CELL_SIZE), a.Y + (k.y * CELL_SIZE));
	}
	public static Cell operator -(Cell a, (int x, int y) k)
	{
		return new(a.X - (k.x * CELL_SIZE), a.Y - (k.y * CELL_SIZE));
	}

	public int X
	{
		get => _x;
		set
		{
			if (value <= -CELL_SIZE)
				value = 600 - CELL_SIZE;
			else if (value >= 600)
				value = 0;

			_x = value;
		}
	}
	public int Y
	{
		get => _y;
		set
		{
			if (value <= -CELL_SIZE)
				value = 600 - CELL_SIZE;
			else if (value >= 600)
				value = 0;

			_y = value;
		}
	}
	public Cell(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Cell() { }
}
