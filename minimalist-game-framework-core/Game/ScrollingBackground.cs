using System;
using System.Collections.Generic;
using System.Text;

public class ScrollingBackground
{
    PictureBackground background;
    Cloud cloud1;
    Cloud cloud2;
    Cloud cloud3;
    Tree tree1;
    Tree tree2;
    Tree tree3;  
  

    BlockStructure blocks1;

    //make this appear at the start
    BlockStructure blocks2;
    BottomBlocks bottomBlocks;

    FloatingBlockStructure floatingBlocks1;
    FloatingBlockStructure floatingBlocks2;

    FloatingBlockStructure[] floatingBlockPlatforms;

    public int yMax;
    public int xMax;

    
       
    public ScrollingBackground(int xLeft, int xRight, int yMin, int yMax, double blockStructureHeight = 0.6, double blockStructureWalkLength = 0.3, int amountOfPlatforms = 3, string backgroundImage = "jungleBackground.png")
    {
        this.yMax = yMax;
        this.xMax = xRight;
        var r = new Random();

        background = new PictureBackground(xMax, yMax, backgroundImage, 500);

        
       

        blocks1 = new BlockStructure((int)(yMax * blockStructureHeight) / Block.blockSize, (int)(xRight * blockStructureWalkLength) / Block.blockSize, false, xRight, yMax);
        //blocks2 = new BlockStructure(3, 4, false, xRight, yMax);

        bottomBlocks = new BottomBlocks(xRight, yMax);
        //chnage to params having y max and then blocks from bottom as numbers so you can multiply with random
        floatingBlocks1 = new FloatingBlockStructure(4, yMax - Block.blockSize * 3, 5, xRight);
        floatingBlocks2 = new FloatingBlockStructure(4, yMax - Block.blockSize * 5, 2, xRight);

        floatingBlockPlatforms = new FloatingBlockStructure[amountOfPlatforms];

        for (var i = 0; i < amountOfPlatforms; i++)
        {
            floatingBlockPlatforms[i] = new FloatingBlockStructure(4, yMax - Block.blockSize * 3, i + 1, xRight);
        }

        cloud1 = new Cloud(r.Next(xLeft, xRight/3 - 50), r.Next(yMin, yMax / 4), xRight);
        cloud2 = new Cloud(r.Next(xLeft + xRight/3, xRight *2/3 - 50), r.Next(yMin, yMax / 4), xRight);
        cloud3 = new Cloud(r.Next(xRight * 2/3, xRight), r.Next(yMin, yMax / 4), xRight);

        tree1 = new Tree(r.Next(xLeft, xRight / 3), 550, xRight);
        tree2 = new Tree(r.Next(xRight /3, xRight * 2/3), 550, xRight);
        tree3 = new Tree(r.Next(xRight * 2 / 3, xRight), 550, xRight);

        if(backgroundImage.Equals("jungleBackground.png") == false && backgroundImage.Equals("firstjungle.png") == false)
        {
            tree1.isVisible = false;
            tree2.isVisible = false;
            tree3.isVisible = false;
        }

        if (backgroundImage.Equals("cavebackground.png") || backgroundImage.Equals("firstjungle.png"))
        {
            cloud1.isVisible = false;
            cloud2.isVisible = false;
            cloud3.isVisible = false;
        }
       

    }

    public void moveBackground(Boolean movingRight, float distance)
    {


        

        blocks1.move(movingRight, distance);
        //blocks2.move(movingRight, distance);
        bottomBlocks.move(movingRight, distance);

        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            floatingBlockPlatforms[i].move(movingRight, distance);

        }
        cloud1.move(movingRight, distance);
        cloud2.move(movingRight, distance);
        cloud3.move(movingRight, distance);

