using System.Collections;
using System.Linq;
using System;
using ICities;
using UnityEngine;

namespace LaneChanger
{
    public class LaneChangerSerializableDataExtension : SerializableDataExtensionBase
    {
        public static readonly string dataID = "LaneChangerV1";

        public override void OnCreated(ISerializableData serializableData)
        {
            base.OnCreated(serializableData);
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }

        public override void OnLoadData()
        {
            base.OnLoadData();
            if (!serializableDataManager.EnumerateData().Contains(dataID))
            {
                Debug.Log("No LaneChanger data found in save file.");
                return;
            }
            byte[] data = serializableDataManager.LoadData(dataID);
            Debug.Log(string.Format("Read {0} bytes for {1}", data.Length, dataID));

            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            int posn = 0;
            for (ushort i = 0; i < 32768; i++) 
            {
                ushort segCount = (ushort) data[posn++];
                if (segCount > 0)
                {
                    pathManager.laneChangerSegments[i] = new LaneChangerSegment();
                    for (int j = 0; j < segCount; j++)
                    {
                        pathManager.laneChangerSegments[i].AddPermittedConnection(BitConverter.ToUInt16(data, posn));
                        posn = posn + 2;
                        SegmentSelector.UpdateLaneMarkers(i);
                    }
                }
            }
        }

        public override void OnSaveData()
        {
            base.OnSaveData();
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            FastList<byte> data = new FastList<byte>();
            for (int i = 0; i < 32768; i++)
            {
                if (pathManager.laneChangerSegments[i] == null)
                {
                    data.Add(0);
                }
                else
                {
                    byte npc = (byte) pathManager.laneChangerSegments[i].numPermittedConnections;
                    data.Add(npc);
                    for (int j = 0; j < pathManager.laneChangerSegments[i].numPermittedConnections; j++)
                    {
                        byte[] connectionBytes = BitConverter.GetBytes(pathManager.laneChangerSegments[i].permittedConnections[j]);
                        foreach (byte permittedConnection in connectionBytes)
                            data.Add(permittedConnection);                       
                    }
                }
            }
            byte[] dataToSave = data.ToArray();
            serializableDataManager.SaveData(dataID, dataToSave);
        }
    }
}
