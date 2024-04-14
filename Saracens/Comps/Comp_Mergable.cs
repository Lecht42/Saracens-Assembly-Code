using UnityEngine;
using Verse;

namespace Saracens.Comps
{
    public class CompProperties_Mergable : CompProperties
    {
        public int copiesOnDeath = 2;

        public IntRange bloodDuringMerge = IntRange.one;

        public bool stunOnMerge = true;

        public int stunDuration = 80;

        public Texture2D mergeIcon = ContentFinder<Texture2D>.Get("Animal/BigIfrit_south");

        public CompProperties_Mergable()
        {
            compClass = typeof(CompMergable);
        }
    }
}
