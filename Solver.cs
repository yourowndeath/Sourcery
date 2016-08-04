using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sourcery
{
  public class Solver<TPathNode, TUserContext> : SpatialAStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
  {
    protected override Double Heuristic(PathNode inStart, PathNode inEnd)
    {
      return Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y);
    }

    protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
    {
      return Heuristic(inStart, inEnd);
    }

    public Solver(TPathNode[,] inGrid)
      : base(inGrid)
    {
    }
  }
}
