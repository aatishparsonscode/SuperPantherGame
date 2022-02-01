using System;
using System.Collections.Generic;
using System.Threading;
class Game
{
    //basic starters
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(975, 600);
    float pantherFrameIndex = 0;
    static readonly float Framerate = 10;



    //importing all the fonts that we want throughout the game. These inclode the sizing and bold versions of the font. 
    public static readonly Font fontTitle = Engine.LoadFont("AGENCYR.ttf", 200);
    public static readonly Font fontMiddle = Engine.LoadFont("AGENCYR.ttf", 100);
    public static readonly Font fontSmall = Engine.LoadFont("AGENCYR.ttf", 20);
    public static readonly Font fontLevel = Engine.LoadFont("AGENCYB.ttf", 50);


    //mario
    //ResizableTexture marioR = Engine.LoadResizableTexture("tempChar.png", 0, 0, 0, 0);
    // ResizableTexture marioL = Engine.LoadResizableTexture("tempCharL.png", 0, 0, 0, 0);
    Texture marioR = Engine.LoadTexture("resizesquaresheet5.png");
    Texture marioL = Engine.LoadTexture("resizesquaresheetreverse.png");


    //setting variables to figure out positioning of the main character
    static Vector2 marioPosition;
    Bounds2 marioHitBox;
    Boolean gamestart = true;
    float x;
    float y;
    double distanceMoved = 0;

    float aniFrames = 1.0f;

    float xVel = 0;
    float yVel = 0;
    float speed = 5;
    float floor = Resolution.Y - Block.blockSize;

    Boolean jumping = false;
    int jumps = 0;
    Boolean facing = true;
    int hitBottomTriggers = 0;
    Boolean goingDownFromHit = false;


    //more textures added for the backgrounds. These are all the different level backgrounds
    ResizableTexture texBlueBackground = Engine.LoadResizableTexture("blueBackground.jpg", 0, 0, 0, 0);
    ResizableTexture texJungleBackground = Engine.LoadResizableTexture("jungleBackground.png", 0, 0, 500, 0);
    ResizableTexture texJungleBlurBackground = Engine.LoadResizableTexture("blurredJungleBackground.png", 0, 0, 500, 0);
    ResizableTexture texCreditScreen = Engine.LoadResizableTexture("credits.png", 0, 0, 0, 0);
    ResizableTexture texLevelSquare = Engine.LoadResizableTexture("levelSquare.png", 0, 0, 0, 0);

    //managing screens
    Boolean playing = false;
    Boolean levelScreen = false;
    Boolean titleScreen = true;
    Boolean scoreScreen = false;
    Boolean creditScreen = false;

    Boolean playerDead = false;

    //timer and scoring
    Timer timer = new Timer();

    Level level;
    int levelNum;

    String finalScore;
    
    //main game method
    public Game()
    {
        
    }

