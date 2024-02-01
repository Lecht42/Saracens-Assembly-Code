using Verse;

namespace Saracens.Comps
{
    public class CompProperties_Mergable : CompProperties
    {
        public int copiesOnDeath = 2;

        public IntRange bloodDuringMerge = IntRange.one;

        public CompProperties_Mergable()
        {
            compClass = typeof(CompMergable);
        }
    }

    public class CompMergable : ThingComp
    {
        public CompProperties_Mergable Props => props as CompProperties_Mergable;

        public int CopiesOnDeath => Props.copiesOnDeath;

        public IntRange BloodNumberRange => Props.bloodDuringMerge;

        public void GenerateBlood()
        {
            for (int i = 0; i < Rand.Range(BloodNumberRange.min, BloodNumberRange.max) * (parent as Pawn).BodySize; i++)
            {
                (parent as Pawn).health.DropBloodFilth();
            }
        }

        public void Divide(Pawn pawn)
        {
            if (CopiesOnDeath > 0 && pawn.ageTracker.CurLifeStageIndex > 0)
            {
                var request = new PawnGenerationRequest(pawn.kindDef, fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat / CopiesOnDeath);

                for (int i = 0; i < CopiesOnDeath; i++)
                {
                    Pawn genPawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(genPawn, pawn.Position, pawn.Map);
                }

                GenerateBlood();
            }
        }
    }
}
