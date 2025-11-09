

namespace TN.Setup.Application.Setup.Command.UpdateSubModules.RequestCommandMapper
{
 public static class UpdateSubModulesCommandMapper 
    {
        public static UpdateSubModulesCommand ToCommand(this UpdateSubModulesRequest request, string subModulesId)
        {
            return new UpdateSubModulesCommand
                (
                    subModulesId,
                    request.name, 
                    request.iconUrl,
                    request.targetUrl,
                    request.modulesId,
                    request.rank,
                    request.isActive
                    
                );

        }

    }
}
