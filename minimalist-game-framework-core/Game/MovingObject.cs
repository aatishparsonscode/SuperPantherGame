using System;
using System.Collections.Generic;
using System.Text;

public class MovingObject
{
    public float x;
    public float y;
    Vector2 position;

    float xVel = 0;
    float yVel = 0;

    public readonly Boolean speedChanges = false;
    public float speed = 5;
    public readonly float speedMin;
    public readonly float speedMax;

    public Boolean stepsAreConstant;
    private int stepsTotal;
    private int stepsMoved = 0;

    public float floor;
    public ScrollingBackground background;
    public Boolean movingLeft = true;

    public Boolean isDead = false;

    public MovingObject(float initialX, float initialY, ScrollingBackground background, float speed, Boolean startMovingLeft = true, Boolean changeSpeed = false, float speedMin = 1, float speedMax = 10, Boolean stepsConstant = true, int steps = 50)
    {
        this.x = initialX;
        this.y = initialY;
        this.background = background;
        position = new Vector2(initialX, initialY);
        this.speed = speed;
        this.movingLeft = startMovingLeft;
        this.speedChanges = changeSpeed;
        this.speedMax = speedMax;
        this.speedMin = speedMin;
        this.stepsAreConstant = stepsConstant;
        this.stepsTotal = steps;
    }

    public float settleOnGround()
    {
        y = background.getFloor(x, y) - Block.blockSize;
        position.Y = y;
        return y;
    }

    public float moveBackAndForthStepsXCoord(float backgroundSpeed, Boolean backgroundMovingLeft)
    {
        float speedChange = backgroundSpeed;
        if (movingLeft)
        {
            //Console.WriteLine("OBJECT MOVING LEFT");
            if (!backgroundMovingLeft)
            {
                speedChange *= -1;
            }
            if (stepsMoved < stepsTotal && background.checkHittingBlockLeft(x, y) == false)
            {
                //Console.WriteLine("STEPS MOVED: " + stepsMoved);
                x -= (speed + speedChange);
                position.X = x;
                stepsMoved += 1;
                return x;
            }
            else
            {
                movingLeft = !movingLeft;
                stepsMoved = 0;
                return x;
            }
        }
        else
        {

            //Console.WriteLine("OBJECT MOVING RIGHT");
            if (backgroundMovingLeft)
            {
                speedChange *= -1;
            }
            if (stepsMoved < stepsTotal && background.checkHittingBlockRight(x, y) == false)
            {
                x += (speed + speedChange);
                position.X = x;
                stepsMoved += 1;
                return x;
            }
            else
            {
                movingLeft = !movingLeft;
                stepsMoved = 0;
                return x;
            }

        }
    }
    public float moveBackAndForthXCoord(float backgroundSpeed, Boolean backgroundMovingLeft)
    {
        if (stepsAreConstant)
        {
            //Console.WriteLine("STEPS CONSTANT");
            return moveBackAndForthStepsXCoord(backgroundSpeed, backgroundMovingLeft);
        }
        float speedChange = backgroundSpeed;
        if (movingLeft)
        {
            if (!backgroundMovingLeft)
            {
                speedChange *= -1;
            }
            if (background.checkHittingBlockLeft(x, y) == false)
            {
                //can move left
                x -= (speed + speedChange);
                position.X = x;
                return x;
            }
            else
            {
                movingLeft = !movingLeft;
                x += speed;
                position.X = x;
                return x;
            }
        }
        else
        {
            if (backgroundMovingLeft)
            {
                speedChange *= -1;
            }

            if (background.checkHittingBlockRight(x, y) == false)
            {
                //can move left
                x += (speed + speedChange);
                position.X = x;
                return x;
            }
            else
            {
                movingLeft = !movingLeft;
                x -= speed;
                position.X = x;
                return x;
            }
        }

    }

    public int topOrSideCollision(float marioPositionX, float marioPositionY, float enemyX, float enemyY)
    {
        if (marioPositionY <= enemyY)
        {
            Console.WriteLine("Player jumped on top of the object");
            return 1;
        }
        else
        {
            Console.WriteLine("Player moved in front of or behind the object");
            return 0;
        }
        //return 0;
    }

    public int topOrSide2(float playerX, float playerY)
    {
        if (playerY <= y - Block.blockSize)
        {
            return 0;
        }
        else if (playerY >= y + Block.blockSize)
        {
            return 1;
        }
        else if (playerX.Equals(x - Block.blockSize) || playerX.Equals(x + Block.blockSize))
        {
            return 2;
        }
        else
        {
            return 3;
        }
        //return 0;
    }

