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
            get { return "Allows changing directions of individual lanes."; }
        }
    }
}
