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

        private const int size = 30;

        private BitmapImage m_user_image;

        public MainWindow()
        {
            InitializeComponent();

            m_client = new SmallLabyClient();
            m_player_id = m_client.AddPlayer("alex");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(10000);
            timer.Start();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            m_user_image = new BitmapImage();
            m_user_image.BeginInit();
            m_user_image.UriSource = new Uri("player.bmp", UriKind.Relative);
            m_user_image.DecodePixelHeight = size;
            m_user_image.DecodePixelWidth = size;
            m_user_image.EndInit();
        }

        private void DrawPlayer(int x, int y)
        {
            Image image = new Image();
            Canvas.SetTop(image, y * size);
            Canvas.SetLeft(image, x * size);

            image.Source = m_user_image;
            m_paint_canvas.Children.Add(image);
        }
        private void DrawMap()
        {
            int[] map = m_client.GetMap();
            int width = m_client.GetMapWidth();
            int height = m_client.GetMapHeight();

            m_paint_canvas.Children.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int field = map[y * width + x];

                    var cell = new Rectangle();
                    cell.Width = size;
                    cell.Height = size;

                    switch (field)
                    {
                        case 1:
                            cell.Fill = Brushes.Yellow;
                            break;
                        case 2:
                            cell.Fill = Brushes.Brown;
                            break;
                        default:
                            cell.Fill = Brushes.Red;
                            break;
                    }

                    Canvas.SetTop(cell, y * size);
                    Canvas.SetLeft(cell, x * size);
                    m_paint_canvas.Children.Add(cell);
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
