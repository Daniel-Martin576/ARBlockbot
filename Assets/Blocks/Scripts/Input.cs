using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Blockly
{
    public class Input
    {
        public Category category;
        public string name;
        public Connection connection;
        public List<Field> fields;
        public enum Category { Value, Dummy, Statement }

        public Input(Category category, string name)
        {
            this.category = category;
            this.name = name;
            fields = new List<Field>();
        }

        public Input setCheck(string[] str)
        {
            if (category == Category.Value)
                connection = new Connection(Connection.Category.Input, str);
            else if (category == Category.Statement)
                connection = new Connection(Connection.Category.Next, str);
            return this;
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            System.Random random = new System.Random();
            for (int i = 0; i < size; i++)
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            return builder.ToString();
        }

        public Input appendField(string text)
        {
            fields.Add(new FieldLabelSerializable(text));
            return this;
        }

        public Input appendField(Field field, string name)
        {
            fields.Add(field);
            field.name = name;
            return this;
        }
    }
}