        tree1.move(movingRight, distance);
        tree2.move(movingRight, distance);
        tree3.move(movingRight, distance);


    }

    public void drawBackground()
    {
        background.drawStill();
    }

    public float getFloor(float xCoord, float yCoord)
    {
        //Console.WriteLine("FLOOR CHECK Y: " + yCoord);
        Block[,] structureBlocks2D = blocks1.getStructureBlocks();
        Block[] bottomBlock = bottomBlocks.getBlocks();


        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            for (var f = 0; f < floatingBlocks.Length; f++)
            {
                if (floatingBlocks[f].isVisible && floatingBlocks[f].onTop(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + bottomBlock[bottomB].yCoord);
                    return floatingBlocks[f].topYCoordinate();
                }
            }
        }

        for (var row = 0; row < structureBlocks2D.GetLength(0); row++)
        {
            for (var col = 0; col < structureBlocks2D.GetLength(1); col++)
            {
                if (structureBlocks2D[row, col].isVisible && structureBlocks2D[row, col].onTop(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + structureBlocks2D[row, col].yCoord);
                    return structureBlocks2D[row, col].topYCoordinate();
                }
            }
        }

        for (var bottomB = 0; bottomB < bottomBlock.Length; bottomB++)
        {
            //Console.WriteLine(bottomBlock[bottomB].xCoord + " " + bottomBlock[bottomB].yCoord + " " + xCoord + " " + yCoord);
            if (bottomBlock[bottomB].isVisible && bottomBlock[bottomB].onTop(xCoord, yCoord))
            {
                //Console.WriteLine("Match Y: " + bottomBlock[bottomB].yCoord);
                return bottomBlock[bottomB].topYCoordinate();
            }
        }
        return yMax - Block.blockSize;

    }

    public Boolean checkHittingBlockBottom(float xCoord, float yCoord)
    {


        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            for (var f = 0; f < floatingBlocks.Length; f++)
            {
                if (floatingBlocks[f].onBottom(xCoord, yCoord))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Boolean checkHittingCoin(float xCoord, float yCoord)
    {


        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            for (var f = 0; f < floatingBlocks.Length; f++)
            {
                if (floatingBlocks[f].coinOnTop.onBottom(xCoord, yCoord) || floatingBlocks[f].coinOnTop.onTop(xCoord, yCoord) || floatingBlocks[f].coinOnTop.onRight(xCoord, yCoord) || floatingBlocks[f].coinOnTop.onLeft(xCoord, yCoord))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public Boolean checkHittingBlockLeft(float xCoord, float yCoord)
    {
        //Console.WriteLine("FLOOR CHECK Y: " + yCoord);
        Block[,] structureBlocks2D = blocks1.getStructureBlocks();


        for (var row = 0; row < structureBlocks2D.GetLength(0); row++)
        {
            for (var col = 0; col < structureBlocks2D.GetLength(1); col++)
            {
                if (structureBlocks2D[row, col].isVisible && structureBlocks2D[row, col].onRight(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + structureBlocks2D[row, col].yCoord);
                    return true;
                }
            }
        }

        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            for (var f = 0; f < floatingBlocks.Length; f++)
            {
                if (floatingBlocks[f].onRight(xCoord, yCoord))
                {
                    return true;
                }
            }
        }
        return false;

    }

    public Boolean checkHittingBlockRight(float xCoord, float yCoord)
    {
        //Console.WriteLine("FLOOR CHECK Y: " + yCoord);
        Block[,] structureBlocks2D = blocks1.getStructureBlocks();


        for (var row = 0; row < structureBlocks2D.GetLength(0); row++)
        {
            for (var col = 0; col < structureBlocks2D.GetLength(1); col++)
            {
                if (structureBlocks2D[row, col].isVisible && structureBlocks2D[row, col].onLeft(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + structureBlocks2D[row, col].yCoord);
                    return true;
                }
            }
        }


        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            for (var f = 0; f < floatingBlocks.Length; f++)
            {
                if (floatingBlocks[f].onLeft(xCoord, yCoord))
                {
                    return true;
                }
            }
        }


        return false;
    }

    public float getHitPositionRightX(float xCoord, float yCoord)
    {
        //Console.WriteLine("FLOOR CHECK Y: " + yCoord);
        Block[,] structureBlocks2D = blocks1.getStructureBlocks();


        for (var row = 0; row < structureBlocks2D.GetLength(0); row++)
        {
            for (var col = 0; col < structureBlocks2D.GetLength(1); col++)
            {
                if (structureBlocks2D[row, col].isVisible && structureBlocks2D[row, col].onRight(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + structureBlocks2D[row, col].xCoord +" ROW: " + row + " COL: " + col);
                    return structureBlocks2D[row, col].rightXBufferCoordinate();
                }
            }
        }

        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            if (floatingBlocks[floatingBlocks.Length - 1].onRight(xCoord, yCoord))
            {
                return floatingBlocks[floatingBlocks.Length - 1].rightXBufferCoordinate();
            }
        }
        return xCoord;

    }

    public float getHitPositionLeftX(float xCoord, float yCoord)
    {
        //Console.WriteLine("FLOOR CHECK Y: " + yCoord);
        Block[,] structureBlocks2D = blocks1.getStructureBlocks();


        for (var row = 0; row < structureBlocks2D.GetLength(0); row++)
        {
            for (var col = 0; col < structureBlocks2D.GetLength(1); col++)
            {
                if (structureBlocks2D[row, col].isVisible && structureBlocks2D[row, col].onLeft(xCoord, yCoord))
                {
                    //Console.WriteLine("Match Y: " + structureBlocks2D[row, col].yCoord);
                    return structureBlocks2D[row, col].leftXBufferCoordinate();
                }
            }
        }

        for (var i = 0; i < floatingBlockPlatforms.Length; i++)
        {
            Block[] floatingBlocks = floatingBlockPlatforms[i].getBlocks();
            if (floatingBlocks[floatingBlocks.Length - 1].onLeft(xCoord, yCoord))
            {
                return floatingBlocks[floatingBlocks.Length - 1].leftXBufferCoordinate();
            }
        }

        return xCoord;

    }
}

