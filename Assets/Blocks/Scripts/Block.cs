using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blockly
{
    public class Block
    {
        public string name;
        public List<Input> inputs;
        public List<Connection> connections;
        public Color color;
        public bool inline;

        private bool[] whereInline;

        public Block(string name)
        {
            this.name = name;
            inputs = new List<Input>();
            connections = new List<Connection>();
            inline = true;
        }

        public Input appendValueInput(string name)
        {
            Input input = new Input(Input.Category.Value, name);
            inputs.Add(input);
            return input;
        }

        public Input appendDummyInput()
        {
            Input input = new Input(Input.Category.Dummy, null);
            inputs.Add(input);
            return input;
        }

        public Input appendStatementInput(string name)
        {
            Input input = new Input(Input.Category.Statement, name);
            inputs.Add(input);
            return input;
        }

        public void setOutput(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Output, str));
        public void setPreviousStatement(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Prev, str));
        public void setNextStatement(bool _, string[] str) => connections.Add(new Connection(Connection.Category.Next, str));

        public void setColour(float H) => color = Color.HSVToRGB(H / 360.0f, 1.0f, 1.0f, false);
        public void setColour(float H, float S, float V) => color = Color.HSVToRGB(H, S, V, false);

        public void setInputsInline(bool inline) => this.inline = inline;

        public void setTooltip(string str) { }
        public void setHelpUrl(string str) { }

        public void build(Transform transform) => new BlockFactory(this, transform);



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

        public bool isDoubleStatementInput(int index)
        {
            return inputs[index].category == Input.Category.Statement 
                && index + 1 < inputs.Count 
                && inputs[index + 1].category == Input.Category.Statement;
        }




    }
}
