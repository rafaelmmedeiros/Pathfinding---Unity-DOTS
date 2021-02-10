using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

// This is based on classes of Pathfind.
// Leaning the bests ways.

public class Pathfinding : MonoBehaviour {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private void FindPath(int2 startPositon, int2 endPosition) {
        int2 gridSize = new int2(4, 4);

        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; x < gridSize.y; y++) {
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.index = CalculateIndex(x, y, gridSize.x);

                pathNode.gCost = int.MaxValue;
                pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                pathNode.CalculateFCost();

                pathNode.isWalkable = true;
                pathNode.cameFromNodeIndex = -1;

                pathNodeArray[pathNode.index] = pathNode;
            }
        }

        pathNodeArray.Dispose();
    }

    private int CalculateIndex(int x, int y, int gridWidth) {
        return x + y * gridWidth;
    }

    private int CalculateDistanceCost(int2 aPosition, int2 bPosition) {
        int xDistance = math.abs(aPosition.x - bPosition.x);
        int yDistance = math.abs(aPosition.y - bPosition.y);
        int remaining = math.abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // It´s a pure struct using values types, perfect for performance.
    private struct PathNode {
        public int x;
        public int y;
        public int index;
        public int gCost;
        public int hCost;
        public int fCost;
        public bool isWalkable;
        public int cameFromNodeIndex;
        public void CalculateFCost() {
            fCost = gCost + hCost;
        }
    }
}
