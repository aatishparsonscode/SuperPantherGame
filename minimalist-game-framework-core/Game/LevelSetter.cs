
using System;
using System.Collections.Generic;
using System.Text;



class LevelSetter
{
    public LevelSetter()
    {

    }

}

class Level
{
    //platforms
    //enemies
    //speed
    /// <summary>
    //time
    /// </summary>
    public Mushroom[] mushrooms;
    public ScrollingBackground background;
    private Vector2 Resolution;

    private float speed;

    double score = 0;
    int coinsCollected = 0;
    int enemiesDefeated = 0;

    Boolean justKilledEnemy = false;

    private Random r = new Random();
    public Level(int amountOfGoombas, double blockMaxHeightDecimal, int amountOfFloatingPlatforms, Vector2 Resolution, string backgroundImagePath, float speed)
    {
        this.Resolution = Resolution;
        mushrooms = new Mushroom[amountOfGoombas];
        background = new ScrollingBackground(0, (int)Resolution.X, 0, (int)Resolution.Y, blockStructureHeight: blockMaxHeightDecimal, amountOfPlatforms: amountOfFloatingPlatforms, backgroundImage: backgroundImagePath);
        this.speed = speed;
        StartupLevel();
    }

    private void StartupLevel()
    {
        for (var i = 0; i < mushrooms.Length; i++)
        {
            if(this.speed > 3)
            {
                this.speed -= 2;
            }
            Boolean beTrueFalse = r.Next(0, 2) == 1;
            int minSpeed = (int)this.speed - 3;
            int speed = (int) this.speed;
            int maxSpeed = (int) this.speed + 1;
            mushrooms[i] = new Mushroom(r.Next((int) Resolution.X/2 + Block.blockSize * 3, (int)Resolution.X - 5), 0, background, startMovingLeft: false, changeSpeed: beTrueFalse, speedMin: minSpeed, speedMax: maxSpeed, speed: speed);
            mushrooms[i].move(0, true);
        }
        background.moveBackground(true, 0);
    }

    public void moveGame(float speed, Boolean facing)
    {
        
        for (var i = 0; i < mushrooms.Length; i++)
        {
            mushrooms[i].move(speed, facing);
        }
        background.moveBackground(facing, speed);
    }


    public void drawBackground()
    {
        background.drawBackground();
    }

    public float getFloor(float x, float y)
    {
        return background.getFloor(x, y);
    }

    public ScrollingBackground getBackgroundInstance()
    {
        return background;
    }

    public Boolean playerGotKilled(float x, float y)
    {
        justKilledEnemy = false;
        for (var i = 0; i < mushrooms.Length; i++)
        {
            var shroom = mushrooms[i];
            //Console.WriteLine("SHRROM STATUS DEAD: " + shroom.isDead);
            if (!shroom.isDead)
            {
                //Console.WriteLine("EVAL IF PLAER KILLED");
                if (shroom.killedPlayer(x, y))
                {
                    Console.WriteLine("killed player");
                    return true;
                }
                else if (shroom.selfKilled(x, y))
                {
                    //Console.WriteLine("SHRROM DEAD");
                    enemiesDefeated++;
                    justKilledEnemy = true;
                }
                
                
            }

        }
        return false;
    }

    public Boolean playerKilledEnemy()
    {
        return justKilledEnemy;
    }

    public void updateCoins(float x, float y)
    {
        if (background.checkHittingCoin(x, y))
        {
            coinsCollected++;
        }
    }

    public double calculateScore(double time, double distanceMoved)
    {
        return ScoreCalculator.calculateRawScore(time, distanceMoved, coinsCollected, enemiesDefeated);
    }

    public string getScoreString(double time, double distanceMoved)
    {
        return ScoreCalculator.getPrintableScore(time, distanceMoved, coinsCollected, enemiesDefeated);
    }
}


