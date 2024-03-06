using Verse;
using Verse.AI;

namespace Saracens.Jobs
{
    public class ThinkNode_ConditionalFeelPain : ThinkNode_Conditional
    {
        protected float painThreeshold = 0.2f;

        public float PainThreeshold => painThreeshold;

        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.health.hediffSet.PainTotal >= PainThreeshold;
        }
    }
}
