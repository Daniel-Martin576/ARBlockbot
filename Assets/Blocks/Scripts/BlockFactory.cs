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

        private Block block;
        private Transform transform;
        private GameObject blockObj;

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
            BlockObject blockObject = blockObj.AddComponent<BlockObject>();

            int row = 0;
            int i = 0;
            foreach (Input input in block.inputs)
            {
                if (row == rowLists.Count) rowLists.Add(new List<GameObject>{ makeParentedBox(getNextPosition(true), "Extender")});
                
                foreach (Field field in input.fields)
                    addField(field); // call default adds getNextPosition(false)

                if (block.isInline(i)) {
                    addInlineConnection(input);
                    if (i + 1 < block.inputs.Count && !block.isInline(i + 1)) {
                        addExtender();
                        row++;
                    }
                }
                else {
                    addExternalConnection(input);  //With Highlight, add in dict
                    row++;
                }
                
                if (block.inputNeedsFloor(i)) {
                    addDummyRow();
                    row++;
                }

                i++;
            }
            addMainConnections();

            adjustSize();

            blockObject.addConnections(connectionDict);

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

        private GameObject makeUnitSquare(Vector3 localPosition, Transform parentTransfrom) => makeUnitSquare(localPosition, "UnitBlock", "Square", false, parentTransfrom);
        private GameObject makeUnitSquare(Vector3 localPosition, string name = "UnitBlock", string spriteName = "Square", bool highlight = false, Transform parentTransfrom = null)
        {
            if (parentTransfrom == null) parentTransfrom = blockObj.transform;
            GameObject square = makeObject(name, parentTransfrom, localPosition); // Hope same name doesn't break anything
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
                total_height += (r == null) ? LAYER_HEIGHT : r.rect.height;
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
            rowLists.Add(new List<GameObject> { makeParentedBox(getNextPosition(true), "Extender") });
            rowLists[rowLists.Count - 1].Add(makeParentedBox(getNextPosition(false), "Extender"));

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

            if (input.category == Input.Category.Value)
            {
                GameObject obj = makeUnitSquare(localPosition, "InlineConnection", "HorzFemale");
                connectionDict.Add(input.connection, makeUnitSquare(localPosition, "Highlight", "VertHigh", true));
                rowLists[rowLists.Count - 1].Add(obj);
                rowLists[rowLists.Count - 1].Add(makeUnitSquare(getNextPosition(false), "Opening", "OpenSquare"));
            }
        }

        private void addExternalConnection(Input input)
        {   
            GameObject obj;

            if (input.category == Input.Category.Statement) {
                Vector3 localPosition = getNextPosition(false);
                obj = makeUnitSquare(localPosition, "StatementEndConnection", "VertMale");
                connectionDict.Add(input.connection, makeUnitSquare(localPosition, "Highlight", "VertHigh", true));
                rowLists[rowLists.Count - 1].Add(obj);
            } else if (input.category == Input.Category.Value) {
                addExtender();
                Vector3 localPosition = getNextPosition(false);
                obj = makeUnitSquare(localPosition, "ValueEndConnection", "HorzFemale");
                connectionDict.Add(input.connection, makeUnitSquare(localPosition, "Highlight", "HorzHigh", true));
                rowLists[rowLists.Count - 1].Add(obj);
            } else
                addExtender();
        }

        private void addExtender()
        {
            rowLists[rowLists.Count - 1].Add(makeParentedBox(getNextPosition(false), "Extender"));
        }

        private GameObject makeParentedBox(Vector3 localPosition, string name = "UnitBlock")
        {
            GameObject parentRect = makeObject(name, blockObj.transform, localPosition);
            RectTransform r = parentRect.AddComponent<RectTransform>();
            r.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);

            GameObject square = makeUnitSquare(new Vector3(0, 0, 0), parentRect.transform);
            return parentRect;
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

            GameObject fieldSquare = makeUnitSquare(new Vector3(0, 0, 0), parentRect.transform);
            fieldSquare.transform.SetParent(parentRect.transform);

            GameObject fieldObj = makeObject("Field", parentRect.transform, new Vector3(0, 0, 0));

            Text text = fieldObj.AddComponent<Text>();
            text.font = Resources.Load<Font>("Anonymous_Pro");
            text.fontSize = 50;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = str;
            text.color = new Color(0, 0, 0);
            text.supportRichText = false;

            float shrink = .3f;
            float magic_size = 8.2f;
            float width = str.Length * magic_size;

           
            RectTransform rect = text.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
            rect.localScale = new Vector3(shrink, shrink, 0f);

            float afterFieldGap = 5;
            RectTransform rect1 = fieldSquare.GetComponent<RectTransform>();
            rect1.sizeDelta = new Vector2(LAYER_WIDTH, LAYER_HEIGHT);
            rect1.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width + afterFieldGap);

            r.transform.position = new Vector3(r.transform.position.x + (rect1.sizeDelta.x - r.sizeDelta.x) / 2, r.transform.position.y, r.transform.position.z);
            r.sizeDelta = rect1.sizeDelta;

            return parentRect;
        }

        private void addMainConnections()
        {
            foreach (Connection connection in block.connections)
            {
                if (connection.category == Connection.Category.Prev)
                {
                    rowLists[0][0].GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("VertFemale");
                    connectionDict.Add(connection, makeUnitSquare(new Vector3(0,0,0), "Highlight", "VertHigh", true));
                }
                else if (connection.category == Connection.Category.Next)
                {
                    Vector3 localPostion = getNextPosition(true);
                    makeUnitSquare(localPostion, "Next", "VertMale");
                    connectionDict.Add(connection, makeUnitSquare(localPostion, "Highlight", "VertHigh", true));
                } else if (connection.category == Connection.Category.Output)
                {
                    makeUnitSquare(new Vector3(-LAYER_WIDTH, 0, 0), "Next", "HorzMale");
                    connectionDict.Add(connection, makeUnitSquare(new Vector3(-LAYER_WIDTH,0,0), "Highlight", "HorzHigh", true));
                }
            }
        }

        private void adjustSize()
        {
            float max_width1 = 0f;
            float max_width2 = 0f;
            // total_height = 0;


            bool[] rowShort = new bool[rowLists.Count];
            float[] rowWidth = new float[rowLists.Count];
            int i = 0;
            // Calculate
            foreach (List<GameObject> row in rowLists)
            {
                float width = 0;
                foreach (GameObject obj in row)
                {
                    RectTransform r = obj.GetComponent<RectTransform>();
                    width += (r == null) ? LAYER_WIDTH : r.rect.width;
                }

                if (rowShort[i] = (row[row.Count - 1].name == "StatementEndConnection"))
                    if (width > max_width1)
                        max_width1 = width;
                if (width > max_width2)
                    max_width2 = width;

                rowWidth[i] = width;
                i++;
            }

            i = 0;
            foreach (List<GameObject> row in rowLists)
            {
                Debug.Log(rowShort[i]);
                if (rowShort[i] && row[0].name == "Extender")
                {
                    RectTransform rect = row[0].transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                    rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, LAYER_WIDTH + (max_width1 - rowWidth[i]));
                    int j = 0;
                    foreach (GameObject obj in row) 
                        if (j++ != 0)
                            obj.transform.position = obj.transform.position + new Vector3((max_width1 - rowWidth[i]), 0, 0);  
                }
                else
                {
                    GameObject obj = null;
                    if (row.Count - 1 >= 0 && row[row.Count - 1].name == "Extender")
                        obj = row[row.Count - 1];
                    else if (row.Count - 2 >= 0 && row[row.Count - 2].name == "Extender")
                    {
                        obj = row[row.Count - 2];
                        row[row.Count - 1].transform.position = row[row.Count - 1].transform.position + new Vector3((max_width2 - rowWidth[i]), 0, 0);
                    }
                    

                    if (obj != null)
                    {
                        RectTransform rect = obj.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, LAYER_WIDTH + (max_width2 - rowWidth[i]));
                    }
                            
                }
                i++;
            }
            
        }

    }
}
