using AnimalBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
