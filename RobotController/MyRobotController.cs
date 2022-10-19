﻿using System;
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

            _startedEx2 = false; // Reset for ex2
            _startedEx3 = false; // Reset for ex3

            //todo: change this, use the function Rotate declared below
            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _initialAngle[0]);
            rot1 = Rotate(rot0, MyVec.right, _initialAngle[1]);
            rot2 = Rotate(rot1, MyVec.right, _initialAngle[2]);
            rot3 = Rotate(rot2, MyVec.right, _initialAngle[3]);
        }



        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            _startedEx3 = false; // Reset for ex3
            if (!_startedEx2)
            {
                _startedEx2 = true;
                ResetT();
            }

            //todo: add a check for your condition
            bool myCondition = t < 1f;          

            if (myCondition)
            {
                //todo: add your code here
                rot0 = Rotate(MyQuat.NullQ, MyVec.up, Utils.Lerp(_initialAngle[0], _ex2FinalAngle[0], t));
                rot1 = Rotate(rot0, MyVec.right, Utils.Lerp(_initialAngle[1], _ex2FinalAngle[1], t));
                rot2 = Rotate(rot1, MyVec.right, Utils.Lerp(_initialAngle[2], _ex2FinalAngle[2], t));
                rot3 = Rotate(rot2, MyVec.right, Utils.Lerp(_initialAngle[3], _ex2FinalAngle[3], t));

                t = RobotController.Utils.Clamp(t + _robotSpeedEx2, 0f, 1f);

                return true;
            }


            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _ex2FinalAngle[0]);
            rot1 = Rotate(rot0, MyVec.right, _ex2FinalAngle[1]);
            rot2 = Rotate(rot1, MyVec.right, _ex2FinalAngle[2]);
            rot3 = Rotate(rot2, MyVec.right, _ex2FinalAngle[3]);

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            _startedEx2 = false; // Reset for ex2
            if (!_startedEx3)
            {
                _startedEx3 = true;
                ResetT();
            }


            bool myCondition = t < 1f;

            if (myCondition)
            {
                //todo: add your code here
                rot0 = Rotate(MyQuat.NullQ, MyVec.up, Utils.Lerp(_initialAngle[0], _ex2FinalAngle[0], t));
                rot1 = Rotate(rot0, MyVec.right, Utils.Lerp(_initialAngle[1], _ex2FinalAngle[1], t));
                rot2 = Rotate(rot1, MyVec.right, Utils.Lerp(_initialAngle[2], _ex2FinalAngle[2], t));
                rot3 = Rotate(rot2, MyVec.right, Utils.Lerp(_initialAngle[3], _ex2FinalAngle[3], t));
                rot3 = Rotate(rot3, _ex3TwistAxis, Utils.Lerp(_ex3InitialTwistAngle, _ex3FinalTwistAngle, t));


                t = RobotController.Utils.Clamp(t + _robotSpeedEx2, 0f, 1f);

                return true;
            }


            //todo: remove this once your code works.
            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _ex2FinalAngle[0]);
            rot1 = Rotate(rot0, MyVec.right, _ex2FinalAngle[1]);
            rot2 = Rotate(rot1, MyVec.right, _ex2FinalAngle[2]);
            rot3 = Rotate(rot2, MyVec.right, _ex2FinalAngle[3]);
            rot3 = Rotate(rot3, _ex3TwistAxis, _ex3FinalTwistAngle);
           

            return false;
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return rot3 * MyQuat.Inverse(GetTwist(rot3));
        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return new MyQuat(_ex3TwistAxis.x * rot3.x, _ex3TwistAxis.y * rot3.y, _ex3TwistAxis.z * rot3.z, rot3.w);
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


        // Exercise 1

        private float[] _initialAngle = { 73f, 350f, 90f, 30f };

        // Exercise 2
        public float t = 0f;
        private void ResetT()
        {
            t = 0f;
        }
        private bool _startedEx2 = false;
        private float _robotSpeedEx2 = 0.005f;

        private float[] _ex2FinalAngle = { 40f, 358f, 90f, 9f };

        // Exercise 3
        private bool _startedEx3 = false;
        private static MyVec _ex3TwistAxis = MyVec.up;
        private float _ex3InitialTwistAngle = 33.617f;
        private float _ex3FinalTwistAngle = -90f;

    }
}
