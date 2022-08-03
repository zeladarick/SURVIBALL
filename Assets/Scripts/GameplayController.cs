using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;
using System.Threading;
using System;


public class GameplayController : MonoBehaviour
{

    float timeLapse = 0;
    int actualSecond = 0;
    int secondsBound = 15;

    public GameObject ghostPrefab;
    GameObject bot1;

    InputPlayerStatus mainPlayerStatus;

    InputPlayerStatus bot1Status;

    InputAllPlayerStatus allPlayersStatus;

    int assignedId;

    // Start is called before the first frame update
    void Start()
    {
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect("127.0.0.1");
        }).Start();

        bot1 = Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity);
        bot1Status = new InputPlayerStatus();

        bot1Status.playerNumber = -1;
        bot1Status.name = "";
        bot1Status.color = "";
        bot1Status.isAlive = true;
        bot1Status.positionX = 0;
        bot1Status.positionY = 0;
        bot1Status.animation_isAlive = true;
        bot1Status.animation_isTouchingTheGround = true;


        mainPlayerStatus = new InputPlayerStatus();

        mainPlayerStatus.playerNumber = 0;
        mainPlayerStatus.name = "";
        mainPlayerStatus.color = "";
        mainPlayerStatus.isAlive = true;
        mainPlayerStatus.positionX = 0;
        mainPlayerStatus.positionY = 0;
        mainPlayerStatus.animation_isAlive = true;
        mainPlayerStatus.animation_isTouchingTheGround = true;

        allPlayersStatus = new InputAllPlayerStatus();

        allPlayersStatus.playerNumber0 = 0;
        allPlayersStatus.name0 = "";
        allPlayersStatus.color0 = "";
        allPlayersStatus.isAlive0 = true;
        allPlayersStatus.positionX0 = 0;
        allPlayersStatus.positionY0 = 0;
        allPlayersStatus.animation_isAlive0 = true;
        allPlayersStatus.animation_isTouchingTheGround0 = true;

        allPlayersStatus.playerNumber1 = 1;
        allPlayersStatus.name1 = "";
        allPlayersStatus.color1 = "";
        allPlayersStatus.isAlive1 = true;
        allPlayersStatus.positionX1 = -3;
        allPlayersStatus.positionY1 = -5;
        allPlayersStatus.animation_isAlive1 = true;
        allPlayersStatus.animation_isTouchingTheGround1 = true;

        allPlayersStatus.playerNumber2 = 2;
        allPlayersStatus.name2 = "";
        allPlayersStatus.color2 = "";
        allPlayersStatus.isAlive2 = true;
        allPlayersStatus.positionX2 = 0;
        allPlayersStatus.positionY2 = 0;
        allPlayersStatus.animation_isAlive2 = true;
        allPlayersStatus.animation_isTouchingTheGround2 = true;

        allPlayersStatus.playerNumber3 = 3;
        allPlayersStatus.name3 = "";
        allPlayersStatus.color3 = "";
        allPlayersStatus.isAlive3 = true;
        allPlayersStatus.positionX3 = 0;
        allPlayersStatus.positionY3 = 0;
        allPlayersStatus.animation_isAlive3 = true;
        allPlayersStatus.animation_isTouchingTheGround3 = true;

        allPlayersStatus.playerNumber4 = 4;
        allPlayersStatus.name4 = "";
        allPlayersStatus.color4 = "";
        allPlayersStatus.isAlive4 = true;
        allPlayersStatus.positionX4 = 0;
        allPlayersStatus.positionY4 = 0;
        allPlayersStatus.animation_isAlive4 = true;
        allPlayersStatus.animation_isTouchingTheGround4 = true;


    }

    void Connect(string server)
    {
        
        try
        {
            Debug.Log("Iniciando conexion");
            int port = 1225;
            TcpClient client = new TcpClient(server, port);

            NetworkStream stream = client.GetStream();

            int count = 0;
            while (true)
            {


                string serializedPlayerInfo = JsonUtility.ToJson(mainPlayerStatus);


                Byte[] data = System.Text.Encoding.ASCII.GetBytes(serializedPlayerInfo);
                stream.Write(data, 0, data.Length);
                data = new Byte[4096];
                String response = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);







                
                allPlayersStatus = JsonUtility.FromJson<InputAllPlayerStatus>(response);
                
                bot1Status = getSpecificPlayer(1);

                Debug.Log("X: " + bot1Status.positionX);
                Debug.Log("Y: " + bot1Status.positionY);

                Thread.Sleep(2000);
            }

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
        }

    }

    // Update is called once per frame
    void Update()
    {

        bot1.transform.position = Vector3.Lerp( bot1.transform.position ,new Vector3( bot1Status.positionX, bot1Status.positionY), 0.5f);

        GameObject playerReference = GameObject.FindGameObjectWithTag("Player");
        mainPlayerStatus.positionX = playerReference.transform.position.x;
        mainPlayerStatus.positionY = playerReference.transform.position.y;
        mainPlayerStatus.animation_isAlive = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetBool("isAlive");
        mainPlayerStatus.animation_isTouchingTheGround = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetBool("isOnTheGround");


    }

    private void FixedUpdate()
    {
        timeLapse += Time.deltaTime;
        updateTimer(timeLapse);
    }

    private void updateTimer(float currentTime)
    {
        int newSecond = Mathf.FloorToInt(currentTime % secondsBound);

        if (newSecond != actualSecond)
        {
            actualSecond = newSecond;

            if (actualSecond == secondsBound -1)
            {
                //Debug.Log("PASARON " + secondsBound + " segundos.");
            }
        }
    }

    private InputPlayerStatus getSpecificPlayer(int playerId)
    {
        InputPlayerStatus toReturn = new InputPlayerStatus();
        switch (playerId)
        {
            case 0:
                toReturn.playerNumber = allPlayersStatus.playerNumber0;
                toReturn.name = allPlayersStatus.name0;
                toReturn.color = allPlayersStatus.color0;
                toReturn.isAlive = allPlayersStatus.isAlive0;
                toReturn.positionX = allPlayersStatus.positionX0;
                toReturn.positionY = allPlayersStatus.positionY0;
                toReturn.animation_isAlive = allPlayersStatus.animation_isAlive0;
                break;
            case 1:
                toReturn.playerNumber = allPlayersStatus.playerNumber1;
                toReturn.name = allPlayersStatus.name1;
                toReturn.color = allPlayersStatus.color1;
                toReturn.isAlive = allPlayersStatus.isAlive1;
                toReturn.positionX = allPlayersStatus.positionX1;
                toReturn.positionY = allPlayersStatus.positionY1;
                toReturn.animation_isAlive = allPlayersStatus.animation_isAlive1;
                break;
            case 2:
                toReturn.playerNumber = allPlayersStatus.playerNumber2;
                toReturn.name = allPlayersStatus.name2;
                toReturn.color = allPlayersStatus.color2;
                toReturn.isAlive = allPlayersStatus.isAlive2;
                toReturn.positionX = allPlayersStatus.positionX2;
                toReturn.positionY = allPlayersStatus.positionY2;
                toReturn.animation_isAlive = allPlayersStatus.animation_isAlive2;
                break;
            case 3:
                toReturn.playerNumber = allPlayersStatus.playerNumber3;
                toReturn.name = allPlayersStatus.name3;
                toReturn.color = allPlayersStatus.color3;
                toReturn.isAlive = allPlayersStatus.isAlive3;
                toReturn.positionX = allPlayersStatus.positionX3;
                toReturn.positionY = allPlayersStatus.positionY3;
                toReturn.animation_isAlive = allPlayersStatus.animation_isAlive3;
                break;
            case 4:
                toReturn.playerNumber = allPlayersStatus.playerNumber4;
                toReturn.name = allPlayersStatus.name4;
                toReturn.color = allPlayersStatus.color4;
                toReturn.isAlive = allPlayersStatus.isAlive4;
                toReturn.positionX = allPlayersStatus.positionX4;
                toReturn.positionY = allPlayersStatus.positionY4;
                toReturn.animation_isAlive = allPlayersStatus.animation_isAlive4;
                break;

            default:
                break;
        }

        return toReturn;
    }



}

