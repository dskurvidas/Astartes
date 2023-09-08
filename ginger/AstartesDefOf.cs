using RimWorld;
using Verse;

namespace AstartesKytheron
{
    [DefOf]
    public static class AstartesDefOf
    {
        public static PawnKindDef AstartesPawn;

        static AstartesDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AstartesDefOf));
        }
    }
}
