using System.ServiceModel;

namespace SmallLabyServer
{
    [ServiceContract]
    interface IPlayer
    {
        void Move();
    }

    [ServiceContract]
    interface ISmallLaby
    {
        [OperationContract]
        IPlayer AddPlayer(string name);
        [OperationContract]
        void RemovePlayer(IPlayer player);
    }
}
