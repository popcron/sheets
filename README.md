![Woah spreadsheet woah!!!](https://cdn.discordapp.com/attachments/377316629220032523/498001441990901760/unknown.png)

# Sheets
This is an API that matches the version 4 of [Google's sheets API](https://developers.google.com/sheets/api/reference/rest/) for the most part. So if you'd like documentation on how to use this API, you have to consult their API instead, and use the C# bindings.
There is also a smaller high level layer on top, which is useful for people who just need to access the data from a 2D array.

## Requirements
- .NET Framework 4.5

## Setting up the serializer
An abstract SheetsSerializer class exists so that you can use any JSON serializer you'd like.

<details>
    <summary>Unity serializer</summary>
    
```cs
using UnityEngine;

public class JSONSerializer : SheetsSerializer
{
    public override T DeserializeObject<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }

    public override string SerializeObject(object data)
    {
        return JsonUtility.ToJson(data);
    }
}
```
</details>

<details>
    <summary>Netwonsoft.Json serializer</summary>
    
```cs
using Newtonsoft.Json;

public class JSONSerializer : SheetsSerializer
{
    public override T DeserializeObject<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public override string SerializeObject(object data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
```
</details>

## Example
The requirements to using the Google Sheets API, is to have the spreadsheetId and an access token. The spreadsheetId can be retrieved from a url. You will also need to provide the `SheetsSerializer.Serializer` property with your own serializer object.

`https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}`

The API token can be created from the Google API Console, for more info on this, visit [this article by google](https://cloud.google.com/docs/authentication/api-keys)

```cs
public async void Start()
{
    SheetsSerializer.Serializer = new SheetsSerializer(); //TODO: Create your own custom serializer/deserializer
    string spreadsheetId = ""; //TODO: Get your own spreadsheetId
    string key = "";         //TODO: Get your own key token
    
    Authorization authorization = await Authorization.Authorize(key);
    Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, authorization);

    Debug.Log("URL: " + spreadsheet.URL);
    Debug.Log("Title: " + spreadsheet.Title);
    
    Sheet sheet = spreadsheet.Sheets[0];
    Debug.Log("Rows: " + sheet.Rows);
    Debug.Log("Columns: " + sheet.Columns);
    
    Cell[,] data = sheet.Data;
    for (int x = 0; x < sheet.Columns; x++)
    {
        for (int y = 0; y < sheet.Rows; y++)
        {
            Debug.Log(data[x, y].Value);
        }
    }
}
```

If you'd like to use the low level API, you can use the `GetRaw()` method instead of `Get()`. The raw method alternative will give out a `SpreadsheetRaw` object, which is identical to the Google API reference.

If you want to work with both the low level and high level, you can create a spreadsheet from the low level spreadsheet by passing it into the constructor. The same can be done for converting a raw sheet to a high level sheet. This can not be done the other way around, and its by design.

```cs
SheetsSerializer.Serializer = new SheetsSerializer();
string key = ""; //TODO: Get a key
Authorization authorization = await Authorization.Authorize(key);

SpreadsheetRaw raw = await SpreadsheetRaw.Get(spreadsheetId, token, includeGridData);
Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, token, serializer);

//create a new spreadsheet from the raw data
Spreadsheet spreadsheetConverted = new Spreadsheet(raw);
```

## Authorization
Two ways to authorize are implemented. Using OAuth client ID and client secret, or using a provided key from the [Google Developer Console](https://console.developers.google.com/apis/credentials).

<details>
    <summary>Using keys</summary>

```cs
string key = "";
Authorizer authorizer = await Authorizer.Authorizer(key);
```
</details>
    
<details>
    <summary>Using OAuth</summary>
    
```cs
string clientId = "";
string clientSecret = "";
Authorizer authorizer = await Authorizer.Authorize(clientId, clientSecret);
```
</details>

*NOTE: Modifying calls require OAuth authorization*

## FAQ
- **How do I add this to my unity project?**
Download the dll file from the releases, and place it in your Plugins folder.
- **Why can't I do **`new SheetSerializer()`**?**
Because it is an abstract class, and you have to make your own implementation of this class.
- **What namespace?**
 Popcron.Sheets
- **I got some kind forbidden exception**
Ensure that your API key works, and that your spread sheet is accessible publicly.
- **Is it rows then columns, or columns then rows?**
No.
- **Is there XML documentation?**
 Yes.
- **Does the BatchUpdate method work?**
 Untested, I don't know.
- **What about the Create method?**
 Untested.
- **What works?**
 The Get method, and most of the high level and low level api.
- **I'm using the low level API and I don't know what X does?**
Look at the Google Sheets API, because neither do I.
- **Can I send pull requests?**
Sure.
- **I got an error**
Send me the steps to reproduce it and I'll try to fix it.
