using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneChanger
{
    class LaneDirectionToggleButton : UIButton
    {
        public uint laneId;
        public ushort toggleFlag;

        public override bool canFocus
        {
            get
            {
                return false;
            }
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            NetManager netManager = Singleton<NetManager>.instance;
            if((netManager.m_lanes.m_buffer[laneId].m_flags & toggleFlag) == toggleFlag) 
            {
                netManager.m_lanes.m_buffer[laneId].m_flags &= (ushort) ~toggleFlag;
            }
            else
            {
                netManager.m_lanes.m_buffer[laneId].m_flags |= toggleFlag;
            }
            //Let's see if updating the node does anything
            NetSegment segment = netManager.m_segments.m_buffer[netManager.m_lanes.m_buffer[laneId].m_segment];
            if (segment.m_startNode != 0)
                netManager.UpdateNode(segment.m_startNode, netManager.m_lanes.m_buffer[laneId].m_segment, 0);
            if (segment.m_endNode != 0)
                netManager.UpdateNode(segment.m_endNode, netManager.m_lanes.m_buffer[laneId].m_segment, 0);

            UpdateButtonState();
        }


        public void DrawButton(uint lane, NetLane.Flags flag, Vector3 relativePosition) 
        {
            
            this.laneId = lane;
            this.toggleFlag = (ushort) flag;
            this.relativePosition = relativePosition;
            this.normalBgSprite = "ButtonMenu";
            this.disabledBgSprite = "ButtonMenuDisabled";
            this.hoveredBgSprite = "ButtonMenuHovered";
            this.pressedBgSprite = "ButtonMenuPressed";
            this.eventClick += ButtonClick;
            this.text = flag == NetLane.Flags.Forward ? "F" : flag == NetLane.Flags.Left ? "L" : "R";
            this.height = 30;
            this.width = 30;
            UpdateButtonState();
        }

        protected override void OnMouseLeave(UIMouseEventParameter p)
        {
            base.OnMouseLeave(p);
            UpdateButtonState();
        }

        protected override void OnMouseEnter(UIMouseEventParameter p)
        {
            base.OnMouseEnter(p);
            UpdateButtonState();
        }

        public void UpdateButtonState()
        {
            NetManager netManager = Singleton<NetManager>.instance;
            if ((toggleFlag & netManager.m_lanes.m_buffer[laneId].m_flags) == toggleFlag)
            {
                this.state = ButtonState.Pressed;
            }
            else
            {
                this.state = ButtonState.Normal;
            }
            
        }
    }
}
