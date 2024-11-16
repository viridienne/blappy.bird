using UnityEngine;

public static class CollisionHelper
{    public struct CollisionContext
    {
        public Vector2 PositionA;
        public Vector2 SizeA;
        public Vector2 PositionB;
        public Vector2 SizeB;
    }
    
    public static bool CheckAABB(CollisionContext context)
    {
        var posA = context.PositionA;
        var sizeA = context.SizeA;
        var posB = context.PositionB;
        var sizeB = context.SizeB;
        
        return (posA.x < posB.x + sizeB.x && posA.x + sizeA.x > posB.x &&
                posA.y < posB.y + sizeB.y && posA.y + sizeA.y > posB.y);
    }
}
