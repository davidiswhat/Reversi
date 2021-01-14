using GameAI.GamePlaying.Core;
using System.Collections.Generic;

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        // TODO: Methods go here
        public ComputerMove Run(int color, Board board, int lookAheadDepth)
        {
            //throw new System.NotImplementedException();
            ComputerMove bestMove = null; //row, column, rank
            //int bestRank = -2147483648;
            int bestRank = 0;
            if (color == -1)
                bestRank = int.MaxValue; 
            else
                bestRank = int.MinValue;
            int bestRow = 0;
            int bestCol = 0;
            List<ComputerMove> MoveList = new List<ComputerMove>();
            Board newState = new Board();

            newState.Copy(board);
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    if (newState.IsValidMove(color, row, col))
                        MoveList.Add(new ComputerMove(row, col));

            foreach (ComputerMove cm in MoveList)
            {
                newState.Copy(board);
                newState.MakeMove(color, cm.row, cm.column);
                if (newState.IsTerminalState() || lookAheadDepth == 0)
                    cm.rank = Evaluate(newState);
                else
                {
                    if (newState.HasAnyValidMove(color * -1))
                        cm.rank = Run(color * -1, newState, (lookAheadDepth - 1)).rank;
                    else
                        cm.rank = Run(color, newState, (lookAheadDepth - 1)).rank;
                }
                if (color == -1)
                {
                    if (bestRank > cm.rank)
                    {
                        bestRank = cm.rank;
                        bestRow = cm.row;
                        bestCol = cm.column;
                    }
                }
                else
                {
                    if (bestRank < cm.rank)
                    {
                        bestRank = cm.rank;
                        bestRow = cm.row;
                        bestCol = cm.column;
                    }
                }
            }
            bestMove = new ComputerMove(bestRow, bestCol);
            bestMove.rank = bestRank;
            return bestMove;
        }
        private int Evaluate(Board board)
        {
            int heuristic = 0;
            Board newState = new Board();
            int color = 0;
            newState.Copy(board);
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    color = newState.GetTile(row, col);
                    if(color != 0)
                    {
                        if ((row == 0 && col == 0) || (row == 7 && col == 7) || (row == 0 && col == 7) || (row == 7 && col == 0))
                            color = color * 100;
                        else if (row == 0 || row == 7 || col == 0 || col == 7)
                            color = color * 10;
                    }
                    heuristic += color;
                }
            if (newState.IsTerminalState())
            {
                if (heuristic > 0)
                    heuristic = heuristic + 10000;
                else if (heuristic < 0)
                    heuristic = heuristic - 10000;
            }
            return heuristic;
                        
        }
    }
}
