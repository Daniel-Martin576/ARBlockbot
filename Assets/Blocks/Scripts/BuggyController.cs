using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class BuggyController : MonoBehaviour
    {
        public Transform modelJoystick;
        public float joystickRot = 20;

        public Transform modelTrigger;
        public float triggerRot = 20;

        public BuggyBuddy buggy;

        public Transform buttonBrake;
        public Transform buttonReset;

        //ui stuff

        public Canvas ui_Canvas;
        public Image ui_rpm;
        public Image ui_speed;
        public RectTransform ui_steer;

        public float ui_steerangle;

        public Vector2 ui_fillAngles;

        public Transform resetToPoint;

        private float usteer;

        private Quaternion trigSRot;

        private Quaternion joySRot;

        private Coroutine resettingRoutine;

        private Vector3 initialScale;

        private System.Diagnostics.Stopwatch watch;
        private bool checkPointReached = false;
        public float kEpsilon;
        public Text timeText;
        public Transform checkPoint;
        private bool raceGoing = false;
        public Text textScore;

        private void Start()
        {
            joySRot = modelJoystick.localRotation;
            trigSRot = modelTrigger.localRotation;


            StartCoroutine(DoBuzz());
            buggy.controllerReference = transform;
            initialScale = buggy.transform.localScale;
            DoReset();
        }

        private void Update()
        {
            Vector2 steer = Vector2.zero;
            float throttle = 0;
            float brake = 0;

            bool reset = false;

            bool b_brake = false;
            bool b_reset = false;


            if (reset && resettingRoutine == null)
            {
                resettingRoutine = StartCoroutine(DoReset());
            }

            if (ui_Canvas != null)
            {

                usteer = Mathf.Lerp(usteer, steer.x, Time.deltaTime * 9);
                ui_steer.localEulerAngles = Vector3.forward * usteer * -ui_steerangle;
                ui_rpm.fillAmount = Mathf.Lerp(ui_rpm.fillAmount, Mathf.Lerp(ui_fillAngles.x, ui_fillAngles.y, throttle), Time.deltaTime * 4);
                float speedLim = 40;
                ui_speed.fillAmount = Mathf.Lerp(ui_fillAngles.x, ui_fillAngles.y, 1 - (Mathf.Exp(-buggy.speed / speedLim)));

            }

            modelJoystick.localRotation = joySRot;
            /*if (input.AttachedHand != null && input.AttachedHand.IsLeft)
            {
                Joystick.Rotate(steer.y * -joystickRot, steer.x * -joystickRot, 0, Space.Self);
            }
            else if (input.AttachedHand != null && input.AttachedHand.IsRight)
            {
                Joystick.Rotate(steer.y * -joystickRot, steer.x * joystickRot, 0, Space.Self);
            }
            else*/
            //{
            modelJoystick.Rotate(steer.y * -joystickRot, steer.x * -joystickRot, 0, Space.Self);
            //}

            modelTrigger.localRotation = trigSRot;
            modelTrigger.Rotate(throttle * -triggerRot, 0, 0, Space.Self);
            buttonBrake.localScale = new Vector3(1, 1, b_brake ? 0.4f : 1.0f);
            buttonReset.localScale = new Vector3(1, 1, b_reset ? 0.4f : 1.0f);

            buggy.steer = steer;
            buggy.throttle = throttle;
            buggy.handBrake = brake;
            buggy.controllerReference = transform;

            // Me!!!!!!!!!!!!!!!!

            if (!raceGoing && Vector3.Distance(buggy.transform.position, resetToPoint.transform.position) >= kEpsilon ) {
                watch = System.Diagnostics.Stopwatch.StartNew();
                raceGoing = true;
                textScore.text = "Score: 0";
            }

            if (Vector3.Distance(buggy.transform.position, checkPoint.transform.position) <= kEpsilon) {
                checkPointReached = true;
            }

            if (raceGoing && checkPointReached && Vector3.Distance(buggy.transform.position, resetToPoint.transform.position) <= kEpsilon) {
                watch.Stop();
                timeText.text = "Fastest Time: " + watch.ElapsedMilliseconds / 1000.0 + " s";
                
                checkPointReached = false;
                raceGoing = false;
            }
        }

        private IEnumerator DoReset()
        {
            // Me 
            checkPointReached = false;
            raceGoing = false;
            textScore.text = "Score: 0";

            float startTime = Time.time;
            float overTime = 1f;
            float endTime = startTime + overTime;

            buggy.transform.position = resetToPoint.transform.position;
            buggy.transform.rotation = resetToPoint.transform.rotation;
            buggy.transform.localScale = initialScale * 0.1f;

            while (Time.time < endTime)
            {
                buggy.transform.localScale = Vector3.Lerp(buggy.transform.localScale, initialScale, Time.deltaTime * 5f);
                yield return null;
            }

            buggy.transform.localScale = initialScale;

            resettingRoutine = null;
        }

        private float buzztimer;
        private IEnumerator DoBuzz()
        {
            while (true)
            {
                while (buzztimer < 1)
                {
                    buzztimer += Time.deltaTime * buggy.mvol * 70;
                    yield return null;
                }

                buzztimer = 0;
            }
        }
    }
