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
            Debug.Log("started on level loaded");
            // this seems to get the default UIView
            UIView v = UIView.GetAView();

            //this adds an UIComponent to the view
            uiComponent = v.AddUIComponent(typeof(LaneUIToggle));

            /*
            //Still more flagrant insanity
            NewPathManager newPathManager = PathManager.instance;
            SimulationManager.RegisterSimulationManager(newPathManager);

            //Screw the old one, we don't need it no more, actually, no, lets keep it.
            //But we are going to return to Crazygonuts University
            int numPrefabs = PrefabCollection<VehicleInfo>.PrefabCount();
            int count = 0;
            Debug.Log("Before while: " + count + " num " + numPrefabs);
            while (count < numPrefabs)
            {
                Debug.Log("Got to while: " + count + " num " + numPrefabs);
                VehicleInfo prefab = PrefabCollection<VehicleInfo>.GetPrefab((uint)count);
                VehicleAI component = prefab.GetComponent<VehicleAI>();
                if(component != null && typeof(PassengerCarAI) == component.GetType()) {
                    Debug.Log("Got to if " + typeof(PassengerCarAI) + " and " + component.GetType());
                    FieldInfo[] fields = component.GetType().GetFields();
	                Dictionary<string, object> dictionary = new Dictionary<string, object>(fields.Length);
	                for (int i = 0; i < fields.Length; i++)
	                {
		                FieldInfo fieldInfo = fields[i];
		                dictionary[fieldInfo.Name] = fieldInfo.GetValue(component);
	                }
                    UnityEngine.Object.DestroyImmediate(component);
                    VehicleAI vehicleAI = prefab.gameObject.AddComponent<LaneControlPassengerCarAI>() as VehicleAI;
                    foreach (KeyValuePair<string, object> current in dictionary)
                    {
                        FieldInfo field = typeof(LaneControlPassengerCarAI).GetField(current.Key);
                        Debug.Log("before field == null");
                        if (field == null)
                        {
                            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, string.Concat(new object[]
			                {
				                "Could not find field ",
				                current.Key,
				                " in ",
				                typeof(LaneControlPassengerCarAI)
			                }));
                        }
                        else
                        {
                            field.SetValue(typeof(LaneControlPassengerCarAI), current.Value);
                            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, string.Concat(new object[]
			                {
				                "Set value ",
				                current.Key,
				                " to ",
				                current.Value
			                }));
                        }
                        Debug.Log("after field == null block");
                    }
                    if (vehicleAI != null)
                    {
                        vehicleAI.m_info = prefab;
                        prefab.m_vehicleAI = vehicleAI;
                        vehicleAI.InitializeAI();
                    }

                }
                Debug.Log("Out of if");
                count++;
            }
            */

            //Insane - and probably unsafe.
            //Need to cleanly stop the old pathfinders before
            //jamming the new ones in there.
            /*
            //STOP THE INSANITY
            FieldInfo field = typeof(PathManager).GetField("m_pathfinds", BindingFlags.NonPublic | BindingFlags.Instance);
            PathFind[] pathFinds = (PathFind[])field.GetValue(PathManager.instance);
            for (int i = 0; i < pathFinds.Length; i++)
            {
                PathFind oldPathFind = pathFinds[i];
                pathFinds[i] = PathManager.instance.gameObject.AddComponent<RePathFind>();
                Object.Destroy(oldPathFind);
                Debug.Log(pathFinds[i].ToString());
                Debug.Log(pathFinds[i].IsAvailable.ToString());
            }
            //This is dumb.
            */
            /*
            PathFind[] oldPathFinds = (PathFind[]) field.GetValue(PathManager.instance);
            PathFind[] newPathFinds = new PathFind[oldPathFinds.Length];
            for (int i = 0; i < oldPathFinds.Length; i++)
	        {
		        newPathFinds[i] = PathManager.instance.gameObject.AddComponent<RePathFind>();
	        }
            field.SetValue(PathManager.instance, newPathFinds);
            DebugMessage(newPathFinds.ToString());
            for (int i = 0; i < oldPathFinds.Length; i++)
            {
                Object.Destroy(oldPathFinds[i]);
            }
            */
            /*
            //Replace Passenger Car AI
            uint prefabCount = (uint) PrefabCollection<VehicleInfo>.PrefabCount();
            uint counter = 0;
            while (counter < prefabCount)
            {
                VehicleInfo prefab = PrefabCollection<VehicleInfo>.GetPrefab(counter);
                VehicleAI oldVehicleAI = prefab.GetComponent<VehicleAI>();
                if (oldVehicleAI.GetType() == typeof(PassengerCarAI))
                {
                    FieldInfo[] fields = typeof(PassengerCarAI).GetFields();
                    Dictionary<string, object> fieldValues = new Dictionary<string, object>(fields.Length);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fieldValues[fields[i].Name] = fields[i].GetValue(oldVehicleAI);
                    }
                    UnityEngine.Object.DestroyImmediate(oldVehicleAI);
                    VehicleAI newVehicleAI = prefab.gameObject.AddComponent(typeof(LaneChangerPasengerCarAI)) as VehicleAI;
                    foreach (KeyValuePair<string, object> current in fieldValues)
                    {
                        FieldInfo field = newVehicleAI.GetType().GetField(current.Key);
                        if (field != null)
                            field.SetValue(newVehicleAI, current.Value);
                    }
                    if (newVehicleAI != null)
                    {
                        newVehicleAI.m_info = prefab;
                        prefab.m_vehicleAI = newVehicleAI;
                        newVehicleAI.InitializeAI();
                    }
                }
                counter++;
            }
            */
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
