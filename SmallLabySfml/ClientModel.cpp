#include "stdafx.h"
#include "ClientModel.h"

#include <vcclr.h>
using namespace System;
//using namespace SmallLabyUser;
using namespace SmallLabyUser::ServiceRefSmallLaby;

struct ClientModel::PrivateModel
{
  gcroot<SmallLabyClient^> client;
  PrivateModel()
  {
    client = gcnew SmallLabyClient();
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
  array<TerrainType>^ map = _model_impl->client->GetMap();

  int map_height = GetHeight();
  int map_width = GetWidth();
  std::vector<std::vector<Terrain>> terrain_map;
  terrain_map.resize(map_height);
  for (int y = 0; y < map_height; y++)
  {
    terrain_map[y].resize(map_width);
    for (int x = 0; x < map_width; x++)
    {
      TerrainType field = map[y * map_width + x];
      switch (field)
      {
      case TerrainType::Mountain:
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