class PictureBackground
{
    float xCoord1;
    float xCoord2;
    float yCoord1;
    float yCoord2;

    ResizableTexture texBackground;
    int width;
    int height;
    public PictureBackground(int width, int height, string imagePath, int bottomOffset)
    {
        if (imagePath.Equals("blueBackground.jpg"))
        {
            bottomOffset = 0;
        }
        texBackground = Engine.LoadResizableTexture(imagePath, 0, 0, bottomOffset, 0);
        Engine.DrawResizableTexture(texBackground, new Bounds2(0, 0, width, height));
        Engine.DrawResizableTexture(texBackground, new Bounds2(width, 0, width, height));
        this.height = height;
        this.width = width;
        xCoord1 = 0;
        xCoord2 = width;
        yCoord1 = 0;
        yCoord2 = 0;

    }
    public void move(Boolean right, float distance)
    {
        if (right)
        {
            xCoord1 -= distance;
            xCoord2 -= distance;
        }
        else
        {
            xCoord1 += distance;
            xCoord2 += distance;
        }
        Engine.DrawResizableTexture(texBackground, new Bounds2(xCoord1, yCoord1, width, height));
        Engine.DrawResizableTexture(texBackground, new Bounds2(xCoord2, yCoord2, width, height));
    }

    public void drawStill()
    {
        Engine.DrawResizableTexture(texBackground, new Bounds2(xCoord1, yCoord1, width, height));
    }
}
class BottomBlocks
{
    Block[] bottomBlocks;
    int width;
    int height;
    public BottomBlocks(int width, int height)
    {
        this.width = width;
        this.height = height;
        bottomBlocks = new Block[(int)width / Block.blockSize + 20];
        int xPos = width;
        for (var i = 0; i < bottomBlocks.GetLength(0); i++)
        {
            bottomBlocks[i] = new Block(xPos, height - Block.blockSize, width);
            bottomBlocks[i].isVisible = true;
            xPos -= Block.blockSize - 10;
            //Console.WriteLine(xPos + " " + width + " " + bottomBlocks[i].isVisible);
        }
    }

    public Block[] getBlocks()
    {
        return bottomBlocks;
    }
    public void move(Boolean movingRight, float distance)
    {
        for (var i = 0; i < bottomBlocks.Length; i++)
        {

            bottomBlocks[i].move(movingRight, distance);

            if (bottomBlocks[i].xCoord < 0 - Block.blockSize * 10)
            {
                bottomBlocks[i].setCoordinates(width, height - Block.blockSize);

            }
        }
    }
}

class FloatingBlockStructure
{
    Block[] structureShape;
    int blockSize = Block.blockSize;
    int walkLength;
    int yBottom;
    int yBlocksUp;
    int xRightLimit;

