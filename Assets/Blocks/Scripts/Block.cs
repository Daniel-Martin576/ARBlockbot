using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

namespace Blockly
{
    public class Block
    {
        public string name;
        public List<Input> inputs;
        public List<Connection> connections;
        public Color color;
        public bool inline;
        public Func<object, object> function;
        public bool start;
        public BuggyBuddy buggyBuddy;

        private bool[] whereInline;

        public Block(string name)
        {
            this.name = name;
            inputs = new List<Input>();
            connections = new List<Connection>();
            inline = true;
            function = delegate (object o) { return null; };
            start = false;
        }

        public Input appendValueInput(string name)
        {
            Input input = new Input(Input.Category.Value, name, this);
            inputs.Add(input);
            return input;
        }

        public Input appendDummyInput()
        {
            Input input = new Input(Input.Category.Dummy, null, this);
            inputs.Add(input);
            return input;
        }

        public Input appendStatementInput(string name)
        {
            Input input = new Input(Input.Category.Statement, name, this);
            inputs.Add(input);
            return input;
        }

        public void setOutput(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Output, str, this));
        public void setPreviousStatement(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Prev, str, this));
        public void setNextStatement(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Next, str, this));

        public void setColour(float H) => color = Color.HSVToRGB(H / 360.0f, 0.5f, 1.0f, false);
        public void setColour(float H, float S, float V) => color = Color.HSVToRGB(H, S, V, false);

        public void setInputsInline(bool inline) => this.inline = inline;

        public void setTooltip(string str) { }
        public void setHelpUrl(string str) { }

        public void setStart(bool start) => this.start = start;

        public object callNext() {
            Block othBlock = null;
            foreach (Connection connection in connections)
                if (connection.category == Connection.Category.Next)
                    othBlock = connection.connectedBlock();
            return (othBlock != null) ? othBlock.function(null) : null;
        }

        public object callInput(string name)
        {
            Block othBlock = null;
            foreach (Input input in inputs)
                if (input.name == name && input.connection != null)
                    othBlock = input.connection.connectedBlock();
            return (othBlock != null) ? othBlock.function(null) : null;
        }

        public GameObject build(Transform transform) => (new BlockFactory(this, transform)).build();



        // ~~~~~~ Helper Functions ~~~~~~
        private bool[] builcWhereInline()
        {
            bool currInline = false;
            bool[] isInline = new bool[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
            {   
                if (currInline && inputs[i].category != Input.Category.Statement)
                    isInline[i] = true;
                else if (inputs[i].category == Input.Category.Statement)
                    isInline[i] = false;
                if (!currInline && inputs[i].category == Input.Category.Dummy)
                {
                    int j = i;
                    while (j >= 0)
                    {
                        if (inputs[j].category != Input.Category.Statement)
                            isInline[j] = true;
                        else break;
                        j--;
                    }
                }
                currInline = isInline[i];
            }
            return isInline;
        }

        public bool isInline(int index)
        {
            if (!inline) return false;
            if (whereInline == null) whereInline = builcWhereInline();
            return whereInline[index];
        }

        public bool inputNeedsFloor(int index)
        {
            return (inputs[index].category == Input.Category.Statement 
                && index + 1 < inputs.Count
                && inputs[index + 1].category == Input.Category.Statement)
                || (index == inputs.Count - 1 && inputs[index].category == Input.Category.Statement);
        }

        public object execute(object o)
        {
            return null;
        }
    }
}
