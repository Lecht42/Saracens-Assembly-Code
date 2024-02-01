using AnimalBehaviours;
using Verse;

namespace Saracens.ActionWorkers
{
    internal class DeathActionWorker_NoCorpseAndLoot : DeathActionWorker_DropOnDeath
    {
        public override void PawnDied(Corpse corpse)
        {
            base.PawnDied(corpse);
            corpse.Destroy();
        }
    }
}
