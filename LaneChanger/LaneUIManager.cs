/*
 * LaneUIManager.cs - Adds the Lanes button and generally kicks off the process.
 * 
 */
using UnityEngine;
using ICities;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class LaneUIManager : LoadingExtensionBase
    {

        UIComponent uiComponent;

        public static void DebugMessage(string message)
        {
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, message);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {

            // this seems to get the default UIView
            UIView v = UIView.GetAView();

            //this adds an UIComponent to the view
            uiComponent = v.AddUIComponent(typeof(LaneUIToggle));
        }


        // Not sure if these two are needed

        public override void OnReleased()
        {
            ReleaseComponent();
        }

        void ReleaseComponent()
        {
            if (uiComponent != null)
                Object.Destroy(uiComponent);
        }
    }
}
