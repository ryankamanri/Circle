using System;

namespace dotnet.Services.Extensions
{
    public static class RandomGenerator
    {
        public static long GenerateID()
        {
            return DateTime.Now.Ticks - new DateTime(2020,1,1).Ticks;
        }

        //生成五位数的验证码
        public static int GenerateAuthCode()
        {
            return ((int)((DateTime.Now.Ticks % 90000) + 10000));
        }
    }
}