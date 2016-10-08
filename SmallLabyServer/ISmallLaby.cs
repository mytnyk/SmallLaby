using System.ServiceModel;

namespace SmallLabyServer
{
    enum MovementStrategy
    {
        StandStill,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        RandomDirection,
    }

    [ServiceContract]
    interface ISmallLaby
    {
        [OperationContract]
        int AddPlayer(string name);

        [OperationContract]
        void RemovePlayer(int player_id);

        [OperationContract]
        int[] GetMap(int player_id);

        [OperationContract]
        int GetMapWidth();

        [OperationContract]
        int GetMapHeight();

        [OperationContract]
        void GetPosition(int player_id, out int x, out int y);

        [OperationContract]
        void SetPosition(int player_id, int x, int y);

        [OperationContract]
        void SetMovementStrategy(int player_id, MovementStrategy strategy);
    }
}
