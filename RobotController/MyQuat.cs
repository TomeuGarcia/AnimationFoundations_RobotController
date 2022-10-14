using System;

namespace RobotController
{
    public static class Utils
    {
        public static float Deg2Rad = (float)(Math.PI / 180.0);
    }
        

    public struct MyQuat
    {

        public float w;
        public float x;
        public float y;
        public float z;


        public MyQuat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            Normalize();
        }

        public static MyQuat operator *(MyQuat q1, MyQuat q2)
        {
            float w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z);
            float x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y);
            float y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x);
            float z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w);

            return new MyQuat(x, y, z, w).Normalize();
        }

        public static MyQuat ToUnityQuaternion(MyQuat myQ)
        {
            return new MyQuat(myQ.x, myQ.y, myQ.z, myQ.w);
        }

        public static MyQuat FromUnityQuaternion(MyQuat unityQ)
        {
            return new MyQuat(unityQ.x, unityQ.y, unityQ.z, unityQ.w);
        }

        public string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ", " + w + ")";
        }


        public static MyQuat Identity()
        {
            return new MyQuat(0f, 0f, 0f, 1f);
        }

        public static MyQuat Inverse(MyQuat q)
        {
            return new MyQuat(-q.x, -q.y, -q.z, q.w);
        }


        public MyQuat Normalize()
        {
            float length = (float)Math.Sqrt(x * x + y * y + z * z + w * w);
            x /= length;
            y /= length;
            z /= length;
            w /= length;
            return this;
        }


        public static MyQuat FromAxisAngle(MyVec axis, float angle)
        {
            float halfAngle = (angle / 2f) * Utils.Deg2Rad;
            float x = axis.x * (float)Math.Sin(halfAngle);
            float y = axis.y * (float)Math.Sin(halfAngle);
            float z = axis.z * (float)Math.Sin(halfAngle);
            float w = (float)Math.Cos(halfAngle);

            return new MyQuat(x, y, z, w).Normalize();
        }

        public void ToAxisAngle(out MyVec axis, out float angle)
        {
            float v = (float)Math.Sqrt(1f - (w * w));
            axis = new MyVec(x / v, y / v, z / v);
            angle = 2f * (float)Math.Acos(w) * Utils.Deg2Rad;
        }

    }
}