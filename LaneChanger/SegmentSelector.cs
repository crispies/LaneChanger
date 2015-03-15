using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class SegmentSelector : ToolBase
    {
        public LaneUIToggle button;
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
                    LaneUIManager.DebugMessage("Segment Flags: " + segment.m_flags);       
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
                LaneUIManager.DebugMessage("Raycast success " + output.m_netSegment.ToString());
                return output.m_netSegment;
            }
            else
            {
                return 0;
            }
        }

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
            button.CancelLaneSelect();
            base.OnDisable();
        }
    }
}
