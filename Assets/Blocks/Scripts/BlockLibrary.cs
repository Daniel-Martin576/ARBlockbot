using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blockly 
{
    public class BlockLibrary : MonoBehaviour
    {
        public void createStartBlock()
        {

            Block block = new Block("StartBlock");
            block.appendDummyInput()
                .appendField("Start")
                .appendField(new Blockly.FieldLabelSerializable("lol"), "NAME");
            block.appendValueInput("NAME")
                .setCheck(null);
            block.appendStatementInput("NAME")
                .setCheck(null);
            block.setPreviousStatement(true, null);
            block.setNextStatement(true, null);
            block.setOutput(true, null);
            block.setColour(165);
            block.build(transform);
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