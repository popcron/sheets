![Woah spreadsheet woah!!!](https://cdn.discordapp.com/attachments/377316629220032523/498001441990901760/unknown.png)

# Sheets
This is an API that matches the version 4 of [Google's sheets API](https://developers.google.com/sheets/api/reference/rest/) for the most part. So if you'd like documentation on how to use this API, you have to consult their API instead, and use the C# bindings.
There is also a smaller high level layer on top, which is useful for people who just need to access the data from a 2D array.

## Requirements
- UnityEngine
- .NET Framework 4.5

## Removing the UnityEngine.dll dependency
This exists so that I could use it in my games. Taking the dll an adding it to your plugins folder will work out of the box.
To remove this dependency, the SheetsClient contains two methods for serializing and deserializing json data. These methods can be rewritten to use any other JSON API, like Newtonsoft.Json.

## Example
The requirements to using the Google Sheets API, is to have the spreadsheetId and an access token. The spreadsheetId can be retrieved from a url.

`https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}`

The API token can be created from the Google API Console, for more info on this, visit [this article by google](https://cloud.google.com/docs/authentication/api-keys)

```cs
public async void Start()
{
    string spreadsheetId = ""; //TODO: Get your own spreadsheetId
    string token = "";         //TODO: Get your own api token
    Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, token);

    Debug.Log("URL: " + spreadsheet.URL);
    Debug.Log("Title: " + spreadsheet.Title);
    Debug.Log("Rows: " + spreadsheet.Sheets[0].Rows);
    Debug.Log("Columns: " + spreadsheet.Sheets[0].Columns);
    
    Cell[,] data = spreadsheet.Sheets[0].Data;
    for (int x = 0; x < spreadsheet.Sheets[0].Columns; x++)
    {
        for (int y = 0; y < spreadsheet.Sheets[0].Rows; y++)
        {
            Debug.Log(data[x, y].Value);
        }
    }
}
```

If you'd like to use the low level API, you can use the `GetRaw()` method instead of `Get()`. The raw method alternative will out `SpreadsheetRaw` which is identical to the Google API reference.

If you want to work with both the low level and high level, you can create a raw spreadsheet from the high level spreadsheet by passing it into the constructor. The same can be done for converting a raw sheet to a high level sheet. This can not be done the other way around, and its by design.

```cs
SpreadsheetRaw raw = await SpreadsheetRaw.Get(spreadsheetId, token, includeGridData);
Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, token);

//create a new spreadsheet from the raw data
Spreadsheet spreadsheetConverted = new Spreadsheet(raw);
```

## FAQ
- **How do I add this to my unity project?**
Download the dll file from the releases, and place it in your Plugins folder.
- **What namespace is it under?**
 Popcron.Sheets
- **I got some forbidden exception**
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
- **GetRaw doesn't return any grid data!!**
 Use the hidden `includeGridData` parameter.
- **I'm using the low level API and I don't know what X does?**
Look at the Google Sheets API, because neither do I.
- **Can I send pull requests?**
Sure.
- **I got an error**
Send me the steps to reproduce it and I'll fix it.
