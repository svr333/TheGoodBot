using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace TheGoodBot.Core.Preconditions
{
    public class NoNegativeValue : ParameterPreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            ParameterInfo parameter, object value, IServiceProvider services)
        {
            if (Convert.ToInt32(value) > 0) return PreconditionResult.FromError("NegativeValueInput");
            return PreconditionResult.FromSuccess();
        }
    }
}
