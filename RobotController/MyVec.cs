using System;

namespace RobotController
{
    public struct MyVec
    {

        public float x;
        public float y;
        public float z;


        public static MyVec right = new MyVec(1f, 0f, 0f);
        public static MyVec up = new MyVec(0f, 1f, 0f);
        public static MyVec forward = new MyVec(0f, 0f, 1f);


        public MyVec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public MyVec(MyVec v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static MyVec operator +(MyVec v1, MyVec v2)
        {
            return new MyVec(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static MyVec operator *(float scalar, MyVec v)
        {
            return new MyVec(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static MyVec operator* (MyVec v, MyQuat q)
        {
            //MyVec qv = new MyVec(q.x, q.y, q.z);
            //return (2f * MyVec.Dot(qv, v) * qv) + ((q.w * q.w - MyVec.Dot(qv, qv)) * v) + (2f * q.w * MyVec.Cross(qv, v)).Normalize();

            MyQuat p = new MyQuat(v.x, v.y, v.z, 0f);
            p = q * p * MyQuat.Inverse(q);
            return new MyVec(p.x, p.y, p.z);
        }


        public MyVec Normalize()
        {
            float length = (float)Math.Sqrt((x * x) + (y * y) + (z * z));

            x /= length;
            y /= length;
            z /= length;

            return this;
        }

        public static MyVec Cross(MyVec v1, MyVec v2)
        {
            return new MyVec((v1.y * v2.z) - (v1.z * v2.y),
                             (v1.z * v2.x) - (v1.x * v2.z),
                             (v1.x * v2.y) - (v1.y * v2.x));
        }

        public static float Dot(MyVec v1, MyVec v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
        }


        public string Tostring()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }


    }

}