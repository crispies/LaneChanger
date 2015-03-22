/*
 * LaneDirectionToggleButton.cs - Buttons to toggle lane flags
 * 
 */
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
                netManager.m_lanes.m_buffer[laneId].m_flags &= (ushort) ~toggleFlag;
            else
                netManager.m_lanes.m_buffer[laneId].m_flags |= toggleFlag;

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
            this.pressedColor = new Color(0f, 0.6f, 0f);
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
