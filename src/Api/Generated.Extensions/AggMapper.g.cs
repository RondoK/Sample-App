using Api.Generated.Models;
using App.Data.Models;

namespace Api.Generated.Models
{
    public static partial class AggMapper
    {
        public static Agg AdaptTo(this AggPatch p1, Agg p2)
        {
            if (p1 == null)
            {
                return null;
            }
            Agg result = p2 ?? new Agg();
            
            result.Text = p1.Text;
            return result;
            
        }
    }
}