#pragma once
#include <memory>
#include <vector>

enum class Terrain
{
  Wall,
  Road
};

class ClientModel
{
private:
  struct PrivateModel;

  std::unique_ptr<PrivateModel> _model_impl;

  ClientModel(const ClientModel&);
  ClientModel& operator=(const ClientModel&);

public:
  ClientModel();
  ~ClientModel();
  int GetWidth();
  int GetHeight();
  std::vector<std::vector<Terrain>> GetMap();
};

