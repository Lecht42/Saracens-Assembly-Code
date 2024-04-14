using Saracens.Comps;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Saracens
{

    public class JobDriver_Merge : JobDriver
    {
        protected Pawn Victim => (Pawn)pawn.CurJob.targetA.Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Victim, job, 1, -1, null, false);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            var compMergable = pawn.GetComp<CompMergable>();

            yield return compMergable.Toil_GoToThingToMerge(TargetIndex.A, PathEndMode.Touch);

            yield return Toils_General.Wait(30);

            var merge = compMergable.Toil_Merge(Victim);
            merge.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return merge;
        }
    }
}
