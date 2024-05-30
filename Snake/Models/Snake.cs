using System.Windows.Media;

namespace Snake.Models;
class Snake
{
	public List<Cell> Celle { get; } = new() { new(300,300) };

	public void Advance((int x, int y) kFac)
	{
        Celle.Add(Celle[Celle.Count - 1] + kFac);
		Celle.RemoveAt(0);
	}
}
