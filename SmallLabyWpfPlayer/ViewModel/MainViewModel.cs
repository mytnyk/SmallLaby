using System.Windows.Input;

namespace SmallLabyWpfPlayer
{
    public class MainViewModel : ViewModelBase
    {
        public SessionViewModel CurrentSessionViewModel { get; private set; }

        private string m_player_name;

        private ClientModel m_model;

        public MainViewModel(ClientModel model)
        {
            m_model = model;
            CurrentSessionViewModel = new SessionViewModel(model);
        }

        public string PlayerName
        {
            get { return m_player_name; }
            set
            {
                m_player_name = value;
                OnPropertyChanged("PlayerName");
            }
        }

        public bool PlayerNameEnabled => !m_model.IsConnected;

        public bool ConnectEnabled => !m_model.IsConnected;

        public ICommand MoveUpCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.MoveUp));
        public ICommand MoveDownCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.MoveDown));
        public ICommand MoveLeftCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.MoveLeft));
        public ICommand MoveRightCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.MoveRight));
        public ICommand DoNotMoveCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.DoNotMove));
        public ICommand MoveRandomCommand => new RelayCommand(
            param => m_model.SetMovementStrategy(ClientModel.MovementStrategy.MoveRandom));
        public ICommand ConnectCommand => new RelayCommand(param => Connect());

        public void Connect()
        {
            m_model.Connect(m_player_name);
            OnPropertyChanged("PlayerNameEnabled");
            OnPropertyChanged("ConnectEnabled");
        }

        public void Disconnect()
        {
            m_model.Disconnect();
            OnPropertyChanged("PlayerNameEnabled");
            OnPropertyChanged("ConnectEnabled");
        }
    }
}
