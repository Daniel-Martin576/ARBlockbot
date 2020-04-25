using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blockly
{
    public class Field {
        public string name;
    }

    public class FieldLabelSerializable : Field
    {
        public string text;
        public FieldLabelSerializable(string text) => this.text = text;
    }

    public class FieldTextInput : Field
    {
        public string text;
        public FieldTextInput(string text) => this.text = text;
    }

    public class FieldNumber : Field
    {
        public float number;
        public FieldNumber(float number) => this.number = number;
    }

    public class FieldAngle : Field
    {
        public float angle;
        public FieldAngle(float angle) => this.angle = angle;
    }

    public class FieldDropdown : Field
    {
        public FieldDropdown(string[,] options) { }
    }

    public class FieldCheckbox : Field
    {
        public bool check;
        public FieldCheckbox(string boolStr) => check = System.Boolean.Parse(boolStr);
    }

    public class FieldColour : Field
    {
        public FieldColour(string colorStr) { }
    }

    public class FieldVariable : Field
    {
        public FieldVariable(string variable) { }
    }
}
