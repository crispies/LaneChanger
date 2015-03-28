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
        public ushort segmentId;
        //UILabel segmentLabel;

        public override void Start()
        {
            this.backgroundSprite = "GenericPanel";
            this.color = new Color32(180, 180, 180, 240);
        }   

        void changeLaneFlags(uint laneId, ushort newFlags)
        {

        }

        public void changeSegment(NetSegment segment, ushort segmentId)
        {
            currentSegment = segment;
            this.segmentId = segmentId;
            DrawSegmentInfoBox();
        }

        private void DrawEnableButton()
        {
            UIButton enableToggle = this.AddUIComponent<UIButton>();
            enableToggle.relativePosition = new Vector3(5f, 15f);
            enableToggle.width = 150f;
            enableToggle.height = 30f;
            enableToggle.normalBgSprite = "ButtonMenu";
            enableToggle.disabledBgSprite = "ButtonMenuDisabled";
            enableToggle.hoveredBgSprite = "ButtonMenuHovered";
            enableToggle.pressedBgSprite = "ButtonMenuPressed";
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            if (pathManager.laneChangerSegments[segmentId] == null)
                enableToggle.text = "Enable Lane Control";
            else
                enableToggle.text = "Disable Lane Control";
            enableToggle.eventClick += ToggleLaneControl;
        }

        private void DrawSegmentInfoBox() 
        {
            NetManager netManager = Singleton<NetManager>.instance;
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            this.width = 250f;
            this.height = 60f;
            this.transformPosition = calculateBoxPosition();
            
            //ushort startNodeID = currentSegment.m_startNode;
            //ushort endNodeID = currentSegment.m_endNode;
            //NetNode startNode = netManager.m_nodes.m_buffer[startNodeID];
            //NetNode endNode = netManager.m_nodes.m_buffer[endNodeID];
            UILabel boxLabel = this.AddUIComponent<UILabel>();
            boxLabel.relativePosition = new Vector3(1f, 1f);
            boxLabel.text = "Segment #" + this.segmentId;
            DrawEnableButton();
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

        void ToggleLaneControl(UIComponent component, UIMouseEventParameter eventParam)
        {
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            UIButton button = (UIButton)component;
            if (pathManager.laneChangerSegments[segmentId] == null)
            {
                pathManager.laneChangerSegments[segmentId] = new LaneChangerSegment();
                AddNodeConnections(NetManager.instance.m_nodes.m_buffer[currentSegment.m_startNode]);
                AddNodeConnections(NetManager.instance.m_nodes.m_buffer[currentSegment.m_endNode]);
                button.text = "Disable Lane Control";
            }
            else
            {
                pathManager.laneChangerSegments[segmentId] = null;
                NetManager.instance.UpdateSegment(segmentId);
                button.text = "Enable Lane Control";
            }
        }

        void AddNodeConnections(NetNode node)
        {
            LaneChangerPathManager pathManager = (LaneChangerPathManager)PathManager.instance;
            for (int i = 0; i < node.CountSegments(); i++)
            {
                ushort seg = node.GetSegment(i);
                if (seg != this.segmentId)
                    pathManager.laneChangerSegments[segmentId].AddPermittedConnection(seg);
            }
        }

        Vector3 calculateBoxPosition()
        {
            return new Vector3(-1.43f, 0.97f);
        }
    }
}
