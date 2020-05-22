/*
 * Web Service
 * 
 * A simple HTTP get / post wrapper built on SimpleJSON.
 * 
 * Max Felker | felkerm@gmail.com
 * 
*/

using System; 
using UnityEngine;
using UnityEngine.Networking;
using System.Collections; 
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;

public class SimpleWebService : MonoBehaviour 
{
		
		// Set up our call back delegation, passing back a JSONNode obkect
  	protected delegate void CallBack(JSONNode response);
	
		// Make a GET request inside a Coroutine and pass the callback function upon completion 
		protected void Get(string URL, CallBack callback) => StartCoroutine(
			StartRequest(UnityWebRequest.Get(URL), callback)
		);

		// Sent the HTTP request, parse the JSON response and fire the callback 
    protected IEnumerator StartRequest( UnityWebRequest request, CallBack callback)
		{
				using (request) {

					yield return request.SendWebRequest();

					if ( request.isNetworkError || request.isHttpError ) {
						Debug.LogError(request.error);
					}

					JSONNode responseJSON = JSON.Parse(request.downloadHandler.text);

					if(callback != null) {
						callback(responseJSON);
					}
					
				}		
		}

    // POST to the server
    protected void Post(string URL, Dictionary<string,string> payload, CallBack callback)
		{

        // create the new form		
        WWWForm form = new WWWForm();
		
        // Add key,value pairs to POST array
	    foreach(KeyValuePair<String,String> data in payload) {
	   		form.AddField(data.Key, data.Value);
	    }
	    
        // Post the values to the URL		 
	    StartRequest(UnityWebRequest.Post(URL, form), callback);

    }

}