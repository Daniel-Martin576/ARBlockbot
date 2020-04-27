using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blockly
{
    public class BlockLibrary : MonoBehaviour
    {
        private Transform touchTransform;

        public void Start() {
          touchTransform = transform;
          createStartBlock();
        }


        public void createStartBlock()
        {

            Block block = new Block("StartBlock");
            block.appendDummyInput()
                .appendField("Start");
            block.setNextStatement(true, null);
            block.setColour(165);
            block.setInputsInline(false);
            // block.function = delegate (object o) {
            //     return block.connections[0].parentBlock.function(null);
            // };

            block.build(touchTransform);

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

        public void Update() {
          Touch touch = Input.GetTouch(0);
          touchTransform.position = touch.position;
        }

    }
}
