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
            if (__instance.Downed) return;

            var compMergable = __instance.GetComp<CompMergable>();
            compMergable?.Divide();
        }
    }

    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
    class DownedPatch
    {
        static void Prefix(Pawn ___pawn, out CompMergable __state)
        {
            __state = ___pawn.GetComp<CompMergable>();

            __state?.Divide();
        }

        static void Postfix(Pawn ___pawn, CompMergable __state)
        {
            if (__state == null) return;

            __state?.parent.Kill();
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
