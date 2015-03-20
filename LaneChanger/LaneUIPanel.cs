/*
 * LaneUIPanel.cs - Defines the main UI panel for editing lanes
 */
using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class LaneUIPanel : UIPanel
    {
        NetSegment currentSegment;
        UILabel segmentLabel;

        public override void Start()
        {
            this.backgroundSprite = "GenericPanel";
            this.color = new Color32(128, 128, 128, 200);
        }

        void changeLaneFlags(uint laneId, ushort newFlags)
        {

        }

        public void changeSegment(NetSegment segment, ushort segmentId)
        {
            currentSegment = segment;
            DrawSegmentInfoBox();
        }

        public void CloseClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            Object.Destroy(this);
        }

        private void DrawCloseButton()
        {
            UIButton closeButton = this.AddUIComponent<UIButton>();
            closeButton.relativePosition = new Vector3(155f, 2f);
            closeButton.width = 20f;
            closeButton.height = 15f;
            closeButton.text = "X";
            closeButton.normalBgSprite = "ButtonMenu";
            closeButton.disabledBgSprite = "ButtonMenuDisabled";
            closeButton.hoveredBgSprite = "ButtonMenuHovered";
            closeButton.pressedBgSprite = "ButtonMenuPressed";
            closeButton.eventClick += CloseClick;
        }

        private void DrawSegmentInfoBox() 
        {
            NetManager netManager = Singleton<NetManager>.instance;
            this.width = 180;
            this.height = 55 + 40 * currentSegment.Info.m_lanes.Length;
            this.transformPosition = calculateBoxPosition();                      
            segmentLabel = this.AddUIComponent<UILabel>();
            segmentLabel.relativePosition = new Vector3(1f, 1f);
            segmentLabel.wordWrap = false;
            segmentLabel.width = 178f;
            segmentLabel.height = 35f;
            segmentLabel.text = currentSegment.Info.name + "\n" + currentSegment.Info.m_lanes.Length + " lanes";
            DrawCloseButton();
            uint currentLane = currentSegment.m_lanes;
            int laneCounter = 0;
            while (laneCounter < currentSegment.Info.m_lanes.Length && currentLane != 0)
            {
                NetLane lane = netManager.m_lanes.m_buffer[currentLane];
                UILabel laneLabel = this.AddUIComponent<UILabel>();
                laneLabel.relativePosition = new Vector3(1f, 40f + 40f * laneCounter);
                laneLabel.text = currentLane.ToString();
                NetLane.Flags flags = (NetLane.Flags)netManager.m_lanes.m_buffer[currentLane].m_flags;
                
                LaneDirectionToggleButton leftToggle = this.AddUIComponent<LaneDirectionToggleButton>();
                leftToggle.DrawButton(currentLane, NetLane.Flags.Left, new Vector3(55f, 40f + 40f * laneCounter));

                LaneDirectionToggleButton straightToggle = this.AddUIComponent<LaneDirectionToggleButton>();
                straightToggle.DrawButton(currentLane, NetLane.Flags.Forward, new Vector3(95f, 40f + 40f * laneCounter));
                
                LaneDirectionToggleButton rightToggle = this.AddUIComponent<LaneDirectionToggleButton>();
                rightToggle.DrawButton(currentLane, NetLane.Flags.Right, new Vector3(135f, 40f + 40f * laneCounter));

                currentLane = lane.m_nextLane;
                laneCounter++;
            }
        }

        /*
         * 
         * if ((flags & NetLane.Flags.Left) == NetLane.Flags.Left)
                {
                    LaneUIManager.DebugMessage("Direction " + currentSegment.Info.m_lanes[i].m_direction + " Final " + currentSegment.Info.m_lanes[i].m_finalDirection);
                    LaneUIManager.DebugMessage("Removing left from " + currentLane);
                    flags &= ~NetLane.Flags.Left;
                    netManager.m_lanes.m_buffer[currentLane].m_flags = (ushort)flags;
                    LaneUIManager.DebugMessage("Direction " + currentSegment.Info.m_lanes[i].m_direction + " Final " + currentSegment.Info.m_lanes[i].m_finalDirection);
                    netManager.m_segments.m_buffer[segmentId].m_flags |= NetSegment.Flags.StopLeft;
                }
         */

        Vector3 calculateBoxPosition()
        {
            return new Vector3(-1.76f, 0.9f);
        }
    }
}
