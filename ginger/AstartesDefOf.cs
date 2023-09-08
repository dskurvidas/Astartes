using RimWorld;
using Verse;

namespace AstartesKytheron
{
    [DefOf]
    public static class AstartesDefOf
    {
        public static PawnKindDef AstartesColonist;

        static AstartesDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AstartesDefOf));
        }
    }
}
