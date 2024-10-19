using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beginer_Test_01
{
    public sealed class User_Config : IConfig
    {
        //??? start
        public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Debug { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //??? end


        //The following 4 things are configs for the Surface Rienforcement thing
        //The higher this value is, the more NTF/Chaos there will have to be for the % chance for a rienforcement to happen to increase (defualt = 1), If set to 0 there will always be a 100 % chance for a spawnwave of the opposite team to happen.
        public static float Slope { get; set; } = 1;
        //A range from -1 to 1, determines what the calculation will output when there is 0 NTF and 0 Chaos (Defaut=0) EX. XYInterceptOnPercentAxis = -1 TheNumberOfCampingCHI = 0 TheNumberOfCampingNTF = 0 then NTFPercentChance & CHIPercentChance = -1
        public static float XYInterceptOnPercentAxis { get; set; } = 0;
        //----> LOOK AT THE CODE AND MATH BEFORE CHANGING THIS VALUE <----  The # of seconds to wait between checking for if there is a spawnwave. Because of the way % chances work when checking them over and over again when this number is lower (more frequent) there is a higher chance for a spawn to happen. Ex. Flipping a coin (50%/50%) has a 50% chance of landing heads (true) on the first flip, then theres a 75% chance that one of the flips are heads on the second flip, ect ect, meaning as this number aprocahes 0 the more counterwaves spawn.
        public static float SurfaceReinforcementsSecondsPerCheck { get; set; } = 60f;
        //The mininum ammount of time between reinforcement waves
        public static int Surface_Reinforcement_Cooldown = 120;
        //Gives Debug shit (elaborate later)
        public static bool debug_toggle { get; set; } = true;
        //Determines how frequently in seconds to fetch and give debug shit.
        public static float debug_refreshrate { get; set; } = 5f;








    }
}
