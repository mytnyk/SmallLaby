using System.ServiceModel;

namespace SmallLabyServer
{
    [ServiceContract]
    interface ISmallLaby
    {
        [OperationContract]
        int AddPlayer(string name);

        [OperationContract]
        void RemovePlayer(int player_id);

        [OperationContract]
        int[] GetMap();

        [OperationContract]
        int GetMapWidth();

        [OperationContract]
        int GetMapHeight();

        [OperationContract]
        void GetPosition(int player_id, out int x, out int y);

        [OperationContract]
        void SetPosition(int x, int y, int player_id);
    }
}
