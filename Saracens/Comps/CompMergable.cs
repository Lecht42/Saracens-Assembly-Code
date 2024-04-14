using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Saracens.Comps
{
    [StaticConstructorOnStartup]
    public class CompMergable : ThingComp
    {

        public CompProperties_Mergable Props => props as CompProperties_Mergable;

        public Pawn Parent => parent as Pawn;

        public Texture2D MergeIcon => Props.mergeIcon; 

        public int CopiesOnDeath => Props.copiesOnDeath;

        public bool StunOnMerge => Props.stunOnMerge;

        public int BloodNumber => (int)(Rand.Range(Props.bloodDuringMerge.min, Props.bloodDuringMerge.max) * Parent.BodySize);

        public int StunDuration => (int)(Props.stunDuration * Parent.BodySize);

        private void DropBlood()
        {
            for (int i = 0; i < BloodNumber; i++)
            {
                Parent.health.DropBloodFilth();
            }
        }

        public void Divide()
        {
            if (CopiesOnDeath > 0 && Parent.ageTracker.CurLifeStageIndex > 0 && !Parent.Downed)
            {
                var request = new PawnGenerationRequest(Parent.kindDef,
                    fixedBiologicalAge: Parent.ageTracker.AgeBiologicalYearsFloat / CopiesOnDeath,
                    fixedChronologicalAge: Parent.ageTracker.AgeChronologicalYears,
                    faction: Parent.Faction);

                for (int i = 0; i < CopiesOnDeath; i++)
                {
                    var genPawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(genPawn, Parent.Position.RandomAdjacentCellCardinal(), Parent.Map);
                    PostSpawn(genPawn);
                }
                DropBlood();
            }
        }

        private void PostSpawn(Pawn pawn)
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
                pawn.stances.stunner.StunFor(StunDuration, Parent, false, false);
            }
            pawn.inventory.DestroyAll();
        }

        public Toil Toil_GoToThingToMerge(TargetIndex ind, PathEndMode peMode)
        {
            Toil toil = ToilMaker.MakeToil("GotoThingToMerge");
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
            };
            toil.tickAction = delegate
            {
                MoteMaker.MakeSpeechBubble(Parent, MergeIcon);
            };
            toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            toil.FailOnDespawnedOrNull(ind);

            return toil;
        }

        public Toil Toil_Merge(Pawn victim)
        {
            Toil toil = ToilMaker.MakeToil("Merge");
            toil.initAction = delegate
            {
                Parent.ageTracker.DebugSetAge(Parent.ageTracker.AgeBiologicalTicks + Parent.ageTracker.AgeBiologicalTicks);
                DropBlood();
                if (StunOnMerge)
                {
                    Parent.stances.stunner.StunFor(StunDuration, Parent, false, false);
                }
                victim.inventory.innerContainer.TryTransferAllToContainer(Parent.inventory.innerContainer);
                victim.Destroy(DestroyMode.Vanish);
                Parent.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            toil.PlaySustainerOrSound(Parent.def.soundInteract);


            return toil;
        }
    }
}
