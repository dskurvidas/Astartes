using System;
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
            Pawn newPawnTemplate = PawnGenerator.GeneratePawn(new PawnGenerationRequest(AstartesDefOf.AstartesColonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, fixedGender: pawn.gender, fixedBirthName: pawn.story.birthLastName, fixedLastName: pawn.story.birthLastName));
            newPawnTemplate.needs.food.CurLevel = pawn.needs.food.CurLevel;
            newPawnTemplate.needs.rest.CurLevel = pawn.needs.rest.CurLevel;
            newPawnTemplate.training.SetWantedRecursive(TrainableDefOf.Obedience, true);
            newPawnTemplate.training.Train(TrainableDefOf.Obedience, null, true);
            newPawnTemplate.Name = pawn.Name;
            Pawn newPawn = (Pawn)GenSpawn.Spawn(newPawnTemplate, pawn.PositionHeld, pawn.MapHeld, WipeMode.Vanish);
            pawn.apparel.DropAll(pawn.PositionHeld, true);
            pawn.equipment.DropAllEquipment(pawn.PositionHeld, true);
            pawn.Destroy(DestroyMode.Vanish);

            newPawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
        }
    }
}
