using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using System;
using System.Collections.Generic;
using UnityStandardAssets;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.Extra;
using System.Security.Cryptography.X509Certificates;
using Exiled.Events.EventArgs.Server;
using UnityEngine;
using Exiled.Events.Commands.PluginManager;
using PlayerRoles.PlayableScps;
using PlayerRoles;
using PluginAPI.Roles;

namespace Beginer_Test_01
{
    public class MainSluginStarter : Plugin<User_Config>
    {
        //??? start
        private static readonly Lazy<MainSluginStarter> lazyinstance = new Lazy<MainSluginStarter>(valueFactory: () => new MainSluginStarter());
        public MainSluginStarter Instance => lazyinstance.Value;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public CoroutineHandle CallTimer;

        public static System.Random something = new System.Random();

        private MainSluginStarter()
        {

        }
        //??? end


        //Determines when to enable or disable the plugin
        public override void OnEnabled()
        {            
            Exiled.Events.Handlers.Server.RoundStarted += T_F_EnableCoroutineDeterminer;
            Exiled.Events.Handlers.Server.RoundEnded += T_F_DisableCoroutineDeterminer;
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= T_F_EnableCoroutineDeterminer;
            Exiled.Events.Handlers.Server.RoundEnded -= T_F_DisableCoroutineDeterminer;
        }

        //Values that have to be set outside of calls.
        //The following value is for declaring a value to be used as the surface reinforcements cooldown.
        int Cooldown_For_Surface_Reinforcements;







        //Enables the "Call every X seconds" stuff
        public void T_F_EnableCoroutineDeterminer()
        {

            //Surface Reinforcements Coroutines
            Cooldown_For_Surface_Reinforcements = 0;
            Timing.RunCoroutine(Calls_For_Surface_Reinforcements());
            //Debug If statement
            if (User_Config.debug_toggle == true)
            {
                Timing.RunCoroutine(Calls_For_Surface_Reinforcements_debug());
            }
            
        }

        //Disables the "Call every X seconds" stuff
        public void T_F_DisableCoroutineDeterminer (RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines();
        }


        
        //Surface reinforcements caller
        public IEnumerator<float> Calls_For_Surface_Reinforcements()
        {
            //Surface_Reinforcements_Calculation Calls
            Surface_Reinforcements_Calculation.Calculation_TheNumberOfCampingNTF();
            Surface_Reinforcements_Calculation.Calculation_TheNumberOfCampingCHI();
            Surface_Reinforcements_Calculation.NTFPercentCalculation();
            Surface_Reinforcements_Calculation.CHIPercentCalculation();


            //Values/Bools and convenience re-definitions
            bool T_F_NTFDeterminer = false;
            bool T_F_CHIDeterminer = false;
            float RefreshrateForSubtractingCooldown = User_Config.SurfaceReinforcementsSecondsPerCheck;
            double RandomnessReinforcements;
            float TheNumberOfCHIToSpawnForReinforcement = 0;
            float TheNumberOfNTFToSpawnForReinforcement = 0;
            float NumberOfCampingNTF = Surface_Reinforcements_Calculation.TheNumberOfCampingNTF;
            float NumberOfCampingCHI = Surface_Reinforcements_Calculation.TheNumberOfCampingCHI;
            float NTFPercentChance = Surface_Reinforcements_Calculation.NTFPercentChance;
            float CHIPercentChance = Surface_Reinforcements_Calculation.CHIPercentChance;
            RandomnessReinforcements = something.NextDouble();

            //Uses % between 0-1 to get a T/F statement for NTF Reinforcement
            if (RandomnessReinforcements <= NTFPercentChance)
            {
                T_F_NTFDeterminer = true;
            }

            //Uses % between 0-1 to get a T/F statement for CHI Reinforcement
            if (RandomnessReinforcements <= CHIPercentChance)
            {
                T_F_CHIDeterminer = true;
            }

            //Because it is possible for both the conditions for reinforcement waves of CHI/NTF to be true this has to be added to prevent both teams from spawning at once
            //(there is a failsafe down the line if this does fail.)
            if ((T_F_CHIDeterminer == true) && (T_F_NTFDeterminer == true))
            {
                //Implement some way for it to repeat to line 95 when the 2 values are true
            }

            //Executes the reinforcement wave (unimplemented)
            if ((T_F_NTFDeterminer == true) && (T_F_CHIDeterminer == false))
            {
                //Calculates the size of the spawnwave
                TheNumberOfCHIToSpawnForReinforcement = NumberOfCampingNTF + ((float)0.25 * NumberOfCampingNTF);
                //

                //Sets the minimum time between consecutive reinforcement waves (called Cooldown)
                Cooldown_For_Surface_Reinforcements = User_Config.Surface_Reinforcement_Cooldown;
            }
            if ((T_F_CHIDeterminer == true) && (T_F_NTFDeterminer == false) && (Cooldown_For_Surface_Reinforcements <= 0))
            {
                //Values
                

                //Calculates the size of the spawnwave
                TheNumberOfNTFToSpawnForReinforcement = NumberOfCampingCHI + ((float)0.25 * NumberOfCampingCHI);
                //Sets the minimum time between consecutive reinforcement waves (called Cooldown)
                Exiled.API.Features.Roles.Role.Set(RoleTypeId.Tutorial, PlayerRoles.RoleChangeReason : "stinky"); 




                Cooldown_For_Surface_Reinforcements = User_Config.Surface_Reinforcement_Cooldown; ;
            }
            
            //Makes the cooldown work by subtracting the number of seconds between each call from the cooldown number
            if (Cooldown_For_Surface_Reinforcements > 0)
            {
                while (RefreshrateForSubtractingCooldown > 0)
                {
                    Cooldown_For_Surface_Reinforcements--;
                    RefreshrateForSubtractingCooldown--;
                }
            }

            //Failsafe to make sure that should Cooldown_For_Surface_Reinforcements get below 0 somehow it fixes itself.
            if (Cooldown_For_Surface_Reinforcements < 0)
            {
                Cooldown_For_Surface_Reinforcements = 0;
            }


            //Repeats calling the Surface Reinforcements every X seconds
            yield return Timing.WaitForSeconds(User_Config.SurfaceReinforcementsSecondsPerCheck);
        }

