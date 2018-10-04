using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkService
{
    private const string xmlApi =
        "http://api.openweathermap.org/data/2.5/weather?q=London";
    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private bool isResponceValid(WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("bad connections..");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text))
        {
            Debug.Log("bad data...");
            return false;
        }
        else
            return true;
    }
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }
    private IEnumerator CallApi(string url, Action<string> callback)
    {
        WWW www = new WWW(url);

        if (!isResponceValid(www))
            yield break;
        callback(www.text);
    }
    public IEnumerator getWeatherXML(Action<string> callback)
    {
        return CallApi(xmlApi, callback);
    }
}
