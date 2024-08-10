using AnimalBehaviours;
using Verse;
using Verse.AI.Group;

namespace Saracens.ActionWorkers
{
    internal class DeathActionWorker_NoCorpseAndLoot : DeathActionWorker_DropOnDeath
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            base.PawnDied(corpse, prevLord);
            corpse.Destroy();
        }
    }
}
