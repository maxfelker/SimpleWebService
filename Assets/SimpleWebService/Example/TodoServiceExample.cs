/* 
 * Todo Example
 * ------------------------------
 * An example of how to extend the SimpleWebService class and make 
 * a request to web API that returns JSON. This examples
 * uses https://jsonplaceholder.typicode.com/ and it's 
 * free Todo API to illustrate the round trip.
 *
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class TodoServiceExample : SimpleWebService
{

    public int todoID = 1;
    string baseURL = "https://jsonplaceholder.typicode.com/todos";

    // Get a list of todos when the script initializes 
    void Start()
    {
        GetTodoById();
    }

    // Make a request to the API to get specific Todo by ID
    void GetTodoById() 
    {
        Debug.Log("Getting Todo ID " + todoID.ToString() + " from API...");
        string URL = baseURL + "/" + todoID;
        base.Get(URL, TodobyIdSuccess);
    }

    // Log the API response of GetTodoById()
    void TodobyIdSuccess(JSONNode response) 
    {
        Debug.Log("Todo Id: " + response["id"]);
        Debug.Log("Todo title: " + response["title"]);
        Debug.Log("Todo is completed: " + response["completed"]);
    }
}
