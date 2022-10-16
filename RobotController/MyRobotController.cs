using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotController
{
    public class MyRobotController
    {

        #region public methods



        public string Hi()
        {

            string s = "Hi, we are Adrià Campos & Tomeu Garcia, from dll";
            return s;

        }


        //EX1: this function will place the robot in the initial position

        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3) {

            startedEx2 = false; // Reset for ex2

            //todo: change this, use the function Rotate declared below
            rot0 = Rotate(MyQuat.NullQ, MyVec.up, 73f);
            rot1 = Rotate(Multiply(rot0, MyQuat.NullQ), MyVec.right, -10f);
            rot2 = Rotate(Multiply(rot1, MyQuat.NullQ), MyVec.right, 90f);
            rot3 = Rotate(Multiply(rot2, MyQuat.NullQ), MyVec.right, 30f);
        }



        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {          
            if (!startedEx2)
            {
                startedEx2 = true;
                ResetT();
            }

            //todo: add a check for your condition
            bool myCondition = t < 1f;

            MyQuat initialRot0 = Rotate(MyQuat.NullQ, MyVec.up, 73f);
            MyQuat initialRot1 = Rotate(Multiply(initialRot0, MyQuat.NullQ), MyVec.right, -10f);
            MyQuat initialRot2 = Rotate(Multiply(initialRot1, MyQuat.NullQ), MyVec.right, 90f);
            MyQuat initialRot3 = Rotate(Multiply(initialRot2, MyQuat.NullQ), MyVec.right, 30f);

            MyQuat finalRot0 = Rotate(MyQuat.NullQ, MyVec.up, 40f);
            MyQuat finalRot1 = Rotate(Multiply(finalRot0, MyQuat.NullQ), MyVec.right, -2f);
            MyQuat finalRot2 = Rotate(Multiply(finalRot1, MyQuat.NullQ), MyVec.right, 82f);
            MyQuat finalRot3 = Rotate(Multiply(finalRot2, MyQuat.NullQ), MyVec.right, 9f);

            if (myCondition)
            {
                //todo: add your code here
                rot0 = lerpFunctionEx2(initialRot0, finalRot0, t);
                rot1 = lerpFunctionEx2(initialRot1, finalRot1, t);
                rot2 = lerpFunctionEx2(initialRot2, finalRot2, t);
                rot3 = lerpFunctionEx2(initialRot3, finalRot3, t);

                t = RobotController.Utils.Clamp(t + robotSpeedEx2, 0f, 1f);

                return true;
            }


            rot0 = finalRot0;
            rot1 = finalRot1;
            rot2 = finalRot2;
            rot3 = finalRot3;

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            startedEx2 = false; // Reset for ex2


            bool myCondition = false;
            //todo: add a check for your condition



            while (myCondition)
            {
                //todo: add your code here


            }

            //todo: remove this once your code works.
            rot0 = MyQuat.NullQ;
            rot1 = MyQuat.NullQ;
            rot2 = MyQuat.NullQ;
            rot3 = MyQuat.NullQ;

            return false;
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return MyQuat.NullQ;

        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return MyQuat.NullQ;

        }




        #endregion


        #region private and internal methods

        internal int TimeSinceMidnight { get { return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond; } }




        internal MyQuat Multiply(MyQuat q1, MyQuat q2) 
        {
            //todo: change this so it returns a multiplication:
            return q1 * q2;
        }

        internal MyQuat Rotate(MyQuat currentRotation, MyVec axis, float angle)
        {
            //todo: change this so it takes currentRotation, and calculate a new quaternion rotated by an angle "angle" radians along the normalized axis "axis"
            return Multiply(currentRotation, MyQuat.FromAxisAngle(axis, angle));
        }




        //todo: add here all the functions needed

        #endregion


        // Exercise 2
        public float t = 0f;
        private void ResetT()
        {
            t = 0f;
        }
        private bool startedEx2 = false;
        private float robotSpeedEx2 = 0.005f;
        private delegate MyQuat LerpFunction(MyQuat q1, MyQuat q2, float t);
        private LerpFunction lerpFunctionEx2 = MyQuat.Lerp;

    }
}
