using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Snake.Models;

namespace Snake;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		Background = (Brush)new BrushConverter().ConvertFrom("#578a34");
		gameCanvas.Background = (Brush)new BrushConverter().ConvertFrom("#a7d34f");

		_food = Food.GenFood(_snake);

		_timer.Tick += (sender, e) => Game();
		_timer.Interval = TimeSpan.FromMilliseconds(50);
		_timer.Start();
	}

	private (int x, int y) _kFac = (0,0);
	private int _point = 0;
    private Food _food;

    private readonly SortedSet<int> _pointsList = new();
	private readonly Snake.Models.Snake _snake = new();
	private readonly DispatcherTimer _timer = new();
    private const int bufferAcceleration = 10;
	private readonly TimeSpan defaultTimeUpdate = TimeSpan.FromMilliseconds(50);
	private readonly Color snakeColor = Color.FromRgb(68, 113, 229);

	private readonly SoundPlayer soundMove = new(@"..\..\..\Sounds\move.wav");
	private readonly SoundPlayer soundFood = new(@"..\..\..\Sounds\food.wav");
	private readonly SoundPlayer soundGameOver = new(@"..\..\..\Sounds\gameover.wav");

	private void Window_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.P)
		{
			_timer.IsEnabled = !_timer.IsEnabled;
			labelPause.Visibility = !_timer.IsEnabled ? Visibility.Visible : Visibility.Hidden;
		}

		if (_timer.IsEnabled)
		{
			switch (e.Key)
			{
				case Key.Z:
					if (_timer.Interval.TotalMilliseconds > 0)
						_timer.Interval -= TimeSpan.FromMilliseconds(bufferAcceleration);
					break;

				case Key.X:
					if (_timer.Interval.TotalMilliseconds < TimeSpan.MaxValue.TotalMilliseconds)
						_timer.Interval += TimeSpan.FromMilliseconds(bufferAcceleration);
                    break;

				case Key.Space:
					_timer.Interval = defaultTimeUpdate;
					break;

                case Key.W or Key.Up:
					if (_kFac != (0, 1))
					{ 
						_kFac = (0, -1);
						soundMove.Play();
					}
					break;

				case Key.S or Key.Down:
					if (_kFac != (0, -1))
					{
						_kFac = (0, 1);
						soundMove.Play();
					}
					break;

				case Key.A or Key.Left:
					if (_kFac != (1, 0))
					{
						_kFac = (-1, 0);
						soundMove.Play();
					}
					break;

				case Key.D or Key.Right:
					if (_kFac != (-1, 0))
					{
						_kFac = (1, 0);
						soundMove.Play();
					}
					break;
			}
		}

	}
	private bool IsLosed()
	{
		return _snake.Celle.Where(c => c == _snake.Celle[_snake.Celle.Count - 1]).Count() > 1;
	}
	private bool FoodIsEaten()
	{
		return _snake.Celle.Where(c => c == _food.Cella).Any();
	}
	private void PrintSnake()
	{
        foreach (Cell c in _snake.Celle)
		{
            Ellipse ellipse = new()
			{
				Stroke = new SolidColorBrush(snakeColor),
                Fill = new SolidColorBrush(snakeColor),
                Width = Width / Cell.CELL_SIZE,
                Height = Height / Cell.CELL_SIZE
            };

            Canvas.SetLeft(ellipse, c.X);
			Canvas.SetTop(ellipse, c.Y);

			gameCanvas.Children.Add(ellipse);
		}
	}
	private void PrintFood()
	{
        Ellipse ellipse = new()
        {
            Stroke = new SolidColorBrush(Colors.Red),
            Fill = new SolidColorBrush(Colors.Red),
            Width = Width / Cell.CELL_SIZE,
            Height = Height / Cell.CELL_SIZE
        };

        Canvas.SetLeft(ellipse, _food.Cella.X);
		Canvas.SetTop(ellipse, _food.Cella.Y);

		gameCanvas.Children.Add(ellipse);
	}
	public void Game()
	{
        if (IsLosed())
		{
			soundGameOver.Play();
			End();
		}
     
		if (FoodIsEaten())
        {
			soundFood.Play();
            _point++;
            labelPoints.Content = $"Punteggio: {_point}";
            _snake.Celle.Add(_food.Cella);
            _food = Food.GenFood(_snake);
        }

        _snake.Advance(_kFac);

        gameCanvas.Children.Clear();

        PrintFood();
        PrintSnake();
    }
	public void End()
	{
		_timer.Stop();
		_pointsList.Add(_point);

		if (MessageBox.Show($"Hai perso :/\nPunteggio piu alto: {_pointsList.Max}\nPunteggio attuale: {_point}\nVuoi fare un altra partita?", Title, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
		{
			_snake.Celle.Clear();
			_snake.Celle.Add(new(300, 300));

            _food = Food.GenFood(_snake);
			_point = 0;
			_kFac = (0,0);
			labelPoints.Content = $"Punteggio: {_point}";

			_timer.Start();
		}
		else
			Environment.Exit(0);
	}

    private void buttonIstruzioni_Click(object sender, RoutedEventArgs e)
    {
		MessageBox.Show("ciao che si dice");
    }
}