    public FloatingBlockStructure(int walkLength, int yBottom, int yBlocksUp, int xRightLimit)
    {
        this.walkLength = walkLength;
        this.yBottom = yBottom;
        this.yBlocksUp = yBlocksUp;
        this.xRightLimit = xRightLimit;
        structureShape = new Block[walkLength];
        createStructure();
    }

    public Block[] getBlocks()
    {
        return structureShape;
    }

    public void createStructure()
    {
        var r = new Random();
        for (var i = 0; i < structureShape.Length; i++)
        {
            Boolean breakable = false;
            if (r.Next(0, 10) > 7)
            {
                breakable = true;
            }
            structureShape[i] = new Block(xRightLimit + blockSize * (i + 1), yBottom, yBottom, breakable: breakable);
            structureShape[i].isVisible = true;
        }
    }



    public void move(Boolean right, float distance)
    {
        var r = new Random();
        var yPos = yBottom - r.Next(1, yBlocksUp + 1) * blockSize;
        //Console.WriteLine("YPOS: " + yPos);

        var offset = r.Next(0, 3);

        if (structureShape[walkLength - 1].xCoord < 0 - Block.blockSize)
        {
            for (var i = 0; i < structureShape.Length; i++)
            {
                structureShape[i].setCoordinates(xRightLimit + Block.blockSize * (i + 1) + Block.blockSize * offset, yPos);
                if (r.Next(0, 5) > 3)
                {
                    structureShape[i].breakable = true;
                }
                else
                {
                    structureShape[i].breakable = false;
                }
                structureShape[i].isVisible = true;
            }
        }
        else
        {
            for (var i = 0; i < structureShape.Length; i++)
            {
                structureShape[i].move(right, distance);
            }
        }

    }

}
class BlockStructure
{
    Block[,] structureShape;
    Boolean cliffDrop = false;
    double xRightLimit;
    int blockSize = Block.blockSize;
    int walkLength;
    int height;
    double yBottom;
    int maxWalkLength;
    int maxHeight;
    //3,5,true
    public BlockStructure(int maxHeight, int maxWalkLength, Boolean cliffDrop, double xRightLimit, double yBottom)
    {
        this.height = maxHeight;
        this.walkLength = maxWalkLength + 2;
        this.maxHeight = maxHeight;
        this.maxWalkLength = maxWalkLength + 2;
        this.cliffDrop = cliffDrop;
        this.xRightLimit = xRightLimit;
        this.yBottom = yBottom;
        createStructure();
        assignVisibility();
    }

    public Block[,] getStructureBlocks()
    {
        return structureShape;
    }
    public void createStructure()
    {

        //walklength is how many blocks at the top of a structure
        int add = 0;
        if (!cliffDrop)
        {
            add = height;
        }

        structureShape = new Block[height, height + walkLength + add];

        int rightX = (int)xRightLimit;
        int bottomY = (int)yBottom - blockSize;
        int xPos = rightX - (blockSize * structureShape.GetLength(0));
        int yPos = bottomY - (blockSize * height);

        for (var row = 0; row < structureShape.GetLength(0); row++)
        {
            xPos = (int)xRightLimit;

            for (var col = 0; col < structureShape.GetLength(1); col++)
            {
                //Console.WriteLine("ROW: " + row + " COL: " + col + " xPOS: " + xPos + " yPOS: " + yPos);
                structureShape[row, col] = new Block(xPos, yPos, (int)xRightLimit);
                xPos += blockSize;
            }

            yPos += blockSize;
        }



    }

