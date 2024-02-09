using RimWorld;

namespace Saracens.Incidents
{
    [DefOf]
    public static class RaidStrategyDefOf
    {
        public static RaidStrategyDef SFE_ArriveAndDefend;

        static RaidStrategyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.RaidStrategyDefOf));
        }
    }
}
