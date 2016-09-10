using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class MySQLController : MonoBehaviour
{
	private string addScoreURL = "http://localhost/add_fb_score.php"; //be sure to add a ? to your url
	private string getScoreURL = "http://localhost/get_fg_score.php";


	private static MySQLController hsControllerInstance;
	public List<string> userId=new List<string>();
	public Dictionary<string,string> userName=new Dictionary<string, string>();
	public Dictionary<string,string> userScore=new Dictionary<string, string>();

	void Awake(){
		hsControllerInstance=this;
	}

	void Start()
	{
		//StartCoroutine(GetScores());
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static MySQLController Instance{
		get{
			return hsControllerInstance;
		}
	}

	/// <summary>
	/// Saves the score.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="score">Score.</param>
	public void SaveScore(string fbId,int score){
		StartCoroutine(PostScoreCoroutine(fbId,score));
	}

	/// <summary>
	/// Gets the score.
	/// </summary>
	public void GetScore(){
		StartCoroutine(GetScores());
	}

	
	// remember to use StartCoroutine when calling this function!
	IEnumerator PostScoreCoroutine(string fbid, int score)
	{

		string temp_fbId=fbid;

		//wait while userName=""
		while(FacebookLeaderboardManager.Instance.UserName==""){
			Debug.Log("Getting user Name");
			yield return 0;
		}

		//concate Name with fb id
		temp_fbId+="#"+FacebookLeaderboardManager.Instance.UserName;

		temp_fbId=WWW.EscapeURL(temp_fbId);
		Debug.Log("UserName:"+temp_fbId+"-"+score);

		string post_url = addScoreURL + "?fbid=" + temp_fbId + "&score=" + score;

	
		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done
		
		if (hs_post.error != null)
		{
			print("There was an error posting the high score: " + hs_post.error);

		}else{
			Debug.Log("Score Saved Successfully");

			//gettting the score
			StartCoroutine(GetScores());
		}



	}

	// Get the scores from the MySQL DB to display in a GUIText.
	// remember to use StartCoroutine when calling this function!
	IEnumerator GetScores()
	{
		//clear the dictionary
		userName.Clear();
		userScore.Clear();


		WWW hs_get = new WWW(getScoreURL);
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			print("There was an error getting the high score: " + hs_get.error);
		}
		else
		{
			Debug.Log(hs_get.text);
			ExtractResult(hs_get.text);
		}
	}

	//Extracting string
	public void ExtractResult(string resultText){
		string []data=resultText.Split(new char[]{','});
		for (int i = 0; i < data.Length; i++) {
			//spit fbid and score
			string []fbIdAndScore=data[i].Split(new char[]{'@'});
			//split name and score
			string []idAndName=fbIdAndScore[0].Split(new char[]{'#'});

			Debug.Log(fbIdAndScore[0]+" len:"+idAndName.Length);
			//FbId
			string id=idAndName[0];

			//Name
			string name=idAndName[1];

			//Score
			string score=fbIdAndScore[1];
			userId.Add(id);
			if(!userName.ContainsKey(id))
				userName.Add(id,name);

			if(!userScore.ContainsKey(id))
			userScore.Add(id,score);
		}

		//load score list
		ScoreListScript.Instance.LoadScoreList();
	}


	
}
