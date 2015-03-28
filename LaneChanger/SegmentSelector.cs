/*
 * SegmentSelector.cs - Defines the segment selector tool
 * 
 */
using UnityEngine;
using UnityEngine.EventSystems;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class SegmentSelector : ToolBase
    {
        public LaneUIToggle button;
        public ushort currentSegment;
        LaneUIPanel box;
       
        override protected void OnToolUpdate() 
        {
            base.OnToolUpdate();
            // Cast ray, find segment
            // Display lane info
            if (Input.GetMouseButtonDown(0) && !this.m_toolController.IsInsideUI)
            {
                ushort segmentId = GetSegmentAtCursor();
                LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
                if (segmentId != 0)
                {
                    if (currentSegment == 0)
                    {
                        if (box == null)
                        {
                            UIView v = UIView.GetAView();
                            box = (LaneUIPanel)v.AddUIComponent(typeof(LaneUIPanel));
                        }
                        NetSegment segment = NetManager.instance.m_segments.m_buffer[segmentId];
                        box.changeSegment(segment, segmentId);
                        currentSegment = segmentId;
                    }
                    else if(pathManager.laneChangerSegments[currentSegment] != null && IsSegmentConnected(currentSegment, segmentId))
                    {
                        
                        if (pathManager.laneChangerSegments[currentSegment].PermittedConnectionTo(segmentId))
                        {
                            pathManager.laneChangerSegments[currentSegment].RemovePermittedConnection(segmentId);
                        }
                        else
                        {
                            pathManager.laneChangerSegments[currentSegment].AddPermittedConnection(segmentId);
                        }
                        UpdateSegmentLaneMarkers();
                    } 
                    else 
                    {
                        Object.Destroy(box);
                        UIView v = UIView.GetAView();
                        box = (LaneUIPanel)v.AddUIComponent(typeof(LaneUIPanel));
                        NetSegment segment = NetManager.instance.m_segments.m_buffer[segmentId];
                        box.changeSegment(segment, segmentId);
                        currentSegment = segmentId;
                    }
                    
                }
            }
            else if (Input.GetMouseButtonDown(1) && !this.m_toolController.IsInsideUI)
            {
                Object.Destroy(box);
                currentSegment = 0;
            }

            //if (box == null && Input.GetMouseButtonDown(0))
            //{
            //    ushort segmentId = GetSegmentAtCursor();
            //    if (segmentId != 0)
            //    {   
            //        NetSegment segment = Singleton<NetManager>.instance.m_segments.m_buffer[segmentId];
            //        if (box != null)
            //        {
            //            Object.Destroy(box);
            //        }
            //        UIView v = UIView.GetAView();
            //        box = (LaneUIPanel)v.AddUIComponent(typeof(LaneUIPanel));
            //        box.changeSegment(segment, segmentId);
            //    }
            //}
            
        }

        public static void UpdateLaneMarkers(ushort segmentId) 
        {
            NetManager netManager = NetManager.instance;
            NetSegment segment = netManager.m_segments.m_buffer[segmentId];
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            ushort startLeftSegment = segment.m_startLeftSegment;
            ushort startRightSegment = segment.m_startRightSegment;
            ushort endLeftSegment = segment.m_endLeftSegment;
            ushort endRightSegment = segment.m_endRightSegment;
            if (!pathManager.laneChangerSegments[segmentId].PermittedConnectionTo(startLeftSegment))
                UpdateLaneMarker(NetLane.Flags.Left, NetInfo.Direction.Backward, segment);
            if (!pathManager.laneChangerSegments[segmentId].PermittedConnectionTo(startRightSegment))
                UpdateLaneMarker(NetLane.Flags.Right, NetInfo.Direction.Backward, segment);
            if (!pathManager.laneChangerSegments[segmentId].PermittedConnectionTo(endLeftSegment))
                UpdateLaneMarker(NetLane.Flags.Left, NetInfo.Direction.Forward, segment);
            if (!pathManager.laneChangerSegments[segmentId].PermittedConnectionTo(endRightSegment))
                UpdateLaneMarker(NetLane.Flags.Right, NetInfo.Direction.Forward, segment);
        }

        private void UpdateSegmentLaneMarkers()
        {
            UpdateLaneMarkers(currentSegment);  
        }

        private static void UpdateLaneMarker(NetLane.Flags flag, NetInfo.Direction direction, NetSegment segment)
        {
            NetManager netManager = NetManager.instance;
            NetLane currentLane = netManager.m_lanes.m_buffer[segment.m_lanes];
            ulong laneID = segment.m_lanes;
            for (int i = 0; i < segment.Info.m_lanes.Length; i++)
            {
                if (segment.Info.m_lanes[i].m_direction == direction && segment.Info.m_lanes[i].m_laneType == NetInfo.LaneType.Vehicle && ((NetLane.Flags)currentLane.m_flags & flag) == flag)
                {
                    NetLane.Flags newFlags = (NetLane.Flags)currentLane.m_flags;
                    newFlags &= ~flag;
                    newFlags |= NetLane.Flags.Forward;
                    netManager.m_lanes.m_buffer[laneID].m_flags = (ushort)newFlags;
                }
                laneID = currentLane.m_nextLane;
                currentLane = netManager.m_lanes.m_buffer[currentLane.m_nextLane];
            }
        }

        private bool IsSegmentConnected(ushort seg1, ushort seg2)
        {
            bool ret = false;

            ret = CheckForSegmentConnectionThroughNode(seg2, NetManager.instance.m_nodes.m_buffer[NetManager.instance.m_segments.m_buffer[seg1].m_startNode]);
            if(!ret)
                ret = CheckForSegmentConnectionThroughNode(seg2, NetManager.instance.m_nodes.m_buffer[NetManager.instance.m_segments.m_buffer[seg1].m_endNode]);
            
            return ret;
        }

        private bool CheckForSegmentConnectionThroughNode(ushort seg, NetNode node)
        {
            for (int i = 0; i < node.CountSegments(); i++)
            {
                if (node.GetSegment(i) == seg)
                {
                    return true;
                    
                }
            }
            return false;
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            if (currentSegment != 0)
            {
                NetTool.RenderOverlay(RenderManager.instance.CurrentCameraInfo, ref NetManager.instance.m_segments.m_buffer[currentSegment], new Color(0.0f, 0.0f, 0.5f, 0.6f), new Color(0f, 0.0f, 0.5f, 0.9f));
                LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
                NetManager netManager = NetManager.instance;
                if (pathManager.laneChangerSegments[currentSegment] != null)
                {
                    RenderOverlayForSegments(cameraInfo, netManager.m_nodes.m_buffer[netManager.m_segments.m_buffer[currentSegment].m_startNode]);
                    RenderOverlayForSegments(cameraInfo, netManager.m_nodes.m_buffer[netManager.m_segments.m_buffer[currentSegment].m_endNode]);
                }
            }
            base.RenderOverlay(cameraInfo);
        }

        void RenderOverlayForSegments(RenderManager.CameraInfo cameraInfo, NetNode node)
        {
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            for (int i = 0; i < node.CountSegments(); i++)
            {
                ushort segId = node.GetSegment(i);
                if (segId != currentSegment)
                {
                    if (pathManager.laneChangerSegments[currentSegment].PermittedConnectionTo(segId))
                        NetTool.RenderOverlay(RenderManager.instance.CurrentCameraInfo, ref NetManager.instance.m_segments.m_buffer[segId], new Color(0.0f, 1f, 0.0f, 0.6f), new Color(0f, 1f, 0f, 0.9f));
                    else
                        NetTool.RenderOverlay(RenderManager.instance.CurrentCameraInfo, ref NetManager.instance.m_segments.m_buffer[segId], new Color(1f, 0.0f, 0.0f, 0.6f), new Color(1f, 0f, 0f, 0.9f));
                }
            }
        }

        private ushort GetSegmentAtCursor() 
        {
            Vector3 mousePosition = Input.mousePosition;
            RaycastInput input = new RaycastInput(Camera.main.ScreenPointToRay(mousePosition), Camera.main.farClipPlane);
            RaycastOutput output;
            input.m_netService = new RaycastService(ItemClass.Service.Road, ItemClass.SubService.None, ItemClass.Layer.Default);
            input.m_ignoreSegmentFlags = NetSegment.Flags.None;
            input.m_ignoreTerrain = true;
            if (RayCast(input, out output))
            {
                return output.m_netSegment;
            }
            else
            {
                return 0;
            }
            
        }

        // I am not sure why the tool controller needs to be injected here.
        // ToolBase suggests this should happen on its own, but it does not.
        protected override void OnEnable()
        {
            m_toolController = GameObject.FindObjectOfType<ToolController>();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (box != null)
            {
                Object.Destroy(box);
            }
            button.textColor = new Color32(255, 255, 255, 255);
            button.laneSelectEnabled = false;
            base.OnDisable();
        }
    }
}
