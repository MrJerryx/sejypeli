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

    Image Linnankuva1 = LoadImage("LinnanKuva1");
    Image LinnanKuva2 = LoadImage("LinnanKuva2");

    DoubleMeter Laskuri1;
    DoubleMeter Laskuri2;

    IntMeter ElamaLaskuri1;
    IntMeter ElamaLaskuri2;

    Image Nuoli = LoadImage("Nuoli");
    Image Jouskari = LoadImage("jouskari");
    Image liekki = LoadImage("liekki");

    SoundEffect aani1 = LoadSoundEffect("aani1");

 

    public override void Begin()
    {
        valikko();
       
        Keyboard.Listen(Key.A, ButtonState.Down,
         LiikuVasempaan, null);
        Keyboard.Listen(Key.D, ButtonState.Down,
          LiikuOikeaan, null);
        Keyboard.Listen(Key.W, ButtonState.Down,
          Hyppää1, null);

        Keyboard.Listen(Key.Left, ButtonState.Down,
          LiikuVasempaan2, null);
        Keyboard.Listen(Key.Right, ButtonState.Down,
          LiikuOikeaan2, null);
        Keyboard.Listen(Key.Up, ButtonState.Down,
          Hyppää2, null);



        Keyboard.Listen(Key.Space, ButtonState.Down, AmmuAseella, "Ammu", pelaaja1);
        Keyboard.Listen(Key.NumPad0, ButtonState.Down, AmmuAseella, "Ammu", pelaaja2);

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void valikko()
    {
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Pelin alkuvalikko",
        "Aloita peli", "Lopeta");
        Add(alkuValikko);

        alkuValikko.AddItemHandler(0, LuoKentta);
        alkuValikko.AddItemHandler(1, Exit);
    }

    void LuoKentta()
    {
        
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta");


        ruudut.SetTileMethod(Color.Red, LuoPelaaja1);
        ruudut.SetTileMethod(Color.BrightGreen, LuoPelaaja2);
        ruudut.SetTileMethod(Color.Black, LuoTaso);
        ruudut.SetTileMethod(Color.Gray, LuoLinna1);
        ruudut.SetTileMethod(Color.DarkGray, LuoLinna2);
        ruudut.SetTileMethod(Color.Rose, LuoAnsa1);
        ruudut.SetTileMethod(Color.Orange, LuoAnsa2);
     
       
        
        ruudut.Execute(50, 20);
        
        Gravity = new Vector(0.0, -800.0);


        LuoLaskuri1();
        LuoLaskuri2();
        LuoElamaLaskuri1();
        LuoElamaLaskuri2();
        LuoAjastin();

        Camera.ZoomToLevel();

    }

  
  
    void LuoLaskuri1()
    {
        Laskuri1 = new DoubleMeter(3000);
        Laskuri1.MaxValue = 3000;
        Laskuri1.MinValue = 0;
        Laskuri1.LowerLimit += Pelaaja1Haviaa;


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
        Laskuri2 = new DoubleMeter(3000);
        Laskuri2.MaxValue = 3000;
        Laskuri2.MinValue = 0;
        Laskuri2.LowerLimit += Pelaaja2Haviaa;
      

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
        ElamaLaskuri1 = new IntMeter(10);
        ElamaLaskuri1.MinValue = 0;
        ElamaLaskuri1.LowerLimit += UusiPelaaja1;

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
        ElamaLaskuri2 = new IntMeter(10);
        ElamaLaskuri2.MinValue = 0;
        ElamaLaskuri2.LowerLimit += UusiPelaaja2;

        Label ElamaNaytto2 = new Label();
        ElamaNaytto2.X = Screen.Right - 100;
        ElamaNaytto2.Y = Screen.Top - 100;
        ElamaNaytto2.TextColor = Color.Black;
        ElamaNaytto2.Color = Color.LightBlue;

        ElamaNaytto2.BindTo(ElamaLaskuri2);
        Add(ElamaNaytto2);

    }


    void LuoAjastin()
    {
        Timer ajastin1 = new Timer();
        ajastin1.Interval = 10.0;
        ajastin1.Timeout += Liekit;
        ajastin1.Timeout += Liekit2;
        ajastin1.Start();
    }

    void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
   
    {
        pelaaja1 = new PlatformCharacter(20, 30);
        pelaaja1.Position = paikka;
        pelaaja1.Tag = "pelaaja1";
        pelaaja1.TurnsWhenWalking = true;
        AddCollisionHandler(pelaaja1, tormays);
        Add(pelaaja1);


        pelaaja1.Weapon = new Cannon(15, 45);

        pelaaja1.Weapon.X = 10.0;

        pelaaja1.Weapon.Image = Jouskari;

        pelaaja1.Weapon.AttackSound = aani1;

        pelaaja1.Weapon.Power.DefaultValue = 10000;

        pelaaja1.Weapon.Ammo.Value = 1000;

        pelaaja1.Weapon.CanHitOwner = false;

        pelaaja1.Weapon.ProjectileCollision = AmmusOsui;

        pelaaja1.Add(pelaaja1.Weapon);
    }

    void LuoPelaaja2(Vector paikka, double leveys, double korkeus)
    {
        pelaaja2 = new PlatformCharacter(20, 30);
        pelaaja2.Position = paikka;
        pelaaja2.Tag = "pelaaja2";
        pelaaja2.TurnsWhenWalking = true;
        AddCollisionHandler(pelaaja2, tormays);
        Add(pelaaja2);

        pelaaja2.Weapon = new Cannon(15, 45);

        pelaaja2.Weapon.X = -10.0;

        pelaaja2.Weapon.Image = Jouskari;

        pelaaja2.Weapon.AttackSound = aani1;

        pelaaja2.Weapon.Ammo.Value = 1000;

        pelaaja2.Weapon.CanHitOwner = false;

        pelaaja2.Weapon.ProjectileCollision = AmmusOsui2;

        pelaaja2.Add(pelaaja2.Weapon);
    }

    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)

    {
        if (kohde.Tag == "pelaaja2")

        {
            pelaaja2kuoli();
        }

        if (kohde.Tag == "Linna2")
        {
            Laskuri2.Value -= 250;
        }

    }

    void AmmusOsui2(PhysicsObject ammus, PhysicsObject kohde)
    {

        if (kohde.Tag == "pelaaja1")
        {
            pelaaja1kuoli();
        }

        if (kohde.Tag == "Linna1")
        {
            Laskuri1.Value -= 250;
        }

    }
    void AmmuAseella(PlatformCharacter pelaaja)
    {
        PhysicsObject ammus = pelaaja1.Weapon.Shoot();

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

        PhysicsObject Linna1 = new PhysicsObject(200, 120);
        Linna1.Image = Linnankuva1;
        Linna1.Position = paikka;
        Linna1.Tag = "Linna1";
        Linna1.Mass = 20000;
        Add(Linna1);
    }


    void LuoLinna2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Linna2 = new PhysicsObject(100, 60);
        Linna2.Position = paikka;
        Linna2.Image = LinnanKuva2;
        Linna2.Tag = "Linna2";
        Linna2.Mass = 20000;
        Add(Linna2);


    }

    void Liekit()
    {
        int y = -290;
        for (int x=-725; x >= -825; x -= 50)
        {
            LuoLiekki(x, y);
        }
    }

    void LuoLiekki(int x, int y)
    {
        Flame Liekki = new Flame(liekki);
        Liekki.LifetimeLeft = TimeSpan.FromSeconds(6.0);
        Liekki.X = x;
        Liekki.Y = y;
        Add(Liekki);

        PhysicsObject liekki1 = new PhysicsObject(20, 200);
        liekki1.LifetimeLeft = TimeSpan.FromSeconds(6.0);
        liekki1.X = x;
        liekki1.Y = y;
        liekki1.CollisionIgnoreGroup = 1;
        liekki1.IsVisible = false;
        liekki1.Tag = "liekki";
        Add(liekki1);
    }

    void Liekit2()
    {
        int y = -290;
        for (int x = 725; x <= 825; x += 50)
        {
            LuoLiekki2(x, y);
        }
    }

    void LuoLiekki2(int x, int y)
    {
        Flame Liekki2 = new Flame(liekki);
        Liekki2.LifetimeLeft = TimeSpan.FromSeconds(6.0);
        Liekki2.X = x;
        Liekki2.Y = y;
        Add(Liekki2);

        PhysicsObject liekki2 = new PhysicsObject(30, 200);
        liekki2.LifetimeLeft = TimeSpan.FromSeconds(6.0);
        liekki2.X = x;
        liekki2.Y = y;
        liekki2.CollisionIgnoreGroup = 1;
        liekki2.IsVisible = false;
        liekki2.Tag = "liekki";
        Add(liekki2);

    }

    void tormays(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        if (kohde.Tag == "liekki")
        {
            if (tormaaja.Tag == "pelaaja1")
            {
                pelaaja1kuoli();
            }

            if (tormaaja.Tag == "pelaaja2")
            {
                pelaaja2kuoli();
            }
        }
    }

    void LiikuOikeaan()
    {
        pelaaja1.Walk(300);
        pelaaja1.FacingDirection = Direction.Right;
    }

    void LiikuVasempaan()
    {
        pelaaja1.Walk(-300);
        pelaaja1.FacingDirection = Direction.Left;
    }

    void LiikuOikeaan2()
    {
        pelaaja2.Walk(300);
        pelaaja2.FacingDirection = Direction.Right;
    }

    void LiikuVasempaan2()
    {
        pelaaja2.Walk(-300);
        pelaaja2.FacingDirection = Direction.Left;
    }

    void Hyppää1()
    {
        pelaaja1.Jump(500);
    }

    void Hyppää2()
    {
        pelaaja2.Jump(500);
    }

    void Pelaaja1Haviaa()
    {
        MessageDisplay.Add("Pelaaja 1 hävisi pelin.");
    }

    void Pelaaja2Haviaa()
    {
        MessageDisplay.Add("Pelaaja 2 hävisi pelin.");
    }

    void UusiPelaaja1()
    {
        Vector paikka = new Vector(0, 0);
        pelaaja1.Position = paikka;
        ElamaLaskuri1.Reset();
    }

    void UusiPelaaja2()
    {
        Vector paikka = new Vector(0, 0);
        pelaaja2.Position = paikka;
        ElamaLaskuri2.Reset();
    }

    void pelaaja1kuoli()
    {
        pelaaja1.Y = -10000;
        ElamaLaskuri1.AddOverTime(-10, 10);
    }

    void pelaaja2kuoli()
    {
        pelaaja2.Y = -10000;
        ElamaLaskuri2.AddOverTime(-10, 10);
    }

}


