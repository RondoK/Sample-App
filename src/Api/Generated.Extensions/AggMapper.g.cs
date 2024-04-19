using Api.Generated.Models;
using App.Data.Models;

namespace Api.Generated.Models
{
    public static partial class AggMapper
    {
        public static Agg AdaptToAgg(this AggPatch p1)
        {
            return p1 == null ? null : new Agg() {Text = p1.Text};
        }
        public static Agg AdaptTo(this AggPatch p2, Agg p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Agg result = p3 ?? new Agg();
            
            result.Text = p2.Text;
            return result;
            
        }
    }
}