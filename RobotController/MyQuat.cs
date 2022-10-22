using System;

namespace RobotController
{
    public static class Utils
    {
        public static float Deg2Rad = (float)(Math.PI / 180.0);
        public static float Rad2Deg = 57.2957795f; // 180f / PI

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Lerp(float start, float end, float t)
        {
            return ((1 - t) * start) + (t * end);
        }
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

        public static MyQuat NullQ
        {
            get
            {
                MyQuat a;
                a.w = 1;
                a.x = 0;
                a.y = 0;
                a.z = 0;
                return a;

            }
        }

        public static MyQuat operator *(MyQuat q1, MyQuat q2)
        {
            float w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z);
            float x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y);
            float y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x);
            float z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w);

            return new MyQuat(x, y, z, w).Normalize();
        }

        public static MyQuat operator *(float scalar, MyQuat q)
        {
            return new MyQuat(scalar * q.x, scalar * q.y, scalar * q.z, scalar * q.w);
        }

        private static MyQuat Add(MyQuat q1, MyQuat q2)
        {
            return new MyQuat(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w).Normalize();
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
            float sinHalfAngle = (float)Math.Sin(halfAngle);

            float x = axis.x * sinHalfAngle;
            float y = axis.y * sinHalfAngle;
            float z = axis.z * sinHalfAngle;
            float w = (float)Math.Cos(halfAngle);

            return new MyQuat(x, y, z, w).Normalize();
        }

        public void ToAxisAngle(out MyVec axis, out float angle)
        {
            float v = (float)Math.Sqrt(1f - (w * w));
            axis = new MyVec(x / v, y / v, z / v);
            angle = 2f * (float)Math.Acos(w) * Utils.Rad2Deg;
        }

        public static void GetMinAxisAngleBetweenQuaternions(MyQuat q1, MyQuat q2, out MyVec axis, out float minAngle)
        {
            MyQuat offsetRotation = q2 * MyQuat.Inverse(q1);

            offsetRotation.ToAxisAngle(out axis, out minAngle);
            minAngle = minAngle > 180f ? 360f - minAngle : minAngle;
        }

        public static MyQuat Lerp(MyQuat q1, MyQuat q2, float t)
        {
            // (1-t) p + t q
            MyVec axis1;
            float angle1;
            q1.ToAxisAngle(out axis1, out angle1);

            MyVec axis2;
            float angle2;
            q2.ToAxisAngle(out axis2, out angle2);

            MyVec axisT = ((1f - t) * axis1) + (t * axis2);
            float angleT = ((1f - t) * angle1) + (t * angle2);
               
            return MyQuat.FromAxisAngle(axisT, angleT);
        }

        public static MyQuat Slerp(MyQuat q1, MyQuat q2, float t)
        {
            MyVec axis;
            float angle;
            GetMinAxisAngleBetweenQuaternions(q1, q2, out axis, out angle);

            float sinAlpha = (float)Math.Sin(angle);
            float q1_t = (float)Math.Sin((1f - t) * angle);
            float q2_t = (float)Math.Sin(t * angle);

            return Add((q1_t / sinAlpha) * q1, (q2_t / sinAlpha) * q2);
        }


        public static MyQuat FastSlerp(MyQuat q1, MyQuat q2, float t)
        {
            MyVec axis;
            float angle;
            GetMinAxisAngleBetweenQuaternions(q1, q2, out axis, out angle);


            float f = 1.0f - 0.7878088f * (float)Math.Sin(angle);
            float k = 0.5069269f;
            f *= f;
            k *= f;
            float b = 2 * k;
            float c = -3 * k;
            float d = 1 + k;
            t = t * (b * t + c) + d;

            return Lerp(q1, q2, t);
        }

    }
}