        //This is the Debug area, it (is planned to) broadcasts to ALL PLAYERS information about the Surface Reinforcement plugin every X seconds (configurable)
        public IEnumerator<float> Calls_For_Surface_Reinforcements_debug()
        {
            //Calls
            Surface_Reinforcements_Calculation.Calculation_TheNumberOfCampingNTF();
            Surface_Reinforcements_Calculation.Calculation_TheNumberOfCampingCHI();
            Surface_Reinforcements_Calculation.NTFPercentCalculation();
            Surface_Reinforcements_Calculation.CHIPercentCalculation();

            //Values / bools / strings /convience redefinitions
            float NumberOfCampingNTF = Surface_Reinforcements_Calculation.TheNumberOfCampingNTF;
            float NumberOfCampingCHI = Surface_Reinforcements_Calculation.TheNumberOfCampingCHI;
            float NTFPercentChance = Surface_Reinforcements_Calculation.NTFPercentChance;
            float CHIPercentChance = Surface_Reinforcements_Calculation.CHIPercentChance;
            string NTFBroadcastCampingNumber = $"Camping NTF: {NumberOfCampingNTF}";
            string CHIBroadcastCampingNumber = $"Camping CHI: {NumberOfCampingCHI}";
            string NTFBroadcastPercentChance = $"NTF % Chance: %{NTFPercentChance * 100}";
            string CHIBroadcastPercentChance = $"CHI % Chance: %{CHIPercentChance * 100}";


            //The Broadcast
            Map.ClearBroadcasts();
            Map.Broadcast(new Exiled.API.Features.Broadcast($"{NTFBroadcastCampingNumber}/n{CHIBroadcastCampingNumber}/n{NTFBroadcastPercentChance}/n{CHIBroadcastPercentChance}", 10, true, Broadcast.BroadcastFlags.Normal));


            //Calls the above values and does the above broadcasts every x seconds
            yield return Timing.WaitForSeconds(User_Config.debug_refreshrate);
        }
    }
    
}