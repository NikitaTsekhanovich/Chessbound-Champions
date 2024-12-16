using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameBoard
{
    public class FlexibleGridLayout : LayoutGroup
    {    
        public enum FitType
        {
            UNIFORM,
            WIDTH,
            HEIGHT,
            FIXEDROWS,
            FIXEDCOLUMNS
        }

        [Header("Flexible Grid")]
        public FitType fitType = FitType.UNIFORM;

        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.WIDTH || fitType == FitType.HEIGHT || fitType == FitType.UNIFORM)
            {
                switch (fitType)
                {
                    case FitType.WIDTH:
                        fitX = true;
                        fitY = false;
                        break;
                    case FitType.HEIGHT:
                        fitX = false;
                        fitY = true;
                        break;
                    case FitType.UNIFORM:
                        fitX = fitY = true;
                        break;
                }
            }

            if (fitType == FitType.WIDTH || fitType == FitType.FIXEDCOLUMNS)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if (fitType == FitType.HEIGHT || fitType == FitType.FIXEDROWS)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }
            

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns) * (columns - 1))
                - (padding.left / (float)columns) - (padding.right / (float)columns);
            float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * (rows - 1))
                - (padding.top / (float)rows) - (padding.bottom / (float)rows); ;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {

        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }
    }
}