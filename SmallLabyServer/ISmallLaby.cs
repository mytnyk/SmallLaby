using System.ServiceModel;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace SmallLabyServer
{
    [StructLayout(LayoutKind.Sequential)]
    [DataContract]
    public struct PlayerInfo
    {
        [DataMember]
        public int Id;
        [DataMember]
        public int X;
        [DataMember]
        public int Y;
    }
    enum Item
    {
        Gold,
        Bonus,
        Exit,
    }
    [StructLayout(LayoutKind.Sequential)]
    [DataContract]
    struct ItemInfo
    {
        [DataMember]
        public Item Item;
        [DataMember]
        public int X;
        [DataMember]
        public int Y;
    }
    enum TerrainType
    {
        Grass,
        Forest,
        Water,
        Mountain,
    }
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
        int GetGold(int player_id);

        // Call it once on start up
        [OperationContract]
        TerrainType[] GetMap();

        [OperationContract]
        PlayerInfo[] GetPlayers();

        [OperationContract]
        ItemInfo[] GetItems();

        [OperationContract]
        int GetMapWidth();

        [OperationContract]
        int GetMapHeight();

        [OperationContract]
        void SetMovementStrategy(int player_id, MovementStrategy strategy);
    }
}