    public void assignVisibility()
    {
        int rightX = (int)xRightLimit;
        int bottomY = (int)yBottom - blockSize;
        int xPos = rightX - (blockSize * structureShape.GetLength(0));
        int yPos = bottomY - (blockSize * height);

        for (var row = 0; row < structureShape.GetLength(0); row++)
        {
            xPos = (int)xRightLimit;

            for (var col = 0; col < structureShape.GetLength(1); col++)
            {
                //Console.WriteLine("ROW: " + row + " COL: " + col + " xPOS: " + xPos + " yPOS: " + yPos);
                structureShape[row, col].setCoordinates(xPos, yPos);
                structureShape[row, col].isVisible = false;
                xPos += blockSize;
            }

            yPos += blockSize;
        }

        int blocksInRow = 0;
        xPos = (int)xRightLimit;
        yPos = (int)yBottom - blockSize;
        for (var row2 = height - 1; row2 >= 0; row2--)
        {
            int startCol = height - 1 - row2;

            if (!cliffDrop)
            {
                blocksInRow = walkLength + row2;
            }
            else
            {
                blocksInRow = walkLength;
            }
            //Console.WriteLine("ROW: " + row + " STARTCOL: " + startCol + " BLOCKSInROW: " + blocksInRow);
            //xPos = (int)xRightLimit + startCol * blockSize;
            for (var col2 = startCol; col2 < blocksInRow; col2++)
            {
                //Console.WriteLine("row: " + row2 + " col: " + col2 + " blocksInRow: " + blocksInRow); 

                //structureShape[row, col] = new Block(xPos, yPos, (int)xRightLimit);

                structureShape[row2, col2].isVisible = true;
                //Console.WriteLine(structureShape[row2, col2].xCoord);
                //xPos += blockSize;
            }

            //yPos -= blockSize;
        }
    }
    public void move(Boolean right, float distance)
    {
        float xFurthestLeft = 5;
        for (int row = 0; row < structureShape.GetLength(0); row++)
        {
            for (int col = 0; col < structureShape.GetLength(1); col++)
            {

                if (structureShape[row, col] != null && structureShape[row, col].isVisible)
                {
                    structureShape[row, col].move(right, distance);
                    xFurthestLeft = structureShape[row, col].xCoord;
                }


            }
        }
        if (xFurthestLeft < 0 - blockSize)
        {
            var r = new Random();

            this.height = r.Next(1, maxHeight);
            int cliff = r.Next(0, 2);
            if (cliff == 0)
            {
                this.cliffDrop = true;
            }
            else
            {
                this.cliffDrop = false;
            }
            this.walkLength = r.Next(1, maxWalkLength);
            assignVisibility();
        }
    }
}

class Block
{
    public float xCoord;
    public float yCoord;
    float xRightLim;
    public Boolean isVisible = false;
    ResizableTexture blockTex = Engine.LoadResizableTexture("Brick.png", 0, 0, 0, 0);
    ResizableTexture blockBreaksTex = Engine.LoadResizableTexture("MossyBrick.png", 0, 0, 0, 0);
    public static readonly int blockSize = 50;
    private Boolean hasHitTop = false;
    public Boolean breakable = false;
    public Boolean spawnsCoin = false;
    public Boolean spawnedCoin = false;
    public Coin coinOnTop;
    public Block(float xStart, float yStart, float xRightLimit, Boolean breakable = false)
    {
        xCoord = xStart;
        yCoord = yStart;
        xRightLim = xRightLimit;
        this.breakable = breakable;
        coinOnTop = new Coin(xStart, yStart - Block.blockSize * 2, xRightLimit);
        coinOnTop.isVisible = false;
        if (breakable)
        {
            spawnsCoin = true;
            //when broken coin is available on top, in scrolling background make check for hitting coin -> use in game.cs
            //coin class is identical to block -> change visiblity when block broken
        }

        Engine.DrawResizableTexture(blockTex, new Bounds2(xCoord, yCoord, 50, 50));

    }

    //user on top
    public Boolean onTop(float X, float Y)
    {
        if (isVisible && X <= xCoord + blockSize - 5 && X >= xCoord - blockSize + 5 && Y < yCoord && Y > yCoord - blockSize)
        {
            
            return true;
        }
        return false;
    }

    //user on top
    public Boolean onBottom(float X, float Y)
    {
        if (isVisible && X <= xCoord + blockSize - 5 && X >= xCoord - blockSize + 5 && Y > yCoord && Y < yCoord + blockSize + 1)
        {
            Console.WriteLine("HIT BOTTOM, spawns coin?: " + spawnsCoin);
            if (breakable)
            {
                isVisible = false;
                spawnedCoin = true;
                if (spawnedCoin)
                {
                    coinOnTop.isVisible = true;
                }

            }
            return true;
        }
        return false;
    }

    //user on left
    public Boolean onLeft(float X, float Y)
    {
        float xRightFarBound = X + blockSize;
        if (isVisible && xCoord > X && xRightFarBound >= xCoord && Y < yCoord + blockSize - 1 && Y > yCoord - blockSize + 1)
        {
            return true;
        }
        return false;
    }

