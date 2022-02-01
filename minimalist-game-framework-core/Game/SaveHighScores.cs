using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

class SaveHighScores
{
    public SaveHighScores() { }

    //change path on personal computer to release
    private static readonly String filePath = "scores.txt"; //"C:\\Users\\s-aparson\\source\\repos\\TeslaSTEMCS\\recreate-a-classic-game-supermario-rhys\\minimalist-game-framework-core\\Builds\\Debug\\netcoreapp2.1\\Assets\\scores.txt";
    
    public static void saveScore(int level, String score)
    {
        try
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(filePath,true))
            {
                file.WriteLine(level + " : " + score);
            }
            //Console.WriteLine("Lines written to file successfully.");
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }
    }

    public static String[] getScores()
    {
        String[] scores = new String[6];
        double[] scoresNum = new double[6];
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                // Read and display lines from the file until 
                // the end of the file is reached. 
                while ((line = sr.ReadLine()) != null)
                {
                    
                    var data = line.Split(" : ");
                    int level = Int16.Parse(data[0]);
                    double score = Convert.ToDouble(data[1]);
                    if(scoresNum[level] < score)
                    {
                        scoresNum[level] = score;
                        scores[level] = score.ToString();
                    }
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        return scores;
    }

}

