using Saracens.Comps;
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
                  && pawn.ageTracker.AgeBiologicalYears <= p.ageTracker.AgeBiologicalYears
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
}
