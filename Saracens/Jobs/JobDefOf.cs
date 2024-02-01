using RimWorld;
using Verse;

namespace Saracens
{
    [DefOf]
    public static class JobDefOf
    {
        public static JobDef Merge;

        static JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
        }
    }
}
