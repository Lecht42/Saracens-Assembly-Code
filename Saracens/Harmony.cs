// RimWorld.CompProperties_Reloadable
using HarmonyLib;
using RimWorld;
using Saracens.Comps;
using System;
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

    [HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.IsPawnBeingGeneratedAndNotAllowsDead))]
    class MakeCorpsePatch 
    {
        static bool Prefix(Pawn pawn, out bool __result)
        {
            var compCorpseDestroyerExisted = pawn.GetComp<CompCorpseDestroyer>() == null;

            __result = compCorpseDestroyerExisted;
            return compCorpseDestroyerExisted;
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
