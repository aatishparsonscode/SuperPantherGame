using System;
using System.Collections.Generic;
using System.Text;


class Timer
{
    double time = 0;

    public Timer()
    {

    }

    public void add()
    {
        time += (Engine.TimeDelta);
    }

    public string getTime()
    {
        return time.ToString("F2");
    }

    public double getTimeDouble()
    {
        return time;
    }

    public void resetTimer()
    {
        time = 0;
    }
}

