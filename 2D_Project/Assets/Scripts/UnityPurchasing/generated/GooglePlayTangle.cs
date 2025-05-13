// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("xag5pOuV8+j9JHsskW6r24/rmCuiea/mUIEPsByJKEeTjgPC8YEAzeUG4N+wSbVCsCxQEnLBbeJKfBFddqB+FPxn0ZKNvvBAioz2o/MBHN8hmMN0Dif0VvCZb1qfIlqwtHk9Oj24++P3xeSg37eSzPSmxp99bfU/HNyfo+lMFog7h6FJvgYamCqlhmK8EQd0gl35vjCI3LiVqd3U9WuxPwi6ORoINT4xEr5wvs81OTk5PTg7bkVX7FLB9bam4CtaDBlV8xlm9SI0yKw7F2nRy7TcuiZrXDOXl0V5m8OPCqqAjW0RUJNirQjdrulnn55xxggYcx4Jd6meKxutjnqxRLk7d0u6OTc4CLo5Mjq6OTk46FfmKXqyEiHeho2t3nRgfzo7OTg5");
        private static int[] order = new int[] { 6,12,3,11,9,9,13,11,13,10,13,13,13,13,14 };
        private static int key = 56;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
