using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Agv
{
  
    public enum MoveType
    {
        move,
        //Swerve0,
        //Swerve90,
        //Swerve180,
        //Swerve270,
        arrived,
        stopAvoidConflict,
        cannotReceiveRunCommands,
        cannotReceiveSwerverCommands,
        agvFault,
        clearFault,
        exceedMaxForwordStep

    }
}
