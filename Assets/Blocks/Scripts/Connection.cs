using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blockly
{
    public class Connection
    {
        public Category category;
        public Type[] types;
        public bool lead;
        public Connection pair;

        private bool vert;

        public enum Category { Prev, Next, Output, Input }
        public enum Type { Boolean, Number, String, Array }

        private Type convertType(string str)
        {
            switch (str)
            {
                case ("Boolean"): return Type.Boolean;
                case ("Number"): return Type.Number;
                case ("String"): return Type.String;
                case ("Array"): return Type.Array;
                default: throw new System.FormatException();
            }
        }

        private Type[] stringsToTypeList(string[] typesStr)
        {
            List<Type> list = new List<Type>();
            if (typesStr == null)
                foreach (Type t in System.Enum.GetValues(typeof(Type)))
                    list.Add(t);
            else
                foreach (string typeStr in typesStr)
                    list.Add(convertType(typeStr));
            return list.ToArray();
        }


        public Connection(Category category, string[] typesStr)
        {
            this.category = category;
            types = stringsToTypeList(typesStr);
            lead = (category == Category.Next || category == Category.Input);
            vert = (category == Category.Next || category == Category.Prev);
            pair = null;
        }

        static public bool canConnect(Connection con1, Connection con2)
        {
            return ((con1.lead && !con2.lead && Connection.typeCompatible(con1, con2))
                    || (con2.lead && !con1.lead && Connection.typeCompatible(con2, con1)))
                    && (con1.vert == con2.vert) && (con1.pair == null && con2.pair == null);
        }

        static private bool typeCompatible(Connection parent, Connection child)
        {
            foreach(Type childType in child.types)
            {
                bool found = false;
                foreach (Type parentType in parent.types)
                    if (childType == parentType)
                    {
                        found = true;
                        break;
                    }
                if (found == false) return false;    
            }
            return true;
        }

        public void join(Connection con) => pair = con;
        public void unjoin() => pair = null;
        public bool linked() => pair != null;
    }
}
