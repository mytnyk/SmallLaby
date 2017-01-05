using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using OlivecDx;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OlivecDx.Tex;

namespace SmallLabyWpfPlayer
{
  public class Resource
  {
    public static byte[] ExtractResource(string filename)
    {
      var a = System.Reflection.Assembly.GetExecutingAssembly();
      using (var resFilestream = a.GetManifestResourceStream(filename))
      {
        if (resFilestream == null) return null;
        var ba = new byte[resFilestream.Length];
        resFilestream.Read(ba, 0, ba.Length);
        return ba;
      }
    }
  }
  public class Window3DViewModel : ViewModelBase
  {
    private readonly ClientModel _model;
    private readonly View _view;
    public Window3DViewModel(ClientModel model)
    {
      _model = model;

      var walls = new List<Triangle>();
      var floor = new List<Triangle>();

      var model_map = _model.GetMap();
      var map = model_map.Select(f => f.Terrain == ClientModel.Terrain.Mountain ? 
      ElementType.Wall : ElementType.Road).ToArray();

      var map2d = new Map3DConstructor(map, _model.MapWidth, _model.MapHeight);
      map2d.Generate(walls, floor, 8);

      byte[] wall_triangles_bytes = Resource.ExtractResource("SmallLabyWpfPlayer.Images.GeneticaMortarlessBlocks.jpg");
      var wall_triangles = new Triangles(
        walls.SelectMany(t => new[] { t.A, t.B, t.C }),
        walls.SelectMany(t => new[] { t.At, t.Bt, t.Ct }),
        wall_triangles_bytes);
      var wall_obj = new SceneObject(wall_triangles, Matrix4x4.Identity);

      byte[] floor_triangles_bytes = Resource.ExtractResource("SmallLabyWpfPlayer.Images.mud.bmp");
      var floor_triangles = new Triangles(
        floor.SelectMany(t => new[] { t.A, t.B, t.C }),
        floor.SelectMany(t => new[] { t.At, t.Bt, t.Ct }),
        floor_triangles_bytes);
      var floor_obj = new SceneObject(floor_triangles, Matrix4x4.Identity);


      byte[] gold_triangles_bytes = Resource.ExtractResource("SmallLabyWpfPlayer.Images.gold1.png");
      var gold = Map3DConstructor.GenerateWall(1 * 8, 2 * 8, 1 * 8, 1 * 8, 8);
      var gold_triangles = new Triangles(
        gold.SelectMany(t => new[] { t.A, t.B, t.C }),
        gold.SelectMany(t => new[] { t.At, t.Bt, t.Ct }),
        gold_triangles_bytes);
      var gold_obj = new SceneObject(gold_triangles, Matrix4x4.Identity);

      var scene = new Scene();
      scene.AddObject(wall_obj);
      scene.AddObject(floor_obj);
      //scene.AddObject(gold_obj);

      _view = new OlivecDx.View(600, 600)
      {
        Scene = scene,
        //ViewMatrix = new Matrix4x4()
      };

      _view.InitBuffers();

      //CompositionTarget.Rendering += OnRendering;
      Surface = new D3DImage();

      IntPtr backBuffer = _view.GetBackBufferPtr();
      Surface.Lock();
      Surface.SetBackBuffer(D3DResourceType.IDirect3DSurface9, backBuffer);
      Surface.Unlock();

      _view.Position = new Vector3(4, 4, 4);
      _view.Direction = new Vector3(1, 0, 0);
      /*
      var shellRotationThread = new Thread(() =>
      {
        while (true)
        {
          _view.TestRotate();
          Thread.Sleep(100);
        }
      });
      shellRotationThread.Start();*/
    }

    public void Update()
    {
      var me = _model.GetPlayers().Where(p => p.Me);
      if (me.Any())
      {
        _view.Position = new Vector3(me.First().X*8 + 4, me.First().Y * 8 + 4, 4);
      }

      _view.Render();
      Surface.Lock();
      Surface.AddDirtyRect(new Int32Rect(0, 0, Surface.PixelWidth, Surface.PixelHeight));
      Surface.Unlock();
    }
    /*
    private void OnRendering(object sender, EventArgs e)
    {
      _view.Render();
      Surface.Lock();
      Surface.AddDirtyRect(new Int32Rect(0, 0, Surface.PixelWidth, Surface.PixelHeight));
      Surface.Unlock();
    }*/

    public D3DImage Surface { get; }

    private ICommand _change_color;

    public ICommand ChangeColorCmd
    {
      get { return _change_color ?? (_change_color = new RelayCommand(param => ChangeColor())); }
    }

    private void ChangeColor()
    {
      _view.SetNewBgColor();
    }
  }
}
