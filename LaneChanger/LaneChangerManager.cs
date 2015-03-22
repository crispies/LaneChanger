/*
 * LaneUIManager.cs - Adds the Lanes button and generally kicks off the process.
 * 
 */
using UnityEngine;
using ICities;
using System;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class LaneChangerManager : LoadingExtensionBase
    {

        UIComponent uiComponent;

        public override void OnCreated(ILoading loading)
        {
            FieldInfo pathManagerInstance = typeof(ColossalFramework.Singleton<PathManager>).GetField("sInstance", BindingFlags.Static | BindingFlags.NonPublic);
            PathManager stockPathManager = PathManager.instance;
            LaneChangerPathManager lcPathManager = stockPathManager.gameObject.AddComponent<LaneChangerPathManager>();
            lcPathManager.UpdateWithPathManagerValues(stockPathManager);
            pathManagerInstance.SetValue(null, lcPathManager);
            FastList<ISimulationManager> managers = typeof(SimulationManager).GetField("m_managers", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as FastList<ISimulationManager>;
            managers.Remove(stockPathManager);
            managers.Add(lcPathManager);
            GameObject.Destroy(stockPathManager, 10f);
        }

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
                UnityEngine.Object.Destroy(uiComponent);
        }
    }
}
