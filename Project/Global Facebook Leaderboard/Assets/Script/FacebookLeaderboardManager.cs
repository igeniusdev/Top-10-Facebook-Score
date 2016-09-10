using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.Unity;

public class FacebookLeaderboardManager : MonoBehaviour {

	private static FacebookLeaderboardManager facebookLeaderboardInstance;
	private string userName="";
	void Awake(){
		facebookLeaderboardInstance = this;
	}

	// Use this for initialization
	void Start () {

		userName="";
		FB.Init(this.OnInitComplete, this.OnHideUnity);
	}
	
	//Login And Save Score[I have used Random score you can set your highScore]
	public void CallFBLoginWithReadPermission()
	{
		Debug.Log("Call0");
		if (!FB.IsLoggedIn){
			FB.LogInWithReadPermissions (new List<string> () { "public_profile", "email", "user_friends"}, this.HandleResult);
		}else{

			StartCoroutine(GetUserNameCoroutine(AccessToken.CurrentAccessToken.UserId));

			MySQLController.Instance.SaveScore(AccessToken.CurrentAccessToken.UserId,10);
		}

	}
	
	private void OnInitComplete()
	{
		Debug.Log("FB-Init Completed");
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("On unity hide-isGameShown:"+isGameShown);
	}
	
	protected void HandleResult(IResult result)
	{
		if (result == null)
		{
			Debug.Log("Null");
		}else{

			//gettting the username
			StartCoroutine(GetUserNameCoroutine(AccessToken.CurrentAccessToken.UserId));

			//I have used Random score you can set your highScore
			MySQLController.Instance.SaveScore(AccessToken.CurrentAccessToken.UserId,Random.Range(50,10));
		}
	}

	//getting the username
	IEnumerator GetUserNameCoroutine(string fb_id){

		Debug.Log("Call1");
		string quary = fb_id + "?fields=name";
		FB.API (quary, HttpMethod.GET,UserNameCallBack);
		yield return 0;
	}


	//userName  Property
	public string UserName{
		get{
			return userName;
		}
	}

	
	//user name callback
	void UserNameCallBack (IResult result)
	{
		if (result.Error != null) {                                                                      
			Debug.Log ("Error");
		} else {
			userName = "" + result.ResultDictionary ["name"];
			Debug.Log("userName");
		}
	}


	public static FacebookLeaderboardManager Instance{
		get{
			return facebookLeaderboardInstance;
		}
	}

}
