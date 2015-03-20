using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.UI;
using System;
using System.Threading;
using System.Reflection;
using UnityEngine;
namespace LaneChanger
{
    public class LaneChangerPathFind : PathFind
    {
        private struct BufferItem
        {
            public PathUnit.Position m_position;
            public float m_comparisonValue;
            public float m_methodDistance;
            public uint m_laneID;
            public NetInfo.Direction m_direction;
            public NetInfo.LaneType m_lanesUsed;
        }

        public int IGotHere = 0;
        public int IGotThere = 0;

        //Expose the private fields
        FieldInfo fieldpathUnits;
        FieldInfo fieldQueueFirst;
        FieldInfo fieldQueueLast;
        FieldInfo fieldCalculating;
        FieldInfo fieldQueueLock;
        FieldInfo fieldBufferLock;
        FieldInfo fieldPathFindThread;
        FieldInfo fieldTerminated;
        FieldInfo fieldBufferMinPos;
        FieldInfo fieldBufferMaxPos;
        FieldInfo fieldLaneLocation;
        FieldInfo fieldLaneTarget;
        FieldInfo fieldBuffer;
        FieldInfo fieldBufferMin;
        FieldInfo fieldBufferMax;
        FieldInfo fieldMaxLength;
        FieldInfo fieldStartLaneA;
        FieldInfo fieldStartLaneB;
        FieldInfo fieldEndLaneA;
        FieldInfo fieldEndLaneB;
        FieldInfo fieldVehicleLane;
        FieldInfo fieldStartOffsetA;
        FieldInfo fieldStartOffsetB;
        FieldInfo fieldVehicleOffset;
        FieldInfo fieldIsHeavyVehicle;
        FieldInfo fieldIgnoreBlocked;
        FieldInfo fieldStablePath;
        FieldInfo fieldPathRandomizer;
        FieldInfo fieldPathFindIndex;
        FieldInfo fieldLaneTypes;
        FieldInfo fieldVehicleTypes;

        private Array32<PathUnit> m_pathUnits
        {
            get { return fieldpathUnits.GetValue(this) as Array32<PathUnit>; }
            set { fieldpathUnits.SetValue(this, value); }
        }

        private uint m_queueFirst
        {
            get { return (uint)fieldQueueFirst.GetValue(this); }
            set { fieldQueueFirst.SetValue(this, value); }
        }

        private uint m_queueLast
        {
            get { return (uint)fieldQueueLast.GetValue(this); }
            set { fieldQueueLast.SetValue(this, value); }
        }

        private uint m_calculating
        {
            get { return (uint)fieldCalculating.GetValue(this); }
            set { fieldCalculating.SetValue(this, value); }
        }

        private object m_queueLock
        {
            get { return fieldQueueLock.GetValue(this); }
            set { fieldQueueLock.SetValue(this, value); }
        }

        private object m_bufferLock
        {
            get { return fieldBufferLock.GetValue(this); }
            set { fieldBufferLock.SetValue(this, value); }
        }

        private Thread m_pathFindThread
        {
            get { return fieldPathFindThread.GetValue(this) as Thread; }
            set { fieldPathFindThread.SetValue(this, value); }
        }

        private bool m_terminated
        {
            get { return (bool)fieldTerminated.GetValue(this); }
            set { fieldTerminated.SetValue(this, value); }
        }

        private int m_bufferMinPos
        {
            get { return (int)fieldBufferMinPos.GetValue(this); }
            set { fieldBufferMinPos.SetValue(this, value); }
        }

        private int m_bufferMaxPos
        {
            get { return (int)fieldBufferMaxPos.GetValue(this); }
            set { fieldBufferMaxPos.SetValue(this, value); }
        }

        private uint[] m_laneLocation
        {
            get { return fieldLaneLocation.GetValue(this) as uint[]; }
            set { fieldLaneLocation.SetValue(this, value); }
        }

        private PathUnit.Position[] m_laneTarget
        {
            get { return fieldLaneTarget.GetValue(this) as PathUnit.Position[]; }
            set { fieldLaneTarget.SetValue(this, value); }
        }

