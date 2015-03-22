/*
 * LaneUIToggle.cs - Activates/Deactivates the segment selector tool.
 * 
 */
using UnityEngine;
using ColossalFramework.UI;

namespace LaneChanger
{
    public class LaneUIToggle : UIButton
    {
        public bool laneSelectEnabled = false;
        GameObject laneSelector;

        public override bool canFocus
        {
            get
            {
                return false;
            }
        }

        public override void Start()
        {
            base.Start();
            this.text = "Lanes";
            this.transformPosition = new Vector3(-1.65f, 0.96f);
            this.normalBgSprite = "ButtonMenu";
            this.disabledBgSprite = "ButtonMenuDisabled";
            this.hoveredBgSprite = "ButtonMenuHovered";
            this.pressedBgSprite = "ButtonMenuPressed";
            this.textColor = new Color32(255, 255, 255, 255);
            this.disabledTextColor = new Color32(7, 7, 7, 255);
            this.hoveredTextColor = new Color32(7, 132, 255, 255);
            this.pressedTextColor = new Color32(30, 30, 44, 255);
            this.playAudioEvents = true;
            this.eventClick += ButtonClick;
            this.width = 60;
            this.height = 30;
        }

        public void BeginLaneSelect()
        {
            laneSelector = new GameObject("SegmentSelector");
            SegmentSelector segmentSelector = laneSelector.AddComponent<SegmentSelector>();
            segmentSelector.button = this;
            this.textColor = new Color32(0, 255, 0, 255);
            laneSelectEnabled = true;
        }

        public void CancelLaneSelect()
        {
            ToolController toolController = GameObject.FindObjectOfType<ToolController>();
            toolController.CurrentTool = toolController.GetComponent<DefaultTool>();

        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (laneSelectEnabled)
                CancelLaneSelect();
            else
                BeginLaneSelect();
        } 
    }
}
