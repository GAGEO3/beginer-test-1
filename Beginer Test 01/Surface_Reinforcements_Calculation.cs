using Exiled.API.Enums;
using Exiled.Events;
using Exiled.Events.EventArgs.Item;
using Exiled.API.Features;
using Exiled.CustomModules;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using Exiled.Events.EventArgs.Server;
using MEC;
using Beginer_Test_01;

public class Surface_Reinforcements_Calculation
{
    //GameValues
    public static float TheNumberOfCampingNTF = 0;
    public static float TheNumberOfCampingCHI = 0;
    public static float ConnectedPlayerCount = Server.PlayerCount;
    public static float AliveSCPsCount = Player.List.Where(player => player.IsScp).Count();

    //Configs
    public static float Slope = User_Config.Slope;
    public static float XYInterceptOnPercentAxis = User_Config.XYInterceptOnPercentAxis;

    //Outputs
    public static float NTFPercentChance;
    public static float CHIPercentChance;

    //Calculating GameValues
    public static void Calculation_TheNumberOfCampingNTF()
    {
        float ok = 0;
        foreach (Player player in Player.List)
        {
            if (WithinCampingBounds(player.Position) && (player.IsNTF) && (player.AuthenticationType == AuthenticationType.Unknown))
            {
                ok++;
            }
        }
        TheNumberOfCampingNTF = ok;
    }
    public static void Calculation_TheNumberOfCampingCHI()
    {
        float ok = 0;
        foreach (Player player in Player.List)
        {
            if (WithinCampingBounds(player.Position) && (player.IsCHI) && (player.AuthenticationType == AuthenticationType.Unknown))
            {
                ok++;
            }
        }
        TheNumberOfCampingCHI = ok;
    }

    //Ask about this
    public static bool WithinCampingBounds(Vector3 vector)
    {
        if (!WithinRange(vector.x, 40f, -10f))
            return false;

        if (!WithinRange(vector.y, 997f, 1000f))
            return false;

        if (!WithinRange(vector.z, -53, 7f))
            return false;

        return true;
    }
    //Ask about this
    private static bool WithinRange(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    //The Calculation :distressed:
    public static void NTFPercentCalculation ()
    {
        float a = ConnectedPlayerCount - AliveSCPsCount;
        float b = a + TheNumberOfCampingCHI;
        if (b == 0)
        {
            return;
        }
        float temp;
        temp = TheNumberOfCampingNTF / b;
        temp = (float)Math.Pow(temp, Slope);
        NTFPercentChance = temp + XYInterceptOnPercentAxis;
        if (NTFPercentChance < 0)
        {
            NTFPercentChance = 0;
        }
    }
    public static void CHIPercentCalculation()
    {
        float a = ConnectedPlayerCount - AliveSCPsCount;
        float b = a + TheNumberOfCampingNTF;
        if (b == 0)
        {
            return;
        }
        float temp;
        temp = TheNumberOfCampingCHI / b;
        temp = (float)Math.Pow(temp, Slope);
        CHIPercentChance = temp + XYInterceptOnPercentAxis;
        if (CHIPercentChance < 0)
        {
            CHIPercentChance = 0;
        }
    }

}