    //the method used for Mario movement
    public void MarioMovement()
    {
        //conditionals based of certain aspects of the game, such as when the player dies, they resume back at a certain block
        if (gamestart)
        {
            x = Resolution.X / 2;
            y = Block.blockSize;
            gamestart = false;
        }
        level.moveGame(0, true);

        if (Engine.GetKeyHeld(Key.D) || Engine.GetKeyDown(Key.D))
        {
            xVel = 0;
            facing = true;
            distanceMoved += speed;
            level.moveGame(speed, true);
            aniFrames = 4.0f;
        }
        if (Engine.GetKeyHeld(Key.A) || Engine.GetKeyDown(Key.A))
        {
            xVel = 0;
            facing = false;
            distanceMoved -= speed;
            level.moveGame(speed, false);
            aniFrames = 4.0f;
        }
        if (Engine.GetKeyUp(Key.A))//the character moves to the left when the A key is pressed
        {
            while (xVel < 0)
            {
                xVel += 0.25f;
            }
            aniFrames = 1.0f;
        }
        if (Engine.GetKeyUp(Key.D))//the character moves to the right when D is pressed
        {
            while (xVel > 0)
            {
                xVel -= 0.25f;
            }
            aniFrames = 1.0f;
        }
        if (Engine.GetKeyDown(Key.W) && jumps < 2)//when W is pressed the player is able to jump at a certain speed and come down at that speed
        {
            yVel = -10;

            jumps++;
        }

        y += yVel;
        x += xVel;
        yVel += 0.5f;

        if (y >= floor)
        {
            yVel = 0;
            jumps = 0;

            y = floor;
        }

        if (xVel < 0)
        {
            facing = false;
        }
        if (xVel > 0)
        {
            facing = true;
        }
    }
    public void Update()
    {
        //intro title screen with the words, background, and then buttons to start certain levels
        if (titleScreen)
        {
            //play 
            Engine.DrawResizableTexture(texJungleBlurBackground, new Bounds2(0, 0, Resolution.X, Resolution.Y));
            Engine.DrawString("Super Panther", new Vector2(Resolution.X / 2, Resolution.Y / 4), Color.White, fontTitle, alignment: TextAlignment.Center);
            Engine.DrawString("press P to start", new Vector2(Resolution.X / 2, 3 * Resolution.Y / 4), Color.White, fontSmall, alignment: TextAlignment.Center);

            if (Engine.GetKeyHeld(Key.P) || Engine.GetKeyDown(Key.P))
            {
                titleScreen = false;
                levelScreen = true;
            }
        }
        else if (levelScreen) //generates the screen that displays teh different levels for user to pick from. Arranged in a straight line
            //they show the 5 differnt levels

        {
            timer.resetTimer();
            distanceMoved = 0;
            Engine.DrawResizableTexture(texJungleBlurBackground, new Bounds2(0, 0, Resolution.X, Resolution.Y));
            
            //picking which level the player is at
            Engine.DrawString("Select a Level (keyboard)", new Vector2(Resolution.X / 2, Resolution.Y / 5), Color.White, fontMiddle, alignment: TextAlignment.Center);
            Engine.DrawString("W-A-S-D controls", new Vector2(Resolution.X / 2, Resolution.Y / 4 + 100), Color.White, fontLevel, alignment: TextAlignment.Center);

            

            for (var i = 1; i < 6; i++)
            {
                Engine.DrawResizableTexture(texLevelSquare, new Bounds2(Resolution.X * i / 7, Resolution.Y / 2, 150, 150));
                Engine.DrawString(i + "", new Vector2(Resolution.X * i / 7 + 80, Resolution.Y / 2 + 45), Color.Black, fontLevel, alignment: TextAlignment.Center);
                Engine.DrawString("High Score", new Vector2(Resolution.X * i / 7 + 80, Resolution.Y / 2 + 120), Color.White, fontSmall, alignment: TextAlignment.Center);
                var score = SaveHighScores.getScores()[i]; //will display the score of the user
                if (score == null)
                {
                    Engine.DrawString("None", new Vector2(Resolution.X * i / 7 + 80, Resolution.Y / 2 + 150), Color.White, fontSmall, alignment: TextAlignment.Center);
                }
                else
                {
                    Engine.DrawString(score, new Vector2(Resolution.X * i / 7 + 80, Resolution.Y / 2 + 150), Color.White, fontSmall, alignment: TextAlignment.Center);
                }
            }

            //selecting level- each level has a unique backgrounad and a differnt set of jumps and orders to them to follow. each level has its own features 
            //such as the speed being played at, the settings, and the
            if (Engine.GetKeyHeld(Key.NumRow1) || Engine.GetKeyDown(Key.NumRow1))// choosing level 1
            {
                levelNum = 1;
                this.speed = 3;
                //plain
                level = new Level(2, 0.3, 5, Resolution, "blueBackground.jpg", speed);
                levelScreen = false;
                playing = true;
            }
            else if (Engine.GetKeyHeld(Key.NumRow2) || Engine.GetKeyDown(Key.NumRow2))//choosing level 2
            {
                levelNum = 2;
                this.speed = 4;
                //dungeon
                level = new Level(2, 0.3, 5, Resolution, "cavebackground.png", speed);
                levelScreen = false;
                playing = true;
            }
            else if (Engine.GetKeyHeld(Key.NumRow3) || Engine.GetKeyDown(Key.NumRow3))//choosing level 3
            {
                levelNum = 3;
                this.speed = 5;
                //desert
                level = new Level(3, 0.5, 3, Resolution, "desertBackground.png", speed);
                levelScreen = false;
                playing = true;
            }
            else if (Engine.GetKeyHeld(Key.NumRow4) || Engine.GetKeyDown(Key.NumRow4))//choosing level 4
            {
                levelNum = 4;
                this.speed = 5;
                //old jungle
                level = new Level(1, 0.1, 7, Resolution, "firstjungle.png", speed);

                levelScreen = false;
                playing = true;
            }
            else if (Engine.GetKeyHeld(Key.NumRow5) || Engine.GetKeyDown(Key.NumRow5))//choosing level 5
            {
                levelNum = 5;
                //new jungle
                this.speed = 5;
                level = new Level(2, 0.3, 5, Resolution, "jungleBackground.png", speed);
                levelScreen = false;
                playing = true;
            }
        }
        else if (playing) //when the player is playing
        {
            level.drawBackground();
            level.moveGame(0, facing);
            //starting position
            marioPosition = new Vector2(x, y);
            marioHitBox = new Bounds2(x, y, 30, 38);

            //drawing mario depending on where he's facing
            TextureMirror pantherMirror = facing ? TextureMirror.Horizontal : TextureMirror.None;
   

            pantherFrameIndex = (pantherFrameIndex + Engine.TimeDelta * Framerate) % aniFrames;
            Bounds2 pantherFrameBounds = new Bounds2(((int)pantherFrameIndex) * 61, 0, 63, 60);
            Vector2 pantherDrawPos = new Vector2(marioPosition.X, marioPosition.Y);
            Engine.DrawTexture(marioL, pantherDrawPos, source: pantherFrameBounds, mirror: pantherMirror);

            floor = level.getFloor(x, y);
            
            MarioMovement();

            //when player dies, or the character runs into enemies, it will display the final score, as well as give the option to restart
            var playerIsDead = level.playerGotKilled(x, y);
            if (playerIsDead)
            {
                Engine.DrawString("Uh Oh...", new Vector2(Resolution.X / 2, 200), Color.Black, fontTitle, alignment: TextAlignment.Center);
                this.playerDead = true;
                playing = false;
                scoreScreen = true;
                //keeps track of the highest score
                SaveHighScores.saveScore(levelNum, level.getScoreString(timer.getTimeDouble(), distanceMoved));
                finalScore = level.getScoreString(timer.getTimeDouble(), distanceMoved);
            }
            if (level.playerKilledEnemy())
            {
                //Console.WriteLine("KILLED ENEMY");
                yVel = -20;
            }
            
           //background does not scoll, instead the objecs infront of it are the onse moving to make the apperance that it moving.
            ScrollingBackground background = level.getBackgroundInstance();
            //check for hitting right or left or down
            hitBottomTriggers--;
            Boolean hitBottomOfBricks = background.checkHittingBlockBottom(x, y);
            //brick placement
            if (hitBottomOfBricks == false && this.goingDownFromHit)
            {
                this.goingDownFromHit = false;
            }
            else
            {
                if (hitBottomOfBricks && this.goingDownFromHit == false)
                {
                    this.yVel = this.yVel * -1;
                    this.y += 10;
                    this.goingDownFromHit = true;
                    hitBottomTriggers = 2;
                }
                //jumping, and object manuver as brick is the main floor of the game
                else if (background.checkHittingBlockRight(x, y))
                {
                    level.moveGame(speed, !facing);
                }
                else if (background.checkHittingBlockLeft(x, y) && !facing)
                {
                    level.moveGame(speed * -1, facing);
                }
            }
            // adds to the coin count
            level.updateCoins(x, y);

            timer.add();
            //displaying the time and the score
            Engine.DrawString("Time: " + timer.getTime() + " seconds", new Vector2(50, 50), Color.AliceBlue, fontSmall, alignment: TextAlignment.Left);
            Engine.DrawString("Score: " + level.getScoreString(timer.getTimeDouble(), distanceMoved), new Vector2(50, 100), Color.AliceBlue, fontSmall, alignment: TextAlignment.Left);

        }
        else if (scoreScreen) //displays the score screen, with the game over sign, as well as the option to restart. 
        {
            Engine.DrawResizableTexture(texJungleBlurBackground, new Bounds2(0, 0, Resolution.X, Resolution.Y));
            Engine.DrawString("Game Over", new Vector2(Resolution.X / 2, Resolution.Y / 4), Color.White, fontTitle, alignment: TextAlignment.Center);
            Engine.DrawString("Score: " + finalScore + " | To beat: " + SaveHighScores.getScores()[levelNum], new Vector2(Resolution.X / 2, 2 * Resolution.Y / 4 + 75), Color.White, fontLevel, alignment: TextAlignment.Center);
            Engine.DrawString("press R to restart", new Vector2(Resolution.X / 2, 3 * Resolution.Y / 4), Color.White, fontSmall, alignment: TextAlignment.Center);
            Engine.DrawString("press C for credits", new Vector2(Resolution.X / 2, 4 * Resolution.Y / 5), Color.White, fontSmall, alignment: TextAlignment.Center);

            if (Engine.GetKeyHeld(Key.R) || Engine.GetKeyDown(Key.R))
            {
                levelScreen = true;
                scoreScreen = false;
            }

            if (Engine.GetKeyHeld(Key.C) || Engine.GetKeyDown(Key.C))
            {
                creditScreen = true; 
                scoreScreen = false;
            }
        }else if (creditScreen)
        {
            Engine.DrawResizableTexture(texCreditScreen, new Bounds2(0, 0, Resolution.X, Resolution.Y));
            Engine.DrawString("press R to restart", new Vector2(Resolution.X / 2, 5 * Resolution.Y / 6), Color.White, fontSmall, alignment: TextAlignment.Center);
            

            if (Engine.GetKeyHeld(Key.R) || Engine.GetKeyDown(Key.R))
            {
                titleScreen = true;
                scoreScreen = false;
            }
        }
    }
}
