using App.Data.Models;
using FastApi.Endpoints;

namespace Api;

// TODO : it is probably should not be here 
// it probably should be 
public static class Configs
{
    public static NonEditableProperties CreateNonEditableConfig()
    {
        return new NonEditableProperties(
            Config.Create<Agg>(a => a.Id)
        );
    }
}