using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Saracens.ActionWorkers
{
    internal class DeathActionWorker_NoCorpseAndDrop : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            
        }
    }
}