    //user on right
    public Boolean onRight(float X, float Y)
    {
        float xLeftFarBound = X - blockSize;

        //Console.WriteLine("X FAR LEFT BOUND: " + xLeftFarBound + " Xcoord: " + xCoord + " Y: " + Y + " ycoord: " + yCoord);
        if (isVisible && xCoord < X && xLeftFarBound <= xCoord && xCoord + blockSize >= X && Y < yCoord + blockSize - 1 && Y > yCoord - blockSize + 1)
        {
            return true;
        }
        return false;
    }


    public float topYCoordinate()
    {
        return yCoord - blockSize;
    }

    public float bottomYCoordinate()
    {
        return yCoord + blockSize;
    }

    public float rightXBufferCoordinate()
    {
        return xCoord - blockSize;
    }

    public float leftXBufferCoordinate()
    {
        return xCoord + blockSize;
    }

    public void setCoordinates(float x, float y)
    {
        xCoord = x;
        yCoord = y;
    }
    public void move(Boolean right, float distance)
    {

        if (right)
        {
            xCoord -= distance;
        }
        else
        {
            xCoord += distance;
        }
        if (xCoord <= 0 - Block.blockSize)
        {
            coinOnTop.isVisible = false;
        }
        coinOnTop.setCoordinates(xCoord, yCoord - Block.blockSize * 2);
        coinOnTop.draw();
        if (isVisible)
        {

            if (!breakable)
            {
                Engine.DrawResizableTexture(blockTex, new Bounds2(xCoord, yCoord, blockSize, blockSize));
            }
            else
            {
                Engine.DrawResizableTexture(blockBreaksTex, new Bounds2(xCoord, yCoord, blockSize, blockSize));
            }

        }

    }
}

class Coin
{
    public float xCoord;
    public float yCoord;
    float xRightLim;
    public Boolean isVisible = false;
    ResizableTexture coinTex = Engine.LoadResizableTexture("coinFront.png", 0, 0, 0, 0);
    public static readonly Font fontSmall = Engine.LoadFont("AGENCYR.ttf", 20);

    public static readonly int blockSize = 50;
    private Boolean hasHitTop = false;
    public Boolean breakable = false;
    public Boolean spawnsCoin = false;
    private Boolean hasBeenCollected = false;
    private int collectedCounter = 0;
    public Coin(float xStart, float yStart, float xRightLimit)
    {
        xCoord = xStart;
        yCoord = yStart;
        xRightLim = xRightLimit;
        this.breakable = true;

        Engine.DrawResizableTexture(coinTex, new Bounds2(xCoord, yCoord, 50, 50));
    }

    //user on top
    public Boolean onTop(float X, float Y)
    {
        if (isVisible && X <= xCoord + blockSize - 5 && X >= xCoord - blockSize + 5 && Y < yCoord && Y > yCoord - blockSize)
        {
            hasBeenCollected = true;
            return true;
        }
        return false;
    }

    //user on top
    public Boolean onBottom(float X, float Y)
    {
        if (isVisible && X <= xCoord + blockSize - 5 && X >= xCoord - blockSize + 5 && Y > yCoord && Y < yCoord + blockSize + 1)
        {
            hasBeenCollected = true;
            return true;
        }
        return false;
    }

    //user on left
    public Boolean onLeft(float X, float Y)
    {
        float xRightFarBound = X + blockSize;
        if (isVisible && xCoord > X && xRightFarBound >= xCoord && Y < yCoord + blockSize - 1 && Y > yCoord - blockSize + 1)
        {
            hasBeenCollected = true;
            return true;
        }
        return false;
    }

    //user on right
    public Boolean onRight(float X, float Y)
    {
        float xLeftFarBound = X - blockSize;

        //Console.WriteLine("X FAR LEFT BOUND: " + xLeftFarBound + " Xcoord: " + xCoord + " Y: " + Y + " ycoord: " + yCoord);
        if (isVisible && xCoord < X && xLeftFarBound <= xCoord && xCoord + blockSize >= X && Y < yCoord + blockSize && Y > yCoord - blockSize)
        {
            isVisible = false;
            Engine.DrawString("+200", new Vector2(X, Y), Color.White, fontSmall);
            return true;
        }
        return false;
    }

    public float topYCoordinate()
    {
        return yCoord - blockSize;
    }

