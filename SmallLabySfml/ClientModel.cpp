#include "stdafx.h"
#include "ClientModel.h"

#include <vcclr.h>
#include <string>
using namespace System;
using namespace SmallLabyUser;
//using namespace SmallLabyUser::ServiceRefSmallLaby;

struct ClientModel::PrivateModel
{
  gcroot<ServiceRefSmallLaby::SmallLabyClient^> client;
  PrivateModel()
  {
    client = gcnew ServiceRefSmallLaby::SmallLabyClient();
  }
  ~PrivateModel()
  {
    client->Close();
  }
};

ClientModel::ClientModel()
{
  _model_impl = std::make_unique<PrivateModel>();
}


ClientModel::~ClientModel()
{
}

int ClientModel::GetWidth()
{
  return _model_impl->client->GetMapWidth();
}

int ClientModel::GetHeight()
{
  return _model_impl->client->GetMapHeight();
}

std::vector<std::vector<Terrain>> ClientModel::GetMap()
{
  array<ServiceRefSmallLaby::TerrainType>^ map = _model_impl->client->GetMap();
  int map_height = GetHeight();
  int map_width = GetWidth();
  std::vector<std::vector<Terrain>> terrain_map;
  terrain_map.resize(map_height);
  for (int y = 0; y < map_height; y++)
  {
    terrain_map[y].resize(map_width);
    for (int x = 0; x < map_width; x++)
    {
      ServiceRefSmallLaby::TerrainType field = map[y * map_width + x];
      switch (field)
      {
      case ServiceRefSmallLaby::TerrainType::Mountain:
        terrain_map[y][x] = Terrain::Wall;
        break;
      default:
        terrain_map[y][x] = Terrain::Road;
        break;
      }
    }
  }
  return terrain_map;
}

int ClientModel::AddPlayer(std::string name)
{
  return _model_impl->client->AddPlayer("alex");
}

void ClientModel::SetMoveStrategy(int player_id, MoveStrategy movestrategy)
{
  ServiceRefSmallLaby::MovementStrategy movementstrategy;
  switch (movestrategy)
  {
  case MoveStrategy::MoveLeft:
    movementstrategy = ServiceRefSmallLaby::MovementStrategy::MoveLeft;
    break;
  case MoveStrategy::MoveRight:
    movementstrategy = ServiceRefSmallLaby::MovementStrategy::MoveRight;
    break;
  case MoveStrategy::MoveUp:
    movementstrategy = ServiceRefSmallLaby::MovementStrategy::MoveUp;
    break;
  case MoveStrategy::MoveDown:
    movementstrategy = ServiceRefSmallLaby::MovementStrategy::MoveDown;
    break;
  default:
    break;
  }
  _model_impl->client->SetMovementStrategy(player_id, movementstrategy);
}

void ClientModel::RemovePlayer(int player_id)
{
  _model_impl->client->RemovePlayer(player_id);
}

void ClientModel::Close()
{
  _model_impl->client->Close();
}

std::vector<Player> ClientModel::GetPlayers()
{
  array<ServiceRefSmallLaby::PlayerInfo>^ players = _model_impl->client->GetPlayers();
  std::vector<Player> playersv;
  for (int i = 0; i < players->GetLength(0); i++)
  {
    Player player;
    player.X = players[i].X;
    player.Y = players[i].Y;
    player.Id = players[i].Id;
    playersv.push_back(player);
  }
  return playersv;
}

std::vector<Monster> ClientModel::GetMonsters()
{
  array<ServiceRefSmallLaby::MonsterInfo>^ monsters = _model_impl->client->GetMonsters();
  std::vector<Monster> monstersv;
  for (int i = 0; i < monsters->GetLength(0); i++)
  {
    Monster monster;
    monster.X = monsters[i].X;
    monster.Y = monsters[i].Y;
    monstersv.push_back(monster);
  }
  return monstersv;
}