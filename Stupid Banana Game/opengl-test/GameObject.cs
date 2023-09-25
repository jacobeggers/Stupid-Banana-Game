using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class GameObject
    {
        private float x;
        private float y;
        private float width;
        private float height;
        private bool isAffectedByGravity;

        private float previousX;
        private float previousY;

        public GameObject(float x, float y, float width, float height, bool isAffectedByGravity)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.isAffectedByGravity = isAffectedByGravity;
        }

        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }
        public void setX(float x)
        {
            this.x = x;
        }
        public void setY(float y)
        {
            this.y = y;
        }
        public float getPreviousX()
        {
            return previousX;
        }
        public float getPreviousY()
        {
            return previousY;
        }
        public void changeX(float change, float deltaTime)
        {
            previousX = this.x;
            this.x += change * deltaTime;
        }
        public void changeY(float change, float deltaTime)
        {
            previousY = this.y;
            this.y += change * deltaTime;
        }
        public float getWidth()
        {
            return width;
        }
        public float getHeight()
        {
            return height;
        }
        public void setHeight(float height)
        {
            this.height = height;
        }
        public void setWidth(float width)
        {
            this.width = width;
        }
        public bool getGravityState()
        {
            return isAffectedByGravity;
        }
        public bool detectCollision(GameObject other)
        {
            if (this.x + this.width > other.getX() &&
                this.x < other.getX() + other.getWidth() &&
                this.y + this.height > other.getY() &&
                this.y < other.getY() + other.getHeight())
            {
                return true;
            }
            return false;
        }
    }
}
