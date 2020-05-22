SimpleWebSevice
---

A simple HTTP tool for Unity3D that interacts with JSON APIs via [UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html). Built with [SimpleJSON](https://github.com/Bunny83/SimpleJSON) and formally known as _WebServicesforUnity3d_. This tool is free to use, modify and distribute as needed.

## Get Started - Add Into Your Project
First step is to add the SimpleWebService into your project. 

To clone it, browse to the _Assets_ folder of your Unity Project and use `git clone`:

```bash
cd /path/to/unity/project/Assets/
git clone git@github.com:mw-felker/SimpleWebService.git
```

This will create a `SimpleWebService/` folder in your root _Assets/_ directory which will contain the source code you need.

### Methods
When the _SimpleWebService_ class is extended, you have access to the base methods which include:

- `base.Get(string URL, delegate callback)`

Todos (still porting):

  - POST
  - PUT
  - PATCH
  - DELETE

## Extend the SimpleWebService class
The _SimpleWebService_ class is meant to be a starting point for your own custom web services. Below is an example of the one such extension where we make a GET request to a mock JSON API and retrieve a list of todos. 

```C#
public class MyTodoAPIExample : SimpleWebService {

    // Get a list of todos when the script initializes 
    void Start() {
        GetTodoList();
    }

    // Make a request to the API to get a list of todos 
    void GetTodoList() {
        string URL = "https://jsonplaceholder.typicode.com/todos";
        base.Get(URL, TodoListSuccess);
    }

     // Log the API response of GetTodoList()
    void TodoListSuccess(JSONNode response) {
        Debug.Log(response);
    }
}

```
  
### What's Going On
In the above example, we see that `GetTodoList` implements the a `Get` call to a specific URL and provides a callback method of `TodoListSuccess`:

```C#
string URL = "https://jsonplaceholder.typicode.com/todos";
base.Get(URL, TodoListSuccess);
```

Under the hood, a coroutine is established for the HTTP request/response lifecycle. Once the response from the API is recieved, response text is parsed into a [JSONNode](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs#L62) object. This object is passed to the supplied callback method as single argument and when the coroutine completes, the callback method is fired:


```C#
void TodoListSuccess(JSONNode response) {
    Debug.Log(response);
}
```

From here, we can interacte with the _response_ data. Working with JSONNode is similar to the Javascript bracket notation pattern. You can see examples of this in the [_TodoAPIExample_](https://github.com/mw-felker/SimpleWebService/blob/master/TodoAPIExample.cs) that is included in this project.


## Abstract 
The purpose of this tool is to make it easier communicate with web JSON APIs. After writing custom request and response lifecycles for a few Unity Projects, it became apparent that a common approach could be adopted to streamline implementation. 

### UnityWebRequest - A building block, not a solution 
At the base, we have [UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html) (successor of [WWW](https://docs.unity3d.com/ScriptReference/WWW.html)) which is responsibile for the request and response lifecyle when communicating from Unity 3D to a web accessible property via HTTP. There are some examples on how to make GET and POST requests in the documentation, but to bring it beyond proof-of-concept you will need to manage the request/response. This involves establishing a coroutine per request/response lifecycle as well as setting up and firing a callback method upon coroutine completion.

### JSON Support - It still sucks 
The majority of modern web APIs will respond in the JSON and the need to support JSON inside Unity is continually increasing. JSON Serialization and specifically the [JSONUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) requires 1:1 translation of the JSON structure to an object. This is fine if the API response will never change but during iterative development or integrating with an API that is not under your control, this can drastically reduce progress. 

Furthermore, a successful request the `UnityWebRequest.downloadHandler` provides the JSON response as a string which needs to be transformed into useful format that can be accessed by Unity. 

### Why SWS exists
The above problems led to the creation of this project - we were tired of Googling and Stackoverflow combing. The goal was to implement a simple wrapper around UWR and provide flexible JSON parsing upon response so that our team could focus on integrating web APIs into our projects without starting from scratch each time. 