    public float bottomYCoordinate()
    {
        return yCoord + blockSize;
    }

    public float rightXBufferCoordinate()
    {
        return xCoord - blockSize;
    }

    public float leftXBufferCoordinate()
    {
        return xCoord + blockSize;
    }

    public void setCoordinates(float x, float y)
    {
        xCoord = x;
        yCoord = y;
    }

    public void draw()
    {
        if (collectedCounter < 50 && hasBeenCollected)
        {
            collectedCounter++;

            Engine.DrawString("NICE", new Vector2(xCoord, yCoord), Color.White, fontSmall);
        }
        else if (hasBeenCollected)
        {
            hasBeenCollected = false;
            collectedCounter = 0;
            isVisible = false;
            Engine.DrawString("NICE", new Vector2(xCoord, yCoord), Color.White, fontSmall);
        }
        else if (isVisible)
        {
            //Console.WriteLine("COIN VISIBLITY: " + isVisible);
            Engine.DrawResizableTexture(coinTex, new Bounds2(xCoord, yCoord, blockSize, blockSize));
        }


    }
    public void move(Boolean right, float distance)
    {
        if (right)
        {
            xCoord -= distance;
        }
        else
        {
            xCoord += distance;
        }

        if (isVisible)
        {
            Console.WriteLine("COIN VISIBLITY: " + isVisible);
            Engine.DrawResizableTexture(coinTex, new Bounds2(xCoord, yCoord, blockSize, blockSize));
        }

    }
}


class Cloud
{
    float xCoord;
    float yCoord;
    float xRightLim;
    ResizableTexture cloudTex = Engine.LoadResizableTexture("cloud.png", 0, 0, 0, 0);
    public Boolean isVisible = true;
    public Cloud(float xStart, float yStart, float xRightLimit)
    {
        xCoord = xStart;
        yCoord = yStart;
        xRightLim = xRightLimit;
        Engine.DrawResizableTexture(cloudTex, new Bounds2(xCoord, yCoord, 50, 50));
    }

    public void move(Boolean right, float distance)
    {
        if (right)
        {
            xCoord -= distance;
        }
        else
        {
            xCoord += distance;
        }
        if (xCoord < -300)
        {

            var r = new Random();

            xCoord = xRightLim + r.Next(0, 3);
            if (r.Next(0, 2) == 1 && yCoord > 50)
            {
                yCoord -= r.Next(50, 80);
            }
            else
            {
                yCoord += r.Next(20, 40);
            }
        }
        if (isVisible)
        {
            Engine.DrawResizableTexture(cloudTex, new Bounds2(xCoord, yCoord, 300, 150));
        }
        
    }
}

class Tree
{
    float xCoord;
    float yCoord;
    float xRightLim;
    ResizableTexture treeTex = Engine.LoadResizableTexture("tree2.png", 0, 0, 0, 0);
    ResizableTexture treeFlipTex = Engine.LoadResizableTexture("tree2Flip.png", 0, 0, 0, 0);
    Boolean flipped = false;
    int treeHeight = 250;
    public Boolean isVisible = true;
    public Tree(float xStart, float yStart, float xRightLimit)
    {
        xCoord = xStart;
        yCoord = yStart;
        xRightLim = xRightLimit;
        Engine.DrawResizableTexture(treeTex, new Bounds2(xCoord, yCoord, 50, 50));
        flipped = new Random().Next(0, 2) == 0;
        treeHeight = new Random().Next(300, 375);
    }

    public void move(Boolean right, float distance)
    {
        if (right)
        {
            xCoord -= distance;
        }
        else
        {
            xCoord += distance;
        }
        if (xCoord < -300)
        {
            var r = new Random();

            xCoord = xRightLim + r.Next(0, 50);
            flipped = new Random().Next(0, 2) == 0;
            treeHeight = new Random().Next(350, 400);
        }
        if (isVisible)
        {
            if (flipped)
            {
                Engine.DrawResizableTexture(treeFlipTex, new Bounds2(xCoord, 600 - treeHeight, treeHeight * 20 / 35, treeHeight));
            }
            else
            {
                Engine.DrawResizableTexture(treeTex, new Bounds2(xCoord, 600 - treeHeight, treeHeight * 20 / 35, treeHeight));
            }
        }
        
        
    }
}