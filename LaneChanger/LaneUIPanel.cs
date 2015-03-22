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
        //UILabel segmentLabel;

        public override void Start()
        {
            this.backgroundSprite = "GenericPanel";
            this.color = new Color32(180, 180, 180, 230);
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
            GameObject.FindObjectOfType<SegmentSelector>().currentSegment = 0;
            Object.Destroy(this);
        }

        private void DrawCloseButton()
        {
            UIButton closeButton = this.AddUIComponent<UIButton>();
            closeButton.relativePosition = new Vector3(120f * CountLanes()- 3f, 2f);
            closeButton.width = 30f;
            closeButton.height = 20f;
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
            this.width = 120f * CountLanes() + 30f;
            this.height = 100f;
            DrawCloseButton();
            this.transformPosition = calculateBoxPosition();
            ushort startNodeID = currentSegment.m_startNode;
            ushort endNodeID = currentSegment.m_endNode;
            NetNode startNode = netManager.m_nodes.m_buffer[startNodeID];
            NetNode endNode = netManager.m_nodes.m_buffer[endNodeID];
            DirectionSelectButtons(currentSegment, NetInfo.Direction.Backward, 5f);
            DirectionSelectButtons(currentSegment, NetInfo.Direction.Forward, 45f);
        }

        public void DirectionSelectButtons(NetSegment currentSegment, NetInfo.Direction dir, float y)
        {
            float columnOffset = 0f;
            uint currentLane = currentSegment.m_lanes;
            for (int i = 0; i < currentSegment.Info.m_lanes.Length; i++)
            {
                if (currentSegment.Info.m_lanes[i].m_laneType == NetInfo.LaneType.Vehicle && currentSegment.Info.m_lanes[i].m_direction == dir)
                {
                    LaneDirectionToggleButton leftToggleButton = this.AddUIComponent<LaneDirectionToggleButton>();
                    leftToggleButton.DrawButton(currentLane, NetLane.Flags.Left, new Vector3(5f + columnOffset, 5f + y));

                    LaneDirectionToggleButton forwardToggleButton = this.AddUIComponent<LaneDirectionToggleButton>();
                    forwardToggleButton.DrawButton(currentLane, NetLane.Flags.Forward, new Vector3(40f + columnOffset, 5f + y));

                    LaneDirectionToggleButton rightToggleButton = this.AddUIComponent<LaneDirectionToggleButton>();
                    rightToggleButton.DrawButton(currentLane, NetLane.Flags.Right, new Vector3(75f + columnOffset, 5f + y));
                    columnOffset += 120f;
                }
                currentLane = NetManager.instance.m_lanes.m_buffer[currentLane].m_nextLane;
            }
        }

        public int CountLanes()
        {
            return Mathf.Max(CountLanes(NetInfo.Direction.Forward), CountLanes(NetInfo.Direction.Backward));
        }

        public int CountLanes(NetInfo.Direction dir)
        {
            int laneCount = 0;
            for (int i = 0; i < currentSegment.Info.m_lanes.Length; i++)
            {
                if (currentSegment.Info.m_lanes[i].m_laneType == NetInfo.LaneType.Vehicle && currentSegment.Info.m_lanes[i].m_direction == dir)
                    laneCount++;
            }
            return laneCount;
        }

        void NoValidNodeLabel(Vector3 position)
        {
            UILabel nodeLabel = this.AddUIComponent<UILabel>();
            nodeLabel.relativePosition = position;
            nodeLabel.text = "No editable intersections found.";
        }

        Vector3 calculateBoxPosition()
        {
            return new Vector3(-0.5f, 0.5f);
        }
    }
}