        private LaneChangerPathFind.BufferItem[] m_buffer;

        private int[] m_bufferMin
        {
            get { return fieldBufferMin.GetValue(this) as int[]; }
            set { fieldBufferMin.SetValue(this, value); }
        }

        private int[] m_bufferMax
        {
            get { return fieldBufferMax.GetValue(this) as int[]; }
            set { fieldBufferMax.SetValue(this, value); }
        }

        private float m_maxLength
        {
            get { return (float)fieldMaxLength.GetValue(this); }
            set { fieldMaxLength.SetValue(this, value); }
        }

        private uint m_startLaneA
        {
            get { return (uint)fieldStartLaneA.GetValue(this); }
            set { fieldStartLaneA.SetValue(this, value); }
        }

        private uint m_startLaneB
        {
            get { return (uint)fieldStartLaneB.GetValue(this); }
            set { fieldStartLaneB.SetValue(this, value); }
        }

        private uint m_endLaneA
        {
            get { return (uint)fieldEndLaneA.GetValue(this); }
            set { fieldEndLaneA.SetValue(this, value); }
        }

        private uint m_endLaneB
        {
            get { return (uint)fieldEndLaneB.GetValue(this); }
            set { fieldEndLaneB.SetValue(this, value); }
        }

        private uint m_vehicleLane
        {
            get { return (uint)fieldVehicleLane.GetValue(this); }
            set { fieldVehicleLane.SetValue(this, value); }
        }

        private byte m_startOffsetA
        {
            get { return (byte)fieldStartOffsetA.GetValue(this); }
            set { fieldStartOffsetA.SetValue(this, value); }
        }

        private byte m_startOffsetB
        {
            get { return (byte)fieldStartOffsetB.GetValue(this); }
            set { fieldStartOffsetB.SetValue(this, value); }
        }

        private byte m_vehicleOffset
        {
            get { return (byte)fieldVehicleOffset.GetValue(this); }
            set { fieldVehicleOffset.SetValue(this, value); }
        }

        private bool m_isHeavyVehicle
        {
            get { return (bool)fieldIsHeavyVehicle.GetValue(this); }
            set { fieldIsHeavyVehicle.SetValue(this, value); }
        }

        private bool m_ignoreBlocked
        {
            get { return (bool)fieldIgnoreBlocked.GetValue(this); }
            set { fieldIgnoreBlocked.SetValue(this, value); }
        }

        private bool m_stablePath
        {
            get { return (bool)fieldStablePath.GetValue(this); }
            set { fieldStablePath.SetValue(this, value); }
        }

        private Randomizer m_pathRandomizer
        {
            get { return (Randomizer)fieldPathRandomizer.GetValue(this); }
            set { fieldPathRandomizer.SetValue(this, value); }
        }

        private uint m_pathFindIndex
        {
            get { return (uint)fieldPathFindIndex.GetValue(this); }
            set { fieldPathFindIndex.SetValue(this, value); }
        }

        private NetInfo.LaneType m_laneTypes
        {
            get { return (NetInfo.LaneType)fieldLaneTypes.GetValue(this); }
            set { fieldLaneTypes.SetValue(this, value); }
        }

        private VehicleInfo.VehicleType m_vehicleTypes
        {
            get { return (VehicleInfo.VehicleType)fieldVehicleTypes.GetValue(this); }
            set { fieldVehicleTypes.SetValue(this, value); }
        }

