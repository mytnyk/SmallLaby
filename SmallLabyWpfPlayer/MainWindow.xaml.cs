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
        private TerrainType[] m_map;
        private int m_map_width;
        private int m_map_height;
        private const int size = 50;

        private BitmapImage m_player_image;
        private BitmapImage m_grass_image;
        private BitmapImage m_mountain_image;
        private BitmapImage m_enemy_image;

        private string player_name = "alex";

        public MainWindow()
        {
            InitializeComponent();

            m_client = new SmallLabyClient();
            m_player_id = m_client.AddPlayer(player_name);

            m_map = m_client.GetMap();
            m_map_width = m_client.GetMapWidth();
            m_map_height = m_client.GetMapHeight();

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

            m_grass_image = new BitmapImage();
            m_grass_image.BeginInit();
            m_grass_image.UriSource = new Uri("images/grass.bmp", UriKind.Relative);
            m_grass_image.DecodePixelHeight = size;
            m_grass_image.DecodePixelWidth = size;
            m_grass_image.EndInit();

            m_mountain_image = new BitmapImage();
            m_mountain_image.BeginInit();
            m_mountain_image.UriSource = new Uri("images/mud.bmp", UriKind.Relative);
            m_mountain_image.DecodePixelHeight = size;
            m_mountain_image.DecodePixelWidth = size;
            m_mountain_image.EndInit();

            m_enemy_image = new BitmapImage();
            m_enemy_image.BeginInit();
            m_enemy_image.UriSource = new Uri("images/player.bmp", UriKind.Relative);
            m_enemy_image.DecodePixelHeight = size;
            m_enemy_image.DecodePixelWidth = size;
            m_enemy_image.EndInit();
        }

        private void DrawPlayers()
        {
            var players = m_client.GetPlayers();
            foreach (var player in players)
            {
                if (player.Id != m_player_id)
                {
                    DrawEnemy(player.X, player.Y, player.Id);
                }
                else
                    DrawMe(player.X, player.Y);
            }
        }

        private void DrawMe(int x, int y)
        {
            DrawBitmapImage(x, y, m_player_image);

            TextBlock name = new TextBlock();
            name.Text = player_name;
            name.Foreground = Brushes.Aqua;
            Canvas.SetTop(name, y * size);
            Canvas.SetLeft(name, x * size);

            m_paint_canvas.Children.Add(name);
        }

        private void DrawBitmapImage(int x, int y, BitmapImage image_source)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);

            image.Source = image_source;
            m_paint_canvas.Children.Add(image);
        }

        private void DrawMountain(int x, int y)
        {
            DrawBitmapImage(x, y, m_mountain_image);
        }

        private void DrawGrass(int x, int y)
        {
            DrawBitmapImage(x, y, m_grass_image);
        }

        private void DrawEnemy(int x, int y, int id)
        {
            DrawBitmapImage(x, y, m_enemy_image);

            TextBlock name = new TextBlock();
            name.Text = id.ToString();
            name.Foreground = Brushes.Red;
            Canvas.SetTop(name, y * size);
            Canvas.SetLeft(name, x * size);

            m_paint_canvas.Children.Add(name);
        }
        private void DrawMap()
        {
            for (int y = 0; y < m_map_height; y++)
            {
                for (int x = 0; x < m_map_width; x++)
                {
                    var field = m_map[y * m_map_width + x];

                    switch (field)
                    {
                        case TerrainType.Grass:
                            DrawGrass(x, y);
                            break;
                        case TerrainType.Mountain:
                            DrawMountain(x, y);
                            break;
                    }
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // clear canvas
            m_paint_canvas.Children.Clear();

            DrawMap();
            DrawPlayers();
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
