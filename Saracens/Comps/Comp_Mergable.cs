using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Saracens.Comps
{
    public class CompProperties_Mergable : CompProperties
    {
        public int copiesOnDeath = 2;

        public IntRange bloodDuringMerge = IntRange.one;

        public bool stunOnMerge = true;

        public int stunDuration = 80;

        public CompProperties_Mergable()
        {
            compClass = typeof(CompMergable);
        }
    }

    public class CompMergable : ThingComp
    {
        public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Animal/BigIfrit_south");

        public CompProperties_Mergable Props => props as CompProperties_Mergable;

        public Pawn Parent => parent as Pawn;

        public int CopiesOnDeath => Props.copiesOnDeath;

        public bool StunOnMerge => Props.stunOnMerge;

        public int BloodNumber => (int)(Rand.Range(Props.bloodDuringMerge.min, Props.bloodDuringMerge.max) * Parent.BodySize);

        public int StunDuration => (int)(Props.stunDuration * Parent.BodySize);

        public int StugDuration => (int)Math.Round(Props.stunDuration * 1.4f * Parent.BodySize);

        private void DropBlood()
        {
            for (int i = 0; i < BloodNumber; i++)
            {
                Parent.health.DropBloodFilth();
            }
        }

        public void Divide()
        {
            if (CopiesOnDeath > 0 && Parent.ageTracker.CurLifeStageIndex > 0)
            {
                var request = new PawnGenerationRequest(Parent.kindDef,
                    fixedBiologicalAge: Parent.ageTracker.AgeBiologicalYearsFloat / CopiesOnDeath,
                    fixedChronologicalAge: Parent.ageTracker.AgeChronologicalYears,
                    faction: Parent.Faction);

                void PostSpawn(Pawn pawn)
                {
                    Lord lord = Parent.GetLord();
                    if (lord == null)
                    {
                        LordJob_DefendPoint lordJob = new LordJob_DefendPoint(pawn.Position);
                        lord = LordMaker.MakeNewLord(pawn.Faction, lordJob, Find.CurrentMap);
                    }
                    lord.AddPawn(pawn);
                    pawn.Rotation = Parent.Rotation;
                    if (StunOnMerge)
                    {
                        pawn.stances.stagger.StaggerFor(StugDuration);
                    }
                    pawn.inventory.DestroyAll();
                }

                for (int i = 0; i < CopiesOnDeath; i++)
                {
                    var genPawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(genPawn, Parent.Position.RandomAdjacentCellCardinal(), Parent.Map);
                    PostSpawn(genPawn); 
                }
                DropBlood();
            }
        }

        public void Merge(Pawn victim)
        {
            Parent.ageTracker.DebugSetAge(Parent.ageTracker.AgeBiologicalTicks + Parent.ageTracker.AgeBiologicalTicks);
            MoteMaker.MakeSpeechBubble(Parent, moteIcon);
            DropBlood();
            Heal();
            if (StunOnMerge)
            {
                Parent.stances.stunner.StunFor(StunDuration, Parent, false, true);
            }
            victim.inventory.innerContainer.TryTransferAllToContainer(Parent.inventory.innerContainer);
            victim.Destroy(DestroyMode.Vanish);
        }

        private void Heal()
        {
            List<Hediff_Injury> resultHediffs = new List<Hediff_Injury>();
            Parent.health.hediffSet.GetHediffs(ref resultHediffs, (Hediff_Injury x) => x.CanHealNaturally() || x.CanHealFromTending());
            foreach (var hediff in resultHediffs)
            {
                hediff.Heal(hediff.Severity);
            }
        }
    }
}