[System.Serializable]
public class InputPlayerStatus
{
    public int playerNumber;
    public string name;
    public string color;
    public bool isAlive;
    public float positionX;
    public float positionY;
    public bool animation_isAlive;
    public bool animation_isTouchingTheGround;
}

[System.Serializable]
public class InputAllPlayerStatus
{
    public int playerNumber0;
    public string name0;
    public string color0;
    public bool isAlive0;
    public float positionX0;
    public float positionY0;
    public bool animation_isAlive0;
    public bool animation_isTouchingTheGround0;


    public int playerNumber1;
    public string name1;
    public string color1;
    public bool isAlive1;
    public float positionX1;
    public float positionY1;
    public bool animation_isAlive1;
    public bool animation_isTouchingTheGround1;


    public int playerNumber2;
    public string name2;
    public string color2;
    public bool isAlive2;
    public float positionX2;
    public float positionY2;
    public bool animation_isAlive2;
    public bool animation_isTouchingTheGround2;

    public int playerNumber3;
    public string name3;
    public string color3;
    public bool isAlive3;
    public float positionX3;
    public float positionY3;
    public bool animation_isAlive3;
    public bool animation_isTouchingTheGround3;

    public int playerNumber4;
    public string name4;
    public string color4;
    public bool isAlive4;
    public float positionX4;
    public float positionY4;
    public bool animation_isAlive4;
    public bool animation_isTouchingTheGround4;

    public string sys_status;
    public string sys_previousWinner;
}