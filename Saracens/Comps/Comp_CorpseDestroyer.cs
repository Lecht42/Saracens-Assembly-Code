using Verse;

namespace Saracens.Comps
{
    public class CompProperties_CorpseDestroyer : CompProperties
    {
        public CompProperties_CorpseDestroyer()
        {
            compClass = typeof(CompCorpseDestroyer);
        }
    }

    public class CompCorpseDestroyer : ThingComp
    {
        public CompProperties_CorpseDestroyer Props => props as CompProperties_CorpseDestroyer;
    }
}
