using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using SmallLabyWpfPlayer.ServiceRefSmallLaby;
using System.Windows.Media.Imaging;

namespace SmallLabyWpfPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SmallLabyClient m_client;
        private int m_player_id;

        private const int size = 50;

        private BitmapImage m_player_image;
        private BitmapImage m_road_image;
        private BitmapImage m_wall_image;
        private BitmapImage m_enemy_image;

        private string player_name = "alex";

        public MainWindow()
        {
            InitializeComponent();

            m_client = new SmallLabyClient();
            m_player_id = m_client.AddPlayer(player_name);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(10000);
            timer.Start();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            m_player_image = new BitmapImage();
            m_player_image.BeginInit();
            m_player_image.UriSource = new Uri("images/aze1.bmp", UriKind.Relative);
            m_player_image.DecodePixelHeight = (int)(size*0.75);
            m_player_image.DecodePixelWidth = (int)(size*0.75);
            m_player_image.EndInit();

            m_road_image = new BitmapImage();
            m_road_image.BeginInit();
            m_road_image.UriSource = new Uri("images/road.bmp", UriKind.Relative);
            m_road_image.DecodePixelHeight = size;
            m_road_image.DecodePixelWidth = size;
            m_road_image.EndInit();

            m_wall_image = new BitmapImage();
            m_wall_image.BeginInit();
            m_wall_image.UriSource = new Uri("images/mud.bmp", UriKind.Relative);
            m_wall_image.DecodePixelHeight = size;
            m_wall_image.DecodePixelWidth = size;
            m_wall_image.EndInit();

            m_enemy_image = new BitmapImage();
            m_enemy_image.BeginInit();
            m_enemy_image.UriSource = new Uri("images/player.bmp", UriKind.Relative);
            m_enemy_image.DecodePixelHeight = size;
            m_enemy_image.DecodePixelWidth = size;
            m_enemy_image.EndInit();
        }

        private void DrawPlayer(int x, int y)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);
            image.Source = m_player_image;
            m_paint_canvas.Children.Add(image);

            TextBlock name = new TextBlock();
            name.Text = player_name;
            name.Foreground = Brushes.Aqua;
            Canvas.SetTop(name, y * size);
            Canvas.SetLeft(name, x * size);

            m_paint_canvas.Children.Add(name);
        }

        private void DrawWall(int x, int y)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);

            image.Source = m_wall_image;
            m_paint_canvas.Children.Add(image);
        }

        private void DrawRoad(int x, int y)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);

            image.Source = m_road_image;
            m_paint_canvas.Children.Add(image);
        }

        private void DrawEnemy(int x, int y)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);

            image.Source = m_enemy_image;
            m_paint_canvas.Children.Add(image);
        }
        private void DrawMap()
        {
            int[] map = m_client.GetMap(m_player_id);
            int width = m_client.GetMapWidth();
            int height = m_client.GetMapHeight();

            m_paint_canvas.Children.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int field = map[y * width + x];

                    switch (field)
                    {
                        case 1:
                            DrawRoad(x, y);
                            break;
                        case 2:
                            DrawWall(x, y);
                            break;
                        case 9:
                            DrawEnemy(x, y);
                            break;
                    }
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int x, y;
            x = m_client.GetPosition(m_player_id, out y);

            DrawMap();
            DrawPlayer(x, y);
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.MoveDown);
                    break;
                case Key.Up:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.MoveUp);
                    break;
                case Key.Left:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.MoveLeft);
                    break;
                case Key.Right:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.MoveRight);
                    break;
                case Key.Space:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.StandStill);
                    break;
                case Key.R:
                    m_client.SetMovementStrategy(m_player_id, MovementStrategy.RandomDirection);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_client.RemovePlayer(m_player_id);
            m_client.Close();
        }
    }
}
