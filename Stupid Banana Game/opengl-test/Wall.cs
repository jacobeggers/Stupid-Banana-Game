using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Wall : GameObject
    {
        private const string stringId = "wall";
        public Wall(float x, float y) : base(x, y, 75, 75, true) { }

        public char getCollisionSide(GameObject other)
        {
            if (base.detectCollision(other))
            {
                float object1CenterX = base.getX() + base.getWidth() / 2;
                float object1CenterY = base.getY() + base.getHeight() / 2;
                float object2CenterX = other.getX() + other.getWidth() / 2;
                float object2CenterY = other.getY() + other.getHeight() / 2;

                float yintNegative = object2CenterX + object2CenterY;
                float yintPositive = object2CenterY - object2CenterX;

                if (object1CenterY > -1 * object1CenterX + yintNegative && object1CenterY > 1 * object1CenterX + yintPositive)
                {
                    return 'N';
                }
                else if (object1CenterY < -1 * object1CenterX + yintNegative && object1CenterY > 1 * object1CenterX + yintPositive)
                {
                    return 'W';
                }
                else if (object1CenterY > -1 * object1CenterX + yintNegative && object1CenterY < 1 * object1CenterX + yintPositive)
                {
                    return 'E';
                }
                else if (object1CenterY < -1 * object1CenterX + yintNegative && object1CenterY < 1 * object1CenterX + yintPositive)
                {
                    return 'S';
                }
            }
            return '\0';
        }
        public string getStringId()
        {
            return stringId;
        }
    }
}
