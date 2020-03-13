using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketConnectionInteraction : MonoBehaviour
{
	/*
	on hoverbot-1:
	ssh pi@192.168.0.196 <far1 @FAR>
	(192.168.0.241 for hoverbot-2)
	cd ~/chairbot/chairbot-interface-server
	npm start local-config.json &
	cd ~/chairbot/chairbot-motor-websocket-proxy/
	node main.js &
	then go to your chrome browser:
	192.168.0.196:5000
	*/
	public GesturePresentChair present;  //Chair digital model.

	private Queue<string> SendQueue= new Queue<string>();
	bool FinishedSending = true;
	private IEnumerator move;

	WebSocket ws;
	public bool sendF, sendB;
	public float duration = 1.0f;

	void Start()
    {
		present = GetComponent<GesturePresentChair>();

		//ws = new WebSocket("ws://127.0.0.1:8080"); // localhost
		ws = new WebSocket("ws://192.168.0.196:5000/web-controller");  //ws://192.168.0.196:5000/web-controller is on hoverbot-1

		ws.OnOpen += (sender, e) =>
		{
			Debug.Log("We just connected to the Chair.");			
		};
		ws.OnClose += (sender, e) =>
		{
			Debug.Log("We just Closed");
			Debug.Log(e.WasClean);
		};
		ws.OnError += (sender, e) =>
		{
			Debug.Log("Gotan Error");
			Debug.Log(e.Message);
		};
		ws.Connect();
	}

	public void AddAction(float fwd, int turn)
	{
		//FindObjectOfType<WebSocketConnectionInteraction>().AddAction(0.5f, 0);  //In Kinect controller
		BotVariablesInteract bv = new BotVariablesInteract();
		bv.action = "requestForced";
		bv.forward = fwd;
		bv.turn = turn;
		bv.topSpeed = 50;
		bv.accel = 300;

		string s = JsonUtility.ToJson(bv);
		string command = s.Replace("\"bot\":\"\"", "\"bot\":null");
		SendQueue.Enqueue(command);
	}

	private IEnumerator Move(float fwd, int turn)
	{
		AddAction(fwd, turn);
		yield return new WaitForSeconds(duration);
		AddAction(0.0f, 0);
	}

    public void MoveUp()
    {
        move = Move(-0.5f, 0);
        StartCoroutine(move);
        present.MoveUp();
    }

    public void MoveDown()
    {
        move = Move(0.5f, 0);
        StartCoroutine(move);
        present.MoveDown();
    }

    void Update()
    {
		if (sendF)
		{
			sendF = false;
            MoveUp();
		} else if (sendB)
		{
			sendB = false;
            MoveDown();
		}

		if (ws.IsConnected)
		{
			if (FinishedSending && SendQueue.Count > 0) {
				FinishedSending = false;
				ws.SendAsync(SendQueue.Dequeue(), delegate (bool result) { FinishedSending = true; });
			}
		}
		else
		{
			ws.ConnectAsync();
		}
    }

	private void OnApplicationQuit()
	{
		//ws.CloseAsync();
	}
}

public class BotVariablesInteract
{
	public string action;
	public string bot = null;
	public float forward;
	public float turn;
	public int topSpeed;
	public int accel;
}
