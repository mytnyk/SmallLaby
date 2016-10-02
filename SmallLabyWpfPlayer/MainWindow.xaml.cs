using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using SmallLabyWpfPlayer.ServiceRefSmallLaby;

namespace SmallLabyWpfPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int UPWARDS = 8;
        private const int DOWNWARDS = 2;
        private const int TOLEFT = 4;
        private const int TORIGHT = 6;

        // Movement direction initialisation
        private int m_direction = 0;

        private SmallLabyClient m_client;
        private int m_player_id;

        private const int size = 30;

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
        }

        private void DrawPlayer(int x, int y)
        {
            Ellipse newEllipse = new Ellipse();
            newEllipse.Width = size;
            newEllipse.Height = size;
            newEllipse.Fill = Brushes.Blue;
            Canvas.SetTop(newEllipse, y * size);
            Canvas.SetLeft(newEllipse, x * size);
            m_paint_canvas.Children.Add(newEllipse);
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

                    Ellipse newEllipse = new Ellipse();
                    newEllipse.Width = size;
                    newEllipse.Height = size;

                    switch (field)
                    {
                        case 1:
                            newEllipse.Fill = Brushes.Yellow;
                            break;
                        case 2:
                            newEllipse.Fill = Brushes.Brown;
                            break;
                        default:
                            newEllipse.Fill = Brushes.Red;
                            break;
                    }

                    Canvas.SetTop(newEllipse, y * size);
                    Canvas.SetLeft(newEllipse, x * size);
                    m_paint_canvas.Children.Add(newEllipse);
                }
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            int x, y;
            x = m_client.GetPosition(m_player_id, out y);

            switch (m_direction)
            {
                case DOWNWARDS:
                    y++;
                    break;
                case UPWARDS:
                    y--;
                    break;
                case TOLEFT:
                    x--;
                    break;
                case TORIGHT:
                    x++;
                    break;
            }

            m_client.SetPosition(x, y, m_player_id);
            x = m_client.GetPosition(m_player_id, out y);

            DrawMap();
            DrawPlayer(x, y);
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    m_direction = DOWNWARDS;
                    break;
                case Key.Up:
                    m_direction = UPWARDS;
                    break;
                case Key.Left:
                    m_direction = TOLEFT;
                    break;
                case Key.Right:
                    m_direction = TORIGHT;
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
