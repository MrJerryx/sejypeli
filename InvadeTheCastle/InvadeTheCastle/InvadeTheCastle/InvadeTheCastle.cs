using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class InvadeTheCastle : PhysicsGame
{

    PlatformCharacter pelaaja1;
    PlatformCharacter pelaaja2;

    DoubleMeter Laskuri1;
    DoubleMeter Laskuri2;

    Cannon pelaajan1Ase;

    Image LinnanKuva1 = LoadImage("LinnanKuva1");
    Image LinnanKuva2 = LoadImage("LinnanKuva2");
    Image Nuoli = LoadImage("Nuoli");

    public override void Begin()
    {
        LuoKentta();
        LuoLaskuri1();
        LuoLaskuri2();


        Camera.ZoomToLevel();

        Keyboard.Listen(Key.A, ButtonState.Down,
         LiikutaPelaajaa, null, new Vector(-10000, 0));
        Keyboard.Listen(Key.D, ButtonState.Down,
          LiikutaPelaajaa, null, new Vector(10000, 0));
        Keyboard.Listen(Key.W, ButtonState.Down,
          LiikutaPelaajaa, null, new Vector(0, 1000));

        Keyboard.Listen(Key.Left, ButtonState.Down,
          LiikutaPelaajaa2, null, new Vector(-10000, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down,
          LiikutaPelaajaa2, null, new Vector(10000, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down,
          LiikutaPelaajaa2, null, new Vector(0, 1000));

        Keyboard.Listen(Key.A, ButtonState.Released,
         LiikutaPelaajaa, null, new Vector(0, 0));
        Keyboard.Listen(Key.D, ButtonState.Released,
          LiikutaPelaajaa, null, new Vector(0, 0));
        Keyboard.Listen(Key.W, ButtonState.Released,
          LiikutaPelaajaa, null, new Vector(0, 0));

        Keyboard.Listen(Key.Left, ButtonState.Released,
          LiikutaPelaajaa2, null, new Vector(0, 0));
        Keyboard.Listen(Key.Right, ButtonState.Released,
          LiikutaPelaajaa2, null, new Vector(0, 0));
        Keyboard.Listen(Key.Up, ButtonState.Released,
          LiikutaPelaajaa2, null, new Vector(0, 0));

        Keyboard.Listen(Key.Space, ButtonState.Down, AmmuAseella, "Ammu", pelaajan1Ase);

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void LuoKentta()
    {
        
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta2");


        ruudut.SetTileMethod(Color.Red, LuoPelaaja1);
        ruudut.SetTileMethod(Color.BrightGreen, LuoPelaaja2);
        ruudut.SetTileMethod(Color.Black, LuoTaso);
        ruudut.SetTileMethod(Color.Gray, LuoLinna1);
        ruudut.SetTileMethod(Color.DarkGray, LuoLinna2);
     
        
        ruudut.Execute(20, 30);

        Gravity = new Vector(0.0, -800.0);
    }
  
    void LuoLaskuri1()
    {
        Laskuri1 = new DoubleMeter(0);
        Laskuri1.MaxValue = 3000;

        ProgressBar Palkki1 = new ProgressBar(300, 30);
        Palkki1.BindTo(Laskuri1);

        Palkki1.BarColor = Color.Red;
        Palkki1.BorderColor = Color.DarkGray;

        Palkki1.X = Screen.Left + 300;
        Palkki1.Y = Screen.Top - 100;

        Add(Palkki1);

     /*
         for (int i = 0; 1 < 3000; i++)
        {
            Laskuri1.Value += 1;
        }
     */
    }

    void LuoLaskuri2()
    {
        Laskuri2 = new DoubleMeter(0);
        Laskuri2.MaxValue = 3000;

        ProgressBar Palkki2 = new ProgressBar(300, 30);
        Palkki2.BindTo(Laskuri2);

        Palkki2.BarColor = Color.Red;
        Palkki2.BorderColor = Color.DarkGray;

        Palkki2.X = Screen.Right - 300;
        Palkki2.Y = Screen.Top - 100;
        Add(Palkki2);
    }

    void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
   
    {
        pelaaja1 = new PlatformCharacter(20, 30);
        pelaaja1.Position = paikka;
        Add(pelaaja1);

        pelaajan1Ase = new Cannon(30, 10);

        pelaajan1Ase.Ammo.Value = 1000;

        pelaajan1Ase.ProjectileCollision = AmmusOsui;

        pelaaja1.Add(pelaajan1Ase);
    }

    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)

    {
        kohde.Destroy();
    }

    void AmmuAseella(Cannon ase)
    {
        PhysicsObject ammus = ase.Shoot();

        if (ammus != null)
        {
            ammus.Size *= 1;
            ammus.Image = Nuoli;
            //ammus.MaximumLifetime = TimeSpan.FromSeconds(2.0);
        }
    }
    

    void LuoPelaaja2(Vector paikka, double leveys, double korkeus)
    {
        pelaaja2 = new PlatformCharacter(20, 30);
        pelaaja2.Position = paikka;
        Add(pelaaja2);
    }

    void LuoTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Green;
        taso.CollisionIgnoreGroup = 1;
        Add(taso);
    }

    void LuoLinna1(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Linna1 = new PhysicsObject(100, 60);
        Linna1.IgnoresCollisionResponse = true;
        Linna1.Position = paikka;
        Linna1.Image = LinnanKuva1;
        Linna1.Tag = "Linna1";
        Linna1.IgnoresGravity = true;
        Add(Linna1, 1);


    }


    void LuoLinna2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Linna2 = new PhysicsObject(100, 60);
        Linna2.IgnoresCollisionResponse = true;
        Linna2.Position = paikka;
        Linna2.Image = LinnanKuva2;
        Linna2.Tag = "Linna2";
        Linna2.IgnoresGravity = true;
        Add(Linna2, 1);


    }

    void LiikutaPelaajaa2(Vector vektori)
    {
        pelaaja2.Push(vektori);
    }

    void LiikutaPelaajaa(Vector vektori)
    {
        pelaaja1.Push(vektori);
    }


}


