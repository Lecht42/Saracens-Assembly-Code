using Saracens.Comps;
using Verse;
using Verse.AI;

namespace Saracens.Jobs
{
    public class ThinkNode_ConditionalHasMergable : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.GetComp<CompMergable>() != null;
        }
    }
}
