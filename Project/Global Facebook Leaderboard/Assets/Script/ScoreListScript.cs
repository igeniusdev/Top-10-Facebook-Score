using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class ScoreListScript : MonoBehaviour
{

	public Transform[]items;
	private static ScoreListScript scoreListInstance;

	void Awake(){
		scoreListInstance=this;
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//load list
	public void LoadScoreList(){
		StartCoroutine(LoadListCoroutine());
	}

	//load list coroutine
	IEnumerator LoadListCoroutine ()
	{
		for (int i = 0; i < items.Length; i++) {
			if (i < MySQLController.Instance.userId.Count) {
				string fbid=MySQLController.Instance.userId [i];
				items [i].FindChild ("Name-Text").GetComponent<Text> ().text = "" +MySQLController.Instance.userName[fbid];
				items [i].FindChild ("Score-Text").GetComponent<Text> ().text = "" +MySQLController.Instance.userScore[fbid];
				RawImage profile=items [i].FindChild ("Profile-Image").FindChild("Image").GetComponent<RawImage>();
				StartCoroutine(DownloadProfilePicture(fbid,profile));
				items[i].gameObject.SetActive(true);
			}else{
				items[i].gameObject.SetActive(false);
			}
		}
		yield return 0;
	}

	//download profile picture
	IEnumerator DownloadProfilePicture (string fbId, RawImage profile)
	{
		WWW www = new WWW ("https" + "://graph.facebook.com/" + fbId + "/picture?type=square"); //+ "?access_token=" + FB.AccessToken);
		
		Texture2D myTexture;
		yield return www;
		profile.texture = www.texture;
		
	}



	//score list singletone instance
	public static ScoreListScript Instance{
		get{
			return scoreListInstance;
		}
	}
}
