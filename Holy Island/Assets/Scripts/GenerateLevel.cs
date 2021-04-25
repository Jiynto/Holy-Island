using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GenerateLevel : MonoBehaviour
{
    //TODO: set this all up in a coroutine.

    private GameObject[] RoomPrefabs;

    private bool finished = false;

    private Queue<ConnectionController> connections = new Queue<ConnectionController>();
    private (ConnectionController, List<GameObject>) currentConnection;
    private GameObject currentRoomPrefab;
    private Room currentRoom;
    private Queue<ConnectionController> roomConnections = new Queue<ConnectionController>();


    // Start is called before the first frame update
    void Start()
    {
        RoomPrefabs = Resources.LoadAll<GameObject>("Rooms/Prefabs");

        GameObject startRoom = Resources.Load("Rooms/Prefabs/Room_Start") as GameObject;

        //GameObject startRoom = RoomPrefabs[Random.Range(0, RoomPrefabs.Length)];
        //while (startRoom.GetComponent<Room>().Type != RoomType.Room)
           // startRoom = RoomPrefabs[Random.Range(0, RoomPrefabs.Length)];

        startRoom = Instantiate(startRoom);
        startRoom.GetComponent<Room>().Set();

        ConnectionController[] connectionControllers = startRoom.GetComponentsInChildren<ConnectionController>();
        foreach (ConnectionController controller in connectionControllers)
        {
            connections.Enqueue(controller);
        }

        ConnectionController connection = connections.Dequeue();
        List<GameObject> roomsToCheck = new List<GameObject>();
        foreach(GameObject roomPrefab in RoomPrefabs)
        {
            if(connection.ValidConnectionTypes.Contains(roomPrefab.GetComponent<Room>().Type))
            {
                roomsToCheck.Add(roomPrefab);
            }
        }
            //RoomPrefabs.Where(room => connection.ValidConnectionTypes.Contains(room.GetComponent<Room>().Type)).ToList();
        currentConnection = (connection, roomsToCheck);

        
    }


    private void FixedUpdate()
    {
        if(currentRoom == null)
        {
            GenerationStep();
        }
        else
        {
            if (currentRoom.IsSet)
            {
                NextConnection();
                GenerationStep();
                //finished = true;
            }
            else if (currentConnection.Item2.Count == 0 && roomConnections.Count == 0)
            {
                currentRoom.gameObject.transform.MoveToLayer(10);
                Destroy(currentRoom.gameObject);
                currentRoom = null;
                GameObject wall = Resources.Load("Rooms/ComponentPrefabs/wall_pillars") as GameObject;
                wall = Instantiate(wall);
                wall.transform.position = currentConnection.Item1.transform.position;
                wall.transform.Rotate(0, currentConnection.Item1.Rotation + 90, 0);
                NextConnection();
                GenerationStep();

            }
            else
            {
                //currentConnection.Item1.IsConnected = true;
                //finished = true;
                currentRoom.gameObject.transform.MoveToLayer(10);
                Destroy(currentRoom.gameObject);
                currentRoom = null;
                GenerationStep();
            }
        }



 
        
       
    }
    /// <summary>
    /// Method for moving to the next connection in the queue if any exist.
    /// </summary>
    private void NextConnection()
    {
        currentConnection.Item1.IsConnected = true;
        // if there is a working room, i.e. the connection isnt to a wall.
        if (currentRoom != null)
        {
            currentRoom.Set();

            // find all the connections of the working room...
            ConnectionController[] connectionControllers = currentRoom.transform.GetComponentsInChildren<ConnectionController>();
            foreach (ConnectionController controller in connectionControllers)
            {
                // if the connection is in the same position as the current connection.
                if (controller.HasCollided)
                {
                    // set it to connected
                    controller.IsConnected = true;
                }
                else
                {
                    //If this connection is connected but is not in the position of the current connection
                    //something has gone wrong.
                    if(controller.IsConnected)
                    {
                        throw new System.Exception("working rooms has multiple set connections");
                    }


                    // calculate rotations and enqueue other connections.
                    float rotation = controller.Rotation + currentRoom.Rotation;
                    if (rotation >= 360)
                    {
                        rotation = rotation - 360;
                    }
                    controller.Rotation = rotation;
                    connections.Enqueue(controller);
                }
            }
            
        }

        // clear the working room.
        currentRoom = null;
        currentConnection = (null, null);

        // if there are remaining connections to work on
        if (connections.Count > 0)
        {
            /*
            if (connections.Where(x => x.IsConnected == true).Count() > 0)
            {
                throw new System.Exception("thing happened");
            }
            */

            ConnectionController connection = connections.Dequeue();
            List<GameObject> roomsToCheck = RoomPrefabs.Where(room => connection.ValidConnectionTypes.Contains(room.GetComponent<Room>().Type)).ToList();
            currentConnection = (connection, roomsToCheck);
            currentRoomPrefab = null;
            roomConnections.Clear();
        }
        else
        {
            finished = true;
        }

        
    }






    private void GenerationStep()
    {
        //If we are not finished.
        if(!finished)
        {
            //If the currentRoomPrefab has not been selected yet
            //Or we have exhausted the possible orientations of the current working room.
            if (currentRoomPrefab == null || roomConnections.Count == 0)
            {
                //Select a random room prefab from the valid prefabs list.
                currentRoomPrefab = currentConnection.Item2[Random.Range(0, currentConnection.Item2.Count)];

                //Remove selected prefab to ensure it doesnt get examined twice.
                currentConnection.Item2.Remove(currentRoomPrefab);

                //Add each of the new prefabs connections to the roomConections queue.
                foreach (ConnectionController newConnection in currentRoomPrefab.GetComponentsInChildren<ConnectionController>())
                {
                    roomConnections.Enqueue(newConnection);
                }
            }


            //Dequeue the first room connection. 
            ConnectionController possibleConnection = roomConnections.Dequeue();

            //Instantiate a fresh room.
            GameObject newRoom = Instantiate(currentRoomPrefab);

            //Position the room based on the current possible connection.
            Vector3 positionTransform = currentConnection.Item1.transform.position - possibleConnection.transform.position;
            newRoom.transform.position += positionTransform;

            //Rotate the room around the position of the current room connection.
            //This should be in the same position as the possible connection on the instantiated version of the room prefab.
            float rotationValue = Mathf.Abs(180 - (possibleConnection.Rotation - currentConnection.Item1.Rotation));
            newRoom.transform.RotateAround(currentConnection.Item1.transform.position, Vector3.up, rotationValue);

            //set the current working room to the controller of the new room
            currentRoom = newRoom.GetComponent<Room>();
            //set the rotation value of the working room.
            currentRoom.Rotation = rotationValue;
            currentRoom.GetComponent<Room>().IsSet = true;




            //GameObject temp = Instantiate(currentRoomPrefab, newRoom.transform.position, newRoom.transform.rotation);
            //currentRoom = temp.GetComponent<Room>();
            //Destroy(newRoom);
        }
        else
        {
            NavMeshSurface Surface = Object.FindObjectOfType<NavMeshSurface>();
            Surface.BuildNavMesh();
            LevelController levelController = Object.FindObjectOfType<LevelController>();
            levelController.Initialise();
            //Remove the generator so its update isnt called unnecessarily.
            Destroy(this);
        }

    }
}
