using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;


class Linna : PhysicsObject

{
  

    public int elamat { get; set; }

    private IntMeter elamaLaskuri = new IntMeter(3, 0, 3);
    public IntMeter ElamaLaskuri { get { return elamaLaskuri; } }


    public Linna(double leveys, double korkeus)
        : base(leveys, korkeus)

       
    {
        elamaLaskuri.LowerLimit += delegate { this.Destroy(); };
        
    }

    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)

    {
        if (kohde.Tag == "Linna2")
        {
            ElamaLaskuri.Value -= 1;
        }
    }

}

public class InvadeTheCastle : PhysicsGame
{

    PlatformCharacter pelaaja1;
    PlatformCharacter pelaaja2;

    Image Linnankuva1 = LoadImage("LinnanKuva1");
    Image LinnanKuva2 = LoadImage("LinnanKuva2");

    DoubleMeter Laskuri1;
    DoubleMeter Laskuri2;

    IntMeter ElamaLaskuri1;
    IntMeter ElamaLaskuri2;

    Cannon pelaajan1Ase;
    Cannon pelaajan2Ase;

    Image Nuoli = LoadImage("Nuoli");
    Image Jouskari = LoadImage("jouskari");
    Image liekki = LoadImage("liekki");

    public override void Begin()
    {
        LuoKentta();
        LuoLaskuri1();
        LuoLaskuri2();
        LuoElamaLaskuri1();
        LuoElamaLaskuri2();
        Liekit();

        Camera.ZoomToAllObjects();
        

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
        Keyboard.Listen(Key.NumPad0, ButtonState.Down, AmmuAseella, "Ammu", pelaajan2Ase);

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
        ruudut.SetTileMethod(Color.Rose, LuoAnsa1);
        ruudut.SetTileMethod(Color.Orange, LuoAnsa2);
     
       
        
        ruudut.Execute(50, 20);
        
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


    void LuoElamaLaskuri1()

    {
        ElamaLaskuri1 = new IntMeter(0);

        Label ElamaNaytto1 = new Label();
        ElamaNaytto1.X = Screen.Left + 100;
        ElamaNaytto1.Y = Screen.Top - 100;
        ElamaNaytto1.TextColor = Color.Black;
        ElamaNaytto1.Color = Color.LightBlue;

        ElamaNaytto1.BindTo(ElamaLaskuri1);
        Add(ElamaNaytto1);
     
    }

    void LuoElamaLaskuri2()
    {
        ElamaLaskuri2 = new IntMeter(0);

        Label ElamaNaytto2 = new Label();
        ElamaNaytto2.X = Screen.Right - 100;
        ElamaNaytto2.Y = Screen.Top - 100;
        ElamaNaytto2.TextColor = Color.Black;
        ElamaNaytto2.Color = Color.LightBlue;

        ElamaNaytto2.BindTo(ElamaLaskuri2);
        Add(ElamaNaytto2);

    }

   

    void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
   
    {
        pelaaja1 = new PlatformCharacter(20, 30);
        pelaaja1.Position = paikka;
        pelaaja1.Tag = "pelaaja1";
        Add(pelaaja1);

        pelaajan1Ase = new Cannon(30, 30);

        pelaajan1Ase.Image = Jouskari;

        pelaajan1Ase.Ammo.Value = 1000;

        pelaajan1Ase.CanHitOwner = false;

        pelaajan1Ase.ProjectileCollision = AmmusOsui;
        
        pelaaja1.Add(pelaajan1Ase);
    }

    void LuoPelaaja2(Vector paikka, double leveys, double korkeus)
    {
        pelaaja2 = new PlatformCharacter(20, 30);
        pelaaja2.Position = paikka;
        pelaaja2.Tag = "pelaaja2";
        Add(pelaaja2);

        pelaajan2Ase = new Cannon(30, 30);

        pelaajan2Ase.Image = Jouskari;

        pelaajan2Ase.Ammo.Value = 1000;

        pelaajan2Ase.CanHitOwner = false;

        pelaajan2Ase.ProjectileCollision = AmmusOsui2;

        pelaaja2.Add(pelaajan2Ase);
    }

    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)

    {
        if (kohde.Tag == "pelaaja2")

        {
            Remove(kohde);
        }

    }


    void AmmusOsui2(PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag == "pelaaja1")
        {
            Remove(kohde);
        }
    }

    void AmmuAseella(Cannon ase)
    {
        PhysicsObject ammus = ase.Shoot();

        if (ammus != null)
        {
            ammus.Width = 25;
            ammus.Height = 5;
            ammus.Image = Nuoli;
            ammus.CollisionIgnoreGroup = 1;
            ammus.MaximumLifetime = TimeSpan.FromSeconds(2.0);
            ammus.CanRotate = false;
        }
    }
   

    void LuoTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Green;
        Add(taso);
    }

    void LuoAnsa1(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Ansa1 = new PhysicsObject(50, 20);
        Ansa1.Position = paikka;
        Ansa1.IgnoresGravity = true;
        Ansa1.CollisionIgnoreGroup = 1;
        Ansa1.Mass = 20000;
        Add(Ansa1);
    }

    void LuoAnsa2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Ansa2 = new PhysicsObject(50, 20);
        Ansa2.Position = paikka;
        Ansa2.IgnoresGravity = true;
        Add(Ansa2);
    }

    void LuoLinna1(Vector paikka, double leveys, double korkeus)
    {

        Linna Linna1 = new Linna(100, 60);
        Linna1.elamat = 3;
        Linna1.Image = Linnankuva1;
        Linna1.Position = paikka;
        Linna1.Tag = "Linna1";
        Linna1.Mass = 20000;
        Add(Linna1);
    }


    void LuoLinna2(Vector paikka, double leveys, double korkeus)
    {
        Linna Linna2 = new Linna(100, 60);
        Linna2.Position = paikka;
        Linna2.Image = LinnanKuva2;
        Linna2.Tag = "Linna2";
        Linna2.Mass = 20000;
        Add(Linna2);


    }

    void Liekit()

    {
        Flame Liekki = new Flame(liekki);
        Liekki.Position = new Vector(10,10);
        Add(Liekki);
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


