using OpenTK.Graphics.ES11;
using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace opengl_test
{
    internal class Player : GameObject
    {
        private const string stringId = "player";

        private float momentumX = 0;
        private float momentumY = 0;

        private bool canJump = true;

        public Player(float x, float y) : base(x, y, 75, 75, true) { }

        public string getStringId()
        {
            return stringId;
        }

        public void changeX(float change, float momentumX, float deltaTime)
        {
            momentumX *= deltaTime;
            if (momentumX > 0)
            {
                if (this.momentumX < change)
                {
                    this.momentumX += momentumX;
                    base.changeX(this.momentumX, deltaTime);
                }
                else
                {
                    base.changeX(change, deltaTime);
                    this.momentumX = change;
                }
            } else if (momentumX < 0)
            {
                if (this.momentumX > change)
                {
                    this.momentumX += momentumX;
                    base.changeX(this.momentumX, deltaTime);
                }
                else
                {
                    this.momentumX = change;
                    base.changeX(change, deltaTime);
                }
            }
        }

        public void changeY(float change, float momentumY, float deltaTime)
        {
            momentumY *= deltaTime;
            if (momentumY > 0)
            {
                if (this.momentumY < change)
                {
                    this.momentumY += momentumY;
                    base.changeY(this.momentumY, deltaTime);
                }
                else
                {
                    this.momentumY = change;
                    base.changeY(change, deltaTime);
                }
            } else if (momentumY < 0)
            {
                if (this.momentumY > change)
                {
                    this.momentumY += momentumY;
                    base.changeY(this.momentumY, deltaTime);
                }
                else
                {
                    this.momentumY = change;
                    base.changeY(change, deltaTime);
                }
            }
        }

        public float getMomentumX()
        {
            return momentumX;
        }

        public float getMomentumY()
        {
            return momentumY;
        }

        public void setMomentumX(float momentumX)
        {
            this.momentumX = momentumX;
        }

        public void setMomentumY(float momentumY)
        {
            this.momentumY = momentumY;
        }

        public void setCanJump(bool canJump)
        {
            this.canJump = canJump;
        }

        public bool getCanJump()
        {
            return canJump;
        }

        // More precise collision detection (unfished perhaps could be used later)

        /*
        public char getCollisionSide(GameObject other)
        {
            
            float[] xPosPast = { base.getPreviousX(), base.getPreviousX(), base.getPreviousX() + base.getWidth(), base.getPreviousX() + getWidth() };
            float[] yPosPast = { base.getPreviousY(), base.getPreviousY() + base.getHeight(), base.getPreviousY() + base.getHeight(), base.getPreviousY() };

            float[] xPosCurrent = { base.getX(), base.getX(), base.getX() + base.getWidth(), base.getX() + getWidth() };
            float[] yPosCurrent = { base.getY(), base.getY() + base.getHeight(), base.getY() + base.getHeight(), base.getY() };

            float[] xPosOther = { other.getX(), other.getX(), other.getX() + other.getWidth(), other.getX() + getWidth() };
            float[] yPosOther = { other.getY(), other.getY() + other.getHeight(), other.getY() + other.getHeight(), other.getY() };

            for (int i = 0; i < xPosPast.Length; i++)
            {
                for (int j = 0; j < xPosOther.Length; j++)
                {
                    (float pointx1, float pointy1) = (xPosPast[i], yPosPast[i]);
                    (float pointx2, float pointy2) = (xPosCurrent[i], yPosCurrent[i]);
                    (float pointx3, float pointy3) = (xPosOther[j], yPosOther[j]);
                    (float pointx4, float pointy4) = (xPosOther[0], yPosOther[0]);

                    if (j != 3)
                    {
                        (pointx4, pointy4) = (xPosOther[j + 1], yPosOther[j + 1]);
                    }

                    float alpha = ((pointx1 - pointx3) * (pointy3 - pointy4) - (pointy1 - pointy3) * (pointx3 - pointx4)) /
                        ((pointx1 - pointx2) * (pointy3 - pointy4) - (pointy1 - pointy2) * (pointx3 - pointx4));

                    float beta = ((pointx1 - pointx3) * (pointy1- pointy2) - (pointy1 - pointy3) * (pointx1 - pointx2)) /
                        ((pointx1 - pointx2) * (pointy3 - pointy4) - (pointy1 - pointy2) * (pointx3 - pointx4));

                    char getSide()
                    {
                        float object1CenterX = base.getPreviousX() + base.getWidth() / 2;
                        float object1CenterY = base.getPreviousY() + base.getHeight() / 2;
                        float object2CenterX = other.getX() + other.getWidth() / 2;
                        float object2CenterY = other.getY() + other.getHeight() / 2;

                        float yintNegative = object2CenterX + object2CenterY;
                        float yintPositive = object2CenterY - object2CenterX;

                        if (object1CenterY > -1 * object1CenterX + yintNegative && object1CenterY > 1 * object1CenterX + yintPositive)
                        {

                            Console.WriteLine('N');
                            return 'N';
                        }
                        else if (object1CenterY < -1 * object1CenterX + yintNegative && object1CenterY > 1 * object1CenterX + yintPositive)
                        {
                            Console.WriteLine('W');
                            return 'W';
                        }
                        else if (object1CenterY > -1 * object1CenterX + yintNegative && object1CenterY < 1 * object1CenterX + yintPositive)
                        {

                            Console.WriteLine('E');
                            return 'E';
                        }
                        else if (object1CenterY < -1 * object1CenterX + yintNegative && object1CenterY < 1 * object1CenterX + yintPositive)
                        {
                            Console.WriteLine('S');
                            return 'S';
                        }
                        return '\0';
                    }

                    if (alpha > 0 && alpha < 1 && beta > 0 && beta < 1)
                    {
                        return getSide();
                    }
                    else
                    {
                        //Console.WriteLine(xPosOther[2] + " : " + yPosOther[2]);
                        
                    }
                }
            }
            return '\0';
        }
        */
    }
}
