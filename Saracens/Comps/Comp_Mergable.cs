using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

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
                var request = new PawnGenerationRequest(pawn.kindDef,
                    fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat / CopiesOnDeath,
                    fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYears,
                    faction: pawn.Faction);

                for (int i = 0; i < CopiesOnDeath; i++)
                {
                    Pawn genPawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(genPawn, pawn.Position, pawn.Map);
                    PostPawnSpawn(genPawn);
                }
                GenerateBlood();
            }
        }

        public void Merge(Pawn victim) 
        {
            var pawn = parent as Pawn;
            pawn.ageTracker.DebugSetAge(pawn.ageTracker.AgeBiologicalTicks + pawn.ageTracker.AgeBiologicalTicks);
            FullHeal();
            victim.inventory.innerContainer.TryTransferAllToContainer(pawn.inventory.innerContainer);
            victim.Destroy(DestroyMode.Vanish);
        }

        private void PostPawnSpawn(Pawn pawn)
        {
            if (pawn.Spawned)
            {
                Lord lord = ((Pawn)parent).GetLord();
                if (lord == null)
                {
                    LordJob_DefendPoint lordJob = new LordJob_DefendPoint(pawn.Position);
                    lord = LordMaker.MakeNewLord(pawn.Faction, lordJob, Find.CurrentMap);
                }
                lord.AddPawn(pawn);
                pawn.Rotation = ((Pawn)parent).Rotation;
                pawn.inventory.DestroyAll();
            }
        }

        private void FullHeal()
        {
            List<Hediff_Injury> resultHediffs = new List<Hediff_Injury>();
            (parent as Pawn).health.hediffSet.GetHediffs(ref resultHediffs, (Hediff_Injury x) => x.CanHealNaturally() || x.CanHealFromTending());
            foreach (var hediff in resultHediffs)
            {
                hediff.Heal(hediff.Severity); 
            }
        }
    }
}
