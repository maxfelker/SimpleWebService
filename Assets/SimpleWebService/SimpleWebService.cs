/*
 * Simple Web Service
 * 
 * A simple HTTP tool for Unity3D that interacts with JSON APIs via UnityWebRequest
 * 
 * https://github.com/mw-felker/SimpleWebService
 * 
*/

using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SimpleWebService : MonoBehaviour
{

  // Set up our call back delegation, passing back a JSONNode obkect
  protected delegate void CallBack(JSONNode response);

  // Make a GET request inside a Coroutine and pass the callback function upon completion 
  protected void Get(string url, CallBack callback) => StartCoroutine(
    StartRequest(UnityWebRequest.Get(url), callback)
  );

	protected void Post(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, "POST", json);
    StartCoroutine(StartRequest(request, callback));
  }
	
	protected void PostForm(string url, Dictionary<string, string> payload, CallBack callback)
  {
		WWWForm formData = new WWWForm();
    foreach (KeyValuePair<String, String> data in payload)
    {
      formData.AddField(data.Key, data.Value);
    }
    UnityWebRequest request = UnityWebRequest.Post(url, formData);
    StartCoroutine(StartRequest(request, callback));
  }

	// Makes a PATCH request 
	protected void Patch(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, "PATCH", json);
    StartCoroutine(StartRequest(request, callback));
  }

	// Makes a PUT request
	protected void Put(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, "PUT", json);
    StartCoroutine(StartRequest(request, callback));
  }

	protected void Delete(string url, CallBack callback) => StartCoroutine(
    StartRequest(UnityWebRequest.Delete(url), callback)
  );

	// Generates a UnityWebRequest with a configuration HTTP method and sets the request type to JSON
	private UnityWebRequest GenerateUnityWebRequest(string url, string httpMethod, string json) 
	{
		// TODO: Make HTTP method an enum
		UnityWebRequest request = new UnityWebRequest(url, httpMethod);
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    return request;
	}

  // Makes the HTTP request, parse the JSON response and fire the callback 
  private IEnumerator StartRequest(UnityWebRequest request, CallBack callback)
  {
    using (request)
    {

      yield return request.SendWebRequest();

      // if we have an error, log it
      if (request.isNetworkError || request.isHttpError)
      {
        Debug.LogError(request.error);
      }
      else // parse the JSON response and fire call back 
      {

				JSONNode responseJSON = JSON.Parse(request.downloadHandler.text);

        if (callback != null)
        {
          callback(responseJSON);
        }
      }
    }
  }
}