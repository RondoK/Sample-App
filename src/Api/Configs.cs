using App.Data.Models;
using FastApi.Endpoints;

namespace Api;

public class Configs
{
    public static NonEditableProperties CreateNonEditableConfig()
    {
        return new NonEditableProperties(
            Config.Create<Agg>(a => a.Id)
        );
    }
}