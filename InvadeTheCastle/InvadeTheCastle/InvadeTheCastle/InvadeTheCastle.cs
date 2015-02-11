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

    Image LinnanKuva1 = LoadImage("LinnanKuva1");
    Image LinnanKuva2 = LoadImage("LinnanKuva2");

    public override void Begin()
    {
        LuoPelaaja1();
        LuoPelaaja2();
        LuoLinna1();
        LuoLinna2();
        LuoTaso();

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }




    void LuoKentta()
    {
        
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta");

       
        ruudut.SetTileMethod(Color.Blue, LuoPelaaja1);
        ruudut.SetTileMethod(Color.BrightGreen, LuoPelaaja2);
        ruudut.SetTileMethod(Color.Black, LuoTaso);
        ruudut.SetTileMethod(Color.Gray, LuoLinna1);
        ruudut.SetTileMethod(Color.DarkGray, LuoLinna2);

        
        ruudut.Execute(20, 20);
    }

    void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(20, 30);
        pelaaja1.Position = paikka;
        Add(pelaaja1);
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
        PhysicsObject Linna1 = new PhysicsObject(30, 20);
        Linna1.IgnoresCollisionResponse = true;
        Linna1.Position = paikka;
        Linna1.Image = LinnanKuva1;
        Linna1.Tag = "Linna1";
        Add(Linna1, 1);


    }


    void LuoLinna2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Linna2 = new PhysicsObject(30, 20);
        Linna2.IgnoresCollisionResponse = true;
        Linna2.Position = paikka;
        Linna2.Image = LinnanKuva2;
        Linna2.Tag = "Linna2";
        Add(Linna2, 1);


    }

}