        private void Awake()
        {
            Type stockPathFindType = typeof(PathFind);
            BindingFlags fieldFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            //Exposing parent's private fields
            fieldpathUnits = stockPathFindType.GetField("m_pathUnits", fieldFlags);
            fieldQueueFirst = stockPathFindType.GetField("m_queueFirst", fieldFlags);
            fieldQueueLast = stockPathFindType.GetField("m_queueLast", fieldFlags);
            fieldCalculating = stockPathFindType.GetField("m_calculating", fieldFlags);
            fieldQueueLock = stockPathFindType.GetField("m_queueLock", fieldFlags);
            fieldBufferLock = stockPathFindType.GetField("m_bufferLock", fieldFlags);
            fieldPathFindThread = stockPathFindType.GetField("m_pathFindThread", fieldFlags);
            fieldTerminated = stockPathFindType.GetField("m_terminated", fieldFlags);
            fieldBufferMinPos = stockPathFindType.GetField("m_bufferMinPos", fieldFlags);
            fieldBufferMaxPos = stockPathFindType.GetField("m_bufferMaxPos", fieldFlags);
            fieldLaneLocation = stockPathFindType.GetField("m_laneLocation", fieldFlags);
            fieldLaneTarget = stockPathFindType.GetField("m_laneTarget", fieldFlags);
            fieldBuffer = stockPathFindType.GetField("m_buffer", fieldFlags);
            fieldBufferMin = stockPathFindType.GetField("m_bufferMin", fieldFlags);
            fieldBufferMax = stockPathFindType.GetField("m_bufferMax", fieldFlags);
            fieldMaxLength = stockPathFindType.GetField("m_maxLength", fieldFlags);
            fieldStartLaneA = stockPathFindType.GetField("m_startLaneA", fieldFlags);
            fieldStartLaneB = stockPathFindType.GetField("m_startLaneB", fieldFlags);
            fieldEndLaneA = stockPathFindType.GetField("m_endLaneA", fieldFlags);
            fieldEndLaneB = stockPathFindType.GetField("m_endLaneB", fieldFlags);
            fieldVehicleLane = stockPathFindType.GetField("m_vehicleLane", fieldFlags);
            fieldStartOffsetA = stockPathFindType.GetField("m_startOffsetA", fieldFlags);
            fieldStartOffsetB = stockPathFindType.GetField("m_startOffsetB", fieldFlags);
            fieldVehicleOffset = stockPathFindType.GetField("m_vehicleOffset", fieldFlags);
            fieldIsHeavyVehicle = stockPathFindType.GetField("m_isHeavyVehicle", fieldFlags);
            fieldIgnoreBlocked = stockPathFindType.GetField("m_ignoreBlocked", fieldFlags);
            fieldStablePath = stockPathFindType.GetField("m_stablePath", fieldFlags);
            fieldPathRandomizer = stockPathFindType.GetField("m_pathRandomizer", fieldFlags);
            fieldPathFindIndex = stockPathFindType.GetField("m_pathFindIndex", fieldFlags);
            fieldLaneTypes = stockPathFindType.GetField("m_laneTypes", fieldFlags);
            fieldVehicleTypes = stockPathFindType.GetField("m_vehicleTypes", fieldFlags);

            // This might create an extra array of BufferItems hidden in the parent
            // class.
            // TODO: check if this is still needed
            stockPathFindType.GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);

            //Kill the old pathfind thread and allocate a new buffer
            this.m_pathFindThread.Abort();
            this.m_pathfindProfiler = new ThreadProfiler();
            this.m_buffer = new LaneChangerPathFind.BufferItem[65536];
            this.m_bufferLock = LaneChangerPathManager.instance.m_bufferLock;
            this.m_pathUnits = LaneChangerPathManager.instance.m_pathUnits;

            //Launch a new pathfinding thread
            this.m_pathFindThread = new Thread(new ThreadStart(this.PathFindThread));
            this.m_pathFindThread.Name = "Pathfind";
            this.m_pathFindThread.Start();
            if (!this.m_pathFindThread.IsAlive)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, "Path find thread failed to start!");
            }

        }

        //Unmodified from stock
        private void OnDestroy()
        {
            while (!Monitor.TryEnter(this.m_queueLock, SimulationManager.SYNCHRONIZE_TIMEOUT))
            {
            }
            try
            {
                this.m_terminated = true;
                Monitor.PulseAll(this.m_queueLock);
            }
            finally
            {
                Monitor.Exit(this.m_queueLock);
            }
        }
    }
}