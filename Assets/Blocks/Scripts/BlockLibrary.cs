using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

namespace Blockly
{
    public class BlockLibrary
    {
        public Transform transform;
        public Block startBlock;
        BuggyBuddy buggyBuddy;

        public void Play()
        {
            GameObject robot = GameObject.FindGameObjectWithTag("Robot");
            if (robot != null)
                buggyBuddy = robot.GetComponent<BuggyBuddy>();

            if (startBlock != null)
                startBlock.function(null);
        }

        public static BuggyBuddy GetBuggy()
        {
            GameObject robot = GameObject.FindGameObjectWithTag("Robot");
            if (robot != null)
                return robot.GetComponent<BuggyBuddy>();
            return null;
        }

        // =========================
        // LOGIC
        // =========================

        public static GameObject createStartBlock(Transform transform) {
            Block block = new Block("StartBlock");
            block.appendDummyInput()
                 .appendField("Start");
            block.setNextStatement(true, null);
            block.setColour(165);
            block.function = delegate (object o) { return block.callNext(); };
            block.setStart(true);

            // GameObject obj =
            return block.build(transform);
            // startBlock = block;
        }

        public static GameObject createIfDoBlock(Transform transform) {
          Block block = new Block("IfDoBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("if ");
          block.appendStatementInput("NAME")
              .setCheck(null)
              .appendField("do ");
          block.setColour(210);
          return block.build(transform);
        }

        public static GameObject createNotBlock(Transform transform) {
          Block block = new Block("NotBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("not");
          block.setOutput(true, null);
          block.setColour(210);
          return block.build(transform);
        }

        public static GameObject createTestTFBlock(Transform transform) {
          Block block = new Block("TestTFBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("test ");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("if true ");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("if false ");
          block.setOutput(true, null);
          block.setColour(210);
          return block.build(transform);
        }

        public static GameObject createCompareBlock(Transform transform) {
          Block block = new Block("CompareBlock");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.appendDummyInput()
              .appendField(" =");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.setOutput(true, null);
          block.setColour(210);
          return block.build(transform);
        }

        // =========================
        // LOOPS
        // =========================

        public static GameObject createLoopBlock(Transform transform)
        {
            Block block = new Block("LoopBlock");
            block.appendValueInput("INPUT")
                .setCheck(null)
                .appendField("Loop");
            block.appendStatementInput("INPUT1")
                 .setCheck(null);
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setColour(50);
            block.function = delegate (object o) {
                object obj = block.callInput("INPUT");
                if (obj != null && int.TryParse(obj.ToString(), out int loop))
                {
                    for (int i = 0; i < loop; i++)
                        block.callInput("INPUT1");
                }
                return block.callNext();
            };
            return block.build(transform);
        }

        public static GameObject createRepeatXBlock(Transform transform) {
          Block block = new Block("RepeatXBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("repeat ")
              .appendField(new Blockly.FieldNumber(0), "NAME")
              .appendField(" times");
          block.appendStatementInput("NAME")
              .setCheck(null)
              .appendField("do ");
          block.setOutput(true, null);
          block.setColour(135);
          return block.build(transform);
        }

        public static GameObject createRepeatWhileBlock(Transform transform) {
          Block block = new Block("RepeatWhileBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("repeat ")
              .appendField(new Blockly.FieldTextInput("dropdown here"), "NAME");
          block.appendStatementInput("NAME")
              .setCheck(null)
              .appendField("do ");
          block.setOutput(true, null);
          block.setColour(135);
          return block.build(transform);
        }

        public static GameObject createIteratorBlock(Transform transform) {
          Block block = new Block("IteratorBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("count ")
              .appendField(" from")
              .appendField(new Blockly.FieldNumber(0), "NAME")
              .appendField(" to")
              .appendField(new Blockly.FieldNumber(0), "NAME")
              .appendField(" by")
              .appendField(new Blockly.FieldNumber(0), "NAME");
          block.appendStatementInput("NAME")
              .setCheck(null)
              .appendField("do ");
          block.setOutput(true, null);
          block.setColour(135);
          return block.build(transform);
        }

        // =========================
        // MATH
        // =========================

        public static GameObject createNumberBlock(Transform transform)
        {
            Block block = new Block("NumberBlock");
            block.appendDummyInput()
                .appendField(2.ToString());
            block.setOutput(true, null);
            block.setColour(0);
            block.function = delegate (object o) {
                return 2;
            };
            return block.build(transform);
        }

        public static GameObject createPiBlock(Transform transform) {
          Block block = new Block("PiBlock");
          block.appendValueInput("NAME")
              .setCheck(null)
              .appendField("π");
          block.setOutput(true, null);
          block.setColour(0);
          return block.build(transform);
        }

        public static GameObject createAddBlock(Transform transform) {
          Block block = new Block("AddBlock");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.appendDummyInput()
              .appendField(" +");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.setOutput(true, null);
          block.setColour(0);
          return block.build(transform);
        }

        public static GameObject createMinusBlock(Transform transform) {
          Block block = new Block("MinusBlock");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.appendDummyInput()
              .appendField(" -");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.setOutput(true, null);
          block.setColour(0);
          return block.build(transform);
        }

        public static GameObject createTimesBlock(Transform transform) {
          Block block = new Block("TimesBlock");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.appendDummyInput()
              .appendField(" ×");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.setOutput(true, null);
          block.setColour(0);
          return block.build(transform);
        }

        public static GameObject createDivideBlock(Transform transform) {
          Block block = new Block("DivideBlock");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.appendDummyInput()
              .appendField(" /");
          block.appendValueInput("NAME")
              .setCheck(null);
          block.setOutput(true, null);
          block.setColour(0);
          return block.build(transform);
        }


        // =========================
        // MOVEMENT
        // =========================

        public static GameObject createSetThrottleBlock(Transform transform)
        {
            Block block = new Block("ThrottleBlock");
            block.appendValueInput("INPUT")
                .setCheck(null)
                .appendField("setThrottle");
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setColour(180);
            block.function = delegate (object o) {
                object obj = block.callInput("INPUT");
                BuggyBuddy buggyBuddy = GetBuggy();
                if (obj != null && float.TryParse(obj.ToString(), out float throttleValue) &&  buggyBuddy != null) {
                   Debug.Log("Setting Throttle to " + throttleValue.ToString());
                   buggyBuddy.throttle = Mathf.Clamp(throttleValue / 100.0f, -1f, 1f);
                }

                return block.callNext();
            };

            return block.build(transform);
        }

        public static GameObject createSetSteeringBlock(Transform transform)
        {
            Block block = new Block("SteeringBlock");
            block.appendValueInput("INPUT")
                .setCheck(null)
                .appendField("setSteering");
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setColour(180);
            block.function = delegate (object o) {
                object obj = block.callInput("INPUT");
                BuggyBuddy buggyBuddy = GetBuggy();
                if (obj != null &&  float.TryParse(obj.ToString(), out float steeringValue) && buggyBuddy != null)
                {
                    Debug.Log("Setting Steering to " + steeringValue.ToString());
                    steeringValue = Mathf.Clamp(steeringValue / 100.0f, -1f, 1f);
                    buggyBuddy.steer = new Vector2(steeringValue, 1f);
                }
                return block.callNext();
            };
            return block.build(transform);
        }

        public static GameObject createSleepBlock(Transform transform)
        {
            Block block = new Block("SleepBlock");
            block.appendValueInput("INPUT")
                .setCheck(null)
                .appendField("Sleep");
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setColour(180);
            block.function = delegate (object o) {
                object obj = block.callInput("INPUT");
                if (obj != null && int.TryParse(obj.ToString(), out int sleepTime))
                {
                    Debug.Log("Sleeping for " + sleepTime.ToString() + " sec");
                    Thread.Sleep((int)sleepTime * 1000);
                }
                return block.callNext();
            };
            return block.build(transform);
        }

        // =========================
        // FUNCTIONS
        // =========================

        public static GameObject createFunctionBlock(Transform transform)
        {
            Block block = new Block("FunctionBlock");
              block.appendValueInput("NAME")
                  .setCheck(null)
                  .appendField("if ");
              block.appendDummyInput()
                  .appendField(" return");
              block.appendValueInput("NAME")
                  .setCheck(null);
              block.setOutput(true, null);
              block.setColour(0);
            return block.build(transform);
        }


        // =========================
        // IN/OUT
        // =========================

        public static GameObject createInputBlock(Transform transform)
        {
            Block block = new Block("InputBlock");
            block.appendDummyInput()
                 .appendField(new Blockly.FieldTextInput("Input"), "name");
            block.setOutput(true, null);
            block.setInputsInline(false);
            block.setColour(130);
            block.function = delegate (object o) {
                return ((FieldTextInput)block.inputs[0].fields[0]).text;
            };
            return block.build(transform);
        }


        public static GameObject createPrintBlock(Transform transform)
        {
            Block block = new Block("PrintBlock");
            block.appendValueInput("INPUT")
                .setCheck(null)
                .appendField("print");
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setColour(180);
            block.function = delegate (object o) {
                object obj = block.callInput("INPUT");
                if (obj != null)
                    Debug.Log("Printing " + obj.ToString());
                return block.callNext();
            };

            return block.build(transform);
        }





        public void createRandomBlock()
        {
            // Default external
            Block block = new Block("Random");
            block.appendValueInput("NAME")
                .setCheck(new string[] { "Boolean", "String" })
                .appendField("sefsf")
                .appendField(new Blockly.FieldLabelSerializable("lol"), "dsf")
                .appendField(new Blockly.FieldTextInput("default"), "sfs")
                .appendField(new Blockly.FieldNumber(0), "dfgdv")
                .appendField(new Blockly.FieldAngle(195), "fgre")
                .appendField(new Blockly.FieldDropdown(new string[,] { { "option", "OPTIONNAME" }, { "option", "OPTIONNAME" }, { "option", "OPTIONNAME" } }), "dfg")
                .appendField(new Blockly.FieldCheckbox("TRUE"), "bbbb")
                .appendField(new Blockly.FieldColour("#ff0000"), "NAME")
                .appendField(new Blockly.FieldVariable("item"), "fdgfgfd");
            block.appendDummyInput();
            block.appendStatementInput("NAME")
                .setCheck(null);
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setInputsInline(false);
            block.setColour(230);
        }
    }
}
