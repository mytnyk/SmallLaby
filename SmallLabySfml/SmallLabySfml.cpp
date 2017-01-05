// SmallLabySfml.cpp : main project file.

#include "stdafx.h"
#include <SFML/Graphics.hpp>
#include "ClientModel.h"

using namespace System;

using namespace sf;


int main(array<System::String ^> ^args)
{
  ClientModel client_model;
  int w = client_model.GetWidth();
  int h = client_model.GetHeight();
  auto map = client_model.GetMap();
  RenderWindow window(VideoMode(200, 200), "First sfml program!", Style::Fullscreen);
  int size = 50;
  RectangleShape rectangle;
  rectangle.setSize(sf::Vector2f(size, size));
  while (window.isOpen())
  {
    Event event;
    while (window.pollEvent(event))
    {
      if (event.type == Event::Closed)
        window.close();
      if (Keyboard::isKeyPressed(Keyboard::Escape))
        window.close();

    }

    window.clear();
    for (size_t i = 0; i < map.size(); i++)
    {
      for (size_t j = 0; j < map[i].size(); j++)
      {
        if (map[i][j] == Terrain::Road)
        {
          rectangle.setFillColor(Color::Green);
        }
        if (map[i][j] == Terrain::Wall)
        {
          rectangle.setFillColor(Color::Red);
        }
        rectangle.setPosition(j * size, i * size);
        window.draw(rectangle);
      }
    }
    window.display();
  }

  return 0;
}
