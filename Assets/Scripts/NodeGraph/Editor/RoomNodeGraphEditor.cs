using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

    // Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]

    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

       private void OnEnable()
    {
        // Subscribe to the inspector selection changed event
        // Selection.selectionChanged += InspectorSelectionChanged;

        // Define node layout style
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        // Define selected node style
        // roomNodeSelectedStyle = new GUIStyle();
        // roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        // roomNodeSelectedStyle.normal.textColor = Color.white;
        // roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        // roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        // Load Room node types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// <summary>
    /// Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
    /// </summary>

    [OnOpenAsset(0)]  // Need the namespace UnityEditor.Callbacks
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if (roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }

    //Draw Editor Gui
    private void OnGUI()
    {
        // If a scriptable object of type RoomNodeGraphSO has been selected then process
        if (currentRoomNodeGraph != null)
        {
            // Draw Grid
            // DrawBackgroundGrid(gridSmall, 0.2f, Color.gray);
            // DrawBackgroundGrid(gridLarge, 0.3f, Color.gray);

            // Draw line if being dragged
            // DrawDraggedLine();

            // Process Events
            ProcessEvents(Event.current);

            // Draw Connections Between Room Nodes
            // DrawRoomConnections();

            // Draw Room Nodes
            DrawRoomNodes();
        }

        if (GUI.changed)
            Repaint();
    }

    private void ProcessEvents(Event currentEvent)
    {
         ProcessRoomNodeGraphEvents(currentEvent);
        

        // Reset graph drag
        // graphDrag = Vector2.zero;

        // Get room node that mouse is over if it's null or not currently being dragged
        // if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        // {
        //     currentRoomNode = IsMouseOverRoomNode(currentEvent);
        // }

        // if mouse isn't over a room node or we are currently dragging a line from the room node then process graph events
        // if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        // {
        //     ProcessRoomNodeGraphEvents(currentEvent);
        // }
        // else process room node events
        // else
        // {
        //     // process room node events
        //     currentRoomNode.ProcessEvents(currentEvent);
        // }
    }

    /// <summary>
    /// Process Room Node Graph Events
    /// </summary>
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            // Process Mouse Up Events
            // case EventType.MouseUp:
            //     ProcessMouseUpEvent(currentEvent);
            //     break;

            // Process Mouse Drag Event
            // case EventType.MouseDrag:
            //     ProcessMouseDragEvent(currentEvent);

            //     break;

            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse down events on the room node graph (not over a node)
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // Process right click mouse down on graph event (show context menu)
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        // Process left mouse down on graph event
        // else if (currentEvent.button == 0)
        // {
        //     ClearLineDrag();
        //     ClearAllSelectedRoomNodes();
        // }
    }

    /// <summary>
    /// Show the context menu
    /// </summary>
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
        // menu.AddSeparator("");
        // menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);
        // menu.AddSeparator("");
        // menu.AddItem(new GUIContent("Delete Selected Room Node Links"), false, DeleteSelectedRoomNodeLinks);
        // menu.AddItem(new GUIContent("Delete Selected Room Nodes"), false, DeleteSelectedRoomNodes);

        menu.ShowAsContext();
    }

    /// <summary>
    /// Create a room node at the mouse position
    /// </summary>
    private void CreateRoomNode(object mousePositionObject)
    {
        // If current node graph empty then add entrance room node first
        // if (currentRoomNodeGraph.roomNodeList.Count == 0)
        // {
        //     CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        // }

        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    /// <summary>
    /// Create a room node at the mouse position - overloaded to also pass in RoomNodeType
    /// </summary>
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        // create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        // add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        // set room node values
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        // add room node to room node graph scriptable object asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();

        // Refresh graph node dictionary
        // currentRoomNodeGraph.OnValidate();
    }

    /// <summary>
    /// Draw room nodes in the graph window
    /// </summary>
    private void DrawRoomNodes()
    {
        // Loop through all room nodes and draw themm
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {

            roomNode.Draw(roomNodeStyle);

            // if (roomNode.isSelected)
            // {
            //     roomNode.Draw(roomNodeSelectedStyle);
            // }
            // else
            // {
            //     roomNode.Draw(roomNodeStyle);
            // }
        }

        GUI.changed = true;
    }
}
