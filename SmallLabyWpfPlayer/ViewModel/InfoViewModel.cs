namespace SmallLabyWpfPlayer
{
    public class InfoViewModel : ViewModelBase
    {
        private ClientModel m_model;

        public double Speed => m_model.PlayerSpeed;

        public InfoViewModel(ClientModel model)
        {
            m_model = model;
        }

        public int Gold => m_model.PlayerGold;

        public void Update()
        {
            OnPropertyChanged("Gold");
            OnPropertyChanged("Speed");
        }
    }
}
