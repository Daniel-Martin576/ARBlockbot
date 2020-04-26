using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blockly
{
    public class BlockFactory
    {
        private const float LAYER_HEIGHT = 30;
        private const float LAYER_WIDTH = 15;
        private float total_width;

        private Block block;
        private Transform transform;
        private GameObject blockObj;
        private GameObject[,] spine;

        private Dictionary<Connection, GameObject> connectionDict;
        private List<List<GameObject>> rowLists;


        public BlockFactory(Block block, Transform transform)
        {
            this.block = block;
            this.transform = transform;
            rowLists = new List<List<GameObject>>();
            connectionDict = new Dictionary<Connection, GameObject>();
            build();
        }

        public void build()
        {
            blockObj = makeObject(block.name, transform);
            blockObj.AddComponent<GraphicRaycaster>();
            blockObj.tag = "Block";




            //makeSpine();

            BlockObject blockObject = blockObj.AddComponent<BlockObject>();
            // blockObject.addConnections(connectionDict);

            // Fields!!!!!!!!!!!!

            int row = 0;
            int i = 0;
            foreach (Input input in block.inputs)
            {
                if (row == rowLists.Count) rowLists.Add(new List<GameObject>{makeUnitSquare(getNextPosition(true))});

                foreach (Field field in input.fields)
                    addField(field); // call default adds getNextPosition(false)

                if (block.isInline(i))
                    addInlineConnection(input);
                else {
                    addExtender();
                    addExternalConnection(input);  //With Highlight, add in dict
                    row++;
                }

                if (block.isDoubleStatementInput(i)) {
                    addDummyRow();
                    row++;
                }

                i++;
            }


            // Excute!!!!!!!!!!!!
        }

        public void Update()
        {
            // Extenders
        }

        private GameObject makeObject(string name, Transform parentTransfrom) => makeObject(name, parentTransfrom, new Vector3(0, 0, 0));
        private GameObject makeObject(string name, Transform parentTransfrom, Vector3 localPosition)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parentTransfrom);
            obj.transform.localPosition = localPosition;
            return obj;
        }

        private GameObject makeUnitSquare(Vector3 localPosition, string name = "UnitBlock", string spriteName = "Square", bool highlight = false)
        {
            GameObject square = makeObject(name, blockObj.transform, localPosition); // Hope same name doesn't break anything
            Image image = square.AddComponent<Image>();
            RectTransform rect = image.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            image.sprite = Resources.Load<Sprite>(spriteName);
            image.color = !highlight ? block.color : new Color(1f, 1f, 0f, 0f);
            return square;
        }

        private Vector3 getNextPosition(bool newLine)
        {
            float total_height = 0f;
            float total_width = 0f;
            foreach (List<GameObject> row in rowLists)
            {
                if (!newLine && row == rowLists[rowLists.Count - 1]) break;
                RectTransform r = row[0].GetComponent<RectTransform>();
                total_height += (r == null) ? LAYER_HEIGHT : r.rect.width;
            }

            if (!newLine)
                foreach (GameObject obj in rowLists[rowLists.Count - 1])
                {
                    RectTransform r = obj.GetComponent<RectTransform>();
                    total_width += (r == null) ? LAYER_WIDTH : r.rect.width;
                }

            return new Vector3(total_width, -total_height, 0);
        }

        private void addDummyRow()
        {
            rowLists.Add(new List<GameObject> { makeUnitSquare(getNextPosition(true), "AnchorDummy") });
            rowLists[rowLists.Count - 1].Add(makeUnitSquare(getNextPosition(false), "Dummy"));
        }

        private void addSprite(GameObject obj, string spriteName, bool highlight = false)
        {
            Image image = obj.AddComponent<Image>();
            RectTransform rect = image.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            image.sprite = Resources.Load<Sprite>(spriteName);
            image.color = !highlight ? block.color : new Color(1f, 1f, 0f, 0f);
        }

        private void addInlineConnection(Input input)
        {
            Vector3 localPosition = getNextPosition(false);
            GameObject obj;

            if (input.category != Input.Category.Dummy)
            {
                obj = makeUnitSquare(localPosition, "InlineConnection", "HorzFemale");
                connectionDict.Add(input.connection, makeUnitSquare(localPosition, "Highlight", "VertHigh", true));
            } else
                obj = makeUnitSquare(localPosition, "InlineConnection", "Square");

            rowLists[rowLists.Count - 1].Add(obj);

            if (input.category != Input.Category.Dummy)
                rowLists[rowLists.Count - 1].Add(makeUnitSquare(getNextPosition(false), "Opening", "OpenSquare"));
        }

        private void addExternalConnection(Input input)
        {
            GameObject obj;
            Vector3 localPosition = getNextPosition(false);
            if (input.category == Input.Category.Statement)
                obj = makeUnitSquare(localPosition, "Connection", "VertMale");
            else if (input.category == Input.Category.Value)
                obj = makeUnitSquare(localPosition, "Connection", "HorzFemale");
            else
                obj = makeUnitSquare(localPosition, "Connection", "Square");

            if (input.category != Input.Category.Dummy)
                connectionDict.Add(input.connection, makeUnitSquare(localPosition, "Highlight", "VertHigh", true));

            rowLists[rowLists.Count - 1].Add(obj);
        }

        private void addExtender()
        {
            rowLists[rowLists.Count - 1].Add(makeUnitSquare(getNextPosition(false), "Extender"));
        }

        private void addField(Field field)
        {
            GameObject obj;
            if (field is FieldLabelSerializable)
                obj = makeTextBlock(((FieldLabelSerializable) field).text);
            else
                obj = makeTextBlock(field.name);
            rowLists[rowLists.Count - 1].Add(obj);
        }

        private GameObject makeTextBlock(string str)
        {
            // Add parent rect........ then make child
            GameObject parentRect = makeObject("TextBlock", blockObj.transform, getNextPosition(false));
            RectTransform r = parentRect.AddComponent<RectTransform>();
            r.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);

            GameObject fieldSquare = makeUnitSquare(new Vector3(0, 0, 0));
            fieldSquare.transform.SetParent(parentRect.transform);

            GameObject fieldObj = makeObject("Field", parentRect.transform, new Vector3(0, 0, 0));

            Text text = fieldObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 50;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = str;
            text.color = new Color(0, 0, 0);
            text.supportRichText = false;

            int width = CalculateLengthOfMessage(text, text.text);

            RectTransform rect = text.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (float)width / 2);
            rect.localScale = new Vector3(.5f, .5f, 0f);

            float afterFieldGap = 0;
            RectTransform rect1 = fieldSquare.GetComponent<RectTransform>();
            rect1.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            rect1.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (float)width / 2 + afterFieldGap);


           // r.transform.position.Set(r.transform.position.x + (rect1.sizeDelta.x - r.sizeDelta.x) / 2, r.transform.position.y, r.transform.position.z);
            //r.sizeDelta = rect1.sizeDelta;

            return fieldObj;
        }













        private GameObject makeSquare(int r, int c, string spriteName, string name = "Square", bool hide = false)
        {
            GameObject square = makeObject($"{name}_{r}_{c}", blockObj.transform, new Vector3(c * LAYER_WIDTH, -r * LAYER_HEIGHT, 0));
            Image image = square.AddComponent<Image>();
            RectTransform rect = image.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);

            image.sprite = Resources.Load<Sprite>(spriteName);
            image.color = block.color;
            if (hide)
                image.color = new Color(1f, 1f, 0f, 0f);

            return square;
        }


        private void makeSpine()
        {
            connectionDict = new Dictionary<Connection, GameObject>();
            spine = new GameObject[block.inputs.Count,2];
            int row = 0;
            for (int i = 0; i < block.inputs.Count; i++, row++)
            {
                spine[i, 0] = makeSquare(row, 0, "Square");
                switch (block.inputs[i].category)
                {
                    case Input.Category.Value:
                        spine[i, 1] = makeSquare(row, 1, "HorzFemale");
                        connectionDict.Add(block.inputs[i].connection, makeSquare(row, 1, "HorzHigh", "Highlight", true));
                        break;
                    case Input.Category.Dummy:
                        spine[i, 1] = makeSquare(row, 1, "Square");
                        break;
                    case Input.Category.Statement:
                        spine[i, 1] = makeSquare(row, 1, "VertMale");
                        connectionDict.Add(block.inputs[i].connection, makeSquare(row, 1, "VertHigh", "Highlight", true));
                        if (((i + 1 < block.inputs.Count - 1) && block.inputs[i + 1].category == Input.Category.Statement)
                            || (i == block.inputs.Count - 1))
                        {
                            row++;
                            makeSquare(row, 0, "Square");
                            makeSquare(row, 1, "Square");
                        }
                        break;
                }
            }

            foreach(Connection connection in block.connections)
            {
                if (connection.category == Connection.Category.Output) {
                    makeSquare(0, -1, "HorzMale");
                    connectionDict.Add(connection, makeSquare(0, -1, "HorzHigh", "Highlight", true));
                } else if (connection.category == Connection.Category.Prev) {
                    spine[0, 0].GetComponent<Image>().sprite = Resources.Load<Sprite>("VertFemale");
                    connectionDict.Add(connection, makeSquare(0, 0, "VertHigh", "Highlight", true));
                } else if (connection.category == Connection.Category.Next) {
                    makeSquare(row, 0, "VertMale");
                    connectionDict.Add(connection, makeSquare(row, 0, "VertHigh", "Highlight", true));
                }
            }
        }





        private GameObject makeField(int i, int f, Transform parentTransfrom, Field field)
        {
            // Add parent rect........ then make child
            GameObject parentRect = makeObject($"KingRect_{i}_{f}", parentTransfrom, new Vector3(0, 0, 0));
            RectTransform r = parentRect.AddComponent<RectTransform>();
            r.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            GameObject fieldSquare = makeObject($"FieldSquare_{i}_{f}", parentRect.transform, new Vector3(0, 0, 0));
            GameObject fieldObj = makeObject($"Field_{i}_{f}", parentRect.transform, new Vector3(0, 0, 0));

            Text text = fieldObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 50;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Start";
            text.color = new Color(0, 0, 0);
            text.supportRichText = false;

            int width = CalculateLengthOfMessage(text, text.text);
            Debug.Log(width);

            RectTransform rect = text.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (float) width / 2);
            rect.localScale = new Vector3(.5f, .5f, 0f);


            Image image = fieldSquare.AddComponent<Image>();
            RectTransform rect1 = image.gameObject.GetComponent<RectTransform>();
            rect1.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);

            image.sprite = Resources.Load<Sprite>("Square");
            image.color = block.color;
            rect1.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (float)width / 2);

            //ughhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh

            return fieldObj;
        }

        private int CalculateLengthOfMessage(Text chatText, string message)
        {
            int totalLength = 0;
            Font myFont = chatText.font;  //chatText is my Text component
            CharacterInfo characterInfo = new CharacterInfo();
            char[] arr = message.ToCharArray();
            int i = 0;
            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, chatText.fontSize);
                totalLength += characterInfo.advance;
                if (i == arr.Length - 1)
                {
                    totalLength += characterInfo.glyphWidth/2;
                }
                i++;
            }
            return totalLength;
        }
    }
}
