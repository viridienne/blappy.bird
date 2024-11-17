using UnityEngine;

namespace Utility
{
    public static class CollisionUtil
    {    public struct CollisionContext
        {
            public Vector2 PositionA;
            public Vector2 SizeA;
            public Vector2 PositionB;
            public Vector2 SizeB;
        }
    
        public static bool CheckAABB(CollisionContext context)
        {
            // var posA = context.PositionA;
            // var sizeA = context.SizeA;
            // var posB = context.PositionB;
            // var sizeB = context.SizeB;
            //
            // return posA.x < posB.x + sizeB.x && posA.x + sizeA.x > posB.x &&
            //        posA.y < posB.y + sizeB.y && posA.y + sizeA.y > posB.y;
        
            var centerA = context.PositionA;
            var sizeA = context.SizeA;
            var centerB = context.PositionB;
            var sizeB = context.SizeB;
        
            // Calculate top-left positions from the center
            Vector2 posA = centerA - sizeA / 2;
            Vector2 posB = centerB - sizeB / 2;

            // Perform the collision check with adjusted positions
            return (posA.x < posB.x + sizeB.x && posA.x + sizeA.x > posB.x &&
                    posA.y < posB.y + sizeB.y && posA.y + sizeA.y > posB.y);
        }
    }
}
