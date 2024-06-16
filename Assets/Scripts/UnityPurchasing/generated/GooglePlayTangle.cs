// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iCEh1oFkJtbybE97OiTIiQ+7eM3xK8c0XtyDm9LgK3GJB0g/A9/UD1lHWBGMGqKiRjblOi7RlkqTA3vzY2WF+C5P85YjyPS8a6R3CFRlekKQmZ44rcrisKLDtcnAFlKTajPQXKq9oohxnI8WWFQl3iY5ojrXVl9SgjCzkIK/tLuYNPo0Rb+zs7O3srEws72ygjCzuLAws7OyIHrjfw93iJhUB4hclop7PRa0AXdfVDKikT9d5pbWdLi9xzZbi9wR6mETP2NBpkHeop1qiSMUHi/XPCEkbowSdcvqEoaFONl47ka+hbOp5vuTWFxEtBPBwNDFBRE8jP0hpatjSkhn1fZHgC3YqPOtEDpaziYmfj+43my2Mr7b+uDLo0CVY+j3XbCxs7Kz");
        private static int[] order = new int[] { 6,11,10,4,5,9,6,11,10,10,12,11,13,13,14 };
        private static int key = 178;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
