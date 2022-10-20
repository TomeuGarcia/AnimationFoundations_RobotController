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

            if (_currentExercise != Exercise.EX_1)
            {
                _currentExercise = Exercise.EX_1;
            }

            //todo: change this, use the function Rotate declared below
            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _initialAngles[0]);
            rot1 = Rotate(rot0, MyVec.right, _initialAngles[1]);
            rot2 = Rotate(rot1, MyVec.right, _initialAngles[2]);
            rot3 = Rotate(rot2, MyVec.right, _initialAngles[3]);
        }



        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            if (_currentExercise != Exercise.EX_2)
            {
                _currentExercise = Exercise.EX_2;
                ResetT();
            }

            //todo: add a check for your condition
            bool myCondition = _t < 1f;          

            if (myCondition)
            {
                //todo: add your code here
                rot0 = Rotate(MyQuat.NullQ, MyVec.up, Utils.Lerp(_initialAngles[0], _ex2FinalAngles[0], _t));
                rot1 = Rotate(rot0, MyVec.right, Utils.Lerp(_initialAngles[1], _ex2FinalAngles[1], _t));
                rot2 = Rotate(rot1, MyVec.right, Utils.Lerp(_initialAngles[2], _ex2FinalAngles[2], _t));
                rot3 = Rotate(rot2, MyVec.right, Utils.Lerp(_initialAngles[3], _ex2FinalAngles[3], _t));

                _t = RobotController.Utils.Clamp(_t + _robotMoveSpeed, 0f, 1f);

                return true;
            }


            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _ex2FinalAngles[0]);
            rot1 = Rotate(rot0, MyVec.right, _ex2FinalAngles[1]);
            rot2 = Rotate(rot1, MyVec.right, _ex2FinalAngles[2]);
            rot3 = Rotate(rot2, MyVec.right, _ex2FinalAngles[3]);

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            if (_currentExercise != Exercise.EX_3)
            {
                _currentExercise = Exercise.EX_3;
                ResetT();
            }


            bool myCondition = _t < 1f;

            if (myCondition)
            {
                rot0 = Rotate(MyQuat.NullQ, MyVec.up, Utils.Lerp(_initialAngles[0], _ex2FinalAngles[0], _t));
                rot1 = Rotate(rot0, MyVec.right, Utils.Lerp(_initialAngles[1], _ex2FinalAngles[1], _t));
                rot2 = Rotate(rot1, MyVec.right, Utils.Lerp(_initialAngles[2], _ex2FinalAngles[2], _t));
                _ex3TempRot2 = rot2;

                //_localTwist = MyQuat.FromAxisAngle(_ex3TwistAxis, Utils.Lerp(_ex3InitialTwistAngle, _ex3FinalTwistAngle, _t)); //
                _localSwing = MyQuat.FromAxisAngle(MyVec.right, Utils.Lerp(_initialAngles[3], _ex2FinalAngles[3], _t));
                rot3 = Rotate(_localSwing, _ex3TwistAxis, Utils.Lerp(_ex3InitialTwistAngle, _ex3FinalTwistAngle, _t));
                // rot3 contains localSwing + localTwist, without accumulating rot2, which is added later in GetSwing() and GetTwist()


                _t = RobotController.Utils.Clamp(_t + _robotMoveSpeed, 0f, 1f);

                return true;
            }



            rot0 = Rotate(MyQuat.NullQ, MyVec.up, _ex2FinalAngles[0]);
            rot1 = Rotate(rot0, MyVec.right, _ex2FinalAngles[1]);
            rot2 = Rotate(rot1, MyVec.right, _ex2FinalAngles[2]);
            _ex3TempRot2 = rot2;

            //_localTwist = MyQuat.FromAxisAngle(_ex3TwistAxis, _ex3FinalTwistAngle); //
            _localSwing = MyQuat.FromAxisAngle(MyVec.right, _ex2FinalAngles[3]);
            rot3 = Rotate(_localSwing, _ex3TwistAxis, _ex3FinalTwistAngle);
            // rot3 contains localSwing + localTwist, without accumulating rot2, which is added later in GetSwing() and GetTwist()


            return false;
        }

        // Obtain the localSwing and pass it to "world" sapce
        public static MyQuat GetSwing(MyQuat rot3)
        {
            return _ex3TempRot2 * (rot3 * MyQuat.Inverse(GetTwistLocal(rot3)));
        }

        // Obtain the localTwist and pass it to "world" space
        public static MyQuat GetTwist(MyQuat rot3)
        {
            return GetSwing(rot3) * GetTwistLocal(rot3);
            //return _ex3TempRot2 * GetTwistLocal(rot3); // old code, not working properly
        }

        // rot3 contains localTwist + localSwing, rotations with different rotation axis, therefore we can obtain the twist part
        private static MyQuat GetTwistLocal(MyQuat rot3)
        {
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


        enum Exercise { NONE, EX_1, EX_2, EX_3 };
        private Exercise _currentExercise = Exercise.NONE;

        // Exercise 1
        private float[] _initialAngles = { 73f, 350f, 94f, 20f };

        // Exercise 2
        private float _robotMoveSpeed = 0.002f;
        private float[] _ex2FinalAngles = { 40f, 360f, 85f, 20f };
        
        private float _t = 0f;
        private void ResetT()
        {
            _t = 0f;
        }

        // Exercise 3
        private static MyVec _ex3TwistAxis = MyVec.up;
        private float _ex3InitialTwistAngle = 33.617f;
        private float _ex3FinalTwistAngle = -56.4f;
        private static MyQuat _ex3TempRot2;
        private MyQuat _localSwing;
        //private MyQuat _localTwist;


    }
}
