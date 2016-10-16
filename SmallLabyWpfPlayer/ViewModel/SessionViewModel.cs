using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SmallLabyWpfPlayer
{
    public class BaseImageViewModel
    {
        public double Left { get; set; }
        public double Top { get; set; }
    }
    class TextBoxViewModel : BaseImageViewModel
    {
        public string Text { get; set; }
    }

    class ImageBoxViewModel : BaseImageViewModel
    {
        public BitmapImage Bitmap { get; set; }
    }

    public class SessionViewModel : ViewModelBase
    {
        public InfoViewModel CurrentInfoViewModel { get; private set; }
        public ObservableCollection<BaseImageViewModel> Items { get; set; }

        private BitmapImage m_player_image;
        private BitmapImage m_gold_image;
        private BitmapImage m_grass_image;
        private BitmapImage m_mountain_image;
        private BitmapImage m_enemy_image;

        private const int size = 50;
        private ClientModel m_model;

        public SessionViewModel(ClientModel model)
        {
            m_model = model;

            CurrentInfoViewModel = new InfoViewModel(model);

            m_player_image   = CreateBitmapImage("/Images/aze1.bmp");
            m_gold_image     = CreateBitmapImage("/Images/gold1.png");
            m_grass_image    = CreateBitmapImage("/Images/grass.bmp");
            m_mountain_image = CreateBitmapImage("/Images/mud.bmp");
            m_enemy_image    = CreateBitmapImage("/Images/player.bmp");

            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Update);

            timer.Interval = new TimeSpan(10000);
            timer.Start();
        }

        private BitmapImage CreateBitmapImage(string uri)
        {
            var bitmap_image = new BitmapImage();
            bitmap_image.BeginInit();
            bitmap_image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bitmap_image.DecodePixelHeight = size;
            bitmap_image.DecodePixelWidth = size;
            bitmap_image.UriSource = new Uri(uri, UriKind.Relative);
            bitmap_image.EndInit();
            return bitmap_image;
        }

        private void Update(object sender, EventArgs e)
        {
            var new_items = new ObservableCollection<BaseImageViewModel>();

            AddMap(new_items);
            AddPlayers(new_items);
            AddItems(new_items);

            Items = new_items;
            OnPropertyChanged("Items");

            CurrentInfoViewModel.Update();
        }

        private void AddPlayers(ICollection<BaseImageViewModel> items)
        {
            var players = m_model.GetPlayers();
            foreach (var player in players)
            {
                if (player.Me)
                    AddMe(player.X, player.Y, player.Name, items);
                else
                    AddEnemy(player.X, player.Y, player.Name, items);
            }
        }

        private void AddItems(ICollection<BaseImageViewModel> items)
        {
            var elements = m_model.GetItems();
            foreach (var element in elements)
            {
                switch (element.Type)
                {
                    case ClientModel.ItemType.Gold:
                        AddGold(element.X, element.Y, items);
                        break;
                }
            }
        }

        private void AddGold(int x, int y, ICollection<BaseImageViewModel> items)
        {
            items.Add(CreateImageBoxViewModel(x, y, m_gold_image));
        }

        private void AddMe(int x, int y, string name,
            ICollection<BaseImageViewModel> items)
        {
            items.Add(CreateImageBoxViewModel(x, y, m_player_image));
            items.Add(CreateTextBoxViewModel(x, y, name));
        }

        private void AddEnemy(int x, int y, string name,
            ICollection<BaseImageViewModel> items)
        {
            items.Add(CreateImageBoxViewModel(x, y, m_enemy_image));
            items.Add(CreateTextBoxViewModel(x, y, name));
        }

        private BaseImageViewModel CreateTextBoxViewModel(int x, int y, string text)
        {
            return new TextBoxViewModel()
            {
                Top = y * size,
                Left = x * size,
                Text = text
            };
        }

        private BaseImageViewModel CreateImageBoxViewModel(int x, int y, BitmapImage image_source)
        {
            return new ImageBoxViewModel()
            {
                Top = y * size,
                Left = x * size,
                Bitmap = image_source
            };
        }

        private BaseImageViewModel CreateMountain(int x, int y)
        {
            return CreateImageBoxViewModel(x, y, m_mountain_image);
        }

        private BaseImageViewModel CreateGrass(int x, int y)
        {
            return CreateImageBoxViewModel(x, y, m_grass_image);
        }

        private void AddMap(ICollection<BaseImageViewModel> items)
        {
            foreach (var field in m_model.GetMap())
            {
                switch (field.Terrain)
                {
                    case ClientModel.Terrain.Grass:
                        items.Add(CreateGrass(field.X, field.Y));
                        break;
                    case ClientModel.Terrain.Mountain:
                        items.Add(CreateMountain(field.X, field.Y));
                        break;
                }
            }
        }
    }
}
