using System;

namespace Desktop.Tests.TestAPI;

public class Utils
{
    public const int SeveralTimes = 5;

    public static void Repeat(Action what, int times)
    {
        for (var i = 0; i < times; i++)
            what();
    }
}