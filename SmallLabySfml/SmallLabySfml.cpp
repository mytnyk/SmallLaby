// SmallLabySfml.cpp : main project file.

#include "stdafx.h"
#include <SFML/Graphics.hpp>
#include "ClientModel.h"

using namespace System;

using namespace sf;
RenderWindow window(VideoMode(800, 700), "First sfml program!");
using namespace SmallLabyUser::ServiceRefSmallLaby;

bool MonsterIsOnField(int x, int y, const std::vector<Monster> &monsters)
{
  for (size_t i = 0; i < monsters.size(); i++)
  {
    if (monsters[i].X == y && monsters[i].Y == x)
    {
      return true;
    }
  }
  return false;
}

void DrawMap(
  const std::vector<std::vector<Terrain>> &map,
  const std::vector<Player> &players, 
  const std::vector<Monster> &monsters, int player_id)
{
  bool b;
  int size = 30;
  RectangleShape rectangle;
  rectangle.setSize(Vector2f(size, size));
  for (size_t i = 0; i < map.size(); i++)
  {
    for (size_t j = 0; j < map[i].size(); j++)
    {
      for (size_t k = 0; k < players.size(); k++)
      {
        if (players[k].X == j && players[k].Y == i)
        {
          if (player_id == players[k].Id)
            rectangle.setFillColor(Color::Magenta);
          else
            rectangle.setFillColor(Color::Blue);
          b = true;
          break;
        }
      }
      if (MonsterIsOnField(i, j, monsters))
      {
        rectangle.setFillColor(Color::Yellow);
        b = true;
      }
      if (!b)
      {
        if (map[i][j] == Terrain::Road)
        {
          rectangle.setFillColor(Color::Green);
        }
        if (map[i][j] == Terrain::Wall)
        {
          rectangle.setFillColor(Color::Red);
        }
      }
      rectangle.setPosition(j * size, i * size);
      window.draw(rectangle);
      b = false;
    }
  }
}

int main(array<System::String ^> ^args)
{
  ClientModel client_model;
  int player_id = client_model.AddPlayer("alex");
  int w = client_model.GetWidth();
  int h = client_model.GetHeight();
  auto map = client_model.GetMap();
  auto players = client_model.GetPlayers();
  //Thread thread(&DrawMap(map, players, player_id));
  //thread.launch();
  while (window.isOpen())
  {
    Event event;
    while (window.pollEvent(event))
    {
      if (event.type == Event::Closed)
      {
        window.close();
        continue;
      }
      if (Keyboard::isKeyPressed(Keyboard::Escape))
      {
        window.close();
        continue;
      }
      if (Keyboard::isKeyPressed(Keyboard::Down))
        client_model.SetMoveStrategy(player_id, MoveStrategy::MoveDown);
      if (Keyboard::isKeyPressed(Keyboard::Up))
        client_model.SetMoveStrategy(player_id, MoveStrategy::MoveUp);
      if (Keyboard::isKeyPressed(Keyboard::Left))
        client_model.SetMoveStrategy(player_id, MoveStrategy::MoveLeft);
      if (Keyboard::isKeyPressed(Keyboard::Right))
        client_model.SetMoveStrategy(player_id, MoveStrategy::MoveRight);
    }

    window.clear();
    auto players = client_model.GetPlayers();
    auto monsters = client_model.GetMonsters();
    DrawMap(map, players, monsters, player_id);
    window.display();
  }

  client_model.RemovePlayer(player_id);
  client_model.Close();

  return 0;
}
