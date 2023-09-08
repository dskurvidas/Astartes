﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;


namespace AstartesKytheron
{
    public class AstartesGeneImplantation : Recipe_InstallArtificialBodyPart
    {
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(record) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) && !pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && (x.def == recipe.addsHediff || !recipe.CompatibleWithHediff(x.def))));
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
                {
                    return;
                }
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
                {
                    billDoer,
                    pawn
                });
            }
            pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);

            //pawn.ChangeKind(DefDatabase<PawnKindDef>.GetNamed("Drifter_SAS_75"));

            Log.Message(AstartesDefOf.AstartesColonist.defName);
            pawn.ChangeKind(AstartesDefOf.AstartesColonist);
        }
    }
}
