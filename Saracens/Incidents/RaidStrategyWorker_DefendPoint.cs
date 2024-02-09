using RimWorld;
using Saracens.Extensions;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace Saracens.Incidents
{
    public class RaidStrategyWorker_DefendPoint : RaidStrategyWorker
    {
        protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
        {
            IntVec3 originCell = (parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld);
            RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out var result);
            return new LordJob_DefendPoint(originCell);
        }

        public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
        {
            if (!base.CanUseWith(parms, groupKind))
            {
                return false;
            }
            return parms.faction.def.GetModExtension<CanArriveAndDefend>()?.canArriveAndDefend ?? false;
        }
    }
}
