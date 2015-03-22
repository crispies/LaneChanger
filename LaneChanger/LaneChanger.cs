/*
 * LaneChanger.cs - Lane Control for Cities:Skylines
 * 
 */
using ICities;

namespace LaneChanger
{
    public class LaneChanger : IUserMod
    {
        public string Name
        {
            get { return "Lane Changer"; }
        }

        public string Description
        {
            get { return "Adds no left/right turn and ability to change direction of individual lanes."; }
        }
    }
}
