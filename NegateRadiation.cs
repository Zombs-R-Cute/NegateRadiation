using HarmonyLib;
using Rocket.Core.Plugins;

namespace Zombs_R_Cute_NegateRadiation
{
    public class NegateRadiation : RocketPlugin
    {
        protected override void Load()
        {
            Harmony harmony = new Harmony("NegateRadiation");
            Harmony.DEBUG = true;
            harmony.PatchAll();
        }
    }
}