    //overall collision checker
    public Boolean wasThereACollision(float playerX, float playerY)
    {
        if ((playerX >= x - Block.blockSize && playerX <= x + Block.blockSize) && (playerY >= y - Block.blockSize && playerY <= y + Block.blockSize))
        {
            return true;
        }
        return false;
    }

    //checks if the player collided wth the top of the object
    public Boolean topCollisionChecker(float playerX, float playerY)
    {

        if ((playerX >= x - Block.blockSize * 2/3 && playerX <= x + Block.blockSize * 2/3) && (playerY >= y - Block.blockSize * 1.05 && playerY < y ))
        {
            
            //Console.WriteLine("JUMPED ON TOP ");
            //Console.WriteLine("USER COORDS: X: " + playerX + " Y: " + playerY);
            //Console.WriteLine("Shroom COORDS: X: " + x + " Y: " + y);
            return true;
        }
        //Console.WriteLine("FAIL ON TOP ");
        //Console.WriteLine("USER COORDS: X: " + playerX + " Y: " + playerY);
        //Console.WriteLine("Shroom COORDS: X: " + x + " Y: " + y);
        return false;
        //    if(playerY <= y - Block.blockSize)
        //  {
        //    return true;
        // }
        // return false;
    }

    //checks if the player collided with the bottom of the object
    public Boolean bottomCollisionChecker(float playerX, float playerY)
    {
        if ((playerX >= x - Block.blockSize && playerX <= x + Block.blockSize) && (playerY >= y - Block.blockSize && playerY <= y + Block.blockSize))
        {
            if (playerY >= y + Block.blockSize)
            {
                return true;
            }
            return false;
        }
        return false;
        //     if (playerY >= y + Block.blockSize)
        //   {
        //      return true;
        //   }
        // return false;
    }

    //checks if the player collided with the side of the obejct
    public Boolean sideCollisionChecker(float playerX, float playerY)
    {


        if ((playerX >= x - Block.blockSize / 2 && playerX <= x + Block.blockSize / 2) && (playerY >= y - Block.blockSize/2 && playerY <= y + Block.blockSize/2))

        {
            return true;
        }
        return false;
    }



}


public class Mushroom : MovingObject
{
    ResizableTexture mushroomTexRight = Engine.LoadResizableTexture("bowserRight.png", 0, 0, 0, 0);
    ResizableTexture mushroomTexLeft = Engine.LoadResizableTexture("bowserLeft.png", 0, 0, 0, 0);
    public Mushroom(float initialX, float initialY, ScrollingBackground background, float speed = 2, Boolean startMovingLeft = true, Boolean changeSpeed = false, float speedMin = 1, float speedMax = 4)
        : base(initialX, initialY, background, speed, startMovingLeft, changeSpeed, speedMin, speedMax)
    {
        settleOnGround();
        Engine.DrawResizableTexture(mushroomTexRight, new Bounds2(x, y, Block.blockSize, Block.blockSize));
    }

    public Boolean killedPlayer(float x, float y)
    {
        if (isDead)
        {
            return false;
        }
        return sideCollisionChecker(x, y);
    }

    public Boolean selfKilled(float x, float y)
    {
        if (this.isDead)
        {
            return true;
        }
        var isDead = topCollisionChecker(x, y);
        if (isDead)
        {
            this.isDead = true;
        }
        return isDead;
    }

    public void move(float backgroundSpeed, Boolean backgroundMovingLeft)
    {
        //too far out left or right
        if (this.x < -100)
        {
            this.movingLeft = true;
            x = background.xMax + 30;

            this.isDead = false;

            if (speedChanges)
            {
                var r = new Random();
                this.speed = r.Next((int)this.speedMin, (int)this.speedMax);
                Console.WriteLine("new speed: " + this.speed);
            }
        }
        else if (this.x > background.xMax)
        {
            this.isDead = false;
            this.movingLeft = true;
            var r = new Random();
            if(r.Next(0,2) == 1)
            {
                this.stepsAreConstant = !this.stepsAreConstant;
            }
            
        }
        if (this.isDead == false)
        {
            if (movingLeft)
            {
                Engine.DrawResizableTexture(mushroomTexLeft, new Bounds2(moveBackAndForthXCoord(backgroundSpeed, backgroundMovingLeft), y, Block.blockSize, Block.blockSize));
            }
            else
            {
                Engine.DrawResizableTexture(mushroomTexRight, new Bounds2(moveBackAndForthXCoord(backgroundSpeed, backgroundMovingLeft), y, Block.blockSize, Block.blockSize));
            }
            
        }
        else
        {
            moveBackAndForthXCoord(backgroundSpeed, backgroundMovingLeft);
        }

    }
}