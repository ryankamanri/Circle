using System;

namespace Kamanri.Self
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
            return ((new Random().Next() % 90000) + 10000);
        }

        public static int Generate_0_255()
        {
            return new Random().Next() % 256;
        }
    }
}