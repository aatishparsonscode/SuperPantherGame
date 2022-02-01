using System;
using System.Collections.Generic;
using System.Text;


class ScoreCalculator
{
    public static double calculateRawScore(double timeForLevel, double distanceMoved, int coinsCollected, int enemiesBeaten)
    {

        return ((double)200 * coinsCollected + 1.1 * distanceMoved - timeForLevel * 100 + enemiesBeaten * 1000)/100;

    }

    public static String getPrintableScore(double timeForLevel, double distanceMoved, int coinsCollected, int enemiesBeaten)
    {
        return calculateRawScore(timeForLevel, distanceMoved, coinsCollected, enemiesBeaten).ToString("F2");
    }
}