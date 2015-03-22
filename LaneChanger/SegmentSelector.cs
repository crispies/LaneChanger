/*
 * SegmentSelector.cs - Defines the segment selector tool
 * 
 */
using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class SegmentSelector : ToolBase
    {
        public LaneUIToggle button;
        public int currentSegment;
        LaneUIPanel box;
       
        override protected void OnToolUpdate() 
        {
            base.OnToolUpdate();
            // Cast ray, find segment
            // Display lane info
            if (box == null && Input.GetMouseButtonDown(0))
            {
                ushort segmentId = GetSegmentAtCursor();
                if (segmentId != 0)
                {   
                    NetSegment segment = Singleton<NetManager>.instance.m_segments.m_buffer[segmentId];
                    if (box != null)
                    {
                        Object.Destroy(box);
                    }
                    UIView v = UIView.GetAView();
                    box = (LaneUIPanel)v.AddUIComponent(typeof(LaneUIPanel));
                    box.changeSegment(segment, segmentId);
                }
            }
            
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            if (currentSegment != 0)
            {
                NetTool.RenderOverlay(RenderManager.instance.CurrentCameraInfo, ref NetManager.instance.m_segments.m_buffer[currentSegment], new Color(0.0f, 1f, 0.5f, 0.6f), new Color(0, 1, 0, 0.9f));
            }
            base.RenderOverlay(cameraInfo);
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
                currentSegment = output.m_netSegment;
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
