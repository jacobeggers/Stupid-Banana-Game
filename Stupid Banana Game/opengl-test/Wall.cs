﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Wall : GameObject
    {
        private const string stringId = "wall";

        private bool[] neighboringWalls= { false, false, false, false };

        public Wall(float x, float y) : base(x, y, 75, 75, true) { }

        public char getCollisionSide(GameObject other)
        {
            if (base.detectCollision(other))
            {
                float object1CenterX = base.getX() + base.getWidth() / 2;
                float object1CenterY = base.getY() + base.getHeight() / 2;
                float object2CenterX = other.getPreviousX() + other.getWidth() / 2;
                float object2CenterY = other.getPreviousY() + other.getHeight() / 2;

                float yintNegative = object1CenterY + (base.getHeight() / base.getWidth() * object1CenterX);
                float yintPositive = object1CenterY - (base.getHeight() / base.getWidth() * object1CenterX);

                if (object2CenterY > -1 * base.getHeight() / base.getWidth() * object2CenterX + yintNegative &&
                    object2CenterY > 1 * base.getHeight() / base.getWidth() * object2CenterX + yintPositive &&
                    neighboringWalls[0] == false)
                {
                    return 'N';
                }
                else if (object2CenterY < -1 * base.getHeight() / base.getWidth() * object2CenterX + yintNegative &&
                    object2CenterY > 1 * base.getHeight() / base.getWidth() * object2CenterX + yintPositive &&
                    neighboringWalls[2] == false)
                {
                    return 'W';
                }
                else if (object2CenterY > -1 * base.getHeight() / base.getWidth() * object2CenterX + yintNegative &&
                    object2CenterY < 1 * base.getHeight() / base.getWidth() * object2CenterX + yintPositive &&
                    neighboringWalls[3] == false)
                {
                    return 'E';
                }
                else if (object2CenterY < -1 * base.getHeight() / base.getWidth() * object2CenterX + yintNegative &&
                    object2CenterY < 1 * base.getHeight() / base.getWidth() * object2CenterX + yintPositive &&
                    neighboringWalls[1] == false)
                {
                    return 'S';
                }
            }
            return '\0';
        }

        public void setNeighboringWalls(bool value, char direction)
        {
            switch (direction)
            {
                case 'N':
                    neighboringWalls[0] = value;
                    break;
                case 'S':
                    neighboringWalls[1] = value;
                    break;
                case 'E':
                    neighboringWalls[2] = value;
                    break;
                case 'W':
                    neighboringWalls[3] = value;
                    break;
            }
        }

        public bool[] getNeighboringWalls()
        {
            return neighboringWalls;
        }

        public string getStringId()
        {
            return stringId;
        }
    }
}
