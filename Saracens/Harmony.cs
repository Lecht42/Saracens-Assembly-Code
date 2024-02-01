// RimWorld.CompProperties_Reloadable
using HarmonyLib;
using Saracens.Comps;
using Verse;

namespace Saracens
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill))]
    class KillPatch
    {
        static void Prefix(Pawn __instance)
        {
            var compMergable = __instance.GetComp<CompMergable>();

            compMergable?.Divide(__instance);
        }
    }

    [StaticConstructorOnStartup]
    public static class ModInitializer
    {
        static ModInitializer()
        {
            var harmony = new Harmony("Saracens");
            harmony.PatchAll();
        }
    }
}
