using Saracens.Comps;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Saracens
{

    public class JobGiver_Merge : ThinkNode_JobGiver
    {
        protected float maxMergeDistance = 60f;

        public float MaxMergeDistance => maxMergeDistance;

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!(from p in pawn.Map.mapPawns.AllPawnsSpawned
                  where p.def == pawn.def
                  && p.GetComp<CompMergable>() != null
                  && p != pawn
                  && !pawn.ageTracker.Adult
                  && !p.ageTracker.Adult
                  && p.Position.InHorDistOf(pawn.Position, MaxMergeDistance)
                  && pawn.ageTracker.AgeBiologicalTicks <= p.ageTracker.AgeBiologicalTicks
                  select p).TryRandomElement(out var result))
            {
                return null;
            }
            Job job = JobMaker.MakeJob(JobDefOf.Merge, result);
            job.locomotionUrgency = LocomotionUrgency.Walk;
            job.expiryInterval = 3000;

            return job;
        }
    }

    public class JobDriver_Merge : JobDriver
    {
        protected Pawn Victim => (Pawn)pawn.CurJob.targetA.Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            var compMergable = pawn.GetComp<CompMergable>();

            Toil dropFilth = ToilMaker.MakeToil("DropFilth");
            dropFilth.defaultCompleteMode = ToilCompleteMode.Instant;
            dropFilth.initAction = delegate
            {
                compMergable.GenerateBlood();
            };
            yield return dropFilth;

            yield return Toils_Effects.MakeSound(pawn.def.soundInteract);

            Toil merge = ToilMaker.MakeToil("Merge");
            merge.defaultCompleteMode = ToilCompleteMode.Instant;
            merge.initAction = delegate
            {
                compMergable.Merge(Victim);
            };
            yield return merge;
        }
    }
}
