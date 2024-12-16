using GameControllers.GameBoard.CellTypes;

namespace GameControllers.Handlers
{
    public static class FigurePositionHandler
    {
        public static string GetPositionOnBoard(int x, int y)
        {
            return $"{CellDataTypes.HorizontalTypes.GetValue(y)}" + 
                   $"{8 - (int)CellDataTypes.VerticalTypes.GetValue(x)}";
        }
    }
}
